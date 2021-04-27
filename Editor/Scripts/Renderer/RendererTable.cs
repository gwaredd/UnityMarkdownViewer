using Markdig.Extensions.Tables;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using UnityEngine;

namespace MG.MDV
{
    public class RendererTable: MarkdownObjectRenderer<RendererMarkdown, Table>
    {
        protected override void Write(RendererMarkdown renderer, Table table)
        {
            var layout = renderer.Layout;

            if (table.Count == 0) return;

            layout.StartTable();

            for (var i = 0; i <= table.Count - 1; i++)
            {
                var tableRow = table[i] as TableRow;
                if (tableRow == null) continue;

                layout.StartTableRow(tableRow.IsHeader);
                var consumeSpace = renderer.ConsumeSpace;
                renderer.ConsumeSpace = true;
                for (var j = 0; j <= tableRow.Count - 1; j++)
                {
                    var tc = tableRow[j] as TableCell;
                    if(tc == null) continue;
                    if (tc[0].Span.IsEmpty)
                    {
                        renderer.Write(new LiteralInline(" "));
                        if (j != tableRow.Count - 1)
                        {
                            layout.NewLine();    
                        }
                    }
                    else
                    {
                        var consumeNewLine = renderer.ConsumeNewLine;
                        if (j == tableRow.Count - 1)
                        {
                            renderer.ConsumeNewLine = true;
                        }
                        renderer.WriteChildren(tc);
                        renderer.ConsumeNewLine = consumeNewLine;
                    }
                }
                renderer.ConsumeSpace = consumeSpace;
                layout.EndTableRow();
            }
            
            layout.EndTable();
        }
    }
}