////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace MG.MDV
{
    ////////////////////////////////////////////////////////////////////////////////
    // <img src="..." /> || <a href="...">
    /// <see cref="Markdig.Renderers.Html.Inlines.LinkInlineRenderer"/>

    public class RendererInlineLink : MarkdownObjectRenderer<RendererMarkdown, LinkInline>
    {
        protected override void Write( RendererMarkdown renderer, LinkInline node )
        {
            var url = node.GetDynamicUrl?.Invoke() ?? node.Url;

            if( node.IsImage )
            {
                renderer.Image( url, renderer.GetContents( node ), node.Title );
                return;
            }


            renderer.Context.Link = url;

            if( string.IsNullOrEmpty( node.Title ) == false )
            {
                renderer.Context.ToolTip = node.Title;
            }

            renderer.WriteChildren( node );

            // TODO: push and pop context?

            renderer.Context.ToolTip = null;
            renderer.Context.Link    = null;
        }
    }
}
