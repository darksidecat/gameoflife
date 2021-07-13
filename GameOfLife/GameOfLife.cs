using System;


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
            field.Render(field, config);
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

    }

    public ref struct Config
    {
        public float FillPercent;
        private int _updateTime;
        public int Height;
        public int Width;

        public int UpdateTime
        {
            get
            {
                return _updateTime;
            }
            set
            {
                if (value <= 0)
                {
                    _updateTime = 1;
                }
                else
                {
                    _updateTime = value;
                }
            }
        }

        public Config(float fillPercent) : this(fillPercent, 501, 45, 180)
        {
            FillPercent = fillPercent;
        }
        public Config(float fillPercent, int updateTime, int height = 45, int width = 180)
        {
            FillPercent = fillPercent;
            _updateTime = updateTime;
            Height = height;
            Width = width;
        }
    }


    enum Cell
    {
        ALIVE = '#',
        DEAD = ' '
    }


    class Field
    {
        readonly byte Alive = 1;
        readonly byte Dead = 0;
        private readonly int Height;
        private readonly int Width;
        private byte[,] Field_now;
        private byte[,] Field_next;

        public Field()
        {
            Height = 10;
            Width = 10;
            Field_now = new byte[Height, Width];
            Field_next = new byte[Height, Width];

        }
        public Field(int height, int width)
        {
            Height = height;
            Width = width;
            Field_now = new byte[height, width];
            Field_next = new byte[height, width];
        }

        public byte[,] FillRandom(float fillPercent)
        {
            Random rnd = new Random();

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Field_now[i, j] = rnd.NextDouble() <= fillPercent ? Alive : Dead;
                }
            }
            return Field_now;
        }

        public void UpdateField()
        {
            for (int i = 1; i < Height - 1; i++)
            {
                for (int j = 1; j < Width - 1; j++)
                {
                    bool isAlive = Field_now[i, j] == 1;

                    byte numNeigbours =
                        (byte)
                        (Field_now[i - 1, j] +
                        Field_now[i - 1, j - 1] +
                        Field_now[i - 1, j + 1] +
                        Field_now[i, j - 1] +
                        Field_now[i + 1, j] +
                        Field_now[i + 1, j + 1] +
                        Field_now[i + 1, j - 1] +
                        Field_now[i, j + 1]);

                    bool stayAlive = isAlive && (numNeigbours == 2 || numNeigbours == 3);
                    bool born = !isAlive && numNeigbours == 3;

                    Field_next[i, j] = stayAlive | born ? Alive : Dead;

                }
            }
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Field_now[i, j] = Field_next[i, j];
                }
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

        public void Render(Field field, Config config)
        {
            float frameTime = 0;
            int frameCounter = 0;
            DateTime old = DateTime.Now;

            while (true)
            {
                frameCounter += 1;
                if (frameCounter % 10 == 0)
                {
                    DateTime now = DateTime.Now;
                    frameTime = (float)now.Subtract(old).TotalSeconds / 10.0f;
                    old = now;
                }
                if (Console.KeyAvailable == true)
                {
                    ConsoleKeyInfo ski = Console.ReadKey(true);
                    if (ski.Key == ConsoleKey.R)
                    {
                        field.FillRandom(config.FillPercent);
                    }
                    else if (ski.Key == ConsoleKey.X)
                    {
                        config.UpdateTime += 100;
                    }
                    else if (ski.Key == ConsoleKey.Z)
                    {
                        config.UpdateTime -= 100;
                    }

                    else if (ski.Key == ConsoleKey.C)
                    {
                        config.UpdateTime = 17;
                    }
                }

                if (config.UpdateTime != 1) System.Threading.Thread.Sleep(config.UpdateTime);
                Console.SetCursorPosition(0, 0);
                Console.Write(Frame(Field_now));
                Console.Write($"\nR - refresh, Z - faster, X - slower, C - comfort\n" +
                    $"Fill: {config.FillPercent}%     \nSleep time:{config.UpdateTime}     \nFramerate: {1.0f / frameTime}     ");
                UpdateField();
            }
        }

    }


}
