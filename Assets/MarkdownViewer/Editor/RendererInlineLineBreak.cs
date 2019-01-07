////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax.Inlines;
using System;

namespace MG.MDV
{
    /// <see cref="Markdig.Renderers.Html.Inlines.LineBreakInlineRenderer"/>

    public class RendererInlineLineBreak : MarkdownObjectRenderer<RendererMarkdown, LineBreakInline>
    {
        protected override void Write( RendererMarkdown renderer, LineBreakInline obj )
        {
            renderer.Print( Environment.NewLine );
        }
    }
}

