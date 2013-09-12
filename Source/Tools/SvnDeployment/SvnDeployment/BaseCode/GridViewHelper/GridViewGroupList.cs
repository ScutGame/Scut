//------------------------------------------------------------------------------------------
// Copyright © 2006 Agrinei Sousa [www.agrinei.com]
//
// Esse código fonte é fornecido sem garantia de qualquer tipo.
// Sinta-se livre para utilizá-lo, modificá-lo e distribuí-lo,
// inclusive em aplicações comerciais.
// É altamente desejável que essa mensagem não seja removida.
//------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

/// <summary>
/// Summary description for GridViewGroupList
/// </summary>
public class GridViewGroupList : List<GridViewGroup>
{
    public GridViewGroup this[string name]
    {
        get { return this.FindGroupByName(name); }
    }

    public GridViewGroup FindGroupByName(string name)
    {
        foreach (GridViewGroup g in this)
        {
            if (g.Name.ToLower() == name.ToLower()) return g;
        }

        return null;
    }
}
