////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace MG.MDV
{
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

        public override object Render( MarkdownObject document )
        {
            Write( document );
            return null;
        }


        //------------------------------------------------------------------------------

        internal GUIStyle GetStyle( string style )
        {
            return GUI.skin != null ? GUI.skin.GetStyle( style ) : null;
        }


        //------------------------------------------------------------------------------
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


        //------------------------------------------------------------------------------
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


        //------------------------------------------------------------------------------

        StringBuilder mText = new StringBuilder( 2048 );

        internal void Print( string text )
        {
            mText.Append( text );
        }

        internal string GetText()
        {
            var text = mText.ToString();
            mText.Clear();
            return text;
        }

        internal void EnsureLine()
        {
            if( mText.Length > 0 )
            {
                //GUILayout.TextField( GetText() );

                GUILayout.Label( GetText(), GUI.skin.label );
                //EditorGUILayout.SelectableLabel( GetText(), GUI.skin.label );
            }
        }
    }
}
