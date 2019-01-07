////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace MG.MDV
{
    /// <see cref="Markdig.Renderers.Html.Inlines.HtmlEntityInlineRenderer"/>

    public class RendererInlineHtmlEntity : MarkdownObjectRenderer<RendererMarkdown, HtmlEntityInline>
    {
        protected override void Write( RendererMarkdown renderer, HtmlEntityInline node )
        {
            //renderer.WriteEscape( obj.Transcoded );
            throw new System.NotImplementedException();
        }
    }
}
