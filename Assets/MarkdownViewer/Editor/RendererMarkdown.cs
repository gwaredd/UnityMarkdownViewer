////////////////////////////////////////////////////////////////////////////////

using System;
using Markdig.Renderers;
using Markdig.Syntax;
using UnityEngine;

namespace MG.MDV
{
    public class RendererMarkdown : RendererBase
    {
        GUISkin mSkin;

        public RendererMarkdown( GUISkin skin )
        {
            mSkin = skin;

            //ObjectRenderers.Add( new CodeBlockRenderer() );
            //ObjectRenderers.Add( new ListRenderer() );
            //ObjectRenderers.Add( new HeadingRenderer() );
            //ObjectRenderers.Add( new HtmlBlockRenderer() );
            ObjectRenderers.Add( new RendererBlockParagraph() );
            //ObjectRenderers.Add( new QuoteBlockRenderer() );
            //ObjectRenderers.Add( new ThematicBreakRenderer() );

            //ObjectRenderers.Add( new AutolinkInlineRenderer() );
            //ObjectRenderers.Add( new CodeInlineRenderer() );
            ObjectRenderers.Add( new RendererInlineDelimiter() );
            ObjectRenderers.Add( new RendererInlineEmphasis() );
            //ObjectRenderers.Add( new LineBreakInlineRenderer() );
            //ObjectRenderers.Add( new HtmlInlineRenderer() );
            //ObjectRenderers.Add( new HtmlEntityInlineRenderer() );
            //ObjectRenderers.Add( new LinkInlineRenderer() );
            ObjectRenderers.Add( new RendererInlineLiteral() );
        }

        public override object Render( MarkdownObject document )
        {
            Write( document );
            return null;
        }

        //------------------------------------------------------------------------------

        string mLine = string.Empty;

        internal void Print( string text )
        {
            mLine += text;
        }

        internal void EnsureLine()
        {
            if( !string.IsNullOrEmpty( mLine ) )
            {
                GUILayout.Label( mLine, mSkin.label );
                mLine = string.Empty;
            }
        }
    }
}

