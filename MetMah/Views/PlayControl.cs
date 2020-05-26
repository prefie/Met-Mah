using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using MetMah.Creature;
using MetMah.Additionally;

namespace MetMah.Views
{
    public partial class PlayControl : UserControl
    {
        private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        private readonly GameState game;
        private readonly HashSet<Keys> pressedKeys;
        private int tickCount;
        private readonly ProgressBar progressBar;
        private readonly Timer timer;
        private string namePlayer;
        private PauseControl pauseControl;

        public PlayControl(GameState game)
        {
            this.game = game;
            pressedKeys = new HashSet<Keys>();
            progressBar = new ProgressBar
            {
                Location = new Point(200, 6),
                Size = new Size(150, 20),
                Style = ProgressBarStyle.Continuous
            };
            Controls.Add(progressBar);

            PreviewKeyDown += NewPreviewKeyDown;

            BackgroundImage = Image.FromFile(@"Images\Backgrounds\LightBackground.png");
            var imagesDirectory = new DirectoryInfo(@"Images\Creatures");
            foreach (var e in imagesDirectory.GetFiles("*.png"))
                bitmaps[e.Name] = (Bitmap)Image.FromFile(e.FullName);

            ClientSize = new Size(
                50 * 28,
                50 * 13 + 50);

            timer = new Timer { Interval = 20 };
            timer.Tick += TimerTick;
            timer.Tick += (sender, args) =>
            {
                if (game.Stage == GameStage.Finished)
                    timer.Stop();
            };
        }

        public void Configure(string playerName)
        {
            namePlayer = playerName;
            timer.Start();
        }

        private void NewPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Left ||
                    e.KeyCode == Keys.Right ||
                    e.KeyCode == Keys.Up ||
                    e.KeyCode == Keys.Down)
                e.IsInputKey = true;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                timer.Stop();
                tickCount = 0;
                pauseControl = new PauseControl();
                Controls.Add(pauseControl);
                pauseControl.Configure(game, timer);
                pauseControl.Show();
            }
            pressedKeys.Add(e.KeyCode);
            game.SetKeyPressed(e.KeyCode);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Focus();
            DoubleBuffered = true;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            pressedKeys.Remove(e.KeyCode);
            game.SetKeyPressed(pressedKeys.Any() ? pressedKeys.Min() : Keys.None);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImage(bitmaps["Stone.png"], 0, 0);

            e.Graphics.TranslateTransform(0, 50);
            var actions = game.Actions.OrderBy(x => GetPriority(x.Creature)).ToArray();
            foreach (var a in actions)
                e.Graphics.DrawImage(bitmaps[GetImageFileName(a.Creature, a.Command.DeltaX)], a.Location);
            e.Graphics.ResetTransform();

            var scale = game.PatienceScale > 0 ? game.PatienceScale.ToString() : "0";
            e.Graphics.DrawString(scale, new Font("Arial", 16), Brushes.Green, 150, 4);
            e.Graphics.DrawString("Шкала терпения:", new Font("Arial", 12), Brushes.Green, 20, 6);
            progressBar.Value = game.PatienceScale > 0 ?
                game.HeightCurrentLevel * game.WidthCurrentLevel - game.PatienceScale : 0;

            if (game.Stage == GameStage.ActivatedDialogue)
            {
                e.Graphics.FillRectangle(Brushes.LightGray, 0, 32, 32 * Size.Width, 32 * Size.Height + 32);
                var message = "Вы наткнулись на студента!";
                e.Graphics.DrawString(message, new Font("Arial", 48),
                    Brushes.Green, Size.Width / 2 - message.Length * 16, Size.Height / 2 - 32);
                e.Graphics.DrawString("Придётся отвечать на его вопрос ↓", new Font("Arial", 24),
                    Brushes.Green, Size.Width / 2 - message.Length * 16, Size.Height / 2 + 40);
            }
        }

        private string GetImageFileName(ICreature creature, int DeltaX)
        {
            if (creature is Player)
            {
                if (DeltaX == -1)
                    return namePlayer[0] + "PlayerLeft.png";
                return namePlayer[0] + "PlayerRight.png";
            }

            if (creature is Beer)
                return "Beer.png";

            if (creature is Student)
            {
                if (DeltaX == -1)
                    return "Student.Left.png";
                return "Student.Right.png";
            }

            if (creature is Stairs)
                return "Stairs.png";

            if (creature is Terrain)
                return "Terrain.png";

            if (creature is Python)
                return "Snake.png";

            if (creature is CleverStudent)
            {
                if (DeltaX == -1)
                    return "CleverStudentLeft.png";
                return "CleverStudentRight.png";
            }

            return null;
        }

        private static int GetPriority(ICreature creature)
        {
            if (creature is Player)
                return 6;

            if (creature is Beer)
                return 1;

            if (creature is Student)
                return 2;

            if (creature is Stairs)
                return 0;

            if (creature is Terrain)
                return 0;

            if (creature is Python)
                return 5;

            if (creature is CleverStudent)
                return 2;

            return 0;
        }

        private void TimerTick(object sender, EventArgs args)
        {
            if (!game.IsDialogueActivated)
                Focus();
            progressBar.Maximum = game.WidthCurrentLevel * game.HeightCurrentLevel + 1;
            if (tickCount == 0)
            {
                game.BeginAct();
                if (!game.IsDialogueActivated)
                    foreach (var e in game.Actions)
                        e.Location = new Point(e.Location.X * 50, e.Location.Y * 50);
            }

            if (!game.IsDialogueActivated)
                foreach (var e in game.Actions)
                    e.Location = new Point(e.Location.X + 6 * e.Command.DeltaX, e.Location.Y + 6 * e.Command.DeltaY);

            if (tickCount == 7)
                game.EndAct();

            tickCount++;

            if (tickCount == 8) tickCount = 0;
            Invalidate();
        }

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
