////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace MG.MDV
{
    ////////////////////////////////////////////////////////////////////////////////
    // <b><i>...</i></b>
    /// <see cref="Markdig.Renderers.Html.Inlines.EmphasisInlineRenderer"/>

    public class RendererInlineEmphasis : MarkdownObjectRenderer<RendererMarkdown, EmphasisInline>
    {
        protected override void Write( RendererMarkdown renderer, EmphasisInline node )
        {
            var prevStyle = renderer.Style;
            renderer.Style = prevStyle.Set( node.IsDouble ? RenderStyle.Bold : RenderStyle.Italic );
            renderer.WriteChildren( node );
            renderer.Style = prevStyle;
        }
    }
}

