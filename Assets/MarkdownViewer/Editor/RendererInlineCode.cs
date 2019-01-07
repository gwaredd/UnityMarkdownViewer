////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace MG.MDV
{
    public class RendererInlineCode : MarkdownObjectRenderer<RendererMarkdown, CodeInline>
    {
        protected override void Write( RendererMarkdown renderer, CodeInline obj )
        {
            throw new System.NotImplementedException();
        }
    }
}
