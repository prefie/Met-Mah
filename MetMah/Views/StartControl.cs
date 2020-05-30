using System;
using System.Drawing;
using System.Windows.Forms;

namespace MetMah.Views
{
    public partial class StartControl : UserControl
    {
        private GameState game;
        private Button buttonPlay;
        private Button buttonExit;
        private PictureBox picture;
        private HelpControl helpControl;

        public void Configure(GameState game)
        {
            this.game = game;
            ClientSize = new Size(
                50 * 28,
                700);

            var font = new Font("Arial", 16);

            buttonPlay = new Button();
            buttonExit = new Button();
            buttonPlay.Size = new Size(300, 45);
            buttonPlay.BackColor = Color.LightGray;
            buttonPlay.Font = font;
            buttonExit.Size = new Size(300, 45);
            buttonExit.BackColor = Color.LightGray;
            buttonExit.Font = font;

            BackgroundImage = Image.FromFile(@"Images\Backgrounds\Background.png");

            buttonPlay.Location = new Point((ClientSize.Width - buttonPlay.Size.Width) / 2,
                (ClientSize.Height - buttonPlay.Size.Height) / 2 + 30);
            buttonExit.Location = new Point(buttonPlay.Location.X,
                buttonPlay.Location.Y + 50);

            buttonPlay.Text = "Играть";
            buttonExit.Text = "Выйти";

            buttonPlay.Click += StartButton_Click;
            buttonExit.Click += ExitButton_Click;

            picture = new PictureBox
            {
                Location = new Point(buttonExit.Location.X - 60, buttonExit.Location.Y),
                Image = Image.FromFile(@"Images\Backgrounds\Question1.png"),
                Size = new Size(45, 45),
                SizeMode = PictureBoxSizeMode.Normal
            };

            picture.Click += PictureBox_Click;

            picture.MouseEnter +=
                (sender, args) => picture.Image = Image.FromFile(@"Images\Backgrounds\Question2.png");
            picture.MouseLeave +=
                (sender, args) => picture.Image = Image.FromFile(@"Images\Backgrounds\Question1.png");
            BackColor = Color.Transparent;

            Controls.Add(buttonPlay);
            Controls.Add(buttonExit);
            Controls.Add(picture);

            ActiveControl = Controls[0];
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Focus();
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            buttonExit.Hide();
            buttonPlay.Hide();
            picture.Hide();

            helpControl = new HelpControl();
            Controls.Add(helpControl);
            helpControl.Configure();
            helpControl.Disposed += (s, args) =>
            {
                buttonExit.Show();
                buttonPlay.Show();
                picture.Show();
            };
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
