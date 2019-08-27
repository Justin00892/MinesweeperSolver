using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinesweeperSolver
{
    public class Grid : TableLayoutPanel
    {
        private int GridWidth { get; }
        private int GridHeight { get; }
        private int NumBombs { get; }
        private bool FirstClick { get; set; }

        public delegate void bombEventRaiser();
        public event bombEventRaiser OnRemainingBombsChanged;
        private int _remainingBombs;
        public int RemainingBombs
        {
            get => _remainingBombs;
            private set
            {
                _remainingBombs = value;
                OnRemainingBombsChanged?.Invoke();
            }
        }

        public delegate void gameOverEventRaiser();
        public event gameOverEventRaiser OnGameOver;
        private bool _gameOver;

        public bool GameOver
        {
            get { return _gameOver; }
            set
            {
                _gameOver = value;
                OnGameOver?.Invoke();
            }
        }

        public Grid(int width, int height, int numBombs)
        {
            FirstClick = true;
            GridWidth = width;
            GridHeight = height;

            var rnd = new Random();
            NumBombs = numBombs < width*height ? numBombs : rnd.Next(width*height/4,width*height/2);
            RemainingBombs = numBombs;

            for (var i = 0; i < GridWidth; i++)
            {
                for (var j = 0; j < GridHeight; j++)
                {
                    var tile = new Tile();
                    tile.BackColorChanged += (sender, args) =>
                    {
                        if (FirstClick && tile.BackColor == Color.White)
                        {
                            LayBombs(GetPositionFromControl(tile));
                            FirstClick = false;
                        }
                        if (!FirstClick && tile.BackColor == Color.White && tile.Count == 0 && !tile.IsBomb)
                        {
                            var location = GetPositionFromControl(tile);
                            Tile adjacentPanel;

                            if (location.Column != 0 && location.Row != 0)
                            {
                                adjacentPanel = (Tile) GetControlFromPosition(location.Column - 1, location.Row - 1);
                                if (!adjacentPanel.IsBomb)
                                {
                                    var newArgs = new MouseEventArgs(MouseButtons.Left, 1, tile.Location.X, tile.Location.Y, 0);
                                    adjacentPanel.OnClick(newArgs);
                                }
                            }

                            if (location.Column != 0)
                            {
                                adjacentPanel = (Tile)GetControlFromPosition(location.Column - 1, location.Row);
                                if (!adjacentPanel.IsBomb)
                                {
                                    var newArgs = new MouseEventArgs(MouseButtons.Left, 1, tile.Location.X, tile.Location.Y, 0);
                                    adjacentPanel.OnClick(newArgs);
                                }

                                if (location.Row != GridHeight - 1)
                                {
                                    adjacentPanel = (Tile) GetControlFromPosition(location.Column - 1, location.Row + 1);
                                    if (!adjacentPanel.IsBomb)
                                    {
                                        var newArgs = new MouseEventArgs(MouseButtons.Left, 1, tile.Location.X, tile.Location.Y, 0);
                                        adjacentPanel.OnClick(newArgs);
                                    }
                                }
                            }

                            if (location.Row != 0)
                            {
                                adjacentPanel = (Tile)GetControlFromPosition(location.Column, location.Row - 1);
                                if (!adjacentPanel.IsBomb)
                                {
                                    var newArgs = new MouseEventArgs(MouseButtons.Left, 1, tile.Location.X, tile.Location.Y, 0);
                                    adjacentPanel.OnClick(newArgs);
                                }

                                if (location.Column != GridWidth - 1)
                                {
                                    adjacentPanel = (Tile) GetControlFromPosition(location.Column + 1, location.Row - 1);
                                    if (!adjacentPanel.IsBomb)
                                    {
                                        var newArgs = new MouseEventArgs(MouseButtons.Left, 1, tile.Location.X, tile.Location.Y, 0);
                                        adjacentPanel.OnClick(newArgs);
                                    }
                                }
                            }

                            if (location.Row != GridHeight - 1)
                            {
                                adjacentPanel = (Tile)GetControlFromPosition(location.Column, location.Row + 1);
                                if (!adjacentPanel.IsBomb)
                                {
                                    var newArgs = new MouseEventArgs(MouseButtons.Left, 1, tile.Location.X, tile.Location.Y, 0);
                                    adjacentPanel.OnClick(newArgs);
                                }
                            }

                            if (location.Column != GridWidth - 1)
                            {
                                adjacentPanel = (Tile)GetControlFromPosition(location.Column + 1, location.Row );
                                if (!adjacentPanel.IsBomb)
                                {
                                    var newArgs = new MouseEventArgs(MouseButtons.Left, 1, tile.Location.X, tile.Location.Y, 0);
                                    adjacentPanel.OnClick(newArgs);
                                }
                            }

                            if (location.Column != GridWidth - 1 && location.Row != GridHeight - 1)
                            {
                                adjacentPanel = (Tile) GetControlFromPosition(location.Column + 1, location.Row + 1);
                                if (!adjacentPanel.IsBomb)
                                {
                                    var newArgs = new MouseEventArgs(MouseButtons.Left, 1, tile.Location.X, tile.Location.Y, 0);
                                    adjacentPanel.OnClick(newArgs);
                                }
                            }
                        }
                        if (tile.IsBomb && tile.BackColor == Color.Black)
                        {
                            GameOver = true;
                        }
                    };
                    Controls.Add(tile);
                    SetRow(tile,j);
                    SetColumn(tile,i);
                }
            }
        }

        private void LayBombs(TableLayoutPanelCellPosition location)
        {
            var deadZoneList = new List<TableLayoutPanelCellPosition>()
            {
                new TableLayoutPanelCellPosition(location.Column-1,location.Row-1),
                new TableLayoutPanelCellPosition(location.Column-1,location.Row),
                new TableLayoutPanelCellPosition(location.Column-1,location.Row+1),
                new TableLayoutPanelCellPosition(location.Column,location.Row-1),
                location,
                new TableLayoutPanelCellPosition(location.Column,location.Row+1),
                new TableLayoutPanelCellPosition(location.Column+1,location.Row-1),
                new TableLayoutPanelCellPosition(location.Column+1,location.Row),
                new TableLayoutPanelCellPosition(location.Column+1,location.Row+1)
            };
            var rnd = new Random();
            var distribution = RandomList(GridWidth, NumBombs, GridHeight / 2);
            for (var i = 0; i < GridWidth; i++)
            {
                var tempCol = new List<Tile>();
                for (var j = 0; j < GridHeight; j++)
                    tempCol.Add((Tile)GetControlFromPosition(i,j));

                tempCol = tempCol.Where(t => !deadZoneList.Contains(GetPositionFromControl(t))).OrderBy(t => rnd.Next()).ToList();
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
                        ((Tile)GetControlFromPosition(i+1, j+1)).Count++;
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
                    var tracker = new Label
                    {
                        Text = "",
                        Enabled = false,
                        Visible = true,
                        Name = "tracker"
                    };
                    tile.Controls.Add(tracker);
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