////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace MG.MDV
{
    public class RendererInlineEmphasis : MarkdownObjectRenderer<UnityMarkdownRenderer, EmphasisInline>
    {
        protected override void Write( UnityMarkdownRenderer renderer, EmphasisInline obj )
        {
            //string tag = null;
            //if( renderer.EnableHtmlForInline )
            //{
            //    tag = GetTag( obj );
            //    renderer.Write( "<" ).Write( tag ).WriteAttributes( obj ).Write( ">" );
            //}

            renderer.WriteChildren( obj );

            //if( renderer.EnableHtmlForInline )
            //{
            //    renderer.Write( "</" ).Write( tag ).Write( ">" );
            //}
        }
    }
}
