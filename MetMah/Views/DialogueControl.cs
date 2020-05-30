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
                50 * 28,
                150);

            Location = new Point(0, 50 * 7 / 2 + 75);
            BackColor = Color.FromArgb(160, Color.Black);

            var answers = game.CurrentDialogue.GetAnswers().ToList();
            Controls.Clear();
            var font = new Font("Arial", 12);
            for (int i = 0; i < answers.Count; i++)
            {
                var newButton = new Button
                {
                    Text = answers[i],
                    Font = font,
                    Size = new Size(240, 50),
                    BackColor = Color.LightSlateGray,
                    ForeColor = Color.White
                };
                newButton.GotFocus += (sender, e) => (sender as Button).BackColor = Color.Black;
                newButton.LostFocus += (sender, e) => (sender as Button).BackColor = Color.LightSlateGray;
                newButton.Location = new Point(50 + 250 * i, 50);
                newButton.Location = new Point((Width - 250 * answers.Count) / 2 + 250 * i, 70);
                var j = i;
                newButton.Click += (sender, e) => SetKey(j);
                buttons[i] = newButton;
            }

            Controls.AddRange(buttons.ToArray());
            ActiveControl = Controls[0];
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var text = game.CurrentDialogue.Text.Split('.');
            var font = new Font("Arial", 16);
            for (int i = 0; i < text.Length; i++)
            {
                e.Graphics.DrawString(text[i], font, Brushes.White, (Width - text[i].Length * (font.Size - 4)) / 2, 15 + 20 * i);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Focus();
        }

        private void SetKey(int key)
        {
            game.SetKeyPressed((Keys)(key + 49));
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
