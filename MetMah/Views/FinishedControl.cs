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
                32 * 28,
                32 * 13 + 32);

            buttonMenu = new Button();
            buttonExit = new Button();
            buttonMenu.Size = new Size(200, 35);
            buttonMenu.BackColor = Color.LightGray;
            buttonExit.Size = new Size(200, 35);
            buttonExit.BackColor = Color.LightGray;
            if (game.PatienceScale <= 0)
                BackgroundImage = Image.FromFile(@"Images\Backgrounds\FinishLose.png");
            else
                BackgroundImage = Image.FromFile(@"Images\Backgrounds\FinishWin.png");
            buttonMenu.Location = new Point((Size.Width - buttonMenu.Size.Width) / 2,
                (Size.Height - buttonMenu.Size.Height) / 2);
            buttonExit.Location = new Point(buttonMenu.Location.X,
                buttonMenu.Location.Y + 40);
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
