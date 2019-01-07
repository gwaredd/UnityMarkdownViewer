////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace MG.MDV
{
    public class RendererInlineLink : MarkdownObjectRenderer<RendererMarkdown, LinkInline>
    {
        protected override void Write( RendererMarkdown renderer, LinkInline obj )
        {
            throw new System.NotImplementedException();
        }
    }
}

