using System;
using System.Drawing;
using System.Windows.Forms;

namespace MetMah.Views
{
    public partial class FinishedControl : UserControl
    {
        private Button buttonMenu;
        private Button buttonExit;

        public void Configure(GameState game)
        {
            ClientSize = new Size(
                50 * 28,
                50 * 14);

            buttonMenu = new Button();
            buttonExit = new Button();

            var font = new Font("Arial", 16);

            buttonMenu.Size = new Size(300, 45);
            buttonMenu.BackColor = Color.LightGray;
            buttonMenu.Font = font;
            buttonExit.Size = new Size(300, 45);
            buttonExit.BackColor = Color.LightGray;
            buttonExit.Font = font;

            if (game.PatienceScale <= 0)
                BackgroundImage = Image.FromFile(@"Images\Backgrounds\FinishLose.png");
            else
                BackgroundImage = Image.FromFile(@"Images\Backgrounds\FinishWin.png");

            buttonMenu.Location = new Point((Size.Width - buttonMenu.Size.Width) / 2,
                (Size.Height - buttonMenu.Size.Height) / 2 + 30);
            buttonExit.Location = new Point(buttonMenu.Location.X,
                buttonMenu.Location.Y + 50);

            buttonMenu.Text = "В главное меню";
            buttonExit.Text = "Выйти";

            buttonMenu.Click += (sender, e) => game.Initialize();
            buttonExit.Click += ExitButton_Click;

            Controls.Add(buttonMenu);
            Controls.Add(buttonExit);
        }

        protected override void OnLoad(EventArgs e)
        {
            Focus();
            base.OnLoad(e);
            DoubleBuffered = true;
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
