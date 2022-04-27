using GameModel;

namespace Game
{
    public class Game_Map
    {
        private const string testMap = @"
_______________________
)..........G..........(
)..........G..........(
)..........G..........(
)..........G..........(
)..........G..........(
)..........G..........(
)..........G..........(
)..........G..........(
)..........G..........(
)..........G..........(
-----------------------";

        public static IObject[,] Map;
        public static int MapWidth => Map.GetLength(0);
        public static int MapHeight => Map.GetLength(1);

        public static void CreateMap()
        {
            Map = MapCreator.CreateMap(testMap);
        }
    }
}