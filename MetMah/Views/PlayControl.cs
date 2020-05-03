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
        private readonly Dictionary<Keys, Keys> keys = new Dictionary<Keys, Keys>();
        private GameState gameState;
        private readonly HashSet<Keys> pressedKeys = new HashSet<Keys>();
        private int tickCount;

        public PlayControl()
        {
            InitializeComponent();
        }

        public void Configure(GameState game)
        {
            gameState = game;
            ClientSizeChanged += HandleResize;
            ClientSize = new Size(
                32 * game.WidthCurrentLevel,
                32 * game.HeightCurrentLevel + 32);

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
            keys.Add(Keys.W, Keys.Up);
            keys.Add(Keys.S, Keys.Down);
            keys.Add(Keys.D, Keys.Right);
            keys.Add(Keys.A, Keys.Left);
            progressBar1.Maximum = gameState.WidthCurrentLevel * gameState.HeightCurrentLevel * 2 + 5;
        }

        private void HandleResize(object sender, EventArgs e)
        {
            Invalidate();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (keys.ContainsKey(e.KeyCode))
            {
                pressedKeys.Add(keys[e.KeyCode]);
                gameState.SetKeyPressed(keys[e.KeyCode]);
            }
            else
            {
                pressedKeys.Add(e.KeyCode);
                gameState.SetKeyPressed(e.KeyCode);
            }

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffered = true;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (keys.ContainsKey(e.KeyCode))
                pressedKeys.Remove(keys[e.KeyCode]);
            else
                pressedKeys.Remove(e.KeyCode);
            gameState.SetKeyPressed(pressedKeys.Any() ? pressedKeys.Min() : Keys.None);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.TranslateTransform(0, ClientSize.Width / gameState.WidthCurrentLevel);
            e.Graphics.FillRectangle(
                Brushes.White, 0, 0, ClientSize.Width,
                ClientSize.Width / gameState.WidthCurrentLevel * gameState.HeightCurrentLevel);
            // 
            var actions = gameState.Actions.OrderBy(x => GetPriority(x.Creature)).ToList();

            foreach (var a in actions)
                e.Graphics.DrawImage(bitmaps[GetImageFileName(a.Creature, a.Command.DeltaX)], a.Location);
            e.Graphics.ResetTransform();
            e.Graphics.DrawString(gameState.PatienceScale.ToString(), new Font("Arial", 16), Brushes.Green, 0, 0);
            progressBar1.Value = gameState.HeightCurrentLevel * gameState.WidthCurrentLevel * 2 - gameState.PatienceScale;
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
                    return "Student.png";
                else
                    return "Student.png";
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
                    return "CleverStudent.png";
                else
                    return "CleverStudent.png";
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
                gameState.BeginAct();
                // Стартовые размеры на формочке
                if (gameState.CurrentDialogue is null)
                    foreach (var e in gameState.Actions)
                        e.Location = new Point(e.Location.X * 32, e.Location.Y * 32);
            }
            if (gameState.CurrentDialogue is null)
                foreach (var e in gameState.Actions)
                    e.Location = new Point(e.Location.X + 4 * e.Command.DeltaX, e.Location.Y + 4 * e.Command.DeltaY);
            if (tickCount == 7)
                gameState.EndAct();
            tickCount++;
            if (tickCount == 8) tickCount = 0;
            Invalidate();
        }
    }
}
