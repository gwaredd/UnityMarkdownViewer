////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax;

namespace MG.MDV
{
    ////////////////////////////////////////////////////////////////////////////////
    // <ul><li>...</li></ul>
    /// <see cref="Markdig.Renderers.Html.ListRenderer"/>

    public class RendererBlockList : MarkdownObjectRenderer<RendererMarkdown, ListBlock>
    {
        protected override void Write( RendererMarkdown renderer, ListBlock block )
        {
            var layout = renderer.Layout;

            layout.Indent();

            var prevImplicit = renderer.ImplicitParagraph;
            renderer.ImplicitParagraph = true;

            for( var i = 0; i < block.Count; i++ )
            {
                layout.Prefix( block.IsOrdered ? i.ToString() + "." : "\u2022", renderer.Style );
                renderer.WriteChildren( block[ i ] as ListItemBlock );
            }

            renderer.ImplicitParagraph = prevImplicit;
            layout.Outdent();

            renderer.FinishBlock( true );
        }
    }
}
