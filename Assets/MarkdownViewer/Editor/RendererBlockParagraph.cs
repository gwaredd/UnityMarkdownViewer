////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;

namespace MG.MDV
{
    public class RendererBlockParagraph : MarkdownObjectRenderer<RendererMarkdown, ParagraphBlock>
    {
        protected override void Write( RendererMarkdown renderer, ParagraphBlock obj )
        {
            //if( !renderer.ImplicitParagraph && renderer.EnableHtmlForBlock )
            //{
            //    if( !renderer.IsFirstInContainer )
            //    {
            //        renderer.EnsureLine();
            //    }
            //    renderer.Write( "<p" ).WriteAttributes( obj ).Write( ">" );
            //}

            var inline = obj.Inline as Inline;

            while( inline != null )
            {
                renderer.Write( inline );
                inline = inline.NextSibling;
            }

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
