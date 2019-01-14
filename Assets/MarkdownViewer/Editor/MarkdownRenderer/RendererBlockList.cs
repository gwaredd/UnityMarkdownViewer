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
            renderer.Layout.Indent();

            for( var i = 0; i < block.Count; i++ )
            {
                renderer.Layout.Prefix( block.IsOrdered ? i.ToString() : "\u2022" );
                renderer.WriteChildren( block[ i ] as ListItemBlock );
            }

            renderer.Layout.Outdent();
            renderer.FinishBlock();
        }
    }
}
