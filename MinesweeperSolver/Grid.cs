using System;
using NonoGramAI.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MinesweeperSolver
{
    public class Grid
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int NumBombs { get; set; }
        public List<List<Tile>> Tiles { get; set; }

        public Grid(int width, int height, int numBombs)
        {
            Width = width;
            Height = height;
            NumBombs = numBombs < width*height ? numBombs : (width*height)-1;
            Tiles = new List<List<Tile>>();

            var rnd = new Random();
            for (var i = 0; i < Width; i++)
            {
                var row = new List<Tile>();
                for (var j = 0; j < Height; j++)
                {
                    row.Add(new Tile());
                }
                Tiles.Add(row);
            }

            var distribution = RandomList(width,numBombs,height/2);
            for (var i = 0; i < Width; i ++)
            {
                var tempCol = Tiles[i].OrderBy(t => rnd.Next()).ToList();
                var selected = tempCol.Take(distribution[i]);
                foreach (var tile in selected)
                    tile.isBomb = true;
                Console.WriteLine("Bombs in Col: " + selected.Count());
            }
        }

        private static int[] RandomList(int count, int total, int max)
        {
            int LOWERBOUND = 0;
            int UPPERBOUND = max;

            int[] result = new int[count];
            int currentsum = 0;
            int low, high, calc;

            if((UPPERBOUND * count) < total ||
               (LOWERBOUND * count) > total ||
               UPPERBOUND < LOWERBOUND)
                throw new Exception("Not possible.");

            Random rnd = new Random();

            for (int index = 0; index < count; index++)
            {
                calc = (total - currentsum) - (UPPERBOUND * (count - 1 - index));
                low = calc < LOWERBOUND ? LOWERBOUND : calc;
                calc = (total - currentsum) - (LOWERBOUND * (count - 1 - index));
                high = calc > UPPERBOUND ? UPPERBOUND : calc;

                result[index] = rnd.Next(low, high + 1);

                currentsum += result[index];
            }

            // The tail numbers will tend to drift higher or lower so we should shuffle to compensate somewhat.

            int shuffleCount = rnd.Next(count * 5, count * 10);
            while (shuffleCount-- > 0)
                Swap(ref result[rnd.Next(0, count)], ref result[rnd.Next(0, count)]);

            return result;
        }
        private static void Swap(ref int item1, ref int item2)
        {
            int temp = item1;
            item1 = item2;
            item2 = temp;
        }
    }
}