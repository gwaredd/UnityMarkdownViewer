////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace MG.MDV
{
    public class RendererInlineAutoLink : MarkdownObjectRenderer<RendererMarkdown, AutolinkInline>
    {
        protected override void Write( RendererMarkdown renderer, AutolinkInline obj )
        {
            throw new System.NotImplementedException();
        }
    }
}

