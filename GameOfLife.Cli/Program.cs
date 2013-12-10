using System;
using System.Linq;
using System.Threading;
using GameOfLife.Bounded;

namespace GameOfLife.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var cliArgs = ValidateAndCreateArgs(args);
            if (cliArgs == null) return;
            
            var grid = new Grid(cliArgs.Height, cliArgs.Width, cliArgs.Seeds.ToArray());
            Console.WriteLine("Press any key to stop");
            HideCursorAndRun(() =>
                {
                    while (true)
                    {
                        if (Console.KeyAvailable)
                            break;
                        PrintGrid(grid);
                        grid = grid.Generate();
                        Thread.Sleep(800);
                    }
                });
        }

        private static CliArgs ValidateAndCreateArgs(string[] args)
        {
            if (args.Length < 3)
            {
                PrintUsage();
                return null;
            }

            try
            {
                return new CliArgs(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                PrintUsage();
                return null;
            }
        }

        private static void HideCursorAndRun(Action action)
        {
            try
            {
                Console.CursorVisible = false;
                action();
            }
            finally
            {
                Console.CursorVisible = true;
            }
        }

        private static void PrintGrid(Grid grid)
        {
            var gridLines = grid.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in gridLines)
            {
                Console.Write(line);
                Console.SetCursorPosition(0, Console.CursorTop + 1);
            }
            Console.SetCursorPosition(0, Console.CursorTop - gridLines.Length);
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usaged: GameOfLife.Cli.exe [width] [height] [seeds]");
            Console.WriteLine("  width: width of the grid");
            Console.WriteLine("  height: height of the grid");
            Console.WriteLine("  seeds: seed points, e.g. 1-1 2-3 4-5");
            Console.WriteLine("e.g. GameOfLife.Clie.exe 5 5 1-1 3-4 2-2");
        }
    }
}
