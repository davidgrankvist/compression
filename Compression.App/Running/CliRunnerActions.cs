using Compression.App.Parsing;
using Compression.Lib.Plugins;

namespace Compression.App.Running
{
    internal static class CliRunnerActions
    {
        public static void OutputHelp()
        {
            Console.Write(ArgumentParser.HelpText);
        }

        public static void ListEncoders(ICliEncoderPlugin[] plugins)
        {
            var result = "Available encoders:\n";

            foreach (var plugin in plugins.OrderBy(p => p.Id))
            {
                result += $"    - {plugin.Name} ({plugin.Id})\n";
            }

            Console.Write(result);
        }
    }
}
