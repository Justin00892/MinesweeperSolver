using System.Windows.Forms;
using MinesweeperSolver.Properties;

namespace MinesweeperSolver
{
    public partial class MainForm : Form
    {
        private Grid _grid;
        public MainForm()
        {
            InitializeComponent();
            ConstructBoard();
        }

        private void ConstructBoard()
        {
            gridPanel.Visible = false;
            gridPanel.Controls.Clear();
            var settings = Settings.Default;
            _grid = new Grid(settings.Width,settings.Height,settings.Bombs);
            gridPanel.RowCount = _grid.Height;
            gridPanel.ColumnCount = _grid.Width;

            for (var i = 0; i < _grid.Width; i++)
            {
                for (var j = 0; j < _grid.Height; j++)
                {
                    var panel = _grid.Tiles[i][j];
                    gridPanel.Controls.Add(panel);
                    gridPanel.SetRow(panel, i);
                    gridPanel.SetColumn(panel, j);
                }
            }

            gridPanel.Visible = true;
        }

        private void SettingsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var settingsForm = new SettingsForm();
            if(settingsForm.ShowDialog() == DialogResult.OK)
                ConstructBoard();
        }
    }
}
