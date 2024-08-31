using Compression.App.Running;
using Compression.Lib.Plugins;

namespace Compression.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var plugins = PluginLoader.Load<ICliEncoderPlugin>();

            var runner = new CliRunner(plugins);
            runner.Run(args, new FileOrConsoleStreamProvider(), CliRunnerActions.OutputHelp, CliRunnerActions.ListEncoders);
        }
    }
}
