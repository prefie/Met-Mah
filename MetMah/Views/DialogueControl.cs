using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MetMah.Views
{
    public partial class DialogueControl : UserControl
    {
        private GameState game;
        private Button[] buttons;

        public void Configure(GameState game)
        {
            this.game = game;
            buttons = new Button[game.CurrentDialogue.CountAnswers];

            ClientSize = new Size(
                32 * game.WidthCurrentLevel,
                32 * game.HeightCurrentLevel + 32);

            var answers = game.CurrentDialogue.GetAnswers().ToList();
            Controls.Clear();
            for (int i = 0; i < answers.Count; i++)
            {
                var newButton = new Button
                {
                    Text = answers[i],
                    Size = new Size(140, 35),
                    BackColor = Color.LightSlateGray,
                    ForeColor = Color.White
                };
                newButton.GotFocus += (sender, e) => (sender as Button).BackColor = Color.Black;
                newButton.LostFocus += (sender, e) => (sender as Button).BackColor = Color.LightSlateGray;
                newButton.Location = new Point(150 * i, 30);
                var j = i;
                newButton.Click += (sender, e) => SetKey(j);
                buttons[i] = newButton;
            }

            Controls.AddRange(buttons.ToArray());
            ActiveControl = Controls[0];
        }

        protected override void OnPaint(PaintEventArgs e) =>
            e.Graphics.DrawString(game.CurrentDialogue.Text, new Font("Arial", 16), Brushes.Green, 0, 0);

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Focus();
        }

        private void SetKey(int i)
        {
            game.SetKeyPressed((Keys)(i + 49));
        }
                

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
    }
}
