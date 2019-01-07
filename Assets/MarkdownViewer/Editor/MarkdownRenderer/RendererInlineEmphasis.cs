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
            var tag = null as string;

            if( node.DelimiterChar == '*' || node.DelimiterChar == '_' )
            {
                tag = node.IsDouble ? "b" : "i";
            }

            renderer.Print( "<" );
            renderer.Print( tag );
            renderer.Print( ">" );

            renderer.WriteChildren( node );

            renderer.Print( "</" );
            renderer.Print( tag );
            renderer.Print( ">" );

        }
    }
}

