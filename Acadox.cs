// Rob Kellett, team CRLF
// IEEEXtreme 7.0

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

class Solution
{
    enum Operator
    {
        Add,
        Subtract,
        And,
        Or,
        Not,
        Xor
    }

    private static Dictionary<char, Operator> operators;

    static void Main(string[] args)
    {
        operators = new Dictionary<char, Operator>
                        {
                            {'+', Operator.Add},
                            {'-', Operator.Subtract},
                            {'&', Operator.And},
                            {'|', Operator.Or},
                            {'~', Operator.Not},
                            {'X', Operator.Xor}
                        };

        string input;
        while ((input = Console.ReadLine()) != null)
            Parse(input);

        Console.ReadKey(); // Keep the console open
    }

    static void Parse(string input)
    {
        input = input.Replace("~", "0 ~");

        try
        {
            var args = input.Split(' ');

            var inputs = new List<Tuple<ushort, Operator>>();
            for (var i = 1; i < args.Length; i += 2)
                inputs.Add(new Tuple<ushort, Operator>(ushort.Parse(args[i], NumberStyles.AllowHexSpecifier),
                                                       operators[args[i + 1].Single()]));

            var op = Operator.Add;

            Console.WriteLine("{0:X4}",
                              inputs.Aggregate(ushort.Parse(args.First(), NumberStyles.AllowHexSpecifier),
                                               (first, tuple) => Evaluate(first, tuple.Item1, tuple.Item2)));
        }
        catch (Exception)
        {
            Console.WriteLine("ERROR");
        }
    }

    static ushort Evaluate(ushort first, ushort second, Operator op)
    {
        switch (op)
        {
            case Operator.Add:
                return (ushort)Math.Min(ushort.MaxValue, first + second);
            case Operator.Subtract:
                return (ushort)Math.Max(ushort.MinValue, first - second);
            case Operator.And:
                return (ushort)(first & second);
            case Operator.Or:
                return (ushort)(first | second);
            case Operator.Not:
                return (ushort)(~first);
            case Operator.Xor:
                return (ushort)(first ^ second);
        }

        throw new Exception();
    }
}
