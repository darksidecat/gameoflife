using System;


namespace GameOfLife
{
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

        public Config(float fillPercent) : this(fillPercent, 500, 45, 180)
        {
        }
        public Config(float fillPercent, int updateTime) : this(fillPercent, updateTime, 45, 180)
        {
        }
        public Config(float fillPercent, int updateTime, int height = 45, int width = 180)
        {
            _fillPercent = fillPercent;
            _updateTime = updateTime;
            _height = height;
            _width = width;
        }
    }


    class Field
    {
        private readonly byte Alive = 1;
        private readonly byte Dead = 0;
        private readonly int Height;
        private readonly int Width;
        private byte[,] FieldNow;
        private byte[,] FieldNext;

        public byte[,] FieldCurrent
        {
            get => FieldNow;
        }

        public Field() : this(10, 10)
        {
        }
        public Field(int height, int width)
        {
            Height = height;
            Width = width;
            FieldNow = new byte[height, width];
            FieldNext = new byte[height, width];
        }

        public byte[,] FillRandom(float fillPercent)
        {
            Random rnd = new();

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    FieldNow[i, j] = rnd.NextDouble() <= fillPercent ? Alive : Dead;
                }
            }
            return FieldNow;
        }

        public void UpdateField()
        {
            for (int i = 1; i < Height - 1; i++)
            {
                for (int j = 1; j < Width - 1; j++)
                {
                    bool isAlive = FieldNow[i, j] == 1;

                    byte numNeigbours =
                        (byte)
                        (FieldNow[i - 1, j] +
                        FieldNow[i - 1, j - 1] +
                        FieldNow[i - 1, j + 1] +
                        FieldNow[i, j - 1] +
                        FieldNow[i + 1, j] +
                        FieldNow[i + 1, j + 1] +
                        FieldNow[i + 1, j - 1] +
                        FieldNow[i, j + 1]);

                    bool stayAlive = isAlive && (numNeigbours == 2 || numNeigbours == 3);
                    bool born = !isAlive && numNeigbours == 3;

                    FieldNext[i, j] = stayAlive | born ? Alive : Dead;

                }
            }
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    FieldNow[i, j] = FieldNext[i, j];
                }
            }
        }

    }

}
