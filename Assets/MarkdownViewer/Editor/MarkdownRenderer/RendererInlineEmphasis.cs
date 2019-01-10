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
            if( node.IsDouble )
            {
                renderer.Bold = true;
            }
            else
            {
                renderer.Italic = true;
            }

            renderer.WriteChildren( node );

            if( node.IsDouble )
            {
                renderer.Bold = false;
            }
            else
            {
                renderer.Italic = false;
            }
        }
    }
}

