﻿using System.Collections.Generic;
using GameModel;

namespace Game
{
    public static class GameMap
    {
        private const string Mission1Map1 = @"
_______________________
)..........G..........(
)..........G..........(
)..........G..........(
)..........S..........(
)..........S...Z.......
)..........G.........D.
)..........G...........
)..........G..........(
)..........G..........(
).......Z..G..........(
-----------------------";

        private const string Mission1Map2 = @"
_______________________
)..........G..........(
)...Z......G..........(
)..........G..........(
).........._..........(
).....................(
).Z....................
)..........-...........
)..........G...........
)..........G..........(
)..........S..........(
)..........S........M.(
)..........G..........(
)....._____(..........(
).....)....(.Z........(
).....)....(.....Z....(
).....)....(..........(
).Z........(..........(
)..D.....M.(..........(
--...------------------";

        private const string Mission1Map3 = @"
________________...____
)..........G.....E....(
).Z...B....S..........(
)..........G..........(
))..(GGGGG__GGGG)..(GG(
).........M...........(
)................Z....(
)..Z..................(
)...............Z.....(
).....................(
).....................(
).....................(
).....................(
).....................(
)................Z....(
).....................(
).....................(
)..............B......(
).....................(
)..................M..(
--...------------------";

        private const string Mission2Map1 = @"
___________GGGGGGGGGGG_____________________
).........(......Z....)...................(
).........(...........).............Z...M.(
).........(___..._____)...................(
).........................................(
).................................Z.......(
).........................................(
)------------------------------------...--(
).........................................(
)....................................Z....(
)...................Z.....................(
)--...-------GGGSSSGGGG-----...------...--(
)..Z....)..................(..........Z...(
)..D....)....M....M..M.....(..Z...........(
--...--------------------------------------";

        private const string Mission2Map2 = @"
______________________________________...__
)......................................D..(
).Z.........Z...Z.........................(
).........................M...............(
)--...------...---------------------------(
G.........................................(
G.....................................Z...(
G................................Z........(
)------------------------------------...--(
)......................(WWWWWW).M.........(
)..Z...........B..Z....(WWWWWW).M.........(
).......)............M.(WWWWWW)...........(
)--...--)....(GGGGGGGGG--------GGGGGGG)...(
).....M.).................................(
).......)....Z.....Z..........Z...........(
).......).................................(
--...--------------------------------------";

        private const string Mission2Map3 = @"
______________________________________...__
).........................................(
).........................................(
)--...--)....(GGGGGGGGG--------GGGGGGGGGGG(
)......................(WWWWWW)...........(
)..........B............______.....Z......(
).........................................(
).M..M........Z...........................(
)-------)..............(WWWWWW)...........(
(WWWWWWW)..............(WWWWWW)....Z......(
)..M.M..)..............(WWWWWW)...........(
).......)----(GGGGGGGGG--------GGGGGGG)...(
)---..--).................................(
).........................Z...............(
)...........Z.............................(
).......W---------------------------------(
).......).....M...(WWWW).......(WWWW).....(
).......).B.......(WWWW).Z.....(WWWW).Z...(
).........................................(
)........Z..............................E.(
---------------------------------------...-";

        private const string Mission3Map1 = @"
___________GGGGGGGGG_______GGGGGGGGG_______
).........................................(
).........................................(
)...................M.M...................(
)...............W---...---W...............(
)...............(.........)..........Z....(
)...............(....D....)...............(
)...............W___...___W...............(
).........................................(
)..................................Z......(
).........................................(
-------------------------------------------";

        private const string Mission3Map2 = @"
___________________________________________
)...G....M.M............M.M...........G...(
)...S......Z......................Z...S...(
)...S.M...............................S...(
)...G...........W---...---W...........G...(
)...W---G...............M.........G---W...(
).Z.....S.........................SZ....Z..
).......S.......W___...___W.......S......E.
)...W___G.........................G___W...(
)...S.............Z...........Z.......S...(
)...S.....M.M........M.M..............S...(
-------------------------------------------";

        public static readonly Queue<string> Pack1 = FullPack(new Queue<string>(), Mission1Map1, Mission1Map2, Mission1Map3);
        public static readonly Queue<string> Pack2 = FullPack(new Queue<string>(), Mission2Map1, Mission2Map2, Mission2Map3);
        public static readonly Queue<string> Pack3 = FullPack(new Queue<string>(), Mission3Map1, Mission3Map2);

        public static IObject[,] Map;
        public static int MapWidth => Map.GetLength(0);
        public static int MapHeight => Map.GetLength(1);

        public static void CreateMap(string map)
        {
            Map = MapCreator.CreateMap(map);
        }

        private static Queue<string> FullPack(Queue<string> pack, params string[] map)
        {
            foreach (var item in map)
            {
                pack.Enqueue(item);
            }

            return pack;
        }
    }
}