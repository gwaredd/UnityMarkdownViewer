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

        StringBuilder mText = new StringBuilder( 2048 );

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
        // 

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

        public bool Bold    { get; set; }
        public bool Italic  { get; set; }
        //public FontStyle FontStyle = FontStyle.Normal;

        internal GUIStyle GetStyle( string style )
        {
            return GUI.skin != null ? GUI.skin.GetStyle( style ) : null;
        }


        //------------------------------------------------------------------------------

        CharacterInfo mCharacterInfo = new CharacterInfo();
        StringBuilder mWord          = new StringBuilder( 1024 );

        internal void Print( string text )
        {
            // print words (in current style) with wrapping

            var style      = GUI.skin.label;
            var fontSize   = style.fontSize;
            var lineHeight = style.lineHeight;
            var font       = style.font ?? GUI.skin.font;

            // TODO: pre-cache widths?
            font.GetCharacterInfo( ' ', out mCharacterInfo, fontSize, FontStyle.Normal );
            var space = (float) mCharacterInfo.advance;

            var wordWidth = 0.0f;

            mWord.Clear();

            for( var i = 0; i < text.Length; i++ )
            {
                var ch = text[ i ];

                if( char.IsWhiteSpace( ch ) )
                {
                    if( mWord.Length > 0 )
                    {
                        if( ch == '\n' || mCursor.x + wordWidth > mMaxWidth )
                        {
                            mCursor.y += lineHeight;
                            mCursor.x = mContentOffset.x;
                        }

                        var pos = new Rect( mCursor, new Vector2( wordWidth, lineHeight * 2.0f ) );
                        GUI.Label( pos, mWord.ToString(), style );

                        mCursor.x += wordWidth;

                        wordWidth = 0.0f;
                        mWord.Clear();
                    }

                    mCursor.x += space;
                }
                else if( font.GetCharacterInfo( ch, out mCharacterInfo, fontSize, FontStyle.Normal ) )
                {
                    wordWidth += mCharacterInfo.advance;
                    mWord.Append( ch );
                }
                else
                {
                    // TODO: better way to handle unknown characters?
                }
            }

            // print last word

                // does word fit? - new line (or split?)

                // ObjectRenderers.Add( new RendererInlineLiteral() );

                // ObjectRenderers.Add( new RendererInlineEmphasis() );
                // ObjectRenderers.Add( new RendererInlineLineBreak() );

                // ObjectRenderers.Add( new RendererInlineLink() );
                // ObjectRenderers.Add( new RendererInlineAutoLink() );

                // ObjectRenderers.Add( new RendererInlineCode() );

        }


        //------------------------------------------------------------------------------

        internal string GetText()
        {
            throw new NotImplementedException();
        }

        internal void FlushLine()
        {
            if( mText.Length > 0 )
            {
                var text = mText.ToString();
                mText.Clear();
                EditorGUILayout.SelectableLabel( text, GUI.skin.label );
            }
        }
    }
}
