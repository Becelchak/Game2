using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameModel;

namespace Game
{
    public partial class TestMission : Form
    {
        private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        private readonly GameState gameState;
        private readonly HashSet<Keys> pressedKeys = new HashSet<Keys>();
        private bool left;
        private bool right;
        private int tickCount;
        public TestMission(DirectoryInfo imagesDirectory = null)
        {
            InitializeComponent();
            gameState = new GameState();
            ClientSize = new Size(
                GameState.ElementSize * Game_Map.MapWidth,
                GameState.ElementSize * Game_Map.MapHeight + GameState.ElementSize);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            BackgroundImage = Image.FromFile("C:\\Users\\kost4\\source\\repos\\Rep\\Game\\Resources\\Floor.jpg");
            var timer = new Timer();
            timer.Interval = 15;
            timer.Tick += TimerTick;
            timer.Start();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(0, GameState.ElementSize);
            foreach (var a in gameState.Animations)
                e.Graphics.DrawImage(Image.FromFile(a.Creature.GetPathForImage()), a.Location);
            e.Graphics.ResetTransform();
            //e.Graphics.DrawString(Game_Map.HealPoint.ToString(), new Font("Arial", 18), Brushes.Red, 1, 1);
        }

        //private void MovePlayer()
        //{
        //    var control = left ? Turn.Left : (right ? Turn.Right : Turn.None);
        //    ;
        //}
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text = "TestMission";
            DoubleBuffered = true;
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            HandleKey(e.KeyCode, true);
        }

        private void HandleKey(Keys e, bool down)
        {
            switch (e)
            {
                case Keys.A:
                    left = down;
                    break;
                case Keys.D:
                    right = down;
                    break;
            }
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            HandleKey(e.KeyCode, false);
        }
        private void TimerTick(object sender, EventArgs args)
        {
            if (tickCount == 0) gameState.BeginAct();
            foreach (var e in gameState.Animations)
                e.Location = new Point(e.Location.X + 4 * e.Command.DeltaX, e.Location.Y + 4 * e.Command.DeltaY);
            if (tickCount == 7)
                gameState.EndAct();
            tickCount++;
            if (tickCount == 8) tickCount = 0;
            Invalidate();
        }
    }
}
