////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace MG.MDV
{
    // <p>...</p>

    /// <see cref="Markdig.Renderers.Html.ParagraphRenderer"/>

    public class RendererBlockParagraph : MarkdownObjectRenderer<RendererMarkdown, ParagraphBlock>
    {
        protected override void Write( RendererMarkdown renderer, ParagraphBlock block )
        {
            //if( !renderer.ImplicitParagraph && renderer.EnableHtmlForBlock )
            //{
            //    if( !renderer.IsFirstInContainer )
            //    {
            //        renderer.EnsureLine();
            //    }
            //    renderer.Write( "<p" ).WriteAttributes( obj ).Write( ">" );
            //}

            renderer.WriteLeafBlockInline( block );
            renderer.EnsureLine();

            //if( !renderer.ImplicitParagraph )
            //{
            //    if( renderer.EnableHtmlForBlock )
            //    {
            //        renderer.WriteLine( "</p>" );
            //    }
            //    renderer.EnsureLine();
            //}
        }
    }
}
