//------------------------------------------------------------------------------------------
// Copyright ?2006 Agrinei Sousa [www.agrinei.com]
//
// Esse código fonte ?fornecido sem garantia de qualquer tipo.
// Sinta-se livre para utiliz?lo, modific?lo e distribu?lo,
// inclusive em aplicações comerciais.
// ?altamente desejável que essa mensagem não seja removida.
//------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;


public delegate void GroupEvent(string groupName, object[] values, GridViewRow row);

/// <summary>
/// A class that represents a group consisting of a set of columns
/// </summary>
public class GridViewGroup
{
    #region Fields

    private string[] _columns;
    private object[] _actualValues;
    private int _quantity;
    private bool _automatic;
    private bool _hideGroupColumns;
    private bool _isSuppressGroup;
    private bool _generateAllCellsOnSummaryRow;
    private GridViewSummaryList mSummaries;

    #endregion

    #region Properties

    public string[] Columns
    {
        get { return _columns; }
    }

    public object[] ActualValues
    {
        get { return _actualValues; }
    }

    public int Quantity
    {
        get { return _quantity; }
    }

    public bool Automatic
    {
        get { return _automatic; }
        set { _automatic = value; }
    }

    public bool HideGroupColumns
    {
        get { return _hideGroupColumns; }
        set { _hideGroupColumns = value; }
    }

    public bool IsSuppressGroup
    {
        get { return _isSuppressGroup; }
    }

    public bool GenerateAllCellsOnSummaryRow
    {
        get { return _generateAllCellsOnSummaryRow; }
        set { _generateAllCellsOnSummaryRow = value; }
    }

    public string Name
    {
        get { return String.Join("+", this._columns); }
    }

    public GridViewSummaryList Summaries
    {
        get { return mSummaries; }
    }

    #endregion

    #region Constructors

    public GridViewGroup(string[] cols, bool isSuppressGroup, bool auto, bool hideGroupColumns, bool generateAllCellsOnSummaryRow)
    {
        this.mSummaries = new GridViewSummaryList();
        this._actualValues = null;
        this._quantity = 0;
        this._columns = cols;
        this._isSuppressGroup = isSuppressGroup;
        this._automatic = auto;
        this._hideGroupColumns = hideGroupColumns;
        this._generateAllCellsOnSummaryRow = generateAllCellsOnSummaryRow;
    }

    public GridViewGroup(string[] cols, bool auto, bool hideGroupColumns, bool generateAllCellsOnSummaryRow) : this( cols, false, auto, hideGroupColumns, generateAllCellsOnSummaryRow)
    {
    }

    public GridViewGroup(string[] cols, bool auto, bool hideGroupColumns) : this(cols, auto, hideGroupColumns, false)
    {
    }

    #endregion

    internal void SetActualValues( object[] values )
    {
        this._actualValues = values;
    }

    public bool ContainsSummary(GridViewSummary s)
    {
        return mSummaries.Contains(s);
    }

    public void AddSummary( GridViewSummary s)
    {
        if (this.ContainsSummary(s))
        {
            throw new Exception("Summary already exists in this group.");
        }

        if (!s.Validate())
        {
            throw new Exception("Invalid summary.");
        }

        ///s._group = this;
        s.SetGroup(this);
        this.mSummaries.Add(s);
    }

    public void Reset()
    {
        this._quantity = 0;

        foreach (GridViewSummary s in mSummaries)
        {
            s.Reset();
        }
    }

    public void AddValueToSummaries(object dataitem)
    {
        this._quantity++;

        foreach (GridViewSummary s in mSummaries)
        {
            s.AddValue(DataBinder.Eval(dataitem, s.Column));
        }
    }

    public void CalculateSummaries()
    {
        foreach (GridViewSummary s in mSummaries)
        {
            s.Calculate();
        }
    }
}
