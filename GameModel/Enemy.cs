using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GameModel
{
    public class Enemy
    {
        public Point Location;
        private readonly int Tier;
        public int HealPoint = 100;
        private bool IsDeath;
        private bool IsImpact;

        public bool ShowDeath() => IsDeath;
        public bool ShowImpact() => IsImpact;
        public void BackImpact() => IsImpact = false;
        public void BackDeath() => IsDeath = false;
        public Enemy(Point Location, int tier)
        {
            this.Location = Location;
            Tier = tier;
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
                    if (HealPoint - (10 * Tier) < 0)
                        IsDeath = true;
                    HealPoint -= 10 * Tier;
                    IsImpact = true;
                }
                bullets.Remove(bullet);
                break;
            }

        }

    }
}