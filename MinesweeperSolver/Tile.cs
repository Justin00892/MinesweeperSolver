using System.Drawing;
using System.Windows.Forms;

namespace MinesweeperSolver
{
    public sealed class Tile : Panel
    {
        public Tile()
        {
            Width = 30;
            Height = 30;
            Margin = new Padding(0);
            Padding = new Padding(0);
            BackColor = State ? Color.White : Color.LightGray;
            BorderStyle = BorderStyle.FixedSingle;

            MouseClick += (sender, args) =>
            {
                OnClick(args);
            };
        }

        public bool State { get; set; }

        public bool Flag { get; set; }

        public bool IsBomb { get; set; }

        public int Count { get; set; }

        public void OnClick(MouseEventArgs args)
        {
            if (args.Button == MouseButtons.Left)
            {
                if (Flag || State) return;
                if (IsBomb)
                {
                    BackColor = Color.Black;
                    State = !State;
                }
                else
                {
                    State = !State;
                    BackColor = State ? Color.White : Color.LightGray;
                    var controls = Controls.Find("label", false);
                    foreach (var control in controls)
                        control.Visible = true;
                }
            }
            else if (args.Button == MouseButtons.Right)
            {
                if (State) return;
                Flag = !Flag;
                BackColor = Flag ? Color.Red : Color.LightGray;
                var controls = Controls.Find("tracker", false);
                foreach (var control in controls)
                    control.Visible = !Flag;
            }
        }
    }
}