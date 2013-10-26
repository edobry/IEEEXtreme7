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
    static bool o(int[] s, int stopAt)
    {
        var l = stopAt;

        int num;

        var vals = new HashSet<int>();

        for (var a = 0; a < stopAt; a++)
        {
            for (var b = 0; b < stopAt; b++)
            {
                for (var c = 0; c < stopAt; c++)
                {
                    vals.Add(s[a] + s[b] + s[c]);
                }
            }
        }

        num = vals.Count;

        //var intermid = s.SelectMany(a => s.SelectMany(b => s.Select(c => a + b + c))).Distinct();

        return num == (l*(l + 1)*(l + 2)/6);
    }

    static void Main(string[] args)
    {
        var M = int.Parse(Console.ReadLine());
        var N = (int)Math.Pow(3, M);
        var i = 1;
        var s = Enumerable.Repeat(i, M).ToArray();

        while (i != 0)
        {
            if (s[i]-N != 0)
            {
                s[i]++;
                if (o(s, i+1))
                {
                    if (i < M - 1)
                    {
                        i++;
                        s[i] = s[i - 1];
                    }
                    else
                        N = s.Last();
                }
            }
            else
                i--;
        }

        Console.WriteLine(N);

        Console.ReadKey(); // Keep the console open
    }
}