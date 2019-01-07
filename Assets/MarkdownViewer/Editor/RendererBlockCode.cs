////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax;

namespace MG.MDV
{
    public class RendererBlockCode : MarkdownObjectRenderer<RendererMarkdown, CodeBlock>
    {
        protected override void Write( RendererMarkdown renderer, CodeBlock obj )
        {
            throw new System.NotImplementedException();
        }
    }
}

