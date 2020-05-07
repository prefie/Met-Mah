﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace MetMah.Views
{
    public partial class StartControl : UserControl
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
            BackgroundImage = Image.FromFile(@"Images\Backgrounds\Background.png");
            buttonPlay.Location = new Point((ClientSize.Width - buttonPlay.Size.Width) / 2,
                (ClientSize.Height - buttonPlay.Size.Height) / 2);
            buttonExit.Location = new Point(buttonPlay.Location.X,
                buttonPlay.Location.Y + 40);
            buttonPlay.Text = "Играть";
            buttonExit.Text = "Выйти";
            buttonPlay.Click += StartButton_Click;
            buttonExit.Click += ExitButton_Click;
            Controls.Add(buttonPlay);
            Controls.Add(buttonExit);

            ActiveControl = Controls[0];
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Focus();
        }

        private void StartButton_Click(object sender, EventArgs e) => game.ChoiceCharacter();

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
