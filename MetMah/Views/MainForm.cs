using MetMah.Additionally;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MetMah.Views
{
    public partial class MainForm : Form
    {
        private readonly GameState game;
        private readonly TableLayoutPanel table;
        private readonly PlayControl playControl;
        private StartControl startControl;
        private FinishedControl finishedControl;
        private ChoiceCharacterControl choiceControl;
        private DialogueControl dialogueControl;


        public MainForm()
        {
            StartPosition = FormStartPosition.Manual;
            Location = new Point(5, 5);

            game = new GameState();
            game.StageChanged += Game_OnStageChanged;

            table = new TableLayoutPanel();
            startControl = new StartControl();
            playControl = new PlayControl(game);
            finishedControl = new FinishedControl();
            dialogueControl = new DialogueControl();
            choiceControl = new ChoiceCharacterControl();

            table.ColumnCount = 3;
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, game.WidthCurrentLevel * 50));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            table.RowCount = 2;
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, game.HeightCurrentLevel * 50 + 50));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));

            table.Controls.Add(playControl, 1, 0);
            table.Dock = DockStyle.Fill;

            ClientSizeChanged += HandleResize;

            Controls.Add(table);
            ShowStartScreen();
        }

        protected override void OnLoad(EventArgs e)
        {
            MinimumSize = new Size(50 * game.WidthCurrentLevel, 50 * game.HeightCurrentLevel + 210);
            base.OnLoad(e);
            Text = "Escape from MetMah";
            DoubleBuffered = true;
        }

        private void HandleResize(object sender, EventArgs e)
        {
            HideScreens();

            switch(game.Stage)
            {
                case GameStage.NotStarted:
                    startControl.Show();
                    break;
                case GameStage.ChoiceCharacter:
                    choiceControl.Show();
                    break;
                case GameStage.Play:
                    playControl.Show();
                    break;
                case GameStage.ActivatedDialogue:
                    playControl.Show();
                    dialogueControl.Show();
                    break;
                case GameStage.Finished:
                    finishedControl.Show();
                    break;
            }
        }

        private void Game_OnStageChanged(GameStage stage)
        {
            switch (stage)
            {
                case GameStage.ChoiceCharacter:
                    ShowChoiceCharacterScreen();
                    break;
                case GameStage.Play:
                    ShowPlayScreen();
                    break;
                case GameStage.Finished:
                    ShowFinishedScreen();
                    break;
                case GameStage.ActivatedDialogue:
                    ShowDialogueScreen();
                    break;
                case GameStage.NotStarted:
                default:
                    ShowStartScreen();
                    break;
            }
        }

        private void ShowStartScreen()
        {
            HideScreens();
            table.Controls.Remove(startControl);
            startControl = new StartControl();
            table.Controls.Add(startControl, 1, 0);
            startControl.Configure(game);
            startControl.Show();
        }

        private void ShowChoiceCharacterScreen()
        {
            HideScreens();
            table.Controls.Remove(choiceControl);
            choiceControl = new ChoiceCharacterControl();
            table.Controls.Add(choiceControl, 1, 0);
            choiceControl.Configure(game);
            choiceControl.Show();
        }

        private void ShowPlayScreen()
        {
            HideScreens();
            playControl.Configure(choiceControl.PlayerName);
            playControl.Show();
        }

        private void ShowDialogueScreen()
        {
            table.Controls.Remove(dialogueControl);
            dialogueControl = new DialogueControl();
            table.Controls.Add(dialogueControl, 1, 1);
            dialogueControl.Configure(game);
            dialogueControl.Show();
        }

        private void ShowFinishedScreen()
        {
            HideScreens();
            table.Controls.Remove(finishedControl);
            finishedControl = new FinishedControl();
            table.Controls.Add(finishedControl, 1, 0);
            finishedControl.Configure(game);
            finishedControl.Show();
        }

        private void HideScreens()
        {
            startControl.Hide();
            choiceControl.Hide();
            playControl.Hide();
            dialogueControl.Hide();
            finishedControl.Hide();
        }
    }
}
