using System.Windows.Forms;
using System;
using GameModel;

namespace Game
{
    public class Game_Task
    {
        private const string map1 = @"
WWWW
W
W
WWWW";

        public static IObject[,] Map;
        public static int Scores;
        public static bool IsOver;

        public static Keys KeyPressed;
        public static int MapWidth => Map.GetLength(0);
        public static int MapHeight => Map.GetLength(1);

        public static void CreateMap()
        {
            Map = MapCreator.CreateMap(map1);
        }
    }
}