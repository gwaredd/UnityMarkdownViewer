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
        Vector2 mPos;

        StringBuilder mText = new StringBuilder( 2048 );

        public void Init( float headerHeight )
        {
            mContentOffset.x = mPadding;
            mContentOffset.y = headerHeight + mPadding;

            mPos      = mContentOffset;
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

        GUIStyle mCurrentStyle = null;

        internal void Print( string text )
        {
            // print words (in current style) with wrapping

            //GUIStyle.GetCursorStringIndex

            // ObjectRenderers.Add( new RendererInlineLiteral() );

            // ObjectRenderers.Add( new RendererInlineEmphasis() );
            // ObjectRenderers.Add( new RendererInlineLineBreak() );

            // ObjectRenderers.Add( new RendererInlineLink() );
            // ObjectRenderers.Add( new RendererInlineAutoLink() );

            // ObjectRenderers.Add( new RendererInlineCode() );


            mText.Append( text );
        }

        /*

                var lineHeight = Skin.label.lineHeight;
                var fontSize   = Skin.label.fontSize;
                var font       = Skin.font;
                var dim        = new Vector2( mMaxWidth, lineHeight * 2.0f );

                var pos = 0;
                var totalWidth = 0.0f;

                for( var i = 0; i < text.Length; i++ )
                {
                    // TODO: actual word wrapping!

                    if( font.GetCharacterInfo( text[ i ], out mCharacterInfo, fontSize, FontStyle.Normal ) )
                    {
                        var newWidth = totalWidth + mCharacterInfo.advance;

                        if( newWidth > mMaxWidth )
                        {
                            GUI.Label( new Rect( mPos, dim ), text.Substring( pos, i - pos ) );
                            mPos.y += lineHeight;

                            pos = i;
                            totalWidth = mCharacterInfo.advance;
                        }
                        else
                        {
                            totalWidth = newWidth;
                        }
                    }
                    else
                    {
                        // character not in font!
                    }
                }

                GUI.Label( new Rect( mPos, dim ), text.Substring( pos ) );
                mPos.y += lineHeight;

        /**/


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
