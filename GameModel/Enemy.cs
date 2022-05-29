using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace GameModel
{
    public class Enemy
    {
        public Point Location;
        private Point NextPoint;
        public Point SpawnPoint;
        private readonly int Tier;
        public int HealPoint;
        private readonly int Speed;
        public int trying;
        private bool IsDeath;
        private bool IsImpact;
        public bool GoPlayer = true;
        public bool GoSpawn;

        public bool ShowDeath() => IsDeath;
        public bool ShowImpact() => IsImpact;
        public void BackImpact() => IsImpact = false;
        public void BackDeath() => IsDeath = false;
        public Enemy(Point Location, int tier, int speed)
        {
            this.Location = Location;
            SpawnPoint = Location;
            Tier = tier;
            HealPoint = 100;
            Speed = speed;
        }

        public void CheckImpact(List<Bullet> bullets)
        {
            foreach (var bullet in from bullet in bullets
                let radius = new Rectangle(Location.X, Location.Y, 64, 64)
                let bullRadius = new Rectangle(bullet.Location.X, bullet.Location.Y, 32, 32)
                where radius.IntersectsWith(bullRadius) && bullet.stop
                select bullet)
            {
                if (HealPoint > 0)
                {
                    if (HealPoint - 15 * Tier < 0)
                        IsDeath = true;
                    HealPoint -= 15 * Tier;
                    IsImpact = true;
                }
                bullets.Remove(bullet);
                break;
            }

        }
        public void MoveEnemy(Point player)
        {
            var vector = Math.Sqrt((player.X - Location.X) * (player.X - Location.X) +
                                   (player.Y - Location.Y) * (player.Y - Location.Y));
            if (!(vector < 380 && vector != 0)) return;

            Location = NextPoint;
        }

        public Point? TryMoveEnemy(Point player)
        {
            var vector = Math.Sqrt((player.X - Location.X) * (player.X - Location.X) +
                                   (player.Y - Location.Y) * (player.Y - Location.Y));
            if (!(vector < 380)) return null;
            var distanceX = player.X - Location.X;
            var distanceY = player.Y - Location.Y;
            var newX = distanceX / vector * Speed + Location.X;
            var newY = distanceY / vector * Speed + Location.Y;
            if (new Point((int) newX, (int) newY) == new Point(0, 0)) return null;
            NextPoint = new Point((int) newX, (int) newY);
            return NextPoint;
        }

        public void RefreshTarget()
        {
            if (GoPlayer && !GoSpawn)
            {
                GoPlayer = false;
                GoSpawn = true;
            }
            else
            {
                GoPlayer = true;
                GoSpawn = false;
            }
        }
    }
}