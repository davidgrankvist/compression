using Compression.App.Parsing;

namespace Compression.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (ArgumentParser.TryParse(args, out var options))
            {
                PipelineRunner.Run(options);
            }
            else
            {
                Console.WriteLine(ArgumentParser.HelpText);
            }
        }
    }
}
