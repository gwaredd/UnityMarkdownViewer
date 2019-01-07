﻿////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace MG.MDV
{
    ////////////////////////////////////////////////////////////////////////////////
    // <html>...</html>
    /// <see cref="Markdig.Renderers.Html.Inlines.HtmlInlineRenderer"/>

    public class RendererInlineHtml : MarkdownObjectRenderer<RendererMarkdown, HtmlInline>
    {
        protected override void Write( RendererMarkdown renderer, HtmlInline node )
        {
            throw new System.NotImplementedException();
        }
    }
}