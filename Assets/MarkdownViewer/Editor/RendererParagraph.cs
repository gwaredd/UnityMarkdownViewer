////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace MG.MDV
{
    public class RendererParagraph : MarkdownObjectRenderer<UnityMarkdownRenderer, ParagraphBlock>
    {
        protected override void Write( UnityMarkdownRenderer renderer, ParagraphBlock obj )
        {
            //if( !renderer.ImplicitParagraph && renderer.EnableHtmlForBlock )
            //{
            //    if( !renderer.IsFirstInContainer )
            //    {
            //        renderer.EnsureLine();
            //    }

            //    renderer.Write( "<p" ).WriteAttributes( obj ).Write( ">" );
            //}

            // write children ..

            var child = (Inline) obj.Inline;

            while( child != null )
            {
                Write( renderer, child );
                child = child.NextSibling;
            }

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
