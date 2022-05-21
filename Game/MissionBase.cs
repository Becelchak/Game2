using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using GameModel;
using Keys = System.Windows.Forms.Keys;

namespace Game
{
    public partial class MissionBase : Form
    {
        private readonly GameState gameState;
        private readonly Image playerImage = Resource.Player;
        private readonly Image enemyImage;
        private readonly Image enemyBody;
        private readonly Queue<string> levels;
        private readonly string next_map;
        private readonly string nameMission;

        private readonly List<Enemy> enemys = new();
        private readonly int enemyTier;
        private readonly List<Bullet> bullets = new();
        private readonly SoundPlayer soundShoot;
        private readonly SoundPlayer soundReload;
        private readonly SoundPlayer soundDieEnemy;
        private readonly SoundPlayer soundImpact;


        private int healPoint;
        private readonly Player player;
        private const int speed = 8;
        private static bool isAPressed;
        private static bool isWPressed;
        private static bool isSPressed;
        private static bool isDPressed;
        private static readonly Keys[] moveKey = { Keys.A, Keys.S, Keys.W, Keys.D };
        public MissionBase(Queue<string> maps, Point playerLocation, string name, Image enemyLive, Image enemyDead, int enemyTier, SoundPlayer soundDie = null, DirectoryInfo imagesDirectory = null)
        {
            InitializeComponent();
            nameMission = name;
            enemyImage = enemyLive;
            enemyBody = enemyDead;
            this.enemyTier = enemyTier;
            player = new Player(playerLocation);
            levels = maps;
            if (levels.Count != 0)
                next_map = levels.Dequeue();
            Directory.SetCurrentDirectory("C:\\Users\\kost4\\source\\repos\\Rep\\Game\\Resources");
            soundShoot = new SoundPlayer(Path.GetFullPath("Shoot.wav"));
            soundReload = new SoundPlayer(Path.GetFullPath("Reload.wav"));
            soundDieEnemy = soundDie;
            soundImpact = new SoundPlayer(Path.GetFullPath("impact.wav"));

            healPoint = player.HealPoint;
            Cursor = new Cursor("Scoup.cur");
            gameState = new GameState();
            ClientSize = new Size(
                GameState.ElementSize * GameMap.MapWidth,
                GameState.ElementSize * GameMap.MapHeight + GameState.ElementSize);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            BackgroundImage = Resource.Floor;

            enemys.Add(new Enemy(new Point(95, 95), enemyTier));

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
                e.Graphics.DrawImage(bullet.TypeBullet, bullet.Location);
            foreach (var enemy in enemys)
            {
                enemy.CheckImpact(bullets);
                if (enemy.ShowImpact())
                {
                    soundImpact.Play();
                    enemy.BackImpact();
                }
                if (enemy.ShowDeath())
                {
                    soundDieEnemy?.Play();
                    enemy.BackDeath();
                }
                e.Graphics.DrawImage(enemy.HealPoint > 0 ? enemyImage
                    : enemyBody, enemy.Location);
            }
            e.Graphics.DrawImage(playerImage, player.Location);
            e.Graphics.ResetTransform();
            e.Graphics.DrawString(healPoint + " HP", new Font("Arial", 18), Brushes.Red, 1, 1);
            e.Graphics.DrawString(player.ShowAmmo() + " Ammo", new Font("Arial", 18), Brushes.Orange, 85, 1);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text = nameMission;
            DoubleBuffered = true;
        }
        private void TimerTick(object sender, EventArgs args)
        {
            gameState.BeginAct();
            foreach (var e in gameState.Animations)
                e.Location = new Point(e.Location.X, e.Location.Y);
            foreach (var bullet in bullets
                .Where(bullet => bullet.Location.Y <= ClientSize.Height
                                 || bullet.Location.X <= ClientSize.Width))
            {
                bullet.Move();
            }
            if (isAPressed) MovePlayer(isAPressed, Keys.A);
            if (isWPressed) MovePlayer(isWPressed, Keys.W);
            if (isSPressed) MovePlayer(isSPressed, Keys.S);
            if (isDPressed) MovePlayer(isDPressed, Keys.D);
            Invalidate();
        }
        private void MovePlayer(bool keyValue, Keys e)
        {
            var newPoint = new Point(player.Location.X, player.Location.Y);
            switch (e)
            {
                case Keys.W:
                    if (CanMove(new Point(newPoint.X, newPoint.Y - speed)))
                    {
                        player.Location = new Point(newPoint.X, newPoint.Y - speed);
                        isWPressed = keyValue;
                    }
                    else
                        isWPressed = false;
                    break;
                case Keys.A:
                    if (CanMove(new Point(newPoint.X - speed, newPoint.Y)))
                    {
                        player.Location = new Point(newPoint.X - speed, newPoint.Y);
                        isAPressed = keyValue;
                    }
                    else
                        isAPressed = false;
                    break;
                case Keys.S:
                    if (CanMove(new Point(newPoint.X, newPoint.Y + speed)))
                    {
                        player.Location = new Point(newPoint.X, newPoint.Y + speed);
                        isSPressed = keyValue;
                    }
                    else
                        isSPressed = false;
                    break;
                case Keys.D:
                    if (CanMove(new Point(newPoint.X + speed, newPoint.Y)))
                    {
                        player.Location = new Point(newPoint.X + speed, newPoint.Y);
                        isDPressed = keyValue;
                    }
                    else
                        isDPressed = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(e), e, "Неизвестная команда");
            }
        }

        private bool CanMove(Point target)
        {
            var answer = true;
            var playerRadius = new Rectangle(target.X, target.Y, 32, 32);
            foreach (var item in from item in gameState.Animations
                                 let wall = new Rectangle(item.Location.X, item.Location.Y, 32, 32)
                                 where wall.IntersectsWith(playerRadius) && item.Creature is WallDown or WallLeft or WallRight or WallUp or Glass or Door or Exit or Wall
                                 select item)
            {
                switch (item.Creature)
                {
                    case Door:
                        item.Creature.CheckOnDeath(item.Creature);
                        GameMap.CreateMap(next_map);
                        enemys.Clear();
                        new MissionBase(levels, player.Location, nameMission, enemyImage,enemyBody, enemyTier, soundDieEnemy).Show();
                        Hide();
                        break;
                    case Exit:
                        Close();
                        break;
                    default:
                        answer = false;
                        break;
                }
                break;
            }

            foreach (var enemy in from enemy in enemys
                                  let radius = new Rectangle(enemy.Location.X, enemy.Location.Y, 32, 32)
                                  where radius.IntersectsWith(playerRadius)
                                  select enemy)
            {
                answer = false;
            }
            return answer;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (moveKey.Contains(e.KeyCode))
                MovePlayer(true, e.KeyCode);
            if (e.KeyCode != Keys.R) return;
            bullets.Clear();
            soundReload.Play();
            player.Reload();
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (moveKey.Contains(e.KeyCode))
                MovePlayer(false, e.KeyCode);
        }


        private void OnMouseClick(Object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || player.ShowAmmo() <= 0) return;
            var point = e.Location;
            var b = new Bullet(player.Location, point)
            {
                TypeBullet = Resource.Bullet
            };
            bullets.Add(b);
            player.SpendAmmo();
            soundShoot.Play();
        }
    }
}