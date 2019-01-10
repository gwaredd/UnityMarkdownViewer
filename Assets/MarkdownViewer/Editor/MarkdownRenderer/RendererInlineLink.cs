////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax.Inlines;
using UnityEngine;

namespace MG.MDV
{
    ////////////////////////////////////////////////////////////////////////////////
    // <img src="..." /> || <a href="...">
    /// <see cref="Markdig.Renderers.Html.Inlines.LinkInlineRenderer"/>

    public class RendererInlineLink : MarkdownObjectRenderer<RendererMarkdown, LinkInline>
    {
        protected override void Write( RendererMarkdown renderer, LinkInline node )
        {
            if( node.IsImage )
            {
                // alt = renderer.WriteChildren(link);
                // TODO: node.IsImage
                throw new System.NotImplementedException();
            }

            var url = node.GetDynamicUrl?.Invoke() ?? node.Url;

            renderer.Link = url;

            if( string.IsNullOrEmpty( node.Title ) == false )
            {
                renderer.ToolTip = node.Title;
            }

            renderer.WriteChildren( node );

            // TODO: push and pop context?

            renderer.ToolTip = null;
            renderer.Link    = null;
        }
    }
}
