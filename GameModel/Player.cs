using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameModel
{
    public class Player
    {
        public int HealPoint;

        private int Ammo ;

        public Point Location;

        public Player(Point Location, int HealPoint, int Ammo)
        {
            this.Location = Location;
            this.HealPoint = HealPoint;
            this.Ammo = Ammo;
        }

        public int ShowAmmo() => Ammo;
        public void SpendAmmo() => --Ammo; 

        public void Reload()
        {
            Ammo = 15;
        }

        public int GetHealPoint()
        {
            return HealPoint + 40;
        }
    }
}
