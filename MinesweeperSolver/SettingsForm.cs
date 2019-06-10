using System;
using System.Windows.Forms;
using MinesweeperSolver.Properties;

namespace MinesweeperSolver
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            widthSelect.Value = Settings.Default.Width;
            heightSelect.Value = Settings.Default.Height;
            bombSelect.Value = Settings.Default.Bombs;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Settings.Default.Width = (int) widthSelect.Value;
            Settings.Default.Height = (int) heightSelect.Value;
            Settings.Default.Bombs = (int) bombSelect.Value;
            Settings.Default.Save();
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
