////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace MG.MDV
{
    // <code>...</code>

    /// <see cref="Markdig.Renderers.Html.Inlines.CodeInlineRenderer"/>

    public class RendererInlineCode : MarkdownObjectRenderer<RendererMarkdown, CodeInline>
    {
        protected override void Write( RendererMarkdown renderer, CodeInline node )
        {
            throw new System.NotImplementedException();
        }
    }
}
