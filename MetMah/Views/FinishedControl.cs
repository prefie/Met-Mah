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
    public partial class FinishedControl : UserControl
    {
        private GameState game;
        private Button buttonPlay;
        private Button buttonExit;

        public void Configure(GameState game)
        {
            this.game = game;
            ClientSize = new Size(
                32 * 28,
                32 * 13 + 32);

            buttonPlay = new Button();
            buttonExit = new Button();
            buttonPlay.Size = new Size(200, 35);
            buttonPlay.BackColor = Color.LightGray;
            buttonExit.Size = new Size(200, 35);
            buttonExit.BackColor = Color.LightGray;
            if (game.PatienceScale <= 0)
                BackgroundImage = Image.FromFile(@"Images\FinishLose.png");
            else
                BackgroundImage = Image.FromFile(@"Images\FinishWin.png");
            buttonPlay.Location = new Point((Size.Width - buttonPlay.Size.Width) / 2, (Size.Height - buttonPlay.Size.Height) / 2);
            buttonExit.Location = new Point(buttonPlay.Location.X, buttonPlay.Location.Y + 40);
            buttonPlay.Text = "В главное меню";
            buttonExit.Text = "Выйти";
            buttonPlay.Click += (sender, e) => game.Initialize();
            buttonExit.Click += ExitButton_Click;
            Controls.Add(buttonPlay);
            Controls.Add(buttonExit);
        }

        protected override void OnLoad(EventArgs e)
        {
            Focus();
            base.OnLoad(e);
            DoubleBuffered = true;
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

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
