// Rob Kellett, team CRLF
// IEEEXtreme 7.0

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

class IEEEHKN
{
    static void Main(string[] args)
    {
        var inputs = Console.ReadLine().Split(',').Select(int.Parse).ToArray();

        Console.WriteLine(Enumerable.Range(inputs[0], inputs[1] - inputs[0] + 1).Count(IsPalindrome));

        Console.ReadKey(); // Keep the console open
    }

    static bool IsPalindrome(int x)
    {
        var ba = new BitArray(new[] {x});
        var bits = new bool[ba.Length];
        ba.CopyTo(bits, 0);

        bits = bits.Reverse().SkipWhile(b => !b).ToArray();

        for (var i = 0; i < bits.Length / 2; i++)
        {
            if (bits[i] != bits[bits.Length - i - 1])
                return false;
        }

        return true;
    }
}
