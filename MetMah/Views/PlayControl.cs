using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MetMah.Creature;
using MetMah.Additionally;

namespace MetMah.Views
{
    public partial class PlayControl : UserControl
    {
        private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        private GameState game;
        private readonly HashSet<Keys> pressedKeys = new HashSet<Keys>();
        private int tickCount;
        private ProgressBar progressBar;

        public PlayControl()
        {
            progressBar = new ProgressBar();
            progressBar.Location = new Point(150, 4);
            progressBar.Size = new Size(150, 30);
            Controls.Add(progressBar);
        }

        public void Configure(GameState game)
        {
            this.game = game;
            ClientSizeChanged += HandleResize;
            PreviewKeyDown += newPreviewKeyDown;
            ClientSize = new Size(
                32 * game.WidthCurrentLevel,
                32 * game.HeightCurrentLevel + 32);

            BackgroundImage = Image.FromFile(@"Images\phon.png");
            var imagesDirectory = new DirectoryInfo("Images");
            foreach (var e in imagesDirectory.GetFiles("*.png"))
                bitmaps[e.Name] = (Bitmap)Image.FromFile(e.FullName);
            var timer = new Timer();
            timer.Interval = 20;
            timer.Tick += TimerTick;
            timer.Tick += (sender, args) =>
            {
                if (game.IsGameOver) timer.Stop();
            };
            timer.Start();
            progressBar.Maximum = this.game.WidthCurrentLevel * this.game.HeightCurrentLevel * 2 + 5;

            BackgroundImage = Image.FromFile(@"Images\phon2.png");
        }

        private void HandleResize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void newPreviewKeyDown(Object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Left ||
                e.KeyCode == Keys.Right ||
                e.KeyCode == Keys.Up ||
                e.KeyCode == Keys.Down) e.IsInputKey = true;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            pressedKeys.Add(e.KeyCode);
            game.SetKeyPressed(e.KeyCode);

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffered = true;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            pressedKeys.Remove(e.KeyCode);
            game.SetKeyPressed(pressedKeys.Any() ? pressedKeys.First() : Keys.None);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(0, 32);
            // 
            var actions = game.Actions.OrderBy(x => GetPriority(x.Creature)).ToList();

            foreach (var a in actions)
                e.Graphics.DrawImage(bitmaps[GetImageFileName(a.Creature, a.Command.DeltaX)], a.Location);
            e.Graphics.ResetTransform();
            e.Graphics.DrawString(game.PatienceScale.ToString(), new Font("Arial", 16), Brushes.Green, 0, 0);
            progressBar.Value = game.HeightCurrentLevel * game.WidthCurrentLevel * 2 - game.PatienceScale;
        }

        private Point? FindPlayer()
        {
            foreach (var e in game.Actions)
                if (e.Creature is Player)
                    return e.Location;
            return null;
        }

        private static string GetImageFileName(ICreature creature, int DeltaX)
        {
            if (creature is Player)
            {
                if (DeltaX == -1)
                    return "PlayerLeft.png";
                else
                    return "PlayerRight.png";
            }
            if (creature is Beer)
                return "Beer.png";
            if (creature is Student)
            {
                if (DeltaX == -1)
                    return "Student.Left.png";
                else
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
                else
                    return "CleverStudentRight.png";
            }
            return null;
        }

        private static int GetPriority(ICreature creature)
        {
            if (creature is Player)
                return 4;
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
            if (tickCount == 0)
            {
                game.BeginAct();
                // Стартовые размеры на формочке
                if (game.CurrentDialogue is null)
                    foreach (var e in game.Actions)
                        e.Location = new Point(e.Location.X * 32, e.Location.Y * 32);
            }
            if (game.CurrentDialogue is null)
                foreach (var e in game.Actions)
                    e.Location = new Point(e.Location.X + 4 * e.Command.DeltaX, e.Location.Y + 4 * e.Command.DeltaY);
            if (tickCount == 7)
                game.EndAct();
            tickCount++;
            if (tickCount == 8) tickCount = 0;
            Invalidate();
        }
    }
}
