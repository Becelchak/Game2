using System.Windows.Forms;
using System;
using GameModel;

namespace Game
{
    public class Game_Map
    {
        private const string testMap = @"
WWWWWWWWWWWWWWWWWWWWWWW
W.....................W
W.....................W
W.....................W
W.....................W
W.....................W
W........W............W
W........W............W
W........W............W
W........W............W
W........W............W
W........W............W
WWWWWWWWWWWWWWWWWWWWWWW";

        public static IObject[,] Map;
        public static bool IsOver;

        public static Keys KeyPressed;
        public static int MapWidth => Map.GetLength(0);
        public static int MapHeight => Map.GetLength(1);

        public static void CreateMap()
        {
            Map = MapCreator.CreateMap(testMap);
        }
    }
}