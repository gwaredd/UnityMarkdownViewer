////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax;

namespace MG.MDV
{
    // <ul><li>...</li></ul>

    /// <see cref="Markdig.Renderers.Html.ListRenderer"/>

    public class RendererBlockList : MarkdownObjectRenderer<RendererMarkdown, ListBlock>
    {
        protected override void Write( RendererMarkdown renderer, ListBlock block )
        {
            throw new System.NotImplementedException();
        }
    }
}
