using Compression.App.Parsing;
using Compression.App.Running;

namespace Compression.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (ArgumentParser.TryParse(args, out var options))
            {
                PipelineRunner.Run(options, new FileOrConsoleStreamProvider());
            }
            else
            {
                Console.WriteLine(ArgumentParser.HelpText);
            }
        }
    }
}
