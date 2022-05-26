using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Media;
using System.Timers;
using System.Windows.Forms;
using GameModel;
using Keys = System.Windows.Forms.Keys;
using Timer = System.Windows.Forms.Timer;

namespace Game
{
    public partial class MissionBase : Form
    {
        private GameState gameState;
        private Image playerImage = Resource.Player;
        private readonly Image enemyImage;
        private readonly Image enemyBody;
        private readonly Queue<string> levels;
        private readonly string nextMap;
        private readonly string nameMission;

        private readonly List<Enemy> enemyList = new();
        private readonly int enemyTier;
        private readonly List<Bullet> bullets = new();
        private readonly List<Point> medkits = new();
        private readonly SoundPlayer soundShoot;
        private readonly SoundPlayer soundReload;
        private readonly SoundPlayer soundDieEnemy;
        private readonly SoundPlayer soundImpact;
        private readonly SoundPlayer soundMedkit;


        private int healPoint;
        private readonly Player player;
        private const int speed = 8;
        private static bool isAPressed;
        private static bool isWPressed;
        private static bool isSPressed;
        private static bool isDPressed;
        private static readonly Keys[] moveKey = { Keys.A, Keys.S, Keys.W, Keys.D };

        private PointF CursorMouse;
        private float AnglePlayer;

        public MissionBase(Queue<string> maps, Player player, string name, Image enemyLive, Image enemyDead, int enemyTier, SoundPlayer soundDie = null, DirectoryInfo imagesDirectory = null)
        {
            InitializeComponent();
            nameMission = name;
            enemyImage = enemyLive;
            enemyBody = enemyDead;
            this.enemyTier = enemyTier;
            this.player = player;
            levels = maps;
            if (levels.Count != 0)
                nextMap = levels.Dequeue();
            Directory.SetCurrentDirectory("C:\\Users\\kost4\\source\\repos\\Rep\\Game\\Resources");
            soundShoot = new SoundPlayer(Path.GetFullPath("Shoot.wav"));
            soundReload = new SoundPlayer(Path.GetFullPath("Reload.wav"));
            soundDieEnemy = soundDie;
            soundImpact = new SoundPlayer(Path.GetFullPath("impact.wav"));
            soundMedkit = new SoundPlayer(Path.GetFullPath("medshot4.wav"));

            healPoint = player.HealPoint;
            Cursor = new Cursor("Scoup.cur");
            gameState = new GameState();
            ClientSize = new Size(
                GameState.ElementSize * GameMap.MapWidth,
                GameState.ElementSize * GameMap.MapHeight + GameState.ElementSize);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            BackgroundImage = Resource.Floor;
            
            gameState.BeginAct();
            foreach (var state in gameState.Animations.Where(state => state.Creature is ZoneEnemy))
                enemyList.Add(new Enemy(state.Location, enemyTier, 6));
            foreach (var state in gameState.Animations.Where(state => state.Creature is Medkit))
                medkits.Add(state.Location);

            var timer = new Timer();
            timer.Interval = 20;
            timer.Tick += TimerTick;
            timer.Start();

            MouseMove += (_, e) =>
            {
                CursorMouse = new PointF(e.Location.X, e.Location.Y);
                Invalidate();
            };
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(0, GameState.ElementSize);
            foreach (var animation in gameState.Animations)
                e.Graphics.DrawImage(animation.Creature.GetImage(), animation.Location);
            foreach (var medkit in medkits)
                e.Graphics.DrawImage(Resource.Medkit, medkit);
            foreach (var bullet in bullets.Where(bullet => !bullet.stop))
                e.Graphics.DrawImage(bullet.TypeBullet, bullet.Location);
            foreach (var enemy in enemyList)
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
            switch (healPoint)
            {
                case > 0:
                    playerImage = Rotate(e, playerImage);
                    e.Graphics.DrawImage(playerImage, player.Location);
                    e.Graphics.DrawEllipse(new Pen(Color.Brown),new Rectangle(player.Location,new Size(32,32)));
                    break;
                case < 0:
                    player.Location = new Point(0, 0);
                    player.SetDeath();
                    break;
            }
            e.Graphics.ResetTransform();
            if(player.ShowDeath())
                e.Graphics.DrawString("Миссия провалена", new Font("Arial", 32), Brushes.Red, 100, 100);
            e.Graphics.DrawString(player.HealPoint + " HP", new Font("Arial", 18), Brushes.Red, 1, 1);
            e.Graphics.DrawString(player.ShowAmmo() + " Ammo", new Font("Arial", 18), Brushes.Orange, 85, 1);
        }
        private void ChangeAngle()
        {
            AnglePlayer = (float)Math.Atan2( player.Location.Y - CursorMouse.Y, player.Location.X - CursorMouse.X);
        }

