// Rob Kellett, team CRLF
// IEEEXtreme 7.0

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;

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
        var ba = new BitArray(new[] { x });
        var inPad = true;
        var realEnd = 0;
        for (var i = ba.Length - 1; i >= 0; i--)
        {
            if (inPad)
            {
                if (ba[i])
                {
                    inPad = false;
                    realEnd = i;
                }
                else continue;
            }

            if (ba[i] != ba[realEnd - i])
                return false;
        }

        return true;
    }
}
