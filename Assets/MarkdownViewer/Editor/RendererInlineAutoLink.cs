////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace MG.MDV
{
    /// <see cref="Markdig.Renderers.Html.Inlines.AutolinkInlineRenderer"/>

    public class RendererInlineAutoLink : MarkdownObjectRenderer<RendererMarkdown, AutolinkInline>
    {
        protected override void Write( RendererMarkdown renderer, AutolinkInline obj )
        {
            throw new System.NotImplementedException();
        }
    }
}

