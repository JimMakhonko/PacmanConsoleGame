using System;
using System.IO;

namespace Pacman
{
    class PlayerMovement
    {
        public static void Movement(char[,] map, char symbol, ref int X, ref int Y, int DX, int DY)
        {
            Console.SetCursorPosition(Y, X);
            Console.Write(map[X, Y]);
            Y += DY;
            X += DX;
            Console.SetCursorPosition(Y, X);
            Console.Write(symbol);
        }
    }
    class PlayerDirection
    {
        public static void PlayerAutoDirection(ref int DX, ref int DY, ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    DY = 0; DX = -1;
                    break;
                case ConsoleKey.DownArrow:
                    DY = 0; DX = 1;
                    break;
                case ConsoleKey.RightArrow:
                    DY = 1; DX = 0;
                    break;
                case ConsoleKey.LeftArrow:
                    DY = -1; DX = 0;
                    break;
            }
        }
    }
    class EnemyDirection
    {
        public static void EnemyChangeDrection(Random random, ref int DX, ref int DY)
        {
            int randomGhostDirection = random.Next(0, 5);
            switch (randomGhostDirection)
            {
                case 1:
                    DY = 0; DX = -1;
                    break;
                case 2:
                    DY = 0; DX = 1;
                    break;
                case 3:
                    DY = 1; DX = 0;
                    break;
                case 4:
                    DY = -1; DX = 0;
                    break;
            }
        }
    }
    class LoadMap
    {
        public static char[,] ReadMap(string mapName, out int pacmanX, out int pacmanY, ref int allDots, out int ghostX, out int ghostY)
        {
            pacmanX = 0;
            pacmanY = 0;
            ghostX = 0;
            ghostY = 0;
            string[] newFile = File.ReadAllLines($"Maps/{mapName}.txt");
            char[,] map = new char[newFile.Length, newFile[0].Length];
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = newFile[i][j];
                    if (map[i, j] == '@')
                    {
                        pacmanX = i;
                        pacmanY = j;
                    }
                    else if (map[i, j] == '$')
                    {
                        ghostX = i;
                        ghostY = j;
                        map[i, j] = '.';
                    }
                    else if (map[i, j] == ' ')
                    {
                        map[i, j] = '.';
                        allDots++;
                    }
                }
            }
            return map;
        }
    }
    class DisplayMap
    {
        public static void DrawMap(char[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
    class CollectedDots
    {
        public static int CollectDots(int pacmanX, int pacmanY, int collectedDots, char[,] map)
        {
            if (map[pacmanX, pacmanY] == '.')
            {
                collectedDots++;
                map[pacmanX, pacmanY] = ' ';
            }
            return collectedDots;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Random random = new Random();
            bool isPlaying = true;
            bool isAlive = true;
            int pacmanX, pacmanY;
            int pacmanDirX = 0, pacmanDirY = 1;
            int ghostX, ghostY;
            int ghostDirX = 0, ghostDirY = -1;
            int allDots = 0;
            int collectedDots = 0;

            char[,] map = LoadMap.ReadMap("Map1", out pacmanX, out pacmanY, ref allDots, out ghostX, out ghostY);
            DisplayMap.DrawMap(map);

            while (isPlaying)
            {
                Console.SetCursorPosition(0, 30);
                Console.WriteLine($"Collected: {collectedDots}/{allDots}");
                if (Console.KeyAvailable) //Get the input when the key is pressed
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    PlayerDirection.PlayerAutoDirection(ref pacmanDirX, ref pacmanDirY, key);
                }
                if (map[pacmanX + pacmanDirX, pacmanY + pacmanDirY] != '#')
                {
                    collectedDots = CollectedDots.CollectDots(pacmanX, pacmanY, collectedDots, map);
                    PlayerMovement.Movement(map, '@', ref pacmanX, ref pacmanY, pacmanDirX, pacmanDirY);
                }
                if (map[ghostX + ghostDirX, ghostY + ghostDirY] != '#')
                {
                    PlayerMovement.Movement(map, '$', ref ghostX, ref ghostY, ghostDirX, ghostDirY);
                }
                else
                {
                    EnemyDirection.EnemyChangeDrection(random, ref ghostDirX, ref ghostDirY);
                }
                System.Threading.Thread.Sleep(50); // change the speed reduce the number
                if (ghostX == pacmanX && ghostY == pacmanY)
                {
                    isAlive = false;
                }
                if (collectedDots == allDots || isAlive == false)
                {
                    isPlaying = false;
                }
            }
            Console.SetCursorPosition(0, 28);
            if (collectedDots == allDots)
            {
                Console.Write("You Won!");
            }
            else if (!isAlive)
            {
                Console.WriteLine("GAME OVER. You've been eaten!");
            }
            Console.ReadKey();
        }
    }
}