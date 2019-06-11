using System;
using System.Collections.Generic;

using System.Linq;
using System.Windows.Forms;

namespace MinesweeperSolver
{
    public class Grid : TableLayoutPanel
    {
        public int GridWidth { get; }
        public int GridHeight { get; }
        private int NumBombs { get; }

        public Grid(int width, int height, int numBombs)
        {
            GridWidth = width;
            GridHeight = height;

            var rnd = new Random();
            NumBombs = numBombs < width*height ? numBombs : rnd.Next(width*height/4,width*height/2);

            for (var i = 0; i < GridWidth; i++)
            {
                for (var j = 0; j < GridHeight; j++)
                {
                    var tile = new Tile();
                    tile.BackColorChanged += (sender, args) =>
                    {
                        if (tile.Count == 0 && !tile.IsBomb)
                        {
                            var location = GetPositionFromControl(tile);
                            var adjacentPanel = (Tile) GetControlFromPosition(location.Column - 1, location.Row - 1);
                            var newArgs = new MouseEventArgs(MouseButtons.Left,1,tile.Location.X,tile.Location.Y,0);
                            adjacentPanel.OnClick(newArgs);
                        }
                    };
                    Controls.Add(tile);
                    SetRow(tile,i);
                    SetColumn(tile,j);
                }
            }
            LayBombs();
        }

        public void LayBombs()
        {
            var rnd = new Random();
            var distribution = RandomList(GridWidth, NumBombs, GridHeight / 2);
            for (var i = 0; i < GridWidth; i++)
            {
                var tempCol = new List<Tile>();
                for (var j = 0; j < GridHeight; j++)
                    tempCol.Add((Tile)GetControlFromPosition(i,j));

                tempCol = tempCol.OrderBy(t => rnd.Next()).ToList();
                var selected = tempCol.Where(t => !t.State).Take(distribution[i]).ToList();
                foreach (var tile in selected)
                    tile.IsBomb = true;
                //Console.WriteLine("Bombs in Col: " + selected.Count);
            }

            for (var i = 0; i < GridWidth; i++)
            {
                for (var j = 0; j < GridHeight; j++)
                {
                    if (!((Tile)GetControlFromPosition(i,j)).IsBomb) continue;

                    if (i != 0 && j != 0)
                        ((Tile)GetControlFromPosition(i-1, j-1)).Count++;

                    if (i != 0)
                    {
                        ((Tile)GetControlFromPosition(i-1, j)).Count++;
                        if (j != GridHeight - 1)
                            ((Tile)GetControlFromPosition(i-1, j+1)).Count++;
                    }

                    if (j != 0)
                    {
                        ((Tile)GetControlFromPosition(i, j-1)).Count++;
                        if (i != GridWidth - 1)
                            ((Tile)GetControlFromPosition(i+1, j-1)).Count++;
                    }

                    if (j != GridHeight - 1)
                    {
                        ((Tile)GetControlFromPosition(i, j+1)).Count++;
                    }

                    if (i != GridWidth - 1)
                    {
                        ((Tile)GetControlFromPosition(i+1, j)).Count++;
                    }

                    if (i != GridWidth - 1 && j != GridHeight - 1)
                        ((Tile)GetControlFromPosition(i+1, j)).Count++;
                }
            }

            for (var i = 0; i < GridWidth; i++)
            {
                for (var j = 0; j < GridHeight; j++)
                {
                    var tile = (Tile) GetControlFromPosition(i, j);
                    var label = new Label
                    {
                        Text = tile.Count == 0 ? "" : tile.Count.ToString(),
                        Enabled = false,
                        Visible = false,
                        Name = "label"
                    };
                    tile.Controls.Add(label);
                }
            }
        }

        private static int[] RandomList(int count, int total, int max)
        {

            var result = new int[count];
            var currentSum = 0;

            if((max * count) < total ||
               (0 * count) > total ||
               max < 0)
                throw new Exception("Not possible.");

            var rnd = new Random();

            for (var index = 0; index < count; index++)
            {
                var calc = (total - currentSum) - (max * (count - 1 - index));
                var low = calc < 0 ? 0 : calc;
                calc = (total - currentSum) - (0 * (count - 1 - index));
                var high = calc > max ? max : calc;

                result[index] = rnd.Next(low, high + 1);

                currentSum += result[index];
            }

            // The tail numbers will tend to drift higher or lower so we should shuffle to compensate somewhat.

            var shuffleCount = rnd.Next(count * 5, count * 10);
            while (shuffleCount-- > 0)
                Swap(ref result[rnd.Next(0, count)], ref result[rnd.Next(0, count)]);

            return result;
        }
        private static void Swap(ref int item1, ref int item2)
        {
            var temp = item1;
            item1 = item2;
            item2 = temp;
        }
    }
}