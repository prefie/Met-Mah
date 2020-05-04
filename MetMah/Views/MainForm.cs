using MetMah.Additionally;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MetMah.Views
{
    public partial class MainForm : Form
    {
        private GameState game;
        private TableLayoutPanel table;
        private PlayControl playControl;
        private FinishedControl finishedControl;

        public MainForm()
        {
            game = new GameState();
            game.StageChanged += Game_OnStageChanged;
            ClientSizeChanged += HandleResize;

            table = new TableLayoutPanel();
            playControl = new PlayControl();
            finishedControl = new FinishedControl();

            table.ColumnCount = 3;
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, game.WidthCurrentLevel * 32));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            table.RowCount = 2;
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            table.Controls.Add(playControl, 1, 0);
            table.Controls.Add(finishedControl, 1, 1);
            table.Dock = DockStyle.Fill;
            table.Location = new Point(0, 0);
            table.Size = new Size(782, 450);
            table.TabIndex = 3;

            Controls.Add(table);

            ShowPlayScreen();
        }

        private void HandleResize(object sender, EventArgs e)
        {
            Invalidate();
        }

        protected override void OnLoad(EventArgs e)
        {
            MinimumSize = new Size(32 * game.WidthCurrentLevel + 100, 32 * game.HeightCurrentLevel + 210);
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
            playControl.Configure(game);
            playControl.Show();
        }

        private void ShowFinishedScreen()
        {
            HideScreens();
            finishedControl.Configure(game.WidthCurrentLevel, game.HeightCurrentLevel);
            finishedControl.Show();
        }

        private void HideScreens()
        {
            //startControl.Hide();
            playControl.Hide();
            finishedControl.Hide();
        }
    }
}
