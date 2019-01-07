////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax.Inlines;
using System;

namespace MG.MDV
{
    // <br/>

    /// <see cref="Markdig.Renderers.Html.Inlines.LineBreakInlineRenderer"/>

    public class RendererInlineLineBreak : MarkdownObjectRenderer<RendererMarkdown, LineBreakInline>
    {
        protected override void Write( RendererMarkdown renderer, LineBreakInline node )
        {
            renderer.Print( Environment.NewLine );
        }
    }
}

