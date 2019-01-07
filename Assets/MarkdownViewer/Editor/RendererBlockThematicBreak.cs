////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax;

namespace MG.MDV
{
    public class RendererBlockThematicBreak : MarkdownObjectRenderer<RendererMarkdown, ThematicBreakBlock>
    {
        protected override void Write( RendererMarkdown renderer, ThematicBreakBlock obj )
        {
            throw new System.NotImplementedException();
        }
    }
}

