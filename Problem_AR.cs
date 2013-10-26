// Rob Kellett, team CRLF
// IEEEXtreme 7.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

class Solution
{
    static void Main(string[] args)
    {
        var shows = Console.ReadLine().Split(' ').Skip(1).Select(s => int.Parse(s)).OrderBy(i => i).ToList();
        var breakArgs = Console.ReadLine().Split(' ').Skip(1).ToArray();
        var breaks = new List<Tuple<int, int>>();
        for (var i = 0; i < breakArgs.Length; i += 2)
            breaks.Add(new Tuple<int, int>(int.Parse(breakArgs[i]), int.Parse(breakArgs[i + 1])));

        var p =
            Schedule(shows, new List<Tuple<int, int>>()).Select(
                t => new Tuple<Tuple<int, int>, List<Tuple<int, int>>>(GetOverlap(t, breaks), t));

        var optimalSchedule = p.OrderByDescending(s => s.Item1.Item2).ThenBy(s => s.Item1.Item1)/*.ThenBy(t => t.Item2.Aggregate(0, (running, tuple) => running + (int)(Math.Pow(tuple.Item1, tuple.Item2))))*/.First();

        Console.WriteLine(String.Join(" ", optimalSchedule.Item2.Select(t => t.Item1)));
        if (optimalSchedule.Item1.Item1 > 0)
            Console.WriteLine("Overlap " + optimalSchedule.Item1.Item1 + " of Level " + optimalSchedule.Item1.Item2);
        else
            Console.WriteLine("Overlap Zero");

        Console.ReadKey(); // Keep the console open
    }

    static Tuple<int, int> GetOverlap(List<Tuple<int, int>> shows, List<Tuple<int, int>> breaks)
    {
        var consumedTime = 0;
        var overlap = 0;
        var lowestOverlapLevel = int.MaxValue;
        var showsClone = new List<Tuple<int, int>>();
        showsClone.AddRange(shows);

        foreach (var b in breaks)
        {
            while (consumedTime < b.Item1 && showsClone.Count > 0)
            {
                consumedTime += showsClone[0].Item1;
                showsClone.RemoveAt(0);
            }

            var thisOverlap = Math.Max(0, consumedTime - b.Item1);
            if (thisOverlap > 0 && b.Item2 < lowestOverlapLevel)
                lowestOverlapLevel = b.Item2;
            overlap += thisOverlap;
            consumedTime = 0;
        }

        return new Tuple<int, int>(overlap, lowestOverlapLevel);
    }

    static List<List<Tuple<int, int>>> Schedule(List<int> shows, List<Tuple<int, int>> soFar)
    {
        if (shows.Count == 0)
            return new List<List<Tuple<int, int>>> { soFar };

        var schedules = new List<List<Tuple<int, int>>>();

        for (var i = 0; i < shows.Count; i++)
        {
            var show = shows[i];

            var remainingShows = new List<int>();
            remainingShows.AddRange(shows);
            remainingShows.Remove(show);

            var newSoFar = new List<Tuple<int, int>>();
            newSoFar.AddRange(soFar);
            newSoFar.Add(new Tuple<int, int>(show, i));

            schedules.AddRange(Schedule(remainingShows, newSoFar));
        }

        return schedules;
    }
}