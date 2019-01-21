////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using System;

namespace MG.MDV
{
    /******************************************************************************
     * Layout
     ******************************************************************************/

    public class Context
    {
        public StyleCache       Style;
        public IActionHandlers  Action;

        public GUIStyle Apply( Style style ) { return Style.Apply( style ); }
    }

    public class Container
    {
        public List<Row> Rows = new List<Row>();

        public Vector2 Arrange( Context context, Vector2 pos, float maxWidth )
        {
            float oy = pos.y;

            foreach( var row in Rows )
            {
                var size = row.Arrange( context, pos, maxWidth );
                pos.y += size.y;
            }

            return new Vector2( maxWidth, pos.y - oy );
        }
    }

    //------------------------------------------------------------------------------

    public class Row
    {
        public List<Col> Cols = new List<Col>();

        public Vector2 Arrange( Context context, Vector2 pos, float maxWidth )
        {
            float oy = pos.y;

            foreach( var col in Cols )
            {
                var size = col.Arrange( context, pos, maxWidth );
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

        public Vector2 Arrange( Context context, Vector2 pos, float maxWidth )
        {
            float oy = pos.y;

            foreach( var block in Blocks )
            {
                var size = block.Arrange(context, pos, maxWidth );
                pos.y += size.y;
            }

            return new Vector2( maxWidth, pos.y - oy );
        }
    }

    //------------------------------------------------------------------------------

    public abstract class Block
    {
        protected float mIndent  = 0.0f;

        public abstract Vector2 Arrange( Context context, Vector2 pos, float maxWidth );
        public abstract void Draw( Context context );
    }

    public class BlockLine : Block
    {
        private Rect Rect = new Rect();

        public override void Draw( Context context )
        {
            var rect = new Rect( Rect.position.x, Rect.center.y, Rect.width, 1.0f );
            GUI.Label( rect, string.Empty, GUI.skin.GetStyle( "hr" ) );
        }

        public override Vector2 Arrange( Context context, Vector2 pos, float maxWidth )
        {
            Rect.position = pos;
            Rect.width    = maxWidth;
            Rect.height   = 10.0f;

            return Rect.size;
        }
    }

    public class BlockContent : Block
    {
        Rect          mRect      = new Rect();
        bool          mHighlight = false;
        Content       mPrefix    = null;
        List<Content> mContent   = new List<Content>();

        public BlockContent( float indent )
        {
            mIndent = indent;
        }

        public void Add( Content content )
        {
            mContent.Add( content );
        }

        public void Prefix( Content content )
        {
            mPrefix = content;
        }

        public void Highlight()
        {
            mHighlight = true;
        }

        public override Vector2 Arrange( Context context, Vector2 pos, float maxWidth )
        {
            var origin = pos;

            mRect.position = pos;

            pos.x += mIndent;
            maxWidth -= mIndent;

            Assert.IsTrue( maxWidth > 0.0f ); // TODO: handle narrow windows!

            // prefix

            if( mPrefix != null )
            {
                mPrefix.Location.x = pos.x - 20.0f; // IndentSize  / 2.0f
                mPrefix.Location.y = pos.y;
            }

            // content

            if( mContent.Count == 0 )
            {
                return Vector2.zero;
            }

            mContent.ForEach( c => c.Update( context ) );

            var rowWidth   = mContent[0].Width;
            var rowHeight  = mContent[0].Height;
            var startIndex = 0;

            for( var i = 1; i < mContent.Count; i++ )
            {
                var cnt = mContent[i];

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

            if( startIndex < mContent.Count )
            {
                LayoutRow( pos, startIndex, mContent.Count, rowHeight );
                pos.y += rowHeight;
            }

            mRect.size = new Vector2( maxWidth, pos.y - origin.y );

            return mRect.size;
        }

        void LayoutRow( Vector2 pos, int from, int until, float rowHeight )
        {
            for( var i = from; i < until; i++ )
            {
                var cnt = mContent[i];

                cnt.Location.x = pos.x;
                cnt.Location.y = pos.y + rowHeight - cnt.Height;

                pos.x += cnt.Width;
            }
        }

        public override void Draw( Context context  )
        {
            if( mHighlight )
            {
                GUI.Box( mRect, string.Empty );
            }

            mContent.ForEach( c => c.Draw( context ) );
            mPrefix?.Draw( context );
        }
    }

    //------------------------------------------------------------------------------

    public abstract class Content
    {
        public Rect         Location;
        public Style        Style;
        public GUIContent   Payload;
        public string       Link;

        public float Width  { get { return Location.width; } }
        public float Height { get { return Location.height; } }

        public Content( GUIContent payload, Style style, string link )
        {
            Payload = payload;
            Style   = style;
            Link    = link;
        }

        public void Draw( Context context )
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
                    context.Action.SelectPage( Link );
                }
            }
        }

