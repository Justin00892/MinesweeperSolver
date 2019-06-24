using System.Drawing;
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
            Controls.Remove(_grid);
            var settings = Settings.Default;
            bombsLeft.Text = @"Bombs Left: " + settings.Bombs;
            _grid = new Grid(settings.Width, settings.Height, settings.Bombs)
            {
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                AutoSize = true,
                Location = new Point(0, 25),
                Margin = new Padding(0,0,0,0),
                RowCount = settings.Height,
                ColumnCount = settings.Width,
                Visible = false
            };
            Controls.Add(_grid);
            _grid.OnRemainingBombsChanged += new Grid.bombEventRaiser(() => bombsLeft.Text = "Bombs Left: "+_grid.RemainingBombs);
            _grid.OnGameOver += new Grid.gameOverEventRaiser(() =>
            {
                var result = MessageBox.Show("Game Over! Play Again?");
                if (result == DialogResult.OK)
                    ConstructBoard();
                else
                    Application.Exit();
            });
            _grid.OnRemainingBombsChanged += () => bombsLeft.Text = @"Bombs Left: "+_grid.RemainingBombs;
            _grid.Visible = true;
        }

        private void SettingsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var settingsForm = new SettingsForm();
            if(settingsForm.ShowDialog() == DialogResult.OK)
                ConstructBoard();
        }
    }
}
