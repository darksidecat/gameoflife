using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    class GameOfLife
    {
        static void Main(string[] args)
        {
            Config config = new(0.15f);
            ParseArgs(args, ref config);

            Field field = new(config.Height, config.Width);
            field.FillRandom(config.FillPercent);
            Render(field, config);
        }
        static void ParseArgs(string[] args, ref Config config)
        {
            if (args.Length >= 1 && float.TryParse(args[0], out _))
            {
                config.FillPercent = float.Parse(args[0]);
            }
            if (args.Length >= 2 && int.TryParse(args[1], out _))
            {
                config.UpdateTime = int.Parse(args[1]);
            }
            if (args.Length >= 3 && int.TryParse(args[2], out _))
            {
                config.Height = int.Parse(args[2]);
            }
            if (args.Length >= 4 && int.TryParse(args[3], out _))
            {
                config.Width = int.Parse(args[3]);
            }
        }
        public static string Frame(byte[,] field)
        {
            string frame = string.Empty;

            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    frame += field[i, j] == 1 ? ((char)Cell.ALIVE) : ((char)Cell.DEAD);
                }
                frame += '\n';
            }

            return frame;
        }
        public static void ReadInput(ref Field field, ref Config config)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo ski = Console.ReadKey(true);
                switch (ski.Key)
                {
                    case ConsoleKey.R:
                        field.FillRandom(config.FillPercent);
                        break;
                    case ConsoleKey.X:
                        config.UpdateTime += 100;
                        break;
                    case ConsoleKey.Z:
                        config.UpdateTime -= 100;
                        break;
                    case ConsoleKey.C:
                        config.UpdateTime = 20;
                        break;
                }
            }
        }

        public static void Render(Field field, Config config)
        {
            float frameTime = 0;
            int frameCounter = 0;
            DateTime old = DateTime.Now;
            Console.Title = "GameOfLife";

            while (true)
            {
                Console.CursorVisible = false;

                frameCounter += 1;
                if (frameCounter % 10 == 0)
                {
                    DateTime now = DateTime.Now;
                    frameTime = (float)now.Subtract(old).TotalSeconds / 10.0f;
                    old = now;
                }

                ReadInput(ref field, ref config);

                if (config.UpdateTime != 1) System.Threading.Thread.Sleep(config.UpdateTime);
                Console.SetCursorPosition(0, 0);
                Console.Write(Frame(field.FieldCurrent));
                Console.Write($"\nR - refresh, Z - faster, X - slower, C - comfort\n" +
                    $"Fill: {config.FillPercent}%     \nSleep time:{config.UpdateTime}     \nFramerate: {1.0f / frameTime}     ");
                field.UpdateField();
            }
        }

    }

    enum Cell
    {
        ALIVE = '■',
        DEAD = ' '
    }


}
