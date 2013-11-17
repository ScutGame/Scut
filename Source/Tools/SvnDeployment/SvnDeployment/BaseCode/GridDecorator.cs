using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public class GridDecorator
{
    public static void MergeRows(GridView gridView)
    {
        MergeRows(gridView, -1);
    }

    public static void MergeRows(GridView gridView, int hide)
    {
        int rowCount = gridView.Rows.Count;

        if (hide > 0)
        {
            gridView.HeaderRow.Cells[hide].Visible = false;
            gridView.FooterRow.Cells[hide].Visible = false;
            if (rowCount > 1)
                gridView.Rows[rowCount - 1].Cells[hide].Visible = false;
        }
        for (int rowIndex = rowCount - 2; rowIndex >= 0; rowIndex--)
        {
            GridViewRow row = gridView.Rows[rowIndex];
            GridViewRow previousRow = gridView.Rows[rowIndex + 1];

            if (row.Cells[0].Text == previousRow.Cells[0].Text)
            {
                row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 : previousRow.Cells[0].RowSpan + 1;
                previousRow.Cells[0].Visible = false;
            }
            if (hide > 0)
            {
                row.Cells[hide].Visible = false;
            }

            //for (int cellIndex = 0; cellIndex < row.Cells.Count; cellIndex++)
            //{
            //    if (row.Cells[cellIndex].Text == previousRow.Cells[cellIndex].Text)
            //    {
            //        row.Cells[cellIndex].RowSpan = previousRow.Cells[cellIndex].RowSpan < 2 ? 2 : previousRow.Cells[cellIndex].RowSpan + 1;
            //        previousRow.Cells[cellIndex].Visible = false;
            //    }
            //}
        }
    }
}
