// Rob Kellett, team CRLF
// IEEEXtreme 7.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

class Lenovo
{
    static void Main(string[] args)
    {
        var numChains = int.Parse(Console.ReadLine());

        var chains = new List<string>();
        for (var i = 0; i < numChains; i++)
            chains.Add(Console.ReadLine());
            
        var alignments = Align(chains[0], chains[1]);

        Console.WriteLine("Possible Alignments: " + alignments.Count);

        var numIndices = int.Parse(Console.ReadLine());

        for (var i = 0; i < numIndices; i++)
        {
            var index = int.Parse(Console.ReadLine());

            if (index < 1 || index > alignments.Count)
            {
                Console.WriteLine("There is no alignment at position: " + index);
                continue;
            }

            Console.WriteLine("Alignment at Position: " + index);
            Output(alignments[index - 1]);
        }

        Console.ReadKey(); // Keep the console open
    }

    static List<List<Tuple<char, char>>> Align(string seq1, string seq2)
    {
        var length = seq1.Length + seq2.Length;

        var alignments1 = GoDeeper(new List<char>(), seq1, -1, seq2.Length, length);
        var alignments2 = GoDeeper(new List<char>(), seq2, -1, seq1.Length, length);

        var alignments = new List<List<Tuple<char, char>>>();

        for (var x = 0; x < alignments1.Count; x++)
        {
            for (var y = 0; y < alignments2.Count; y++)
            {
                var l = new List<Tuple<char, char>>();
                for (var z = 0; z < length; z++)
                {
                    char char1 = alignments1[x][z], char2 = alignments2[y][z];

                    if (char1 == '-' && char2 == '-')
                        continue;

                    l.Add(new Tuple<char, char>(char1, char2));
                }

                if (!IsListInList(alignments, l))
                    alignments.Add(l);
            }
        }

        alignments.RemoveAll(l => !Match(l));

        return
            alignments.OrderBy(l => l.Count).ThenBy(l => new string(l.Select(t => t.Item1).ToArray())).ThenBy(
                l => new string(l.Select(t => t.Item2).ToArray())).ToList();
    }

    static void Output(List<Tuple<char, char>> alignment)
    {
        var str1 = new string(alignment.Select(t => t.Item1).ToArray());
        var str2 = new string(alignment.Select(t => t.Item2).ToArray());

        Console.WriteLine(str1);
        Console.WriteLine(str2);
    }

    static bool Match(IEnumerable<Tuple<char, char>> alignment)
    {
        bool any = false;
        foreach (Tuple<char, char> t in alignment)
        {
            if (t.Item1 == t.Item2 || t.Item1 == 'G' || t.Item2 == 'G' || t.Item1 == '-' || t.Item2 == '-' || (t.Item1 == 'C' && t.Item2 == 'T') || (t.Item2 == 'C' && t.Item1 == 'T') || (t.Item1 == 'A' && t.Item2 == 'C') || (t.Item2 == 'A' && t.Item1 == 'C')) continue;
            any = true;
            break;
        }
        return !any;
    }

    static List<List<char>> GoDeeper(List<char> sequence, string str, int position, int left, int length)
    {
        if (position >= str.Length)
            return new List<List<char>> { sequence };

        List<List<char>> possibilities = new List<List<char>>();
        for (var i = 0; i <= left; i++)
        {
            var list = new List<char>();
            list.AddRange(sequence);
            if (position >= 0)
                list.Add(str[position]);

            for (var x = 0; x < i; x++)
                list.Add('-');

            var deeper = GoDeeper(list, str, position + 1, left - i, length);
            if (deeper != null)
                possibilities.AddRange(deeper);
        }

        possibilities.RemoveAll(c => c.Count < length);
        return possibilities;
    }

    static bool IsListInList(List<List<Tuple<char, char>>> haystack, List<Tuple<char, char>> needle)
    {
        foreach (var l in haystack)
        {
            if (l.Count != needle.Count)
                continue;

            var found = !l.Where((t, i) => !Equals(t, needle[i])).Any();

            if (found)
                return true;
        }

        return false;
    }
}
