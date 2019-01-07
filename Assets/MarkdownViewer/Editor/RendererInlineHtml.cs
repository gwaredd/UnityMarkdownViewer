////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace MG.MDV
{
    public class RendererInlineHtml : MarkdownObjectRenderer<RendererMarkdown, HtmlInline>
    {
        protected override void Write( RendererMarkdown renderer, HtmlInline obj )
        {
            throw new System.NotImplementedException();
        }
    }
}
