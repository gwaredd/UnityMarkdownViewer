////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace MG.MDV
{
    ////////////////////////////////////////////////////////////////////////////////
    /// <see cref="Markdig.Renderers.HtmlRenderer"/>
    /// <see cref="Markdig.Renderers.Normalize.NormalizeRenderer"/>

    public class RendererMarkdown : RendererBase
    {
        public RenderContext Context;

        Texture                    mPlaceholder  = null;
        Dictionary<string,Texture> mTextureCache = new Dictionary<string, Texture>();

        float           mPadding    = 8.0f;
        float           mIndentSize = 20.0f;

        float           mMaxWidth   = 100.0f;
        Vector2         mContentOrigin;

        //
        Vector2         mCursor;
        float           mMarginLeft;
        float           mMarginRight;
        float           mLineOrigin;
        float           mLineHeight;

        float           mWordWidth = 0.0f;
        StringBuilder   mWord      = new StringBuilder();
        StringBuilder   mLine      = new StringBuilder();

        GUIContent      mContent   = new GUIContent();


        public RendererMarkdown( Texture placeholder, GUISkin skin, Font fontVariable, Font fontFixed )
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

            mPlaceholder = placeholder;
            Context = new RenderContext( skin, fontVariable, fontFixed );
        }


        //------------------------------------------------------------------------------

        internal void Image( string url, string alt, string title )
        {
            Texture tex;

            if( mTextureCache.TryGetValue( url, out tex ) == false )
            {
                Debug.Log( $"Fetch {url}" );

                tex = mPlaceholder;
                mTextureCache[ url ] = mPlaceholder;
            }

            GUI.Label( new Rect( mCursor.x, mCursor.y, tex.width, tex.height ), tex );

            mLineHeight = Mathf.Max( mLineHeight, tex.height );
            mCursor.x += tex.width;

            // TODO: wrap image?
        }


        //------------------------------------------------------------------------------

        internal void Print( string text )
        {
            mLineOrigin = mCursor.x;
            mLineHeight = Context.Style.lineHeight;

            for( var i = 0; i < text.Length; i++ )
            {
                if( text[ i ] == '\n' )
                {
                    NewLine();
                }
                else
                {
                    AddCharacter( text[ i ] );
                }
            }

            AddWord();
            Flush(); // TODO: cause an issue with images?
        }

        private void AddCharacter( char ch )
        {
            if( char.IsWhiteSpace( ch ) )
            {
                ch = ' '; // ensure any WS is treated as a space
            }

            // TODO: chains of ws chars??

            float advance;

            if( Context.CharacterWidth( ch, out advance ) )
            {
                mWord.Append( ch );
                mWordWidth += advance;
            }
            else
            {
                // bad character
                Context.CharacterWidth( '?', out advance );
                mWord.Append( '?' );
                mWordWidth += advance;
            }

            if( ch == ' ' )
            {
                AddWord();
            }
        }

        private void AddWord()
        {
            if( mWord.Length == 0 )
            {
                return;
            }

            // TODO: split long words?
            // TODO: some safety for narrow windows!

            if( mCursor.x + mWordWidth > mMarginRight )
            {
                NewLine();
            }

            mLine.Append( mWord.ToString() );
            mCursor.x += mWordWidth;

            mWord.Clear();
            mWordWidth = 0.0f;
        }

        private void Flush()
        {
            if( mLine.Length == 0 )
            {
                return;
            }

            mContent.text    = mLine.ToString();
            mContent.tooltip = Context.ToolTip;

            var rect = new Rect( mLineOrigin, mCursor.y, mCursor.x - mLineOrigin, Context.Style.lineHeight );

            if( string.IsNullOrWhiteSpace( Context.Link ) )
            {
                GUI.Label( rect, mContent, Context.Style );
            }
            else if( GUI.Button( rect, mContent, Context.Style ) )
            {
                Debug.Log( "TODO: open " + Context.Link );
            }

            mLineOrigin = mCursor.x;
            mLine.Clear();
        }


        //------------------------------------------------------------------------------

        internal void HorizontalBreak()
        {
            NewLine();

            var rect = new Rect( mCursor, new Vector2( Screen.width - 50.0f, 1.0f ) );
            GUI.Label( rect, string.Empty, GUI.skin.GetStyle( "hr" ) );

            NewLine();
        }

        public void Prefix( string prefix )
        {
            // TODO: better prefix!
            Print( ( prefix ?? "  " ) + "  " );
        }

        public void Indent()
        {
            // TODO: safety for narrow windows?
            mMarginLeft += mIndentSize;
        }

        public void Outdent()
        {
            mMarginLeft = Mathf.Max( mMarginLeft - mIndentSize, mContentOrigin.x );
        }

        private void NewLine()
        {
            Flush();

            mCursor.y += mLineHeight;
            mCursor.x = mMarginLeft;

            mLineOrigin = mCursor.x;
            mLineHeight = Context.Style.lineHeight;
        }

        internal void FinishBlock( bool emptyLine = false )
        {
            NewLine();

            if( emptyLine )
            {
                NewLine();
            }
        }





        ////////////////////////////////////////////////////////////////////////////////
        // utils

        /// <summary>
        /// Output child nodes inline
        /// </summary>
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

        /// <summary>
        /// Output child nodes as raw text
        /// </summary>
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
                Print( slices[ i ].ToString() + "\n" );
            }
        }


        ////////////////////////////////////////////////////////////////////////////////
        // setup

        public void Render( MarkdownObject document, float headerHeight )
        {
            mContentOrigin.x = mPadding;
            mContentOrigin.y = headerHeight + mPadding;

            mCursor   = mContentOrigin;
            mMaxWidth = Screen.width - mPadding * 2.0f;

            mLineOrigin  = mContentOrigin.x;
            mMarginLeft  = mContentOrigin.x;
            mMarginRight = mContentOrigin.x + mMaxWidth;

            Context.Reset();

            Write( document );
            FinishBlock();
        }

        public override object Render( MarkdownObject document )
        {
            throw new NotImplementedException();
        }
    }
}
