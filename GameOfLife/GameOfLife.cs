using System;


namespace GameOfLife
{
    class Field
    {
        private const byte Alive = 1;
        private const byte Dead = 0;
        private readonly int _width;
        private readonly int _height;
        private readonly byte[,] _fieldNow;

        public byte[,] FieldCurrent
        {
            get => _fieldNow;
        }

        public Field(int width, int height)
        {
            _width = width;
            _height = height;
            _fieldNow = new byte[width, height];
        }

        public void FillRandom(float fillPercent)
        {
            Random rnd = new();

            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    _fieldNow[i, j] = (rnd.NextDouble() <= fillPercent) ? Alive : Dead;
                }
            }
        }

        public void UpdateField()
        {

            var FieldNext = new byte[_width, _height];

            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    bool isAlive = _fieldNow[i, j] == 1;

                    byte numNeigbours =
                        (byte)
                        (_fieldNow[(i - 1 + _width) % _width, j] +
                        _fieldNow[(i - 1 + _width) % _width, (j - 1 + _height) % _height] +
                        _fieldNow[(i - 1 + _width) % _width, (j + 1 + _height) % _height] +
                        _fieldNow[i, (j - 1 + _height) % _height] +
                        _fieldNow[(i + 1 + _width) % _width, j] +
                        _fieldNow[(i + 1 + _width) % _width, (j + 1 + _height) % _height] +
                        _fieldNow[(i + 1 + _width) % _width, (j - 1 + _height) % _height] +
                        _fieldNow[i, (j + 1 + _height) % _height]);

                    bool stayAlive = isAlive && (numNeigbours == 2 || numNeigbours == 3);
                    bool born = !isAlive && numNeigbours == 3;

                    FieldNext[i, j] = stayAlive || born ? Alive : Dead;

                }
            }

            Array.Copy(FieldNext, _fieldNow, FieldNext.Length);

        }

    }

}
