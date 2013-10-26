// Rob Kellett, team CRLF
// IEEEXtreme 7.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

class Solution
{
    struct Vector2
    {
        public int X;
        public int Y;
    }

    static void Main(string[] args)
    {
        var size = Console.ReadLine().Split(' ');
        var width = int.Parse(size[1]);
        var height = int.Parse(size[0]);

        var mountain = new int[width,height];

        for (var y = 0; y < height; y++)
        {
            var row = Console.ReadLine().Split(' ');
            for (var x = 0; x < width; x++)
                mountain[x, y] = int.Parse(row[x]);
        }

        List<List<Vector2>> routes = new List<List<Vector2>>();
        for (var x = 0; x < width; x++)
            routes.Add(Climb(mountain, new List<Vector2> { new Vector2 { X = x, Y = height - 1} }));

        var safest = routes.OrderBy(r => r.Sum(p => mountain[p.X, p.Y])).First();

        Console.WriteLine("Minimum risk path = " + string.Join("", safest.Select(v => "[" + v.Y + "," + v.X + "]").Reverse()));
        Console.WriteLine("Risks along the path = " + safest.Sum(p => mountain[p.X, p.Y]));

        Console.ReadKey(); // Keep the console open
    }

    static List<Vector2> Climb(int[,] mountain, List<Vector2> soFar, int riskLimit = Int32.MaxValue)
    {
        var position = soFar.Last();
        if (position.Y == 0)
            return soFar;

        var route = new List<Vector2>();
        route.AddRange(soFar);

        if (soFar.Sum(v => mountain[v.X, v.Y]) > riskLimit)
            return null;

        var riskLeft = int.MaxValue;
        var riskCenter = int.MaxValue;
        var riskRight = int.MaxValue;

        if (position.X > 0)
        {
            var leftRoute = new List<Vector2>();
            leftRoute.AddRange(route);
            leftRoute.Add(new Vector2 { X = position.X - 1, Y = position.Y - 1 });
            var l = Climb(mountain, leftRoute, riskLimit);
            if (l != null)
                riskLeft = l.Sum(p => mountain[p.X, p.Y]);
        }

        var centerRoute = new List<Vector2>();
        centerRoute.AddRange(route);
        centerRoute.Add(new Vector2{X = position.X, Y = position.Y - 1});
        var c = Climb(mountain, centerRoute, Math.Min(riskLimit, riskLeft));
        if (c != null)
            riskCenter = c.Sum(p => mountain[p.X, p.Y]);

        if (position.X < mountain.GetLength(0) - 1)
        {
            var rightRoute = new List<Vector2>();
            rightRoute.AddRange(route);
            rightRoute.Add(new Vector2 { X = position.X + 1, Y = position.Y - 1 });
            var r = Climb(mountain, rightRoute, Math.Min(riskLimit, Math.Min(riskLeft, riskCenter)));
            if (r != null)
                riskRight = r.Sum(p => mountain[p.X, p.Y]);
        }

        var lowest = Math.Min(riskLeft, Math.Min(riskCenter, riskRight));

        if (lowest == Int32.MaxValue)
            return null;

        if (riskLeft == lowest)
            route.Add(new Vector2 { X = position.X - 1, Y = position.Y - 1 });
        else if (riskCenter == lowest)
            route.Add(new Vector2 { X = position.X, Y = position.Y - 1 });
        else
            route.Add(new Vector2 { X = position.X + 1, Y = position.Y - 1 });

        return Climb(mountain, route);
    }
}