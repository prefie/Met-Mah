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
        private readonly PlayControl playControl;
        private StartControl startControl;
        private FinishedControl finishedControl;
        private ChoiceCharacterControl choiceControl;
        private DialogueControl dialogueControl;


        public MainForm()
        {
            StartPosition = FormStartPosition.Manual;
            Location = new Point(5, 5);

            MinimizeBox = false;

            game = new GameState();
            game.StageChanged += Game_OnStageChanged;
            startControl = new StartControl();
            playControl = new PlayControl(game);
            finishedControl = new FinishedControl();
            dialogueControl = new DialogueControl();
            choiceControl = new ChoiceCharacterControl();
            Controls.Add(playControl);

            ClientSizeChanged += HandleResize;
            ShowStartScreen();
        }

        protected override void OnLoad(EventArgs e)
        {
            MinimumSize = new Size(50 * 28 + 15, 50 * 14 + 39);
            MaximumSize = new Size(50 * 28 + 15, 50 * 14 + 39);
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
            Controls.Remove(startControl);
            startControl = new StartControl();
            Controls.Add(startControl);
            startControl.Configure(game);
            startControl.Show();
        }

        private void ShowChoiceCharacterScreen()
        {
            HideScreens();
            Controls.Remove(choiceControl);
            choiceControl = new ChoiceCharacterControl();
            Controls.Add(choiceControl);
            choiceControl.Configure(game);
            choiceControl.Show();
        }

        private void ShowPlayScreen()
        {
            HideScreens();
            playControl.Configure(choiceControl.PlayerName);
            playControl.Focus();
            playControl.Show();
        }

        private void ShowDialogueScreen()
        {
            playControl.Controls.Remove(dialogueControl);
            dialogueControl = new DialogueControl();
            playControl.Controls.Add(dialogueControl);
            dialogueControl.Configure(game);
            dialogueControl.Show();
        }

        private void ShowFinishedScreen()
        {
            HideScreens();
            Controls.Remove(finishedControl);
            finishedControl = new FinishedControl();
            Controls.Add(finishedControl);
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
