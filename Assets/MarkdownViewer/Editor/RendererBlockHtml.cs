////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax;

namespace MG.MDV
{
    /// <see cref="Markdig.Renderers.Html.HtmlBlockRenderer"/>

    public class RendererBlockHtml : MarkdownObjectRenderer<RendererMarkdown, HtmlBlock>
    {
        protected override void Write( RendererMarkdown renderer, HtmlBlock obj )
        {
            throw new System.NotImplementedException();
        }
    }
}

