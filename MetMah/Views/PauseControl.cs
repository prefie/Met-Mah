﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace MetMah.Views
{
    public partial class PauseControl : UserControl
    {
        private GameState game;
        private Button buttonContinue;
        private Button buttonRestart;
        private Button buttonMenu;
        private Button buttonExit;
        private Timer timer;

        public void Configure(GameState game, Timer timer)
        {
            this.timer = timer;
            this.game = game;
            ClientSize = new Size(
                50 * 10,
                50 * 8 + 32);

            Location = new Point(50 * 9, 50 * 3);
            BackColor = Color.FromArgb(160, Color.Black);

            var font = new Font("Arial", 16);

            buttonMenu = new Button();
            buttonExit = new Button();
            buttonContinue = new Button();
            buttonRestart = new Button();
            buttonMenu.Size = new Size(6 * 50, 45);
            buttonMenu.BackColor = Color.LightGray;
            buttonMenu.Font = font;
            buttonExit.Size = new Size(6 * 50, 45);
            buttonExit.BackColor = Color.LightGray;
            buttonExit.Font = font;
            buttonContinue.Size = new Size(6 * 50, 45);
            buttonContinue.BackColor = Color.LightGray;
            buttonContinue.Font = font;
            buttonRestart.Size = new Size(6 * 50, 45);
            buttonRestart.BackColor = Color.LightGray;
            buttonRestart.Font = font;

            buttonContinue.Location = new Point((ClientSize.Width - buttonMenu.Size.Width) / 2,
                (ClientSize.Height - buttonMenu.Size.Height) - 270);
            buttonRestart.Location = new Point(buttonContinue.Location.X,
                buttonContinue.Location.Y + 60);
            buttonMenu.Location = new Point(buttonRestart.Location.X,
                buttonRestart.Location.Y + 60);
            buttonExit.Location = new Point(buttonMenu.Location.X,
                buttonMenu.Location.Y + 60);

            buttonContinue.Text = "Продолжить";
            buttonRestart.Text = "Начать сначала";
            buttonMenu.Text = "В главное меню";
            buttonExit.Text = "Выйти";

            buttonContinue.Click += ContinueButton_Click;
            buttonRestart.Click += RestartButton_Click;
            buttonMenu.Click += MenuButton_Click;
            buttonExit.Click += ExitButton_Click;

            Controls.Add(buttonContinue);
            Controls.Add(buttonRestart);
            Controls.Add(buttonMenu);
            Controls.Add(buttonExit);

            ActiveControl = Controls[0];
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawString("Пауза", new Font("Arial", 32),
                Brushes.White, ClientSize.Width / 2 - 70, 25);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Focus();
        }


        private void ContinueButton_Click(object sender, EventArgs e)
        {
            timer.Start();
            Parent.Focus();
            Hide();
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            Hide();
            Parent.Focus();
            game.Initialize();
            game.Start();
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            Hide();
            Parent.Focus();
            game.Initialize();
        }

        private void ExitButton_Click(object sender, EventArgs e) => Application.Exit();

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
