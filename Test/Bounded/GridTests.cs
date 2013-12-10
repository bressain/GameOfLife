using System;
using System.Collections.Generic;
using System.Linq;
using GameOfLife.Bounded;
using NUnit.Framework;

namespace Test.Bounded
{
    [TestFixture]
    public class GridTests
    {
        [Test]
        public void Grid_is_initilized_with_size_and_seed()
        {
            var grid = new Grid(5, 5, Tuple.Create(1, 2), Tuple.Create(3, 3));

            Assert.That(grid[1, 2], Is.EqualTo(CellState.Alive));
            Assert.That(grid[3, 3], Is.EqualTo(CellState.Alive));
            Assert.That(grid[0, 0], Is.EqualTo(CellState.Dead));
        }

        [Test]
        public void Live_cell_dies_with_one_neighbor()
        {
            var grid = CreateAndGenerate(3, 3, Tuple.Create(0, 0), Tuple.Create(1, 1));
            Assert.That(grid[0, 0], Is.EqualTo(CellState.Dead));
        }

        private static Grid CreateAndGenerate(int rows, int columns, params Tuple<int, int>[] seeds)
        {
            return new Grid(rows, columns, seeds).Generate();
        }

        [Test]
        public void Live_cell_dies_with_no_neighbors()
        {
            var grid = CreateAndGenerate(3, 3, Tuple.Create(0, 0));
            Assert.That(grid[0, 0], Is.EqualTo(CellState.Dead));
        }

        [Test]
        public void Live_cell_lives_with_two_neighbors()
        {
            var grid = CreateAndGenerate(3, 3, Tuple.Create(0, 0), Tuple.Create(0, 1), Tuple.Create(1, 1));
            Assert.That(grid[0, 0], Is.EqualTo(CellState.Alive));
        }

        [Test]
        public void Live_cell_lives_with_three_neighbors()
        {
            var grid = CreateAndGenerate(3, 3, Tuple.Create(0, 0), Tuple.Create(0, 1), Tuple.Create(1, 1), Tuple.Create(1, 0));
            Assert.That(grid[0, 0], Is.EqualTo(CellState.Alive));
        }

        [Test]
        public void Live_cell_dies_with_over_three_neighbors()
        {
            var grid = CreateAndGenerate(3, 3, Tuple.Create(0, 0), Tuple.Create(0, 1), Tuple.Create(1, 0), Tuple.Create(0, 2),
                Tuple.Create(1, 1));
            Assert.That(grid[1, 1], Is.EqualTo(CellState.Dead));
        }

        [Test]
        public void Dead_cell_stays_dead_with_two_neighbors()
        {
            var grid = CreateAndGenerate(3, 3, Tuple.Create(0, 1), Tuple.Create(1, 1));
            Assert.That(grid[0, 0], Is.EqualTo(CellState.Dead));
        }

        [Test]
        public void Dead_cell_lives_with_three_neighbors()
        {
            var grid = CreateAndGenerate(3, 3, Tuple.Create(0, 1), Tuple.Create(1, 1), Tuple.Create(1, 0));
            Assert.That(grid[0, 0], Is.EqualTo(CellState.Alive));
        }

        [Test]
        public void Dead_cell_stays_dead_with_four_neighbors()
        {
            var grid = CreateAndGenerate(3, 3, Tuple.Create(0, 1), Tuple.Create(1, 1), Tuple.Create(1, 0));
            Assert.That(grid[0, 0], Is.EqualTo(CellState.Alive));
        }

        [Test]
        public void Three_generation_integration_test()
        {
            // first gen
            var grid = CreateAndGenerate(4, 5, ConvertToTuples(@"
xoxoo
oxxoo
ooxoo
ooooo"));
            Assert.That(GridIsEqual(grid, @"
ooxoo
ooxxo
oxxoo
ooooo"));

            // second gen
            grid = grid.Generate();
            Assert.That(GridIsEqual(grid, @"
ooxxo
oooxo
oxxxo
ooooo"));

            // third gen
            grid = grid.Generate();
            Assert.That(GridIsEqual(grid, @"
ooxxo
oxoox
ooxxo
ooxoo"));
        }

        private static Tuple<int, int>[] ConvertToTuples(string grid)
        {
            var tuples = new LinkedList<Tuple<int, int>>();
            var split = grid.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (int row = 0; row < split.Length; row++)
            {
                var line = split[row];
                for (int col = 0; col < line.Length; col++)
                {
                    if (line[col] == 'x')
                        tuples.AddLast(Tuple.Create(row, col));
                }
            }
            return tuples.ToArray();
        }

        private static bool GridIsEqual(Grid grid, string expected)
        {
            foreach (var cell in ConvertToTuples(expected))
            {
                if (grid[cell.Item1, cell.Item2] == CellState.Dead)
                    return false;
            }
            return true;
        }

        [Test]
        public void ToString_can_print_a_simple_one_line_grid()
        {
            var grid = new Grid(1, 5);
            Assert.That(grid.ToString(), Is.EqualTo("     "));
        }

        [Test]
        public void ToString_can_print_multiple_lines()
        {
            var grid = new Grid(3, 5);
            Assert.That(grid.ToString(), Is.EqualTo(
@"     
     
     "));
        }

        [Test]
        public void ToString_can_print_a_full_grid_with_cells()
        {
            var grid = new Grid(5, 5, Tuple.Create(1, 1), Tuple.Create(0, 3), Tuple.Create(3, 4), Tuple.Create(4, 1), Tuple.Create(1, 2));
            Assert.That(grid.ToString(), Is.EqualTo(
@"   o 
 oo  
     
    o
 o   "));
        }
    }
}
