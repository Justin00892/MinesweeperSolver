using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MinesweeperSolver
{
    public static class Solver
    {
        public static void PredictBombs(Grid grid, TableLayoutPanelCellPosition location)
        {
            for (var i = 0; i < grid.ColumnCount; i++)
            {
                for (var j = 0; j < grid.RowCount; j++)
                {
                    var tile = (Tile)grid.GetControlFromPosition(i, j);
                    if (tile.State && tile.Count > 0)
                    {
                        var adjacentList = new List<Tile>();
                        if (i != 0 && j != 0)
                        adjacentList.Add((Tile)grid.GetControlFromPosition(i-1, j-1));

                        if (i != 0)
                        {
                            adjacentList.Add((Tile)grid.GetControlFromPosition(i-1, j));
                            if (j != grid.RowCount - 1)
                                adjacentList.Add((Tile)grid.GetControlFromPosition(i-1, j+1));
                        }

                        if (j != 0)
                        {
                            adjacentList.Add((Tile)grid.GetControlFromPosition(i, j-1));
                            if (i != grid.ColumnCount - 1)
                                adjacentList.Add((Tile)grid.GetControlFromPosition(i+1, j-1));
                        }

                        if (j != grid.RowCount - 1)
                        {
                            adjacentList.Add((Tile)grid.GetControlFromPosition(i, j+1));
                        }

                        if (i != grid.ColumnCount - 1)
                        {
                            adjacentList.Add((Tile)grid.GetControlFromPosition(i+1, j));
                        }

                        if (i != grid.ColumnCount - 1 && j != grid.RowCount - 1)
                            adjacentList.Add((Tile)grid.GetControlFromPosition(i+1, j+1));

                        adjacentList.RemoveAll(a => a.State || a.Flag);
                        foreach (var adj in adjacentList)
                        {
                            var remaining = tile.Count - adjacentList.Where(a => a.Flag).Count();
                            var chance = (double) remaining / adjacentList.Count;
                            var controls = adj.Controls.Find("tracker", false);
                            foreach (var control in controls)
                            {
                                if (double.TryParse(control.Text, out var current))
                                    control.Text = (chance + current)/2 +"";
                                else
                                    control.Text = chance +"";
                            }
                        }
                    }
                }
            }
        }
    }
}