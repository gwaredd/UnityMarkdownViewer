////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace MG.MDV
{
    public class RendererInlineHtmlEntity : MarkdownObjectRenderer<RendererMarkdown, HtmlEntityInline>
    {
        protected override void Write( RendererMarkdown renderer, HtmlEntityInline obj )
        {
            throw new System.NotImplementedException();
        }
    }
}

