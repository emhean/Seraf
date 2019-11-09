using System.Collections.Generic;

namespace Seraf
{
    public class Grid<T> where T : struct 
    {
        private int 
            _cols, 
            _rows;

        protected T[,] _grid;

        public Grid(int columns, int rows, int chunkSize)
        {
            _cols = columns; // x
            _rows = rows; // y
            ChunkSize = chunkSize;

            _grid = new T[columns, rows]; // x,y

            Count = columns * rows; // Readonly.
        }

        public readonly int Count;
        public int Columns => _cols;
        public int Rows => _rows;

        public int ChunkSize { get; set; }

        public bool IsIndexOutOfRange(int index)
        {
            if ( (index < 0) || ((index) < 0))
                return true;
            else return false;
        }

        /// <summary>
        /// Get object from grid. x is column, y is row.
        /// </summary>
        public T this[int column, int row]
        {
            get
            {
                return _grid[column, row];
            }

            set
            {
                _grid[column, row] = value;
            }
        }

        /// <summary>
        /// *Experimental* Enumerable chunks.
        /// </summary>
        public IEnumerable<T> GetChunk(int chunkx, int chunky)
        {
            chunkx *= ChunkSize;
            chunky *= ChunkSize;

            if (chunkx < 0)
                chunkx = 0;
            else if (chunkx > (Columns -1))
                chunkx = (Columns - ChunkSize);

            if (chunky < 0)
                chunky = 0;
            else if (chunky > (Rows - 1))
                chunky = (Rows - ChunkSize);

            for (int x = chunkx; x < (chunkx + ChunkSize); ++x)
            {
                for (int y = chunky; y < (chunky + ChunkSize); ++y)
                {
                    if (!IsIndexOutOfRange(x) && !IsIndexOutOfRange(y))
                        yield return _grid[x, y];
                }
            }
        }


        #region old garbage

        //public struct Chunk
        //{
        //    public T[,] grid;

        //    public Chunk(int chunkSize)
        //    {
        //        grid = new T[chunkSize, chunkSize];
        //    }
        //}


        //public IEnumerable<T> EnumerateElements()
        //{
        //    foreach (var e in _grid)
        //        yield return e;
        //}

        ///// <summary>
        ///// Get a chunk of columns and tiles.
        ///// </summary>
        //public IEnumerable<T> this[int chunk]
        //{
        //    get
        //    {
        //        int elements_x = Columns / ChunkSize;
        //        int elements_y = Rows / ChunkSize;

        //        int xIndex = (chunk % elements_x) * ChunkSize;
        //        int yIndex = (chunk / elements_y) * ChunkSize;

        //        for (int x = 0; x < ChunkSize; ++x)
        //        {
        //            for (int y = 0; y < ChunkSize; ++y)
        //            {
        //                yield return _grid[xIndex + x, yIndex + y];
        //            }
        //        }
        //    }

        //    set
        //    {
        //        int elements_x = Columns / ChunkSize;
        //        int elements_y = Rows / ChunkSize;

        //        int xIndex = (chunk % elements_x) * ChunkSize;
        //        int yIndex = (chunk / elements_y) * ChunkSize;

        //        for (int x = 0; x < ChunkSize; ++x)
        //        {
        //            for (int y = 0; y < ChunkSize; ++y)
        //            {
        //                _grid[xIndex + x, yIndex + y] = (T)value;
        //            }
        //        }
        //    }
        //}

        //public IEnumerable<T> GetChunk(int chunkIndex)
        //{
        //    int tiles_X = Columns / ChunkSize;
        //    int tiles_Y = Rows / ChunkSize;

        //    int xIndex = (chunkIndex % tiles_X) * ChunkSize;
        //    int yIndex = (chunkIndex / tiles_Y) * ChunkSize;

        //    for (int x = 0; x < ChunkSize; ++x)
        //    {
        //        for (int y = 0; y < ChunkSize; ++y)
        //        {
        //            yield return _grid[xIndex + x, yIndex + y];
        //        }
        //    }
        //}
        #endregion


    }
}
