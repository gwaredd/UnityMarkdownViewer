////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax;

namespace MG.MDV
{
    // <pre><code> ... </code></pre>

    /// <see cref="Markdig.Renderers.Html.CodeBlockRenderer"/>

    public class RendererBlockCode : MarkdownObjectRenderer<RendererMarkdown, CodeBlock>
    {
        protected override void Write( RendererMarkdown renderer, CodeBlock block )
        {
            renderer.EnsureLine();

            renderer.WriteLeafRawLines( block );

            throw new System.NotImplementedException();
        }
    }
}
