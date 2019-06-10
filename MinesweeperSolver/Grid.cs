using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MinesweeperSolver
{
    public class Grid
    {
        public int Width { get; }
        public int Height { get; }
        private int NumBombs { get; }
        public List<List<Tile>> Tiles { get; }

        public Grid(int width, int height, int numBombs)
        {
            Width = width;
            Height = height;

            var rnd = new Random();
            NumBombs = numBombs < width*height ? numBombs : rnd.Next(width*height/4,width*height/2);
            Tiles = new List<List<Tile>>();

            for (var i = 0; i < Width; i++)
            {
                var row = new List<Tile>();
                for (var j = 0; j < Height; j++)
                {
                    var tile = new Tile();
                    row.Add(tile);
                }
                Tiles.Add(row);
            }
            LayBombs();
        }

        public void LayBombs()
        {
            var rnd = new Random();
            var distribution = RandomList(Width, NumBombs, Height / 2);
            for (var i = 0; i < Width; i++)
            {
                var tempCol = Tiles[i].OrderBy(t => rnd.Next());
                var selected = tempCol.Where(t => !t.State).Take(distribution[i]).ToList();
                foreach (var tile in selected)
                    tile.IsBomb = true;
                //Console.WriteLine("Bombs in Col: " + selected.Count);
            }

            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    if (!Tiles[i][j].IsBomb) continue;

                    if (i != 0 && j != 0)
                        Tiles[i - 1][j - 1].Count++;

                    if (i != 0)
                    {
                        Tiles[i - 1][j].Count++;
                        if (j != Height - 1)
                            Tiles[i - 1][j + 1].Count++;
                    }

                    if (j != 0)
                    {
                        Tiles[i][j - 1].Count++;
                        if (i != Width - 1)
                            Tiles[i + 1][j - 1].Count++;
                    }

                    if (j != Height - 1)
                    {
                        Tiles[i][j + 1].Count++;
                    }

                    if (i != Width - 1)
                    {
                        Tiles[i + 1][j].Count++;
                    }

                    if (i != Width - 1 && j != Height - 1)
                        Tiles[i + 1][j + 1].Count++;
                }
            }

            foreach (var row in Tiles)
            {
                foreach (var tile in row)
                {
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