using System;
using System.Collections.Generic;
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
                    var command = creature.Action(x, y);

                    if (x + command.DeltaX < 0 || x + command.DeltaX >= GameMap.MapWidth || y + command.DeltaY < 0 ||
                        y + command.DeltaY >= GameMap.MapHeight)
                        throw new Exception($"The object {creature.GetType()} falls out of the game field");

                    Animations.Add(
                        new Animation
                        {
                            Command = command,
                            Creature = creature,
                            Location = new Point(x * ElementSize, y * ElementSize),
                            TargetLogicalLocation = new Point(x + command.DeltaX, y + command.DeltaY)
                        });
                }

            Animations = Animations.OrderByDescending(z => z.Creature.GetDrawingPriority()).ToList();
        }
    }
}
