using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameModel;

namespace Game
{
    public static class MapCreator
    {
        private static readonly Dictionary<string, Func<IObject>> factory = new Dictionary<string, Func<IObject>>();

        public static IObject[,] CreateMap(string map, string separator = "\r\n")
        {
            var rows = map.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            if (rows.Select(z => z.Length).Distinct().Count() != 1)
                throw new Exception($"Wrong map '{map}'");
            var result = new IObject[rows[0].Length, rows.Length];
            for (var x = 0; x < rows[0].Length; x++)
                for (var y = 0; y < rows.Length; y++)
                    result[x, y] = CreateObjectBySymbol(rows[y][x]);
            return result;
        }

        private static IObject CreateObject(string name)
        {
            if (factory.ContainsKey(name)) return factory[name]();
            var type = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(z => z.Name == name);
            if (type == null)
                throw new Exception($"Can't find object '{name}'");
            factory[name] = () => (IObject)Activator.CreateInstance(type);

            return factory[name]();
        }


        private static IObject CreateObjectBySymbol(char c)
        {
            return c switch
            {
                'W' => CreateObject("Wall"),
                ')' => CreateObject("WallRight"),
                'S' => CreateObject("Shards"),
                'B' => CreateObject("Blood"),
                '-' => CreateObject("WallUp"),
                '_' => CreateObject("WallDown"),
                '(' => CreateObject("WallLeft"),
                'G' => CreateObject("Glass"),
                'D' => CreateObject("Door"),
                'E' => CreateObject("Exit"),
                '.' => null,
                _ => throw new Exception($"Wrong object{c}")
            };
        }
    }
}