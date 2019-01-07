////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace MG.MDV
{
    // <b><i>...</i></b>

    /// <see cref="Markdig.Renderers.Html.Inlines.EmphasisInlineRenderer"/>

    public class RendererInlineEmphasis : MarkdownObjectRenderer<RendererMarkdown, EmphasisInline>
    {
        protected override void Write( RendererMarkdown renderer, EmphasisInline node )
        {
            var tag = null as string;

            if( node.DelimiterChar == '*' || node.DelimiterChar == '_' )
            {
                tag = node.IsDouble ? "b" : "i";
            }

            renderer.Print( "<" + tag + ">" );

            //string tag = null;
            //if( renderer.EnableHtmlForInline )
            //{
            //    tag = GetTag( obj );
            //    renderer.Write( "<" ).Write( tag ).WriteAttributes( obj ).Write( ">" );
            //}

            //return obj.IsDouble ? "strong" : "em";

            renderer.WriteChildren( node );
            renderer.Print( "</" + tag + ">" );

            //if( renderer.EnableHtmlForInline )
            //{
            //    renderer.Write( "</" ).Write( tag ).Write( ">" );
            //}
        }

        //        if (obj.DelimiterChar == '*' || obj.DelimiterChar == '_')
        //{
        //    return obj.IsDouble? "strong" : "em";
        //}

    }
}