        public virtual void Update( Context context )
        {
        }
    }

    public class ContentText : Content
    {
        public ContentText( GUIContent payload, Style style, string link )
            : base( payload, style, link )
        {
        }
    }

    public class ContentImage : Content
    {
        public string URL;
        public string Alt;

        public ContentImage( GUIContent payload, Style style, string link )
            : base( payload, style, link )
        {
        }

        public override void Update( Context context )
        {
            Payload.image = context.Action.FetchImage( URL );
            Payload.text  = null;

            if( Payload.image == null )
            {
                context.Apply( Style );
                var text = !string.IsNullOrEmpty( Alt ) ? Alt : URL;
                Payload.text = $"[{text}]";
            }

            Location.size = context.Style.Active.CalcSize( Payload );
        }
    }

    /******************************************************************************
     * Document
     ******************************************************************************/

    public class Layout
    {
        const float IndentSize = 40.0f; // TODO: calculate from font height?

        public float Height = 100.0f;

        Context         mContext    = new Context();
        List<Container> mContainers = new List<Container>();
        List<Block>     mBlocks     = new List<Block>();

        float           mIndent;
        Col             mColumn;
        BlockContent    mBlock;


        public Layout( StyleCache styleCahe, IActionHandlers actions )
        {
            mContext.Action = actions;
            mContext.Style  = styleCahe;

            mStyleGUI       = styleCahe.Reset();

            var div = new Container();
            var row = new Row();
            var col = new Col();

            mContainers.Add( div );
            div.Rows.Add( row );
            row.Cols.Add( col );

            mIndent = 0.0f;
            mColumn = col;
            mBlock  = null;
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
                NewLine();
            }
        }

        //------------------------------------------------------------------------------

        public void Text( string text, Style style, string link, string toolTip )
        {
            EnsureBlock();

            mStyleLayout = style;
            mStyleGUI    = mContext.Style.Apply( style );
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
            var content = new ContentText( payload, mStyleLayout, mLink );

            content.Location.size = mStyleGUI.CalcSize( payload );

            mBlock.Add( content );

            mWord.Clear();
        }


        //------------------------------------------------------------------------------

        public void Image( string url, string alt, string title )
        {
            EnsureBlock();

            var payload = new GUIContent();
            var content = new ContentImage( payload, mStyleLayout, mLink );

            content.URL     = url;
            content.Alt     = alt;
            payload.tooltip = !string.IsNullOrEmpty( title ) ? title : alt;

            mBlock.Add( content );
        }


        //------------------------------------------------------------------------------

        public void HorizontalLine()
        {
            NewLine();
            var block = new BlockLine();
            mColumn.Blocks.Add( block );
            mBlocks.Add( block );
            NewLine();
        }

        public void NewLine()
        {
            var block = new BlockContent( mIndent );
            mColumn.Blocks.Add( block );
            mBlocks.Add( block );
            mBlock = block;
        }

        public void Indent()
        {
            mIndent += IndentSize;
            NewLine();
        }

        public void Outdent()
        {
            mIndent = Mathf.Max( mIndent - IndentSize, 0.0f );
        }

        public void Prefix( string text, Style style )
        {
            mStyleGUI = mContext.Style.Apply( style );

            var payload = new GUIContent( text );
            var content = new ContentText( payload, style, null );

            content.Location.size = mStyleGUI.CalcSize( payload );

            mBlock.Prefix( content );
        }

        public void HighLightBlock()
        {
            EnsureBlock();
            mBlock.Highlight();
        }


        ////////////////////////////////////////////////////////////////////////////////
        // layout and draw

        public void Arrange( float maxWidth )
        {
            mStyleGUI = mContext.Style.Reset();
            
            var pos = Vector2.zero;

            foreach( var container in mContainers )
            {
                var size = container.Arrange( mContext, pos, maxWidth );
                pos.y += size.y;
            }

            Height = pos.y;
        }

        public void Draw()
        {
            mStyleGUI = mContext.Style.Reset();
            mBlocks.ForEach( block => block.Draw( mContext ) );
        }
    }
}
