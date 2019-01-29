////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MG.MDV
{
    public class Layout
    {
        public float Height = 100.0f;

        Context         mContext;
        List<Container> mContainers = new List<Container>();
        List<Block>     mBlocks     = new List<Block>();

        float           mIndent;
        Col             mColumn;
        BlockContent    mCursor;


        public Layout( StyleCache styleCache, IActions actions )
        {
            mContext = new Context();
            mContext.Actions = actions;
            mContext.StyleLayout    = styleCache;
            mContext.Reset();

            mIndent     = 0.0f;
            mBlockQuote = false;

            var container = new Container();
            var row = new Row();
            var col = new Col();

            mContainers.Add( container );
            container.Rows.Add( row );
            row.Cols.Add( col );

            mColumn = col;
            NewContentBlock();
        }


        ////////////////////////////////////////////////////////////////////////////////
        // add content

        Style         mStyleLayout  = new Style();
        string        mLink         = null;
        string        mTooltip      = null;
        StringBuilder mWord         = new StringBuilder( 1024 );
        bool          mBlockQuote   = false;

        public bool BlockQuote
        {
            get
            {
                return mBlockQuote;
            }

            set
            {
                mCursor.Highlight = mBlockQuote;
                mBlockQuote = value;
            }
        }


        //------------------------------------------------------------------------------

        public void Text( string text, Style style, string link, string tooltip )
        {
            mContext.Apply( style );

            mStyleLayout = style;
            mLink        = link;
            mTooltip     = tooltip;

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

            content.Location.size = mContext.CalcSize( payload );

            mCursor.Add( content );

            mWord.Clear();
        }


        //------------------------------------------------------------------------------

        public void Image( string url, string alt, string title )
        {
            var payload = new GUIContent();
            var content = new ContentImage( payload, mStyleLayout, mLink );

            content.URL     = url;
            content.Alt     = alt;
            payload.tooltip = !string.IsNullOrEmpty( title ) ? title : alt;

            mCursor.Add( content );
        }


        //------------------------------------------------------------------------------

        private void AddBlock( Block block )
        {
            mColumn.Blocks.Add( block );
            mBlocks.Add( block );
        }

        private void NewContentBlock()
        {
            var block = new BlockContent( mIndent );
            AddBlock( block );
            mCursor = block;
            mContext.Reset();
        }

        //------------------------------------------------------------------------------

        public void HorizontalLine()
        {
            AddBlock( new BlockLine() );
            NewContentBlock();
        }

        public void Space()
        {
            AddBlock( new BlockSpace() );
            NewContentBlock();
        }

        public void NewLine()
        {
            if( mCursor.IsEmpty == false )
            {
                NewContentBlock();
            }
        }

        public void Indent()
        {
            mIndent += mContext.IndentSize;
            mCursor.Indent = mIndent;
        }

        public void Outdent()
        {
            mIndent = Mathf.Max( mIndent - mContext.IndentSize, 0.0f );
            mCursor.Indent = mIndent;
        }

        public void Prefix( string text, Style style )
        {
            mContext.Apply( style );

            var payload = new GUIContent( text );
            var content = new ContentText( payload, style, null );
            content.Location.size = mContext.CalcSize( payload );

            mCursor.Prefix( content );
        }


        ////////////////////////////////////////////////////////////////////////////////
        // layout and draw

        public void Arrange( float maxWidth )
        {
            mContext.Reset();
            
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
            mContext.Reset();
            mBlocks.ForEach( block => block.Draw( mContext ) );
        }
    }
}
