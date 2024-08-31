using Compression.App.Parsing;
using Compression.App.Running;

namespace Compression.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CliRunner.Run(args, new FileOrConsoleStreamProvider(), () => Console.WriteLine(ArgumentParser.HelpText));
        }
    }
}
