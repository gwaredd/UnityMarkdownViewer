////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace MG.MDV
{
    // text
    /// <see cref="Markdig.Renderers.Html.Inlines.LiteralInlineRenderer"/>

    public class RendererInlineLiteral : MarkdownObjectRenderer<RendererMarkdown, LiteralInline>
    {
        protected override void Write( RendererMarkdown renderer, LiteralInline node )
        {
            renderer.Text( node.Content.ToString() );
        }
    }
}
