using System;
using System.Drawing;
using System.Windows.Forms;

namespace MetMah.Views
{
    public partial class StartControl : UserControl
    {
        private GameState game;
        private Button buttonMenu;
        private Button buttonExit;

        public void Configure(GameState game)
        {
            this.game = game;
            ClientSize = new Size(
                32 * 28,
                32 * 13 + 32);

            buttonMenu = new Button();
            buttonExit = new Button();
            buttonMenu.Size = new Size(200, 35);
            buttonMenu.BackColor = Color.LightGray;
            buttonExit.Size = new Size(200, 35);
            buttonExit.BackColor = Color.LightGray;
            BackgroundImage = Image.FromFile(@"Images\Backgrounds\Background.png");
            buttonMenu.Location = new Point((ClientSize.Width - buttonMenu.Size.Width) / 2,
                (ClientSize.Height - buttonMenu.Size.Height) / 2);
            buttonExit.Location = new Point(buttonMenu.Location.X,
                buttonMenu.Location.Y + 40);
            buttonMenu.Text = "Играть";
            buttonExit.Text = "Выйти";
            buttonMenu.Click += StartButton_Click;
            buttonExit.Click += ExitButton_Click;
            Controls.Add(buttonMenu);
            Controls.Add(buttonExit);
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