        private Image Rotate(PaintEventArgs e, Image img)
        {
            ChangeAngle(); 
            e.Graphics.TranslateTransform(player.Location.X, player.Location.Y);
            e.Graphics.RotateTransform((float)(AnglePlayer * 180/ Math.PI));
            e.Graphics.TranslateTransform(-player.Location.X, -player.Location.Y); 
            return img; 
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text = nameMission;
            DoubleBuffered = true;
        }
        private void TimerTick(object sender, EventArgs args)
        {
            foreach (var e in gameState.Animations)
            {
                e.Location = new Point(e.Location.X, e.Location.Y);
            }
            foreach (var bullet in bullets
                .Where(bullet => bullet.Location.Y <= ClientSize.Height
                                 || bullet.Location.X <= ClientSize.Width))
            {
                bullet.Move();
            }
            var playerRadius = new Rectangle(player.Location.X, player.Location.Y, 32, 32);
            foreach (var enemy in from enemy in enemyList
                where enemy.HealPoint > 0 && CanMoveEnemy(enemy.TryMoveEnemy(player.Location), enemy)
                                  select enemy)
            {
                var radius = new Rectangle(enemy.Location.X, enemy.Location.Y, 32, 32);
                if(!playerRadius.IntersectsWith(radius))
                    enemy.MoveEnemy(player.Location);
                else if (playerRadius.IntersectsWith(radius))
                {
                    healPoint = player.TakeDamage();
                }
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
                                 where wall.IntersectsWith(playerRadius) && item.Creature is WallDown or WallLeft or WallRight or WallUp or Glass or Door or Exit or Wall or Medkit
                                 select item)
            {
                switch (item.Creature)
                {
                    case Door:
                        GameMap.CreateMap(nextMap);
                        enemyList.Clear();
                        bullets.Clear();
                        player.HealPoint = healPoint;
                        Hide();
                        new MissionBase(levels, player, nameMission, enemyImage,enemyBody, enemyTier, soundDieEnemy).Show();
                        break;
                    case Exit:
                        Close();
                        break;
                    case Medkit:
                        if(medkits.Contains(item.Location))
                        {
                            healPoint = Math.Min(100, player.GetHealPoint());
                            player.HealPoint = healPoint;
                            soundMedkit.Play();
                            medkits.Remove(item.Location);
                        }
                        break;
                    default:
                        answer = false;
                        break;
                }
                break;
            }

            foreach (var enemy in from enemy in enemyList
                                  let radius = new Rectangle(enemy.Location.X, enemy.Location.Y, 32, 32)
                                  where radius.IntersectsWith(playerRadius) && enemy.HealPoint > 0
                                  select enemy)
            {
                answer = false;
            }
            return answer;
        }

        private bool CanMoveEnemy(Point? target, Enemy self)
        {
            var answer = true;
            if (target == null) return false;
            var enemyradius = new Rectangle(self.Location.X, self.Location.Y, 32, 32);
            foreach (var item in from item in gameState.Animations
                                 let wall = new Rectangle(item.Location.X, item.Location.Y, 32, 32)
                                 where enemyradius.IntersectsWith(wall) && item.Creature is WallDown or WallLeft or WallRight or WallUp or Glass or Wall
                                 select item)
            {
                answer = false;
                break;
            }
            foreach (var enemy in from enemy in enemyList
                                  let radius = new Rectangle(enemy.Location.X, enemy.Location.Y, 32, 32)
                                  where radius.IntersectsWith(enemyradius) && enemy.HealPoint > 0 && enemy != self
                                  select enemy)
            {
                answer = false;
                break;
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
            var b = new Bullet(player.Location, point, 20)
            {
                TypeBullet = Resource.Bullet
            };
            bullets.Add(b);
            player.SpendAmmo();
            soundShoot.Play();
        }
    }
}