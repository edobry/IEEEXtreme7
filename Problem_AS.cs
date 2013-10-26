// Rob Kellett, team CRLF
// IEEEXtreme 7.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

using Map = System.Collections.Generic.Dictionary<char, System.Collections.Generic.List<char>>;
using Route = System.Collections.Generic.List<char>;

class Solution
{

    static void Main(string[] args)
    {
        var map = new Map();

        var destination = Console.ReadLine().Single();

        string input;
        while ((input = Console.ReadLine()) != "A A")
        {
            var xasd = input.Split(' ');
            var block1 = xasd[0].Single();
            var block2 = xasd[1].Single();

            if (!map.ContainsKey(block1))
                map.Add(block1, new List<char>());
            if (!map[block1].Contains(block2))
                map[block1].Add(block2);

            if (!map.ContainsKey(block2))
                map.Add(block2, new List<char>());
            if (!map[block2].Contains(block1))
                map[block2].Add(block1);
        }

        var routes = FindRoutes(map, new Route { 'F' }, destination);

        if (routes.Count > 0)
        {
            Console.WriteLine("Total Routes: " + routes.Count);

            var shortest = routes.OrderBy(l => l.Count).ThenBy(l => new string(l.ToArray())).First();
            Console.WriteLine("Shortest Route Length: " + shortest.Count);
            Console.WriteLine("Shortest Route after Sorting of Routes of length " + shortest.Count + ": " + string.Join(" ", shortest));
        }
        else
            Console.WriteLine("No Route Available from F to " + destination);

        Console.ReadKey(); // Keep the console open
    }

    static List<Route> FindRoutes(Map map, Route soFar, char end)
    {
        var here = soFar.Last();

        if (here == end)
            return new List<Route> { soFar };

        var routes = new List<Route>();

        foreach (var next in map[here])
        {
            if (soFar.Contains(next))
                continue;

            var curr = new Route();
            curr.AddRange(soFar);
            curr.Add(next);

            routes.AddRange(FindRoutes(map, curr, end));
        }

        return routes;
    }
}
