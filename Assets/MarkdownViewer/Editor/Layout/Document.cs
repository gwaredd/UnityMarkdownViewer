////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Assertions;

namespace MG.MDV
{
    /******************************************************************************
     * Layout
     ******************************************************************************/

    public class Container
    {
        public List<Row> Rows = new List<Row>();

        public Vector2 Layout( Vector2 pos, float maxWidth )
        {
            float oy = pos.y;

            foreach( var row in Rows )
            {
                var size = row.Layout( pos, maxWidth );
                pos.y += size.y;
            }

            return new Vector2( maxWidth, pos.y - oy );
        }
    }

    //------------------------------------------------------------------------------

    public class Row
    {
        public List<Col> Cols = new List<Col>();

        public Vector2 Layout( Vector2 pos, float maxWidth )
        {
            float oy = pos.y;

            foreach( var col in Cols )
            {
                var size = col.Layout( pos, maxWidth );
                pos.y += size.y;
            }

            return new Vector2( maxWidth, pos.y - oy );
        }
    }

    //------------------------------------------------------------------------------

    public class Col
    {
        public List<Block> Blocks = new List<Block>();

        public bool IsEmpty { get { return Blocks.Count == 0; } }

        public Block NewBlock( float indent )
        {
            var block = new Block( indent );
            Blocks.Add( block );
            return block;
        }

        public Vector2 Layout( Vector2 pos, float maxWidth )
        {
            float oy = pos.y;

            foreach( var block in Blocks )
            {
                var size = block.Layout( pos, maxWidth );
                pos.y += size.y;
            }

            return new Vector2( maxWidth, pos.y - oy );
        }
    }

    //------------------------------------------------------------------------------

    public class Block
    {
        public float         Indent  = 0.0f;
        public string        Prefix  = null;
        public List<Content> Content = new List<Content>();

        public Block( float indent )
        {
            Indent = indent;
        }

        public Vector2 Layout( Vector2 pos, float maxWidth )
        {
            if( Content.Count == 0 )
            {
                return Vector2.zero;
            }

            var origin = pos;

            pos.x    += Indent;
            maxWidth -= Indent;

            Assert.IsTrue( maxWidth > 0.0f ); // TODO: handle narrow windows!

            if( !string.IsNullOrEmpty( Prefix ) )
            {
                Debug.LogWarning( "TODO: Prefix" );
            }

            var rowWidth   = Content[0].Width;
            var rowHeight  = Content[0].Height;
            var startIndex = 0;

            for( var i = 1; i < Content.Count; i++ )
            {
                var cnt = Content[i];

                if( rowWidth + cnt.Width > maxWidth )
                {
                    LayoutRow( pos, startIndex, i, rowHeight );
                    pos.y += rowHeight;

                    startIndex = i;
                    rowWidth   = cnt.Width;
                    rowHeight  = cnt.Height;

                }
                else
                {
                    rowWidth += cnt.Width;
                    rowHeight = Mathf.Max( rowHeight, cnt.Height );
                }
            }

            if( startIndex < Content.Count )
            {
                LayoutRow( pos, startIndex, Content.Count, rowHeight );
                pos.y += rowHeight;
            }

            return new Vector2( maxWidth, pos.y - origin.y );
        }

        void LayoutRow( Vector2 pos, int from, int until, float rowHeight )
        {
            for( var i = from; i < until; i++ )
            {
                var cnt = Content[i];

                cnt.Location.x = pos.x;
                cnt.Location.y = pos.y + rowHeight - cnt.Height;

                pos.x += cnt.Width;
            }
        }
    }

    //------------------------------------------------------------------------------

    public class Content
    {
        public Rect         Location;
        public Style        Style;
        public GUIContent   Payload;
        public string       Link;

        public float    Width    { get { return Location.width;  } }
        public float    Height   { get { return Location.height; } }

        public Content( GUIContent payload, Style style, string link )
        {
            Payload = payload;
            Style   = style;
            Link    = link;
        }

        public void Draw( StyleCache context, IActionHandlers handlers )
        {
            if( string.IsNullOrEmpty( Link ) )
            {
                GUI.Label( Location, Payload, context.Apply( Style ) );
            }
            else if( GUI.Button( Location, Payload, context.Apply( Style ) ) )
            {
                if( Regex.IsMatch( Link, @"^\w+:", RegexOptions.Singleline ) )
                {
                    Application.OpenURL( Link );
                }
                else
                {
                    handlers.SelectPage( Link );
                }
            }
        }
    }

