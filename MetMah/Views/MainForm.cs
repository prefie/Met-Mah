using MetMah.Additionally;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MetMah.Views
{
    public partial class MainForm : Form
    {
        private GameState game;

        public MainForm()
        {
            game = new GameState();
            InitializeComponent();
            game.StageChanged += Game_OnStageChanged;
            ClientSizeChanged += HandleResize;
            ShowPlayScreen();
        }

        private void HandleResize(object sender, EventArgs e)
        {
            Invalidate();
        }

        protected override void OnLoad(EventArgs e)
        {
            MinimumSize = new Size(32 * game.WidthCurrentLevel, 32 * game.HeightCurrentLevel + 210);
            WindowState = FormWindowState.Maximized;
            base.OnLoad(e);
            Text = "Escape from MetMah";
            DoubleBuffered = true;
        }

        private void Game_OnStageChanged(GameStage stage)
        {
            switch (stage)
            {
                case GameStage.Play:
                    ShowPlayScreen();
                    break;
                case GameStage.Finished:
                    ShowFinishedScreen();
                    break;
                case GameStage.NotStarted:
                default:
                    //ShowStartScreen();
                    break;
            }
        }

        //private void ShowStartScreen()
        //{
        //    HideScreens();
        //    startControl.Configure(game);
        //    startControl.Show();
        //}

        private void ShowPlayScreen()
        {
            HideScreens();
            playControl1.Configure(game);
            playControl1.Show();
        }

        private void ShowFinishedScreen()
        {
            HideScreens();
            finishedControl1.Configure(game.WidthCurrentLevel, game.HeightCurrentLevel);
            finishedControl1.Show();
        }

        private void HideScreens()
        {
            //startControl.Hide();
            playControl1.Hide();
            finishedControl1.Hide();
        }
    }
}
