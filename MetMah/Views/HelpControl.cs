using System;
using System.Drawing;
using System.Windows.Forms;

namespace MetMah.Views
{
    public partial class HelpControl : UserControl
    {
        private Button buttonDescription;
        private Button buttonControl;
        private Button buttonCharacters;
        private Button buttonAbilities;
        private Button buttonRules1;
        private Button buttonRules2;
        private Button buttonExit;
        private Button buttonBack;

        public void Configure()
        {
            Dock = DockStyle.Fill;
            BackgroundImage = Image.FromFile(@"Resources\Images\Backgrounds\Help.png");

            var font = new Font("Arial", 16);

            buttonDescription = new Button();
            buttonRules1 = new Button();
            buttonControl = new Button();
            buttonCharacters = new Button();
            buttonAbilities = new Button();
            buttonExit = new Button();
            buttonDescription.Size = new Size(300, 45);
            buttonDescription.BackColor = Color.LightGray;
            buttonDescription.Font = font;

            buttonRules1.Size = new Size(300, 45);
            buttonRules1.BackColor = Color.LightGray;
            buttonRules1.Font = font;

            buttonControl.Size = new Size(300, 45);
            buttonControl.BackColor = Color.LightGray;
            buttonControl.Font = font;

            buttonCharacters.Size = new Size(300, 45);
            buttonCharacters.BackColor = Color.LightGray;
            buttonCharacters.Font = font;

            buttonAbilities.Size = new Size(300, 45);
            buttonAbilities.BackColor = Color.LightGray;
            buttonAbilities.Font = font;

            buttonExit.Size = new Size(300, 45);
            buttonExit.BackColor = Color.LightGray;
            buttonExit.Font = font;


            buttonDescription.Location = new Point((ClientSize.Width - buttonDescription.Size.Width) / 2,
                (ClientSize.Height - buttonDescription.Size.Height) / 2 - 100);
            buttonRules1.Location = new Point(buttonDescription.Location.X,
                buttonDescription.Location.Y + 50);
            buttonControl.Location = new Point(buttonRules1.Location.X,
                buttonRules1.Location.Y + 50);
            buttonCharacters.Location = new Point(buttonControl.Location.X,
                buttonControl.Location.Y + 50);
            buttonAbilities.Location = new Point(buttonCharacters.Location.X,
                buttonCharacters.Location.Y + 50);
            buttonExit.Location = new Point(buttonAbilities.Location.X,
                buttonAbilities.Location.Y + 50);

            buttonDescription.Text = "Описание игры";
            buttonRules1.Text = "Правила";
            buttonControl.Text = "Управление";
            buttonCharacters.Text = "О персонажах";
            buttonAbilities.Text = "О способностях";
            buttonExit.Text = "Назад";

            buttonDescription.Click += ButtonDescription_Click;
            buttonRules1.Click += ButtonRules1_Click;
            buttonControl.Click += ButtonControl_Click;
            buttonCharacters.Click += ButtonCharacters_Click;
            buttonAbilities.Click += ButtonAbilities_Click;
            buttonExit.Click += ButtonExit_Click;

            Controls.Add(buttonDescription);
            Controls.Add(buttonRules1);
            Controls.Add(buttonControl);
            Controls.Add(buttonCharacters);
            Controls.Add(buttonAbilities);
            Controls.Add(buttonExit);

            buttonBack = new Button
            {
                Size = new Size(300, 45),
                BackColor = Color.LightGray,
                Font = new Font("Arial", 16),
                Text = "Назад"
            };

            buttonRules2 = new Button
            {
                Size = new Size(300, 45),
                BackColor = Color.LightGray,
                Font = new Font("Arial", 16),
                Text = "Далее"
            };

            ActiveControl = Controls[0];
        }

        private void ButtonDescription_Click(object sender, EventArgs e)
        {
            HideControls();
            BackgroundImage = Image.FromFile(@"Resources\Images\Backgrounds\Description.png");
            buttonBack.Location = new Point((ClientSize.Width - buttonDescription.Size.Width) / 2,
                ClientSize.Height - 100);
            Controls.Add(buttonBack);
            buttonBack.Click += Return;
        }

        private void ButtonRules1_Click(object sender, EventArgs e)
        {
            HideControls();
            BackgroundImage = Image.FromFile(@"Resources\Images\Backgrounds\Rules1.png");
            buttonBack.Location = new Point((ClientSize.Width - buttonDescription.Size.Width) / 2,
                ClientSize.Height - 100);
            buttonRules2.Location = new Point(buttonBack.Location.X,
                buttonBack.Location.Y - 50);
            Controls.Add(buttonRules2);
            Controls.Add(buttonBack);
            buttonRules2.Click += ButtonRules2_Click;
            buttonBack.Click += Return;
        }

        private void ButtonRules2_Click(object sender, EventArgs e)
        {
            HideControls();
            Controls.Remove(buttonRules2);
            BackgroundImage = Image.FromFile(@"Resources\Images\Backgrounds\Rules2.png");
            buttonBack.Location = new Point((ClientSize.Width - buttonDescription.Size.Width) / 2,
                ClientSize.Height - 100);
            Controls.Add(buttonBack);
            buttonBack.Click += Return;
        }

        private void ButtonControl_Click(object sender, EventArgs e)
        {
            HideControls();
            BackgroundImage = Image.FromFile(@"Resources\Images\Backgrounds\Control.png");
            buttonBack.Location = new Point((ClientSize.Width - buttonControl.Size.Width) / 2,
                ClientSize.Height - 100);
            Controls.Add(buttonBack);
            buttonBack.Click += Return;
        }

        private void ButtonCharacters_Click(object sender, EventArgs e)
        {
            HideControls();
            BackgroundImage = Image.FromFile(@"Resources\Images\Backgrounds\Characters.png");
            buttonBack.Location = new Point((ClientSize.Width - buttonDescription.Size.Width) / 2,
                ClientSize.Height - 100);
            Controls.Add(buttonBack);
            buttonBack.Click += Return;
        }

        private void ButtonAbilities_Click(object sender, EventArgs e)
        {
            HideControls();
            BackgroundImage = Image.FromFile(@"Resources\Images\Backgrounds\Abilities.png");
            buttonBack.Location = new Point((ClientSize.Width - buttonDescription.Size.Width) / 2,
                ClientSize.Height - 100);
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
            BackgroundImage = Image.FromFile(@"Resources\Images\Backgrounds\Help.png");
            buttonDescription.Show();
            buttonRules1.Show();
            buttonControl.Show();
            buttonCharacters.Show();
            buttonAbilities.Show();
            buttonExit.Show();
            Controls.Remove(buttonBack);
            Controls.Remove(buttonRules2);
        }

        private void HideControls()
        {
            buttonDescription.Hide();
            buttonRules1.Hide();
            buttonControl.Hide();
            buttonCharacters.Hide();
            buttonAbilities.Hide();
            buttonExit.Hide();
        }
    }
}
