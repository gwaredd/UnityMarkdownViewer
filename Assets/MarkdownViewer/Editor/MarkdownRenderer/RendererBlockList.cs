////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax;
using UnityEditor;
using UnityEngine;

namespace MG.MDV
{
    ////////////////////////////////////////////////////////////////////////////////
    // <ul><li>...</li></ul>
    /// <see cref="Markdig.Renderers.Html.ListRenderer"/>

    public class RendererBlockList : MarkdownObjectRenderer<RendererMarkdown, ListBlock>
    {
        protected override void Write( RendererMarkdown renderer, ListBlock block )
        {
            renderer.EnsureLine();

            for( var i = 0; i < block.Count; i++ )
            {
                renderer.Print( i.ToString() );
                renderer.Print( "\t" );
                renderer.WriteChildren( block[ i ] as ListItemBlock );
                renderer.EnsureLine();
            }

//             using( new EditorGUILayout.VerticalScope() )
//             {
//                 for( var i = 0; i < block.Count; i++ )
//                 {
//                     using( new EditorGUILayout.HorizontalScope() )
//                     {
//                         // TODO: renderer.ImplicitParagraph = !listBlock.IsLoose;
//                         // TODO: if block.IsOrdered <ol> else <ul>
// 
//                         GUILayout.Label( i.ToString(), GUILayout.Width( 10 ) );
// 
//                         var item = block[i] as ListItemBlock;
//                         renderer.WriteChildren( item );
//                     }
//                 }
//             }
        }
    }
}
