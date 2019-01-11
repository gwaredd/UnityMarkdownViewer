////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MG.MDV
{
    // RenderContext (style)
    // Cursor
    // Indent
    //
    // Print Text, Output Image
    //

    ////////////////////////////////////////////////////////////////////////////////
    // RenderContext

    [Flags]
    public enum RenderStyle
    {
        Normal     = 0x00,
        Bold       = 0x01,
        Italic     = 0x02,
        FixedWidth = 0x04
    }

    public static class RenderStyleExt
    {
        public static bool IsSet( this RenderStyle style, RenderStyle flags )
        {
            return ( style & flags ) != RenderStyle.Normal;
        }

        public static RenderStyle Set( this RenderStyle style, RenderStyle flags )
        {
            return style | flags;
        }

        public static RenderStyle Clear( this RenderStyle style, RenderStyle flags )
        {
            return style & ~flags;
        }
    }

    public struct RenderContext
    {
        public RenderStyle  Style;
        public int          Size;
        public string       ToolTip;
        public string       Link;
    }


    ////////////////////////////////////////////////////////////////////////////////
    /// <see cref="Markdig.Renderers.HtmlRenderer"/>
    /// <see cref="Markdig.Renderers.Normalize.NormalizeRenderer"/>

    public class RendererMarkdown : RendererBase
    {
        public RenderStyle Style
        {
            get
            {
                return mContext.Style;
            }

            set
            {
                mContext.Style = value;
            }
        }

        public string ToolTip
        {
            set
            {
                mContext.ToolTip = value;
            }
        }

        public string Link
        {
            set
            {
                mContext.Link = value;
            }
        }

        public int Size
        {
            set { mContext.Size = Mathf.Clamp( value, 0, 6 ); }
        }

        public void Prefix( string prefix )
        {
            // TODO: better prefix!
            Print( ( prefix ?? "  " ) + "  " );
        }

        public void Indent()
        {
            mIndent.x += mIndentSize;
        }

        public void Outdent()
        {
            mIndent.x = Mathf.Max( mIndent.x - mIndentSize, mOrigin.x );
        }


        //------------------------------------------------------------------------------

        Dictionary<string,Texture> mTextureCache = new Dictionary<string, Texture>();

        float           mPadding    = 8.0f;
        float           mIndentSize = 20.0f;

        float           mMaxWidth   = 100.0f;
        Vector2         mOrigin;

        RenderContext   mContext;
        Vector2         mCursor;
        Vector2         mIndent;


        GUIStyle        mCurStyle   = null;
        float           mLineHeight = 12.0f;

        StringBuilder   mWord       = new StringBuilder( 1024 );
        float           mWordWidth  = 0.0f;


        internal void Setup( float headerHeight, Texture placeholder )
        {
            mOrigin.x = mPadding;
            mOrigin.y = headerHeight + mPadding;

            mCursor   = mOrigin;
            mIndent   = mCursor;
            mMaxWidth = Screen.width - mPadding * 2.0f;

            if( mTextureCache.Count == 0 )
            {
                mTextureCache.Add( string.Empty, placeholder );
            }
        }


        //------------------------------------------------------------------------------

        internal void Image( string url, string alt, string title )
        {
            Debug.Log( "TODO: <img src='" + url + "'/>" );
        }

        internal void Print( string text )
        {
            // TODO: cache working version?

            var fixedWidth = mContext.Style.IsSet( RenderStyle.FixedWidth );

            mCurStyle = fixedWidth ? GUI.skin.GetStyle( "code" ) : GUI.skin.label;

            var fontInfo = fixedWidth ? Fonts.Fixed : Fonts.Variable;
            var fontSize  = 11.0f + ( mContext.Size == 0 ? 0 : 7 - mContext.Size );
            var fontStyle = mContext.Style.IsSet( RenderStyle.Bold ) ? FontStyle.Bold : FontStyle.Normal;

            if( mContext.Style.IsSet( RenderStyle.Italic ) )
            {
                fontStyle = (FontStyle) ( (int) fontStyle + (int) FontStyle.Italic );
            }

            mCurStyle.fontSize = (int) fontSize;
            mCurStyle.fontStyle = fontStyle;

            mCurStyle.normal.textColor = mContext.Link != null ? Color.blue : Color.black;

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
                else if( char.IsWhiteSpace( ch ) )
                {
                    ch = ' ';
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

                if( ch == ' ' )
                {
                    PrintWord();
                }
            }

            PrintWord();
        }

        internal void HorizontalBreak()
        {
            NewLine();

            var rect = new Rect( mCursor, new Vector2( Screen.width - 50.0f, 1.0f ) );
            GUI.Label( rect, string.Empty, GUI.skin.GetStyle( "hr" ) );

            NewLine();
        }

        internal void Flush()
        {
            NewLine();
            NewLine();
        }


        //------------------------------------------------------------------------------


        private void NewLine()
        {
            mCursor.y += mLineHeight;
            mCursor.x = mOrigin.x;
        }

        private void ClearWord()
        {
            mWordWidth = 0.0f;
            mWord.Clear();
        }

        private void PrintWord()
        {
            // TODO: some safety for narrow windows!

            if( mWord.Length == 0 )
            {
                return;
            }

            if( mCursor.x + mWordWidth > mMaxWidth )
            {
                NewLine();
            }

            // TODO: ToolTip
            // TODO: split word?
            var rect = new Rect( mCursor, new Vector2( mWordWidth, mLineHeight ) );

            
            if( mContext.Link == null )
            {
                GUI.Label( rect, mWord.ToString(), mCurStyle );
            }
            else if( GUI.Button( rect, mWord.ToString(), mCurStyle ) )
            {
                Debug.Log( "GOTO: " + mContext.Link );
            }

            mCursor.x += mWordWidth;

            ClearWord();
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

        public override object Render( MarkdownObject document )
        {
            Write( document );
            Flush();

            return this;
        }
    }
}
