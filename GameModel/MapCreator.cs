using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GameModel
{
    public static class MapCreator
    {
        private static readonly Dictionary<string, Func<IObject>> factory = new Dictionary<string, Func<IObject>>();

        public static IObject[,] CreateMap(string map, string separator = "\r\n")
        {
            var rows = map.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            if (rows.Select(z => z.Length).Distinct().Count() != 1)
                throw new Exception($"Wrong test map '{map}'");
            var result = new IObject[rows[0].Length, rows.Length];
            for (var x = 0; x < rows[0].Length; x++)
                for (var y = 0; y < rows.Length; y++)
                    result[x, y] = CreateCreatureBySymbol(rows[y][x]);
            return result;
        }

        private static IObject CreateCreatureByTypeName(string name)
        {
            if (!factory.ContainsKey(name))
            {
                var type = Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .FirstOrDefault(z => z.Name == name);
                if (type == null)
                    throw new Exception($"Can't find type '{name}'");
                factory[name] = () => (IObject)Activator.CreateInstance(type);
            }

            return factory[name]();
        }


        private static IObject CreateCreatureBySymbol(char c)
        {
            switch (c)
            {
                case 'W':
                    return CreateCreatureByTypeName("Wall");
                case ' ':
                    return null;
                default:
                    throw new Exception($"wrong character for ICreature {c}");
            }
        }
    }
}