////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Text;
using UnityEngine;
using UnityEditor;
using System;

namespace MG.MDV
{
    ////////////////////////////////////////////////////////////////////////////////
    /// <see cref="Markdig.Renderers.HtmlRenderer"/>

    public class RendererMarkdown : RendererBase
    {
        public RendererMarkdown()
        {
            ObjectRenderers.Add( new RendererBlockCode() );
            ObjectRenderers.Add( new RendererBlockList() );
            ObjectRenderers.Add( new RendererBlockHeading() );
            ObjectRenderers.Add( new RendererBlockHtml() );
            ObjectRenderers.Add( new RendererBlockParagraph() );
            ObjectRenderers.Add( new RendererBlockQuote() );
            ObjectRenderers.Add( new RendererBlockThematicBreak() );

            ObjectRenderers.Add( new RendererInlineLink() );
            ObjectRenderers.Add( new RendererInlineAutoLink() );
            ObjectRenderers.Add( new RendererInlineCode() );
            ObjectRenderers.Add( new RendererInlineDelimiter() );
            ObjectRenderers.Add( new RendererInlineEmphasis() );
            ObjectRenderers.Add( new RendererInlineLineBreak() );
            ObjectRenderers.Add( new RendererInlineHtml() );
            ObjectRenderers.Add( new RendererInlineHtmlEntity() );
            ObjectRenderers.Add( new RendererInlineLiteral() );
        }

        ////////////////////////////////////////////////////////////////////////////////

        float   mPadding    = 8.0f;
        float   mMaxWidth   = 100.0f;

        Vector2 mContentOffset;
        Vector2 mCursor;

        public void Init( float headerHeight )
        {
            mContentOffset.x = mPadding;
            mContentOffset.y = headerHeight + mPadding;

            mCursor   = mContentOffset;
            mMaxWidth = Screen.width - mPadding * 2.0f;
        }

        public override object Render( MarkdownObject document )
        {
            Write( document );
            FlushLine();

            return null;
        }


        ////////////////////////////////////////////////////////////////////////////////
        /// <see cref="Markdig.Renderers.TextRendererBase.WriteLeafInline"/>

        internal void WriteLeafBlockInline( LeafBlock block )
        {
            var inline = block.Inline as Inline;

            while( inline != null )
            {
                Write( inline );
                inline = inline.NextSibling;
            }
        }

        /// <see cref="Markdig.Renderers.HtmlRenderer.WriteLeafRawLines"/>

        internal void WriteLeafRawLines( LeafBlock block )
        {
            if( block.Lines.Lines == null )
            {
                return;
            }

            var lines  = block.Lines;
            var slices = lines.Lines;

            for( int i = 0; i < lines.Count; i++ )
            {
                Print( slices[ i ].ToString() );
                Print( "\n" );
            }
        }


        ////////////////////////////////////////////////////////////////////////////////
        // style

        public bool     Bold       = false;
        public bool     Italic     = false;
        public bool     FixedWidth = false;
        public string   ToolTip    = null; // TODO: tooltip
        public string   Link       = null;

        int mSize = 0;

        public int Size
        {
            get { return mSize; }
            set { mSize = Mathf.Clamp( value, 0, 6 ); }
        }


        //------------------------------------------------------------------------------

        GUIStyle      mCurStyle   = null;
        float         mLineHeight = 12.0f;

        StringBuilder mWord       = new StringBuilder( 1024 );
        int           mWordStart  = 0;
        float         mWordWidth  = 0.0f;

        public void NewLine()
        {
            mCursor.y += mLineHeight;
            mCursor.x = mContentOffset.x;
        }

        private void ClearWord()
        {
            mWordWidth = 0.0f;
            mWordStart = 0;
            mWord.Clear();
        }

        private void PrintWord()
        {
            if( mWord.Length == mWordStart )
            {
                return;
            }

            if( mCursor.x + mWordWidth > mMaxWidth )
            {
                NewLine();
            }

            // TODO: split word?
            var rect = new Rect( mCursor, new Vector2( mWordWidth, mLineHeight ) );

            if( Link == null )
            {
                GUI.Label( rect, mWord.ToString(), mCurStyle );
            }
            else if( GUI.Button( rect, mWord.ToString(), mCurStyle ) )
            {
                Debug.Log( "GOTO: " + Link );
            }

            mCursor.x += mWordWidth;

            ClearWord();
        }


        //------------------------------------------------------------------------------

        internal void Print( string text )
        {
            // TODO: cache working version?

            mCurStyle = FixedWidth ? GUI.skin.GetStyle( "code" ) : GUI.skin.label;

            var fontInfo = FixedWidth ? Fonts.Fixed : Fonts.Variable;
            var fontSize  = 11.0f + (Size == 0 ? 0 : 7 - Size );
            var fontStyle = Bold ? FontStyle.Bold : FontStyle.Normal;

            if( Italic )
            {
                fontStyle = (FontStyle) ( (int) fontStyle + (int) FontStyle.Italic );
            }

            mCurStyle.fontSize  = (int) fontSize;
            mCurStyle.fontStyle = fontStyle;

            mCurStyle.normal.textColor = Link != null ? Color.blue : Color.black;

            mLineHeight = mCurStyle.lineHeight;


            // TODO: pre-cache
            float space;
            float question;
            float advance;

            fontInfo.GetAdvance( ' ', out space, fontSize, fontStyle );
            fontInfo.GetAdvance( '?', out question, fontSize, fontStyle );

            ClearWord();

            for( var i = 0; i < text.Length; i++ )
            {
                var ch = text[ i ];

                if( ch == '\n' )
                {
                    PrintWord();
                    NewLine();
                    continue;
                }

                if( fontInfo.GetAdvance( ch, out advance, fontSize, fontStyle ) )
                {
                    mWord.Append( ch );
                    mWordWidth += advance;
                }
                else
                {
                    mWord.Append( '?' );
                    mWordWidth += question;
                }

                if( char.IsWhiteSpace( ch ) )
                {
                    PrintWord();
                }
            }

            PrintWord();
        }

        //------------------------------------------------------------------------------

        internal string GetText()
        {
            throw new NotImplementedException();
        }

        internal void FlushLine()
        {
            NewLine();
            NewLine();
        }

        internal void Break()
        {
            NewLine();

            var rect = new Rect( mCursor, new Vector2( Screen.width - 50.0f, 1.0f ) );
            GUI.Label( rect, string.Empty, GUI.skin.GetStyle( "hr" ) );

            NewLine();
        }

    }
}
