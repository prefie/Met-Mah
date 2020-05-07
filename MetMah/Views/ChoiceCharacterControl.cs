using System;
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
                32 * 28,
                32 * 13 + 32);

            BackgroundImage = Image.FromFile(@"Images\Backgrounds\Choice.png");
            pictures = new List<PictureBox>();

            var imagesDirectory = new DirectoryInfo(@"Images\ChoiceCharacter");
            var files = imagesDirectory.GetFiles("*Character.png");
            foreach (var e in files)
            {
                var picture = new PictureBox();
                picture.Image = Image.FromFile(e.FullName);
                picture.Name = e.Name;
                picture.Size = new Size(100, 100);
                picture.SizeMode = PictureBoxSizeMode.Normal;
                picture.Click += StartButton_Click;
                picture.MouseEnter +=
                    (sender, args) => picture.BackColor = Color.LightSkyBlue;
                picture.MouseLeave +=
                    (sender, args) => picture.BackColor = Color.FromArgb(30, Color.Blue);
                picture.BackColor = Color.FromArgb(30, Color.Blue); ;

                pictures.Add(picture);
            }

            var offset = (ClientSize.Width - 220) / 2;
            for (int i = 0; i < 4; i++)
            {
                pictures[i].Location = new Point(offset, i % 2 == 0 ? 120 : 240);
                if (i % 2 != 0)
                    offset += 120;
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
