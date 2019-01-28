////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax;

namespace MG.MDV
{
    ////////////////////////////////////////////////////////////////////////////////
    /// <see cref="Markdig.Renderers.Html.HtmlBlockRenderer"/>
    /// <see cref="Markdig.Renderers.Normalize.HtmlBlockRenderer"/>

    public class RendererBlockHtml : MarkdownObjectRenderer<RendererMarkdown, HtmlBlock>
    {
        protected override void Write( RendererMarkdown renderer, HtmlBlock block )
        {
            renderer.WriteLeafRawLines( block );
            renderer.FinishBlock();
        }
    }
}
