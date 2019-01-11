////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax;

namespace MG.MDV
{
    ////////////////////////////////////////////////////////////////////////////////
    // <h1>...</h1>
    /// <see cref="Markdig.Renderers.Html.HeadingRenderer"/>

    public class RendererBlockHeading : MarkdownObjectRenderer<RendererMarkdown, HeadingBlock>
    {
        protected override void Write( RendererMarkdown renderer, HeadingBlock block )
        {
            var prevStyle = renderer.Context.Size;
            renderer.Context.Size = block.Level;
            renderer.WriteLeafBlockInline( block );
            renderer.Context.Size = prevStyle;

            renderer.FinishBlock();
        }
    }
}
