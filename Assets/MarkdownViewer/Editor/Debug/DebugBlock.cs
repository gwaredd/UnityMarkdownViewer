////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax;

namespace MG.MDV
{
    /// <see cref="RendererBlockCode"/>

    public class DebugBlockCode : MarkdownObjectRenderer<DebugRenderer, CodeBlock>
    {
        protected override void Write( DebugRenderer renderer, CodeBlock block )
        {
            renderer.Begin( "code" );
            renderer.WriteLeafRawLines( block );
            renderer.End( "code" );
        }
    }


    /// <see cref="RendererBlockList"/>
    /// <see cref="Markdig.Renderers.Normalize.ListRenderer"/>

    public class DebugBlockList : MarkdownObjectRenderer<DebugRenderer, ListBlock>
    {
        protected override void Write( DebugRenderer renderer, ListBlock block )
        {
            renderer.Begin( "ul" );

            for( var i = 0; i < block.Count; i++ )
            {
                renderer.Begin( "li" );
                renderer.WriteChildren( block[ i ] as ListItemBlock );
                renderer.End( "li" );
            }

            renderer.End( "ul" );
        }
    }


    /// <see cref="RendererBlockHeading"/>

    public class DebugBlockHeading : MarkdownObjectRenderer<DebugRenderer, HeadingBlock>
    {
        protected override void Write( DebugRenderer renderer, HeadingBlock block )
        {
            renderer.Begin( "h" + block.Level );
            renderer.WriteLeafBlockInline( block );
            renderer.End( "h" + block.Level );
        }
    }


    /// <see cref="RendererBlockHtml"/>

    public class DebugBlockHtml : MarkdownObjectRenderer<DebugRenderer, HtmlBlock>
    {
        protected override void Write( DebugRenderer renderer, HtmlBlock block )
        {
            renderer.Begin( "html" );
            renderer.WriteLeafRawLines( block );
            renderer.End( "html" );
        }
    }


    /// <see cref="RendererBlockParagraph"/>

    public class DebugBlockParagraph : MarkdownObjectRenderer<DebugRenderer, ParagraphBlock>
    {
        protected override void Write( DebugRenderer renderer, ParagraphBlock block )
        {
            renderer.Begin( "p" );
            renderer.WriteLeafBlockInline( block );
            renderer.End( "p" );
        }
    }


    /// <see cref="RendererBlockQuote"/>

    public class DebugBlockQuote : MarkdownObjectRenderer<DebugRenderer, QuoteBlock>
    {
        protected override void Write( DebugRenderer renderer, QuoteBlock block )
        {
            renderer.Begin( "quote" );
            renderer.WriteChildren( block );
            renderer.End( "quote" );
        }
    }


    /// <see cref="RendererBlockThematicBreak"/>

    public class DebugBlockThematicBreak : MarkdownObjectRenderer<DebugRenderer, ThematicBreakBlock>
    {
        protected override void Write( DebugRenderer renderer, ThematicBreakBlock block )
        {
            renderer.Emit( "hr" );
        }
    }
}

