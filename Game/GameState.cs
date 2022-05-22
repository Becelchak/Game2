using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using Game;

namespace GameModel
{
    public class GameState
    {
        public const int ElementSize = 32;
        public List<Animation> Animations = new();
        public void BeginAct()
        {
            Animations.Clear();
            for (var x = 0; x < GameMap.MapWidth; x++)
                for (var y = 0; y < GameMap.MapHeight; y++)
                {
                    var creature = GameMap.Map[x, y];
                    if (creature == null) continue;
                    Animations.Add(
                        new Animation
                        {
                            Creature = creature,
                            Location = new Point(x * ElementSize, y * ElementSize),
                        });
                }

            Animations = Animations.OrderByDescending(z => z.Creature.GetDrawingPriority()).ToList();
        }
    }
}
