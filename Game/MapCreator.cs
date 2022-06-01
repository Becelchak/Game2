using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameModel;

namespace Game
{
    public static class MapCreator
    {
        private static readonly Dictionary<string, Func<IObject>> factory = new();

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
            switch (c)
            {
                case 'W':
                    return CreateObject("Wall");
                case ')':
                    return CreateObject("WallRight");
                case 'S':
                    return CreateObject("Shards");
                case 'B':
                    return CreateObject("Blood");
                case '-':
                    return CreateObject("WallUp");
                case '_':
                    return CreateObject("WallDown");
                case '(':
                    return CreateObject("WallLeft");
                case 'G':
                    return CreateObject("Glass");
                case 'D':
                    return CreateObject("Door");
                case 'E':
                    return CreateObject("Exit");
                case 'Z':
                    return CreateObject("ZoneEnemy");
                case 'M':
                    return CreateObject("Medkit");
                case '.':
                    return null;
                default:
                    throw new Exception($"Wrong object{c}. Ошибка при считывании карты");
            }
        }
    }
}