using System.Windows.Forms;
using System.Drawing;

namespace MinesweeperSolver
{
    public partial class MainForm : Form
    {
        private Grid _grid = new Grid(9,9,10);
        public MainForm()
        {
            InitializeComponent();
            ConstructBoard();
        }

        private void ConstructBoard()
        {
            gridPanel.RowCount = _grid.Height;
            gridPanel.ColumnCount = _grid.Width;
            gridPanel.Controls.Clear();

            for (var i = 0; i < _grid.Width; i++)
            {
                for (var j = 0; j < _grid.Height; j++)
                {
                    var panel = _grid.Tiles[i][j].GetTilePanel();
                    var x = i;
                    var y = j;
                    panel.MouseClick += (o, args) =>
                    {
                        if (args.Button == MouseButtons.Left)
                        {
                            if (!_grid.Tiles[x][y].Flag)
                            {
                                if(_grid.Tiles[x][y].isBomb)
                                    panel.BackColor = Color.Black;
                                else
                                {
                                    _grid.Tiles[x][y].State = !_grid.Tiles[x][y].State;
                                    panel.BackColor = _grid.Tiles[x][y].State ? Color.White : Color.LightGray;
                                }                               
                            }                   
                        }
                        else if (args.Button == MouseButtons.Right)
                        {
                            if (!_grid.Tiles[x][y].State)
                            {
                                _grid.Tiles[x][y].Flag = !_grid.Tiles[x][y].Flag;
                                panel.BackColor = _grid.Tiles[x][y].Flag ? Color.Red : Color.LightGray;
                            }                        
                        }                        
                    };
                    gridPanel.Controls.Add(panel);
                    gridPanel.SetRow(panel, i);
                    gridPanel.SetColumn(panel, j);
                }
            }
        }
    }
}
