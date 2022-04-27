using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameModel;
using Keys = System.Windows.Forms.Keys;

namespace Game
{
    public partial class TestMission : Form
    {
        private readonly GameState gameState;
        private readonly Image playerImage = Resource.Player;
        private int healPoint;
        private readonly Player player = new Player(new Point(50, 50));
        private readonly int speed = 5;
        private Point cursor;
        private int tickCount;
        public TestMission(DirectoryInfo imagesDirectory = null)
        {
            //Paint += (sender, args) =>
            //{
            //    var graphics = CreateGraphics();
            //    graphics.TranslateTransform(0, GameState.ElementSize);
            //    foreach (var a in gameState.Animations)
            //        graphics.DrawImage(Image.FromFile(a.Creature.GetPathForImage()), a.Location);
            //    graphics.DrawImage(playerImage, player.Location);
            //    graphics.ResetTransform();
            //    graphics.DrawString(healPoint.ToString(), new Font("Arial", 18), Brushes.Red, 1, 1);
            //};
            InitializeComponent();

            var cursor = this.PointToClient(Cursor.Position);
            healPoint = player.HealPoint;
            //SetStyle(ControlStyles.OptimizedDoubleBuffer 
            //         | ControlStyles.AllPaintingInWmPaint 
            //         | ControlStyles.UserPaint, true);
            //UpdateStyles();

            gameState = new GameState();
            ClientSize = new Size(
                GameState.ElementSize * Game_Map.MapWidth,
                GameState.ElementSize * Game_Map.MapHeight + GameState.ElementSize);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            BackgroundImage = Resource.Floor;

            var timer = new Timer();
            timer.Interval = 20;
            timer.Tick += TimerTick;
            timer.Start();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            var f = new Rectangle(cursor.X - 10, cursor.Y - 10, 100, 100);
            e.Graphics.TranslateTransform(0, GameState.ElementSize);
            foreach (var a in gameState.Animations)
                e.Graphics.DrawImage(Image.FromFile(a.Creature.GetPathForImage()), a.Location);
            e.Graphics.DrawImage(playerImage, player.Location);
            e.Graphics.DrawImage(Resource., f);
            e.Graphics.ResetTransform();
            e.Graphics.DrawString(healPoint.ToString(), new Font("Arial", 18), Brushes.Red, 1, 1);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text = "TestMission";
            DoubleBuffered = true;
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
        private void MovePlayer(Keys e)
        {
            switch (e)
            {
                case Keys.W:
                    if(CanMove(new Point(player.Location.X, player.Location.Y - speed)))
                        player.Location = new Point(player.Location.X, player.Location.Y - speed);
                    break;
                case Keys.A:
                    if (CanMove(new Point(player.Location.X - speed, player.Location.Y)))
                        player.Location = new Point(player.Location.X - speed, player.Location.Y);
                    break;
                case Keys.S:
                    if (CanMove(new Point(player.Location.X, player.Location.Y + speed)))
                        player.Location = new Point(player.Location.X, player.Location.Y + speed);
                    break;
                case Keys.D:
                    if (CanMove(new Point(player.Location.X + speed, player.Location.Y)))
                        player.Location = new Point(player.Location.X + speed, player.Location.Y);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(e), e, null);
            }
        }

        private bool CanMove(Point target)
        {
            var answer = true;
            var playerRadius = new Rectangle(target.X, target.Y, 32, 32);
            foreach (var item in from item in gameState.Animations let wall = new Rectangle(item.Location.X, item.Location.Y, 32,32) 
                where wall.IntersectsWith(playerRadius) && item.Creature is Wall select item)
            {
                answer = false;
            }
            return answer;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            MovePlayer(e.KeyCode);
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            MovePlayer(e.KeyCode);
        }
    }
}