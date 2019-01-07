////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace MG.MDV
{
    // <img src="..." /> || <a href="...">

    /// <see cref="Markdig.Renderers.Html.Inlines.LinkInlineRenderer"/>

    public class RendererInlineLink : MarkdownObjectRenderer<RendererMarkdown, LinkInline>
    {
        protected override void Write( RendererMarkdown renderer, LinkInline node )
        {
            throw new System.NotImplementedException();
        }
    }
}
