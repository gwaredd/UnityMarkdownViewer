////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace MG.MDV
{
    public class RendererInlineLiteral : MarkdownObjectRenderer<RendererMarkdown, LiteralInline>
    {
        protected override void Write( RendererMarkdown renderer, LiteralInline obj )
        {
            renderer.Print( obj.Content.ToString() );
        }
    }
}

