using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;

namespace GamePlatform.Desktop.Utils;

public static class PrintHelper
{
    public static void PrintDataGrid(DataGrid grid, string title)
    {
        var dlg = new PrintDialog();
        if (dlg.ShowDialog() != true) return;

        var doc = new FlowDocument();
        doc.PageWidth = dlg.PrintableAreaWidth;
        doc.PageHeight = dlg.PrintableAreaHeight;
        doc.PagePadding = new Thickness(50);
        var table = new Table();
        doc.Blocks.Add(table);
        table.CellSpacing = 0;

        foreach (var _ in grid.Columns)
            table.Columns.Add(new TableColumn());

        var headerRow = new TableRow();
        foreach (var col in grid.Columns)
        {
            var cell = new TableCell(new Paragraph(new Run(col.Header?.ToString() ?? string.Empty)))
            { Background = Brushes.LightGray, FontWeight = FontWeights.Bold, Padding = new Thickness(4) };
            headerRow.Cells.Add(cell);
        }
        var rg = new TableRowGroup();
        rg.Rows.Add(headerRow);

        foreach (var item in grid.ItemsSource.Cast<object>())
        {
            var row = new TableRow();
            foreach (var col in grid.Columns)
            {
                string? text = string.Empty;
                if (col is DataGridBoundColumn bc && bc.Binding is System.Windows.Data.Binding b && b.Path is not null)
                {
                    var prop = item.GetType().GetProperty(b.Path.Path!);
                    if (prop != null)
                    {
                        var val = prop.GetValue(item);
                        text = val?.ToString();
                    }
                }
                row.Cells.Add(new TableCell(new Paragraph(new Run(text ?? string.Empty))) { Padding = new Thickness(4) });
            }
            rg.Rows.Add(row);
        }
        table.RowGroups.Add(rg);

        IDocumentPaginatorSource dps = doc;
        dlg.PrintDocument(dps.DocumentPaginator, title);
    }
}