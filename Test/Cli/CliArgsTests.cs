using System;
using GameOfLife.Cli;
using NUnit.Framework;

namespace Test.Cli
{
    [TestFixture]
    public class CliArgsTests
    {
        [Test]
        public void Args_are_parsed_on_construction()
        {
            var args = new CliArgs(new[] { "6", "5", "1-1", "3-4", "0-3" });
            Assert.That(args.Width, Is.EqualTo(6));
            Assert.That(args.Height, Is.EqualTo(5));
            Assert.That(args.Seeds, Has.Member(Tuple.Create(1, 1)));
            Assert.That(args.Seeds, Has.Member(Tuple.Create(3, 4)));
            Assert.That(args.Seeds, Has.Member(Tuple.Create(0, 3)));
        }

        [Test]
        public void Seeds_are_optional()
        {
            var args = new CliArgs(new[] { "3", "4" });
            Assert.That(args.Width, Is.EqualTo(3));
            Assert.That(args.Height, Is.EqualTo(4));
            Assert.That(args.Seeds, Is.Empty);
        }

        [Test]
        public void Inputs_are_assumed_to_be_valid_otherwise_exception()
        {
            Assert.Throws<FormatException>(() => new CliArgs(new[] { "4", "lol" }));
        }

        [Test]
        public void Seeds_have_to_be_formatted_correctly()
        {
            Assert.Throws<FormatException>(() => new CliArgs(new[] { "3", "4", "4|5" }));
        }
    }
}
