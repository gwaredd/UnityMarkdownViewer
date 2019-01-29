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
            var prevImplicit = renderer.ImplicitParagraph;
            renderer.ImplicitParagraph = false;

            renderer.Layout.QuoteBegin();
            renderer.WriteChildren( block );
            renderer.Layout.QuoteEnd();

            renderer.ImplicitParagraph = prevImplicit;

            renderer.FinishBlock( true );
        }
    }
}
