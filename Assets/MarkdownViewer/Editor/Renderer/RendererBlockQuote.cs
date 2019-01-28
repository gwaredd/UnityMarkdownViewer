////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax;

namespace MG.MDV
{
    ////////////////////////////////////////////////////////////////////////////////
    // <blockquote>...</blockquote>
    /// <see cref="Markdig.Renderers.Html.QuoteBlockRenderer"/>

    public class RendererBlockQuote : MarkdownObjectRenderer<RendererMarkdown, QuoteBlock>
    {
        protected override void Write( RendererMarkdown renderer, QuoteBlock block )
        {
            var prevImplicit   = renderer.ImplicitParagraph;
            var prevBlockQuote = renderer.Layout.BlockQuote;

            renderer.ImplicitParagraph = false;
            renderer.Layout.BlockQuote = true;

            renderer.WriteChildren( block );

            renderer.Layout.BlockQuote = prevBlockQuote;
            renderer.ImplicitParagraph = prevImplicit;

            renderer.FinishBlock( true );
        }
    }
}
