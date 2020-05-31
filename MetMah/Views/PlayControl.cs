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
                Location = new Point(1100, 10),
                Size = new Size(250, 25),
                Style = ProgressBarStyle.Continuous
            };
            Controls.Add(progressBar);

            PreviewKeyDown += NewPreviewKeyDown;
            BackgroundImage = Image.FromFile(@"Images\Backgrounds\LightBackground.png");
            BackgroundImageLayout = ImageLayout.Center;
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

            var font = new Font("Arial", 24);
            var brushes = Brushes.Black;

            //Level
            e.Graphics.DrawString("Level: " + (game.IndexCurrentLevel + 1) + " / " + 6, font, brushes, 20, 5);

            //Ability
            Bitmap bitmap = null;
            if (game.NumberPlayer == 0)
                bitmap = bitmaps["SnakeMini.png"];

            if (game.NumberPlayer == 1)
                bitmap = bitmaps["IgnorMini.png"];

            if (game.NumberPlayer == 3)
                bitmap = bitmaps["DeviceMini.png"];

            if (bitmap != null)
            {
                e.Graphics.DrawImage(bitmap, 250, 5);

                e.Graphics.DrawString("x " + game.CurrentLevel.NumberBeer.ToString(), font,
                    brushes, 290, 5);
            }

            //Beer
            e.Graphics.DrawString("Осталось собрать: ", font,
                brushes, 400, 5);
            e.Graphics.DrawString(game.CurrentLevel.CountBeer.ToString(), font,
                brushes, 690, 5);

            //Scale
            var scale = game.PatienceScale > 0 ? game.PatienceScale.ToString() : "0";
            e.Graphics.DrawString(scale, font, brushes, 1030, 4);
            e.Graphics.DrawString("Шкала терпения:", font, brushes, 770, 5);

            progressBar.Value = game.PatienceScale > 0 ?
                game.HeightCurrentLevel * game.WidthCurrentLevel - game.PatienceScale : 0;
        }

        private string GetImageFileName(ICreature creature, int DeltaX)
        {
            if (creature is Player)
            {
                var numberPlayer = namePlayer[0];
                if (numberPlayer == '1' && creature.GetStatus() == Status.Inactive)
                {
                    if (DeltaX == -1)
                        return "1PlayerLeftInactive.png";
                    return "1PlayerRightInactive.png";
                }

                if (DeltaX == -1)
                    return numberPlayer + "PlayerLeft.png";
                return numberPlayer + "PlayerRight.png";
            }

            if (creature is Beer)
                return "Beer.png";

            if (creature is Student)
            {
                if (creature.GetStatus() == Status.Inactive)
                    return "StudentInactive.png";
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
                if (creature.GetStatus() == Status.Inactive)
                    return "StudentInactive.png";
                if (DeltaX == -1)
                    return "CleverStudentLeft.png";
                return "CleverStudentRight.png";
            }

            if (creature is Door)
            {
                if (creature.GetStatus() == Status.Active)
                    return "DoorActive.png";
                else
                    return "DoorInactive.png";
            }

            if (creature is Device)
                return "Device.png";

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

            if (creature is Door)
                return 5;

            if (creature is Device)
                return 5;

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
