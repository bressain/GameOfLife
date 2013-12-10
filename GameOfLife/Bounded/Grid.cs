using System;
using System.Text;

namespace GameOfLife.Bounded
{
    public class Grid
    {
        private readonly int columns;
        private readonly int rows;
        private readonly CellState[,] grid;

        public Grid(int rows, int columns, params Tuple<int, int>[] seeds)
        {
            this.rows = rows;
            this.columns = columns;

            grid = new CellState[rows, columns];
            seeds.Each(x => grid[x.Item1, x.Item2] = CellState.Alive);
        }

        public CellState this[int row, int col]
        {
            get { return grid[row, col]; }
            private set { grid[row, col] = value; }
        }

        public Grid Generate()
        {
            var generation = new Grid(rows, columns);
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    switch (GetLiveNeighborCount(row, col))
                    {
                        case 2:
                            generation[row, col] = this[row, col];
                            break;
                        case 3:
                            generation[row, col] = CellState.Alive;
                            break;
                    }
                }
            }
            return generation;
        }

        private int GetLiveNeighborCount(int row, int col)
        {
            return OneWhenAlive(row - 1, col - 1) +
                   OneWhenAlive(row - 1, col) +
                   OneWhenAlive(row - 1, col + 1) +
                   OneWhenAlive(row, col - 1) +
                   OneWhenAlive(row, col + 1) +
                   OneWhenAlive(row + 1, col - 1) +
                   OneWhenAlive(row + 1, col) +
                   OneWhenAlive(row + 1, col + 1);
        }

        private int OneWhenAlive(int row, int col)
        {
            return IsInsideGrid(row, col) && this[row, col] == CellState.Alive ? 1 : 0;
        }

        private bool IsInsideGrid(int row, int col)
        {
            return row >= 0 && col >= 0 && row < rows && col < columns;
        }

        public override string ToString()
        {
            var grid = new StringBuilder();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    if (this[row, col] == CellState.Dead)
                    {
                        grid.Append(' ');
                    }
                    else
                    {
                        grid.Append('o');
                    }
                }
                if (row + 1 < rows)
                    grid.Append(Environment.NewLine);
            }
            return grid.ToString();
        }
    }
}