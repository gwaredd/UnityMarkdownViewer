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

            renderer.ImplicitParagraph = true;
            layout.Indent();

            for( var i = 0; i < block.Count; i++ )
            {
                layout.Prefix( block.IsOrdered ? i.ToString() + "." : "\u2022", renderer.Style );
                renderer.WriteChildren( block[ i ] as ListItemBlock );
            }

            layout.Outdent();
            renderer.ImplicitParagraph = false;
            renderer.FinishBlock();
        }
    }
}
