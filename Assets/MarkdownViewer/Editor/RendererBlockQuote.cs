////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax;

namespace MG.MDV
{
    public class RendererBlockQuote : MarkdownObjectRenderer<RendererMarkdown, QuoteBlock>
    {
        protected override void Write( RendererMarkdown renderer, QuoteBlock obj )
        {
            throw new System.NotImplementedException();
        }
    }
}

