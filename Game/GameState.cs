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
            for (var x = 0; x < Game_Map.MapWidth; x++)
                for (var y = 0; y < Game_Map.MapHeight; y++)
                {
                    var creature = Game_Map.Map[x, y];
                    if (creature == null) continue;
                    var command = creature.Action(x, y);

                    if (x + command.DeltaX < 0 || x + command.DeltaX >= Game_Map.MapWidth || y + command.DeltaY < 0 ||
                        y + command.DeltaY >= Game_Map.MapHeight)
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

        public void EndAct()
        {
            var creaturesPerLocation = GetCandidatesPerLocation();
            for (var x = 0; x < Game_Map.MapWidth; x++)
                for (var y = 0; y < Game_Map.MapHeight; y++)
                    Game_Map.Map[x, y] = SelectWinnerCandidatePerLocation(creaturesPerLocation, x, y);
        }

        private static IObject SelectWinnerCandidatePerLocation(List<IObject>[,] creatures, int x, int y)
        {
            var candidates = creatures[x, y];
            var aliveCandidates = candidates.ToList();
            foreach (var candidate in candidates)
                foreach (var rival in candidates)
                    if (rival != candidate && candidate.CheckOnDeath(rival))
                        aliveCandidates.Remove(candidate);
            if (aliveCandidates.Count > 1)
                throw new Exception(
                    $"Creatures {aliveCandidates[0].GetType().Name} and {aliveCandidates[1].GetType().Name} claimed the same map cell");

            return aliveCandidates.FirstOrDefault();
        }

        private List<IObject>[,] GetCandidatesPerLocation()
        {
            var creatures = new List<IObject>[Game_Map.MapWidth, Game_Map.MapHeight];
            for (var x = 0; x < Game_Map.MapWidth; x++)
                for (var y = 0; y < Game_Map.MapHeight; y++)
                    creatures[x, y] = new List<IObject>();
            foreach (var e in Animations)
            {
                var x = e.TargetLogicalLocation.X;
                var y = e.TargetLogicalLocation.Y;
                var nextCreature = e.Command.TransformTo ?? e.Creature;
                creatures[x, y].Add(nextCreature);
            }

            return creatures;
        }
    }
}
