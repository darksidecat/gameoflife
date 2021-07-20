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
        public static void Frame(byte[,] field)
        {
           

            for (int i = 0; i < field.GetLength(0); i++)
            {
                string frame = string.Empty;
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    frame += field[i, j] == 1 ? ((char)Cell.ALIVE) : ((char)Cell.DEAD);
                }
                Console.WriteLine(frame);
            }

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

            // Console.SetCursorPosition(0, 0);
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
                Frame(field.FieldCurrent);
                Console.Write($"\nR - refresh, Z - faster, X - slower, C - comfort\n" +
                    $"Fill: {config.FillPercent}%     \nSleep time:{config.UpdateTime}     \nFramerate: {1.0f / frameTime}     ");
                field.UpdateField();

            }
        }

    }

    enum Cell
    {
        ALIVE = '#',
        DEAD = ' '
    }

    public ref struct Config
    {
        private float _fillPercent;
        private int _height;
        private int _width;
        private int _updateTime;


        public float FillPercent
        {
            get => _fillPercent;
            set
            {
                _fillPercent = value;
            }
        }
        public int Height
        {
            get => _height;
            set
            {
                _height = value;
            }
        }
        public int Width
        {
            get => _width;
            set
            {
                _width = value;
            }
        }
        public int UpdateTime
        {
            get => _updateTime;
            set
            {
                if (value < 0)
                {
                    _updateTime = 0;
                }
                else
                {
                    _updateTime = value;
                }
            }
        }

        public Config(float fillPercent) : this(fillPercent, 500)
        {
        }
        public Config(float fillPercent, int updateTime) : this(fillPercent, updateTime, 50, 200)
        {
        }
        public Config(float fillPercent, int updateTime, int height, int width)
        {
            _fillPercent = fillPercent;
            _updateTime = updateTime;
            _height = height;
            _width = width;
        }
    }


}
