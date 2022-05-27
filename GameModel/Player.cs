using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameModel
{
    public class Player
    {
        public int HealPoint;
        public float AnglePlayer;
        private int Ammo ;

        public Point Location;
        private bool IsDead;

        public Player(Point Location, int HealPoint, int Ammo)
        {
            this.Location = Location;
            this.HealPoint = HealPoint;
            this.Ammo = Ammo;
        }

        public bool ShowDeath() => IsDead;
        public void SetDeath() => IsDead = true;
        public int ShowAmmo() => Ammo;
        public void SpendAmmo() => --Ammo; 

        public void Reload()
        {
            Ammo = 15;
        }

        public int GetHealPoint()
        {
            return HealPoint += 40;
        }
        public int TakeDamage()
        {
            return HealPoint -=1;
        }
    }
}
