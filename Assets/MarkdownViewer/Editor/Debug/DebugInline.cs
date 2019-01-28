////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace MG.MDV
{
    /// <see cref="RendererInlineLink"/>

    public class DebugInlineLink : MarkdownObjectRenderer<DebugRenderer, LinkInline>
    {
        protected override void Write( DebugRenderer renderer, LinkInline node )
        {
            var url = node.GetDynamicUrl?.Invoke() ?? node.Url;
            renderer.Emit( url );
        }
    }


    /// <see cref="RendererInlineAutoLink"/>

    public class DebugInlineAutoLink : MarkdownObjectRenderer<DebugRenderer, AutolinkInline>
    {
        protected override void Write( DebugRenderer renderer, AutolinkInline node )
        {
            renderer.Emit( node.Url );
        }
    }


    /// <see cref="RendererInlineCode"/>

    public class DebugInlineCode : MarkdownObjectRenderer<DebugRenderer, CodeInline>
    {
        protected override void Write( DebugRenderer renderer, CodeInline node )
        {
            renderer.Emit( node.Content );
        }
    }


    /// <see cref="RendererInlineDelimiter"/>

    public class DebugInlineDelimiter : MarkdownObjectRenderer<DebugRenderer, DelimiterInline>
    {
        protected override void Write( DebugRenderer renderer, DelimiterInline node )
        {
            renderer.Emit( node.ToLiteral() );
            renderer.WriteChildren( node );
        }
    }


    /// <see cref="RendererInlineEmphasis"/>

    public class DebugInlineEmphasis : MarkdownObjectRenderer<DebugRenderer, EmphasisInline>
    {
        protected override void Write( DebugRenderer renderer, EmphasisInline node )
        {
            renderer.WriteChildren( node );
        }
    }


    /// <see cref="RendererInlineLineBreak"/>
    /// 
    public class DebugInlineLineBreak : MarkdownObjectRenderer<DebugRenderer, LineBreakInline>
    {
        protected override void Write( DebugRenderer renderer, LineBreakInline node )
        {
            renderer.Emit( node.IsHard ? "<BR/>" : "<br/>" );
        }
    }


    /// <see cref="RendererInlineHtml"/>
    /// 
    public class DebugInlineHtml : MarkdownObjectRenderer<DebugRenderer, HtmlInline>
    {
        protected override void Write( DebugRenderer renderer, HtmlInline node )
        {
            renderer.Emit( node.Tag );
        }
    }


    /// <see cref="RendererInlineHtmlEntity"/>

    public class DebugInlineHtmlEntity : MarkdownObjectRenderer<DebugRenderer, HtmlEntityInline>
    {
        protected override void Write( DebugRenderer renderer, HtmlEntityInline node )
        {
        }
    }


    /// <see cref="RendererInlineLiteral"/>
    /// 
    public class DebugInlineLiteral : MarkdownObjectRenderer<DebugRenderer, LiteralInline>
    {
        protected override void Write( DebugRenderer renderer, LiteralInline node )
        {
            renderer.Emit( node.Content.ToString() );
        }
    }
}

