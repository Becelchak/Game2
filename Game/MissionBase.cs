using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using GameModel;
using Keys = System.Windows.Forms.Keys;
using Timer = System.Windows.Forms.Timer;

namespace Game
{
    public sealed partial class MissionBase : Form
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
        private static Player player;
        private const int Speed = 8;
        private static bool isAPressed;
        private static bool isWPressed;
        private static bool isSPressed;
        private static bool isDPressed;
        private static readonly Keys[] moveKey = { Keys.A, Keys.S, Keys.W, Keys.D };

        private PointF CursorMouse;

        public MissionBase(Queue<string> maps, Player player, string name, Image enemyLive, Image enemyDead, int enemyTier, SoundPlayer soundDie = null, DirectoryInfo imagesDirectory = null)
        {
            InitializeComponent();
            nameMission = name;
            enemyImage = enemyLive;
            enemyBody = enemyDead;
            this.enemyTier = enemyTier;
            MissionBase.player = player;
            levels = maps;
            if (levels.Count != 0)
                nextMap = levels.Dequeue();
            // Требуется настроить до запуска игры на новом компьютере
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
                enemyList.Add(new Enemy(state.Location, enemyTier, 4));
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
                e.Graphics.DrawImage(RotateObject(e, bullet.TypeBullet, bullet), bullet.Location);
            e.Graphics.ResetTransform();
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
                e.Graphics.DrawImage(enemy.HealPoint > 0 ? RotateObject(e, enemyImage, enemy)
                    : enemyBody, enemy.Location);
                e.Graphics.ResetTransform();
            }
            switch (healPoint)
            {
                case > 0:
                    playerImage = Rotate(e, playerImage);
                    e.Graphics.DrawImage(playerImage, player.Location);
                    break;
                case < 0:
                    player.Location = new Point(0, 0);
                    player.SetDeath();
                    break;
            }
            e.Graphics.ResetTransform();
            if(player.ShowDeath())
            {
                var location = new Point(ClientSize.Width / 4,
                                ClientSize.Height / 4);
                var back = new Rectangle(location, new Size(400, 70));
                e.Graphics.DrawRectangle(new Pen(Color.Black,15f), back);
                e.Graphics.FillRectangle(Brushes.DarkGray, back);
                e.Graphics.DrawString("Миссия провалена", new Font("Arial", 32), Brushes.Red, location);
            }
            e.Graphics.DrawString(player.HealPoint + " HP", new Font("Arial", 18), Brushes.Red, 1, 1);
            e.Graphics.DrawString(player.ShowAmmo() + " Ammo", new Font("Arial", 18), Brushes.Orange, 85, 1);
        }
        private Image Rotate(PaintEventArgs e, Image img)
        {
            player.ChangeAngle(CursorMouse);
            e.Graphics.TranslateTransform((float)img.Width / 2 + player.Location.X , (float)img.Height / 2  + player.Location.Y);
            e.Graphics.RotateTransform((float)(player.AnglePlayer * 180/ Math.PI));
            e.Graphics.TranslateTransform((float)-img.Width / 2 - player.Location.X, (float)-img.Height / 2  - player.Location.Y); 
            return img; 
        }
        private static Image RotateObject(PaintEventArgs e, Image img, Bullet b)
        {
            e.Graphics.TranslateTransform((float)img.Width / 2 + b.Location.X, (float)img.Height / 2 + b.Location.Y);
            e.Graphics.RotateTransform((float)(b.angle * 180 / Math.PI));
            e.Graphics.TranslateTransform((float)-img.Width / 2 - b.Location.X, (float)-img.Height / 2 - b.Location.Y);
            return img;
        }

        private static Image RotateObject(PaintEventArgs e, Image img, Enemy enemy)
        {
            enemy.ChangeAngle(player);
            e.Graphics.TranslateTransform((float)img.Width / 2 + enemy.Location.X, (float)img.Height / 2 + enemy.Location.Y);
            e.Graphics.RotateTransform((float)(enemy.angle * 180 / Math.PI));
            e.Graphics.TranslateTransform((float)-img.Width / 2 - enemy.Location.X, (float)-img.Height / 2 - enemy.Location.Y);
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
                where enemy.HealPoint > 0
                select enemy)
            {
                var radius = new Rectangle(enemy.Location.X, enemy.Location.Y, 32, 32);
                switch (playerRadius.IntersectsWith(radius))
                {
                    case false when CanMoveEnemy(enemy.TryMoveEnemy(player.Location), enemy) && enemy.GoPlayer:
                        enemy.MoveEnemy(player.Location);
                        break;
                    case false when !CanMoveEnemy(enemy.TryMoveEnemy(enemy.SpawnPoint), enemy) && !enemy.GoSpawn:
                    {
                        enemy.MoveEnemy(enemy.SpawnPoint);
                        break;
                    }
                    default:
                    {
                        if (playerRadius.IntersectsWith(radius))
                        {
                            healPoint = player.TakeDamage();
                        }

                        break;
                    }
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
                    if (CanMove(new Point(newPoint.X, newPoint.Y - Speed)))
                    {
                        player.Location = new Point(newPoint.X, newPoint.Y - Speed);
                        isWPressed = keyValue;
                    }
                    else
                        isWPressed = false;
                    break;
                case Keys.A:
                    if (CanMove(new Point(newPoint.X - Speed, newPoint.Y)))
                    {
                        player.Location = new Point(newPoint.X - Speed, newPoint.Y);
                        isAPressed = keyValue;
                    }
                    else
                        isAPressed = false;
                    break;
                case Keys.S:
                    if (CanMove(new Point(newPoint.X, newPoint.Y + Speed)))
                    {
                        player.Location = new Point(newPoint.X, newPoint.Y + Speed);
                        isSPressed = keyValue;
                    }
                    else
                        isSPressed = false;
                    break;
                case Keys.D:
                    if (CanMove(new Point(newPoint.X + Speed, newPoint.Y)))
                    {
                        player.Location = new Point(newPoint.X + Speed, newPoint.Y);
                        isDPressed = keyValue;
                    }
                    else
                        isDPressed = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(e), e, @"Неизвестная команда");
            }
        }

        private bool CanMove(Point target)
        {
            var answer = true;
            var playerRadius = new Rectangle(target.X, target.Y - 40, 32, 32);
            foreach (var item in from item in gameState.Animations
                                 let wall = new Rectangle(item.Location.X, item.Location.Y, 32, 32)
                                 where wall.IntersectsWith(playerRadius) && item.Creature is WallDown or WallLeft or WallRight or WallUp or Glass or Door or Exit or Wall or Medkit
                                 select item)
            {
                switch (item.Creature)
                {
                    case Door:
                        GameMap.CreateMap(nextMap);
                        if(enemyList.Count > 0)
                        {
                            enemyList.Clear();
                            bullets.Clear();
                            player.HealPoint = healPoint;
                            new MissionBase(levels, player, nameMission, enemyImage, enemyBody, enemyTier,
                                soundDieEnemy).Show();
                            Hide();
                        }
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
            if (target == null) return false;

            var enemyRadius = new Rectangle(self.Location.X, self.Location.Y, 32, 32);
            var answer = !(from item in gameState.Animations 
                let wall = new Rectangle(item.Location.X, item.Location.Y, 32, 32) 
                where wall.IntersectsWith(enemyRadius) && item.Creature is WallDown or WallLeft or WallRight or WallUp or Glass or Wall 
                select item)
                .Any();
            if ((from enemy in enemyList
                let radius = new Rectangle(enemy.Location.X, enemy.Location.Y, 32, 32)
                where radius.IntersectsWith(enemyRadius) && enemy.HealPoint > 0 && enemy != self
                select enemy).Any())
            {
                answer = false;
            }
            switch (answer)
            {
                case false when self.TryingMove < 500:
                    self.TryingMove++;
                    self.RefreshTarget();
                    break;
                case false when self.TryingMove == 5:
                    self.TryingMove = 0;
                    self.RefreshTarget();
                    break;
                case true:
                    self.TryingMove = 0;
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
        private void OnMouseClick(object sender, MouseEventArgs e)
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