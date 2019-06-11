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
            _grid = new Grid(settings.Width, settings.Height, settings.Bombs)
            {
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                AutoSize = true,
                Location = new Point(0, 25),
                Margin = new Padding(0,0,0,0),
                RowCount = settings.Width,
                ColumnCount = settings.Height,
                Visible = false
            };
            Controls.Add(_grid);

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
