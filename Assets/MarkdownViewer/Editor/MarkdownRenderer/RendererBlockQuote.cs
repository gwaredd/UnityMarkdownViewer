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
            renderer.Layout.HighLightBlock();
            renderer.WriteChildren( block );
            renderer.FinishBlock();
        }
    }
}
