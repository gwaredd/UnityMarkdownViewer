////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax;
using UnityEngine;

namespace MG.MDV
{
    // <h1> ... </h1>

    /// <see cref="Markdig.Renderers.Html.HeadingRenderer"/>

    public class RendererBlockHeading : MarkdownObjectRenderer<RendererMarkdown, HeadingBlock>
    {
        protected override void Write( RendererMarkdown renderer, HeadingBlock block )
        {
            renderer.FlushLine();

            renderer.WriteLeafBlockInline( block );

            var heading = renderer.GetText();

            if( !string.IsNullOrEmpty( heading ) )
            {
                var level = block.Level > 0 && block.Level <= 6 ? block.Level : 1;
                GUILayout.Label( heading, renderer.GetStyle( "h" + level ) );
            }
        }
    }
}
