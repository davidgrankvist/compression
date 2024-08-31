using Compression.App.Parsing;
using Compression.App.Running;
using Compression.Lib.Plugins;

namespace Compression.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // TODO(improvement): load dynamically
            var plugins = CliPluginHelpers.GetDefaultPlugins();

            var runner = new CliRunner(plugins);
            runner.Run(args, new FileOrConsoleStreamProvider(), () => Console.WriteLine(ArgumentParser.HelpText));
        }
    }
}
