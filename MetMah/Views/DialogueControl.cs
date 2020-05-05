using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MetMah.Views
{
    public partial class DialogueControl : UserControl
    {
        private GameState game;
        private HashSet<Keys> pressedKeys;
        private Button[] buttons;

        public void Configure(GameState game, HashSet<Keys> pressedKeys)
        {
            this.game = game;
            this.pressedKeys = pressedKeys;
            buttons = new Button[game.CurrentDialogue.CountAnswers];
            ClientSize = new Size(
                32 * game.WidthCurrentLevel,
                32 * game.HeightCurrentLevel + 32);
            var answers = game.CurrentDialogue.GetAnswers().ToList();
            Controls.Clear();
            var offset = 0;
            for (int i = 0; i < answers.Count; i++)
            {
                var newButton = new Button();
                newButton.Text = answers[i];
                newButton.Size = new Size(140, 35);
                newButton.BackColor = Color.LightSlateGray;
                newButton.ForeColor = Color.White;
                newButton.GotFocus += (sender, e) => (sender as Button).BackColor = Color.Black;
                newButton.LostFocus += (sender, e) => (sender as Button).BackColor = Color.LightSlateGray;
                newButton.Location = new Point(offset, 30);
                var j = i;
                newButton.Click += (sender, e) => SetK(j);
                buttons[i] = newButton;
                offset += 150;
            }

            Controls.AddRange(buttons.ToArray());
            ActiveControl = Controls[0];
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawString(game.CurrentDialogue.Text, new Font("Arial", 16), Brushes.Green, 0, 0);
        }

        private void SetK(int i)
        {
            var key = (Keys)(i + 49);
            game.SetKeyPressed(key);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            pressedKeys.Add(e.KeyCode);
            game.SetKeyPressed(e.KeyCode);
        }

        protected override void OnLoad(EventArgs e)
        {
            DoubleBuffered = true;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            pressedKeys.Remove(e.KeyCode);
            game.SetKeyPressed(pressedKeys.Any() ? pressedKeys.Min() : Keys.None);
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
