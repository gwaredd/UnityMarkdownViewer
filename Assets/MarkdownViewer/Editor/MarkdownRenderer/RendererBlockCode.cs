////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax;

namespace MG.MDV
{
    ////////////////////////////////////////////////////////////////////////////////
    // <pre><code>...</code></pre>
    /// <see cref="Markdig.Renderers.Html.CodeBlockRenderer"/>

    public class RendererBlockCode : MarkdownObjectRenderer<RendererMarkdown, CodeBlock>
    {
        protected override void Write( RendererMarkdown renderer, CodeBlock block )
        {
            var prevStyle = renderer.Style;
            renderer.Style.Fixed = true;
            renderer.WriteLeafRawLines( block );
            renderer.Style = prevStyle;

            renderer.FinishBlock();
        }
    }
}
