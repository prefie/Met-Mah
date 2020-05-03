using MetMah;
using MetMah.Additionally;
using MetMah.Creature;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EscapeFromMetMah
{
    public partial class Form1 : Form
    {
        private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        private readonly GameState gameState;
        private readonly HashSet<Keys> pressedKeys = new HashSet<Keys>();
        private int tickCount;


        public Form1(DirectoryInfo imagesDirectory = null)
        {
            var levels = new List<Level>();
            string str = @"
           TTTTT  SB   TT
P             L LTTTTTLTT
TTTLTTTTL  TTTLTT     L  
   L S  LLB   L       L  
TTTTTTTTLTTTTTTTTTTTTLL  
        L            TTTT
        L S   C   C    B 
 B   LTTTTTTTTTTTTTTTTTTT";
            string str2 = @"
P                     CB 
TTTTTTTTTTTTTTTTTTTTTTTT 
 BC                      
 TTTTTTTTTTTTTTTTTTTTTTTT
                       CB
TTTTTTTTTTTTTTTTTTTTTTTTT";
            var str1 = @"
P     S   S  B   
TTTTTTTTTTTTTTTTT";
            var str3 = @"
     S S  B
TTTTTTTTTTT";
            var level = new Level(str);
            var level1 = new Level(str1);
            var level2 = new Level(str2);
            var level3 = new Level(str3);
            levels.Add(level1);
            levels.Add(level);


            gameState = new GameState(levels);
            ClientSize = new Size(
                32 * gameState.WidthCurrentLevel,
                32 * gameState.HeightCurrentLevel + 32);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            imagesDirectory = new DirectoryInfo("Images");
            foreach (var e in imagesDirectory.GetFiles("*.png"))
                bitmaps[e.Name] = (Bitmap)Image.FromFile(e.FullName);
            var timer = new Timer();
            timer.Interval = 20;
            timer.Tick += TimerTick;
            timer.Tick += (sender, args) =>
            {
                if (gameState.IsGameOver) timer.Stop();
            };
            timer.Start();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text = "EscapeFromMetMah";
            DoubleBuffered = true;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            pressedKeys.Add(e.KeyCode);
            gameState.SetKeyPressed(e.KeyCode);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            pressedKeys.Remove(e.KeyCode);
            gameState.SetKeyPressed(pressedKeys.Any() ? pressedKeys.Min() : Keys.None);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (gameState.IsGameOver)
            {
                e.Graphics.DrawString("YOU WIN", new Font("Arial", 64), Brushes.Green, 0, 0);
                return;
            }

            e.Graphics.TranslateTransform(0, 32);
            e.Graphics.FillRectangle(
                Brushes.White, 0, 0, 32 * gameState.WidthCurrentLevel,
                32 * gameState.HeightCurrentLevel);
            // 
            var actions = gameState.Actions.OrderBy(x => GetPriority(x.Creature)).ToList();

            foreach (var a in actions)
                e.Graphics.DrawImage(bitmaps[GetImageFileName(a.Creature)], a.Location);
            e.Graphics.ResetTransform();
            e.Graphics.DrawString(gameState.PatienceScale.ToString(), new Font("Arial", 16), Brushes.Green, 0, 0);

        }

        private static string GetImageFileName(ICreature creature)
        {
            if (creature is Player)
                return "Player.png";
            if (creature is Beer)
                return "Beer.png";
            if (creature is Student)
                return "Student.png";
            if (creature is Stairs)
                return "Stairs.png";
            if (creature is Terrain)
                return "Terrain.png";
            if (creature is Python)
                return "Snake.png";
            if (creature is CleverStudent)
                return "Student.png";
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
                {
                    var d1 = e.Command.DeltaX;
                    ICreature previous = null;
                    if (e.Creature is Student && tickCount == 0)
                    {
                        if (e.Creature == previous)
                        {

                        }
                        previous = e.Creature;
                    }
                    e.Location = new Point(e.Location.X + 4 * e.Command.DeltaX, e.Location.Y + 4 * e.Command.DeltaY);
                }
            if (tickCount == 7)
                gameState.EndAct();
            tickCount++;
            if (tickCount == 8) tickCount = 0;
            Invalidate();
        }
    }
}