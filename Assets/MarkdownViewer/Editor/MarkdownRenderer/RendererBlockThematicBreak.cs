////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax;
using UnityEngine;

namespace MG.MDV
{
    ////////////////////////////////////////////////////////////////////////////////
    // <hr/>
    /// <see cref="Markdig.Renderers.Html.ThematicBreakRenderer"/>

    public class RendererBlockThematicBreak : MarkdownObjectRenderer<RendererMarkdown, ThematicBreakBlock>
    {
        protected override void Write( RendererMarkdown renderer, ThematicBreakBlock block )
        {
            renderer.Break();
        }
    }
}
