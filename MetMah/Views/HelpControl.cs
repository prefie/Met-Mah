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
    public partial class HelpControl : UserControl
    {
        private Button buttonDescription;
        private Button buttonControl;
        private Button buttonCharacters;
        private Button buttonExit;
        private Button buttonBack;

        public void Configure()
        {
            Dock = DockStyle.Fill;
            BackgroundImage = Image.FromFile(@"Images\Backgrounds\Help.png");

            buttonDescription = new Button();
            buttonControl = new Button();
            buttonCharacters = new Button();
            buttonExit = new Button();
            buttonDescription.Size = new Size(200, 35);
            buttonDescription.BackColor = Color.LightGray;
            buttonControl.Size = new Size(200, 35);
            buttonControl.BackColor = Color.LightGray;
            buttonCharacters.Size = new Size(200, 35);
            buttonCharacters.BackColor = Color.LightGray;
            buttonExit.Size = new Size(200, 35);
            buttonExit.BackColor = Color.LightGray;


            buttonDescription.Location = new Point((ClientSize.Width - buttonDescription.Size.Width) / 2,
                (ClientSize.Height - buttonDescription.Size.Height) / 2 - 40);
            buttonControl.Location = new Point(buttonDescription.Location.X,
                buttonDescription.Location.Y + 40);
            buttonCharacters.Location = new Point(buttonControl.Location.X,
                buttonControl.Location.Y + 40);
            buttonExit.Location = new Point(buttonCharacters.Location.X,
                buttonCharacters.Location.Y + 40);
            buttonDescription.Text = "Описание игры";
            buttonControl.Text = "Управление";
            buttonCharacters.Text = "О персонажах";
            buttonExit.Text = "Назад";

            buttonDescription.Click += ButtonDescription_Click;
            buttonControl.Click += ButtonControl_Click;
            buttonCharacters.Click += ButtonCharacters_Click;
            buttonExit.Click += ButtonExit_Click;

            Controls.Add(buttonDescription);
            Controls.Add(buttonControl);
            Controls.Add(buttonCharacters);
            Controls.Add(buttonExit);

            buttonBack = new Button();
            buttonBack.Size = new Size(200, 35);
            buttonBack.BackColor = Color.LightGray;
            buttonBack.Text = "Назад";

            ActiveControl = Controls[0];
        }

        private void ButtonDescription_Click(object sender, EventArgs e)
        {
            HideControls();
            BackgroundImage = Image.FromFile(@"Images\Backgrounds\Description.png");
            buttonBack.Location = new Point((ClientSize.Width - buttonDescription.Size.Width) / 2,
                ClientSize.Height - 100);
            Controls.Add(buttonBack);
            buttonBack.Click += Return;
        }

        private void ButtonControl_Click(object sender, EventArgs e)
        {
            HideControls();
            BackgroundImage = Image.FromFile(@"Images\Backgrounds\Control.png");
            buttonBack.Location = new Point((ClientSize.Width - buttonControl.Size.Width) / 2,
                ClientSize.Height - 100);
            Controls.Add(buttonBack);
            buttonBack.Click += Return;
        }

        private void ButtonCharacters_Click(object sender, EventArgs e)
        {
            HideControls();
            BackgroundImage = Image.FromFile(@"Images\Backgrounds\Characters.png");
            buttonBack.Location = new Point((ClientSize.Width - buttonDescription.Size.Width) / 2,
                ClientSize.Height - 60);
            Controls.Add(buttonBack);
            buttonBack.Click += Return;
        }

        private void ButtonExit_Click(object sender, EventArgs e)
        {
            Hide();
            Dispose();
        }

        private void Return(object sender, EventArgs e)
        {
            BackgroundImage = Image.FromFile(@"Images\Backgrounds\Help.png");
            buttonDescription.Show();
            buttonControl.Show();
            buttonCharacters.Show();
            buttonExit.Show();
            Controls.Remove(buttonBack);
        }

        private void HideControls()
        {
            buttonDescription.Hide();
            buttonControl.Hide();
            buttonCharacters.Hide();
            buttonExit.Hide();
        }
    }
}
