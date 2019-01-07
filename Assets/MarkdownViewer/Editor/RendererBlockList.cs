////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax;

namespace MG.MDV
{
    public class RendererBlockList : MarkdownObjectRenderer<RendererMarkdown, ListBlock>
    {
        protected override void Write( RendererMarkdown renderer, ListBlock obj )
        {
            throw new System.NotImplementedException();
        }
    }
}

