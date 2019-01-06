////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax;

namespace MG.MDV
{
    public class UnityMarkdownRenderer : RendererBase
    {
        public UnityMarkdownRenderer()
        {
            //// Default block renderers
            //ObjectRenderers.Add( new CodeBlockRenderer() );
            //ObjectRenderers.Add( new ListRenderer() );
            //ObjectRenderers.Add( new HeadingRenderer() );
            //ObjectRenderers.Add( new HtmlBlockRenderer() );
            ObjectRenderers.Add( new RendererParagraph() );
            //ObjectRenderers.Add( new QuoteBlockRenderer() );
            //ObjectRenderers.Add( new ThematicBreakRenderer() );

            // Default inline renderers
            //ObjectRenderers.Add( new AutolinkInlineRenderer() );
            //ObjectRenderers.Add( new CodeInlineRenderer() );
            //ObjectRenderers.Add( new DelimiterInlineRenderer() );
            ObjectRenderers.Add( new RendererInlineEmphasis() );
            //ObjectRenderers.Add( new LineBreakInlineRenderer() );
            //ObjectRenderers.Add( new HtmlInlineRenderer() );
            //ObjectRenderers.Add( new HtmlEntityInlineRenderer() );
            //ObjectRenderers.Add( new LinkInlineRenderer() );
            //ObjectRenderers.Add( new LiteralInlineRenderer() );
        }

        //------------------------------------------------------------------------------

        public override object Render( MarkdownObject obj )
        {
            Write( obj );
            return null; // writer
        }
    }
}

