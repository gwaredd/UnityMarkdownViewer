////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace MG.MDV
{
    /// <see cref="Markdig.Renderers.Html.Inlines.DelimiterInlineRenderer"/>

    public class RendererInlineDelimiter : MarkdownObjectRenderer<RendererMarkdown, DelimiterInline>
    {
        protected override void Write( RendererMarkdown renderer, DelimiterInline node )
        {
            //renderer.WriteEscape( obj.ToLiteral() );
            UnityEngine.Debug.Log( "TODO: RendererInlineDelimiter" );
            renderer.WriteChildren( node );
        }
    }
}
