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
using Microsoft.VisualBasic.Devices;
using Keys = System.Windows.Forms.Keys;

namespace Game
{
    public partial class Mission1 : Form
    {
        private readonly GameState gameState;
        private readonly Image playerImage = Resource.Player;
        private readonly Queue<string> levels;
        private readonly string next_map;

        private readonly List<Bullet> bullets = new List<Bullet>();
        private SoundPlayer soundShoot = new SoundPlayer("C:\\Users\\kost4\\source\\repos\\Rep\\Game\\Resources\\Shoot.wav");
        private SoundPlayer soundReload = new SoundPlayer("C:\\Users\\kost4\\source\\repos\\Rep\\Game\\Resources\\Reload.wav");


        private int healPoint;
        private readonly Player player;
        private readonly int speed = 10;
        private int tickCount;
        public Mission1(Queue<string> maps, Point playerLocation, DirectoryInfo imagesDirectory = null)
        {
            InitializeComponent();
            player = new Player(playerLocation);
            levels = maps;
            if(levels.Count != 0)
                next_map = levels.Dequeue();
            Directory.SetCurrentDirectory("C:\\Users\\kost4\\source\\repos\\Rep\\Game\\Resources");
            healPoint = player.HealPoint;
            Cursor = new Cursor("Scoup.cur");

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
            e.Graphics.TranslateTransform(0, GameState.ElementSize);
            foreach (var a in gameState.Animations)
                e.Graphics.DrawImage(a.Creature.GetImage(), a.Location);
            foreach (var bullet in bullets.Where(bullet => !bullet.stop))
            {
                e.Graphics.DrawImage(bullet.TypeBullet, bullet.Location);
            }
            e.Graphics.DrawImage(playerImage, player.Location);
            e.Graphics.ResetTransform();
            e.Graphics.DrawString(healPoint + " HP", new Font("Arial", 18), Brushes.Red, 1, 1);
            e.Graphics.DrawString(player.Ammo + " Ammo", new Font("Arial", 18), Brushes.Orange, 85, 1);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text = "Mission1";
            DoubleBuffered = true;
        }
        private void TimerTick(object sender, EventArgs args)
        {
            if (tickCount == 0) gameState.BeginAct();
            {
                foreach (var e in gameState.Animations)
                    e.Location = new Point(e.Location.X, e.Location.Y);
                foreach (var bullet in bullets)
                {
                    if (bullet.Location.Y <= ClientSize.Height || bullet.Location.X <= ClientSize.Width)
                        bullet.Move();
                }
            }
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
                    throw new ArgumentOutOfRangeException(nameof(e), e, "Неизвестная команда");
            }
        }

        private bool CanMove(Point target)
        {
            var answer = true;
            var playerRadius = new Rectangle(target.X, target.Y, 32, 32);
            foreach (var item in from item in gameState.Animations let wall = new Rectangle(item.Location.X, item.Location.Y, 32,32)
                where wall.IntersectsWith(playerRadius) && item.Creature is WallDown or WallLeft or WallRight or WallUp or Glass or Door or Exit or Wall select item)
            {
                switch (item.Creature)
                {
                    case Door:
                        answer = true;
                        Game_Map.CreateMap(next_map);
                        new Mission1(levels,player.Location).Show();
                        Hide();
                        break;
                    case Exit:
                        answer = true;
                        Close();
                        break;
                    default:
                        answer = false;
                        break;
                }
                break;
            }
            return answer;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            var moveKey = new[] { Keys.A, Keys.S, Keys.W, Keys.D };
            base.OnKeyDown(e);
            if(moveKey.Contains(e.KeyCode))
                MovePlayer(e.KeyCode);
            if (e.KeyCode != Keys.R) return;
            soundReload.Play();
            player.Reload();
        }


        private void OnMouseClick(Object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left && player.Ammo > 0)
            {
                var point = e.Location;
                var b = new Bullet(player.Location, point)
                {
                    TypeBullet = Resource.Bullet
                };
                bullets.Add(b);
                --player.Ammo;
                soundShoot.Play();
            }
        }
    }
}