using System;
using System.Collections.Generic;

namespace GameOfLife.Cli
{
    public class CliArgs
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public IList<Tuple<int, int>> Seeds { get; set; }

        public CliArgs(string[] args)
        {
            Width = int.Parse(args[0]);
            Height = int.Parse(args[1]);
            Seeds = new List<Tuple<int, int>>();
            for (int i = 2; i < args.Length; i++)
            {
                Seeds.Add(ParseSeed(args[i]));
            }
        }

        private static Tuple<int, int> ParseSeed(string value)
        {
            var split = value.Split('-');
            return Tuple.Create(int.Parse(split[0].Trim()), int.Parse(split[1].Trim()));
        }
    }
}
