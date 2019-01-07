////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace MG.MDV
{
    public class RendererInlineDelimiter : MarkdownObjectRenderer<RendererMarkdown, DelimiterInline>
    {
        protected override void Write( RendererMarkdown renderer, DelimiterInline obj )
        {
            //renderer.WriteEscape( obj.ToLiteral() );
            UnityEngine.Debug.Log( "RendererInlineDelimiter" );
            renderer.WriteChildren( obj );
        }
    }
}

