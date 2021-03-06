﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MetMah.Views
{
    public partial class ChoiceCharacterControl : UserControl
    {
        private GameState game;
        private List<PictureBox> pictures;
        public string PlayerName { get; private set; }

        public void Configure(GameState game)
        {
            this.game = game;
            ClientSize = new Size(
                50 * 28,
                50 * 14);

            BackgroundImage = Image.FromFile(@"Resources\Images\Backgrounds\Choice.png");
            pictures = new List<PictureBox>();

            var imagesDirectory = new DirectoryInfo(@"Resources\Images\ChoiceCharacter");
            var files = imagesDirectory.GetFiles("*Character.png");
            foreach (var e in files)
            {
                var picture = new PictureBox
                {
                    Image = Image.FromFile(e.FullName),
                    Name = e.Name,
                    Size = new Size(150, 150),
                    SizeMode = PictureBoxSizeMode.Normal
                };
                picture.Click += StartButton_Click;
                picture.MouseEnter +=
                    (sender, args) => picture.BackColor = Color.LightSkyBlue;
                picture.MouseLeave +=
                    (sender, args) => picture.BackColor = Color.FromArgb(30, Color.Blue);
                picture.BackColor = Color.FromArgb(30, Color.Blue); ;

                pictures.Add(picture);
            }

            var offset = (ClientSize.Width - 320) / 2;
            for (int i = 0; i < 4; i++)
            {
                pictures[i].Location = new Point(offset, i % 2 == 0 ? 170 : 340);
                if (i % 2 != 0)
                    offset += 170;
            }

            Controls.AddRange(pictures.ToArray());
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Focus();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            var box = sender as PictureBox;
            PlayerName = pictures.Where(x => x == box).Select(x => x.Name).FirstOrDefault();
            var a = pictures.IndexOf(box);
            game.SetNumberPlayer(a);
            game.Start();
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