    /******************************************************************************
     * Document
     ******************************************************************************/

    public class Document
    {
        const float IndentSize = 20.0f;

        IActionHandlers mActions    = null;
        StyleCache      mStyleCache = null;
        List<Container> mContainers = new List<Container>();
        List<Content>   mContent    = new List<Content>();

        float mIndent;
        Col   mColumn;
        Block mBlock;

        public Document( StyleCache context, IActionHandlers actions )
        {
            mActions    = actions;
            mStyleCache = context;

            var div = new Container();
            var row = new Row();
            var col = new Col();

            mContainers.Add( div );
            div.Rows.Add( row );
            row.Cols.Add( col );

            mColumn = col;
            mBlock  = null;
            mIndent = 0.0f;
        }


        ////////////////////////////////////////////////////////////////////////////////
        // add content

        GUIStyle      mStyleGUI    = null;
        Style         mStyleLayout = new Style();
        string        mLink        = null;
        string        mTooltip     = null;
        StringBuilder mWord        = new StringBuilder( 1024 );

        void EnsureBlock()
        {
            if( mColumn.IsEmpty )
            {
                mBlock = mColumn.NewBlock( mIndent );
            }
        }

        //------------------------------------------------------------------------------

        public void Text( string text, Style style, string link, string toolTip )
        {
            EnsureBlock();

            mStyleLayout = style;
            mStyleGUI    = mStyleCache.Apply( style );
            mLink        = link;
            mTooltip     = toolTip;

            for( var i = 0; i < text.Length; i++ )
            {
                var ch = text[i];

                if( ch == '\n' )
                {
                    AddWord();
                    NewLine();
                }
                else if( char.IsWhiteSpace( ch ) )
                {
                    mWord.Append( ' ' );
                    AddWord();
                }
                else
                {
                    mWord.Append( ch );
                }
            }

            AddWord();
        }

        void AddWord()
        {
            if( mWord.Length == 0 )
            {
                return;
            }

            var payload = new GUIContent( mWord.ToString(), mTooltip );
            var content = new Content( payload, mStyleLayout, mLink );

            var size = mStyleGUI.CalcSize( payload );
            content.Location.width  = size.x;
            content.Location.height = size.y;

            mBlock.Content.Add( content );
            mContent.Add( content );

            mWord.Clear();
        }


        //------------------------------------------------------------------------------

        public void Image( string url, string alt, string title )
        {
            EnsureBlock();

            var tex     = mActions.FetchImage( url );
            var payload = new GUIContent( mWord.ToString(), mTooltip );

            if( tex != null )
            {
                payload.image   = tex;
                payload.tooltip = !string.IsNullOrEmpty( title ) ? title : alt;
            }
            else
            {
                var text = !string.IsNullOrEmpty( alt ) ? alt : url;
                payload.text = $"[{text}]";
            }

            var content = new Content( payload, mStyleLayout, mLink );

            var size = mStyleGUI.CalcSize( payload );
            content.Location.width = size.x;
            content.Location.height = size.y;

            mBlock.Content.Add( content );
            mContent.Add( content );
        }


        //------------------------------------------------------------------------------

        public void HorizontalLine()
        {
            NewLine();
            Debug.Log( "TODO: horizontal line" );
            NewLine();
        }

        public void NewLine()
        {
            mBlock = mColumn.NewBlock( mIndent );
        }

        public void Indent()
        {
            mIndent += IndentSize;
        }

        public void Outdent()
        {
            mIndent = Mathf.Max( mIndent - IndentSize, 0.0f );
        }

        public void Prefix( string prefix )
        {
            EnsureBlock();
            mBlock.Prefix = prefix;
        }


        ////////////////////////////////////////////////////////////////////////////////
        // layout and draw

        public void Layout( float maxWidth )
        {
            var pos = Vector2.zero;

            foreach( var container in mContainers )
            {
                var size = container.Layout( pos, maxWidth );
                pos.y += size.y;
            }
        }

        public void Draw()
        {
            mStyleCache.Reset();
            mContent.ForEach( c => c.Draw( mStyleCache, mActions ) );
        }
    }
}
