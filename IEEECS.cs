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

    struct Robot
    {
        public Vector2 Position;
        public Walls Direction;
    }

    [Flags]
    enum Walls
    {
        None = 0,

        North = 1,
        South = 2,
        West = 4,
        East = 8,

        All = North | South | West | East
    }

    static Walls RemoveWall(Walls room, Walls breakDownTheWall)
    {
        return room & ~breakDownTheWall;
    }

    static long curRand;
    static long Random(long max = 0)
    {
        const long m = 4294967296;
        const long a = 1664525;
        const long c = 1013904223;

        curRand = (int)(a*curRand + c) % m;

        return max <= 0 ? curRand : Math.Abs(curRand % max + 1);
    }

    static void Main(string[] args)
    {
        curRand = int.Parse(Console.ReadLine());
        var rows = int.Parse(Console.ReadLine());
        var cols = int.Parse(Console.ReadLine());
        var wallProb = int.Parse(Console.ReadLine());
        var movesLimit = int.Parse(Console.ReadLine());

        var map = new Walls[cols, rows];
        for (var y = 0; y < rows; y++)
        {
            for (var x = 0; x < cols; x++)
                map[x, y] = Walls.All;
        }

        var startCell = Random(2*(cols - 2) + 2*(rows - 2)) - 1;
        var endCell = Random(2*(cols - 2) + 2*(rows - 2) - 1) - 1;
        Vector2 startPos = new Vector2(),
                endPos = new Vector2();

        var curBorPos = 0;
        for (var y = 0; y < rows; y++)
        {
            for (var x = 0; x < cols; x++)
            {
                if (x == 0 && y == 0 || x == 0 && y == rows - 1 || x == cols - 1 && y == 0 || x == cols - 1 && y == rows - 1)
                    continue;

                if ((y == 0 || y == (rows - 1)) || (x == 0 || x == (cols - 1)))
                {
                    if (startCell == curBorPos)
                    {
                        startPos = new Vector2 {X = x, Y = y};
                        goto NotInLoop;
                    }
                    curBorPos++;
                }
            }
        }

        throw new Exception("nope.avi");

        NotInLoop:

        curBorPos = 0;
        for (var y = 0; y < rows; y++)
        {
            for (var x = 0; x < cols; x++)
            {
                if (x == startPos.X && y == startPos.Y || x == 0 && y == 0 || x == 0 && y == rows - 1 || x == cols - 1 && y == 0 || x == cols - 1 && y == rows - 1)
                    continue;

                if ((y == 0 || y == (rows - 1)) || (x == 0 || x == (cols - 1)))
                {
                    if (endCell == curBorPos)
                    {
                        endPos = new Vector2 {X = x, Y = y};

                        if (y == 0)
                            map[x, y] = RemoveWall(map[x, y], Walls.North);
                        else if (y == rows - 1)
                            map[x, y] = RemoveWall(map[x, y], Walls.South);
                        else if (x == 0)
                            map[x, y] = RemoveWall(map[x, y], Walls.West);
                        else if (x == cols - 1)
                            map[x, y] = RemoveWall(map[x, y], Walls.East);

                        goto AlsoNotInLoop;
                    }
                    curBorPos++;
                }
            }
        }

        throw new Exception("nope.wmv");

        AlsoNotInLoop:

        var robot = new Robot { Position = startPos, Direction = Walls.East };

        for (var y = 1; y < rows - 2; y++)
        {
            for (var x = 1; x < cols - 2; x++)
            {
                var roll = Random(4);

                switch (roll)
                {
                    case 1:
                        map[x, y] = RemoveWall(map[x, y], Walls.North);
                        map[x, y - 1] = RemoveWall(map[x, y - 1], Walls.South);
                        break;
                    case 2:
                        map[x, y] = RemoveWall(map[x, y], Walls.South);
                        map[x, y + 1] = RemoveWall(map[x, y + 1], Walls.North);
                        break;
                    case 3:
                        map[x, y] = RemoveWall(map[x, y], Walls.West);
                        map[x - 1, y] = RemoveWall(map[x - 1, y], Walls.East);
                        break;
                    case 4:
                        map[x, y] = RemoveWall(map[x, y], Walls.East);
                        map[x + 1, y] = RemoveWall(map[x + 1, y], Walls.West);
                        break;
                }
            }
        }

        int moves = 0;
        while (moves < movesLimit)
        {
            var currentRoom = map[robot.Position.X, robot.Position.Y];

            // Move forward?
            if (currentRoom.HasFlag(TurnRight(robot.Direction)) && !currentRoom.HasFlag(robot.Direction))
            {
                robot.Position = Step(robot.Position, robot.Direction);

                if (robot.Position.X == endPos.X && robot.Position.Y == endPos.Y)
                    goto TerroristsWin; // WOO
            }
            else
                robot.Direction = TurnRight(robot.Direction);

            moves++;

            FuckShitUp(map, rows, cols, wallProb);
        }

        Console.WriteLine("Robbie was trapped in the maze.");

        goto End;

        TerroristsWin:

        Console.WriteLine("Robbie got out of the maze in " + moves + " moves.");

        End:

        Console.ReadKey(); // Keep the console open
    }

    static Vector2 Step(Vector2 position, Walls direction)
    {
        switch (direction)
        {
            case Walls.North:
                return new Vector2 { X = position.X, Y = position.Y - 1 };
            case Walls.East:
                return new Vector2 { X = position.X + 1, Y = position.Y };
            case Walls.South:
                return new Vector2 { X = position.X, Y = position.Y + 1 };
            case Walls.West:
                return new Vector2 { X = position.X - 1, Y = position.Y };
        }

        throw new Exception(); // Like, yoinks, Scoob!
    }

    static Walls TurnRight(Walls direction)
    {
        switch (direction)
        {
            case Walls.North:
                return Walls.East;
            case Walls.East:
                return Walls.South;
            case Walls.South:
                return Walls.West;
            case Walls.West:
                return Walls.North;
        }

        throw new Exception(); // RUH ROH
    }

    static void FuckShitUp(Walls[,] map, int rows, int cols, int wallProbability)
    {
        for (var y = 1; y < rows - 1; y++)
        {
            for (var x = 1; x < cols - 1; x++)
            {
                // north
                if (Random(100) < wallProbability)
                {
                    map[x, y] |= Walls.North;
                    map[x, y - 1] |= Walls.South;
                }
                else
                {
                    map[x, y] = RemoveWall(map[x, y], Walls.North);
                    map[x, y - 1] = RemoveWall(map[x, y - 1], Walls.South);
                }

                // south
                if (Random(100) < wallProbability)
                {
                    map[x, y] |= Walls.South;
                    map[x, y + 1] |= Walls.North;
                }
                else
                {
                    map[x, y] = RemoveWall(map[x, y], Walls.South);
                    map[x, y + 1] = RemoveWall(map[x, y + 1], Walls.North);
                }

                // west
                if (Random(100) < wallProbability)
                {
                    map[x, y] |= Walls.West;
                    map[x - 1, y] |= Walls.East;
                }
                else
                {
                    map[x, y] = RemoveWall(map[x, y], Walls.West);
                    map[x - 1, y] = RemoveWall(map[x - 1, y], Walls.East);
                }

                // east
                if (Random(100) < wallProbability)
                {
                    map[x, y] |= Walls.East;
                    map[x + 1, y] |= Walls.West;
                }
                else
                {
                    map[x, y] = RemoveWall(map[x, y], Walls.East);
                    map[x + 1, y] = RemoveWall(map[x + 1, y], Walls.West);
                }
            }
        }

        //return map;
    }
}
