////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace MG.MDV
{
    /// <see cref="Markdig.Renderers.Html.Inlines.EmphasisInlineRenderer"/>

    public class RendererInlineEmphasis : MarkdownObjectRenderer<RendererMarkdown, EmphasisInline>
    {
        protected override void Write( RendererMarkdown renderer, EmphasisInline obj )
        {
            var tag = null as string;

            if( obj.DelimiterChar == '*' || obj.DelimiterChar == '_' )
            {
                tag = obj.IsDouble ? "b" : "i";
            }

            renderer.Print( "<" + tag + ">" );

            //string tag = null;
            //if( renderer.EnableHtmlForInline )
            //{
            //    tag = GetTag( obj );
            //    renderer.Write( "<" ).Write( tag ).WriteAttributes( obj ).Write( ">" );
            //}

            //return obj.IsDouble ? "strong" : "em";

            renderer.WriteChildren( obj );
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
