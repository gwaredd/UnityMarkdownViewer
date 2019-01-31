////////////////////////////////////////////////////////////////////////////////
#if NET_4_6

using System.Collections.Generic;
using System.Linq;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using UnityEngine;
using UnityEngine.Assertions;

namespace MG.MDV
{
    /// <see cref="RendererMarkdown"/>

    public class DebugRenderer : RendererBase
    {
        public DebugRenderer()
        {
            ObjectRenderers.Add( new DebugBlockCode() );
            ObjectRenderers.Add( new DebugBlockList() );
            ObjectRenderers.Add( new DebugBlockHeading() );
            ObjectRenderers.Add( new DebugBlockHtml() );
            ObjectRenderers.Add( new DebugBlockParagraph() );
            ObjectRenderers.Add( new DebugBlockQuote() );
            ObjectRenderers.Add( new DebugBlockThematicBreak() );

            ObjectRenderers.Add( new DebugInlineLink() );
            ObjectRenderers.Add( new DebugInlineAutoLink() );
            ObjectRenderers.Add( new DebugInlineCode() );
            ObjectRenderers.Add( new DebugInlineDelimiter() );
            ObjectRenderers.Add( new DebugInlineEmphasis() );
            ObjectRenderers.Add( new DebugInlineLineBreak() );
            ObjectRenderers.Add( new DebugInlineHtml() );
            ObjectRenderers.Add( new DebugInlineHtmlEntity() );
            ObjectRenderers.Add( new DebugInlineLiteral() );
        }


        public override object Render( MarkdownObject document )
        {
            Write( document );
            return this;
        }


        internal void WriteLeafBlockInline( LeafBlock block )
        {
            var inline = block.Inline as Inline;

            while( inline != null )
            {
                Write( inline );
                inline = inline.NextSibling;
            }
        }

        internal void WriteLeafRawLines( LeafBlock block )
        {
            if( block.Lines.Lines == null )
            {
                return;
            }

            var lines  = block.Lines;
            var slices = lines.Lines;

            for( int i = 0; i < lines.Count; i++ )
            {
                Emit( slices[ i ].ToString() );
            }
        }

        internal string GetContents( ContainerInline node )
        {
            if( node == null )
            {
                return string.Empty;
            }

            var inline  = node.FirstChild;
            var content = string.Empty;

            while( inline != null )
            {
                var lit = inline as LiteralInline;

                if( lit != null )
                {
                    content += lit.Content.ToString();
                }

                inline = inline.NextSibling;
            }

            return content;
        }

        //------------------------------------------------------------------------------

        List<string> mTree = new List<string>();

        internal void Print( string str )
        {
            var indent = new string( ' ', mTree.Count * 2 );
            Debug.Log( $"{indent}{str}" );
        }

        internal void Begin( string node )
        {
            Print( node );
            mTree.Add( node );
        }

        internal void End( string node )
        {
            Assert.IsTrue( mTree.Count > 0 );
            Assert.IsTrue( mTree.Last() == node );
            mTree.RemoveAt( mTree.Count - 1 );
        }

        internal void Emit( string content )
        {
            Print( $": {content}" );
        }
    }
}

#endif

