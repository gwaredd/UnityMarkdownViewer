////////////////////////////////////////////////////////////////////////////////

using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace MG.MDV
{
    ////////////////////////////////////////////////////////////////////////////////
    // <img src="..." /> || <a href="...">
    /// <see cref="Markdig.Renderers.Html.Inlines.LinkInlineRenderer"/>

    public class RendererInlineLink : MarkdownObjectRenderer<RendererMarkdown, LinkInline>
    {
        protected override void Write( RendererMarkdown renderer, LinkInline node )
        {
            // TODO: links
            renderer.Print( node.Title );

//             if( renderer.EnableHtmlForInline )
//             {
//                 renderer.Write( link.IsImage ? "<img src=\"" : "<a href=\"" );
//                 renderer.WriteEscapeUrl( link.GetDynamicUrl != null ? link.GetDynamicUrl() ?? link.Url : link.Url );
//                 renderer.Write( "\"" );
//                 renderer.WriteAttributes( link );
//             }
//             if( link.IsImage )
//             {
//                 if( renderer.EnableHtmlForInline )
//                 {
//                     renderer.Write( " alt=\"" );
//                 }
//                 var wasEnableHtmlForInline = renderer.EnableHtmlForInline;
//                 renderer.EnableHtmlForInline = false;
//                 renderer.WriteChildren( link );
//                 renderer.EnableHtmlForInline = wasEnableHtmlForInline;
//                 if( renderer.EnableHtmlForInline )
//                 {
//                     renderer.Write( "\"" );
//                 }
//             }
// 
//             if( renderer.EnableHtmlForInline && !string.IsNullOrEmpty( link.Title ) )
//             {
//                 renderer.Write( " title=\"" );
//                 renderer.WriteEscape( link.Title );
//                 renderer.Write( "\"" );
//             }
// 
//             if( link.IsImage )
//             {
//                 if( renderer.EnableHtmlForInline )
//                 {
//                     renderer.Write( " />" );
//                 }
//             }
//             else
//             {
//                 if( renderer.EnableHtmlForInline )
//                 {
//                     if( AutoRelNoFollow )
//                     {
//                         renderer.Write( " rel=\"nofollow\"" );
//                     }
//                     renderer.Write( ">" );
//                 }
//                 renderer.WriteChildren( link );
//                 if( renderer.EnableHtmlForInline )
//                 {
//                     renderer.Write( "</a>" );
//                 }
//             }
        }
    }
}
