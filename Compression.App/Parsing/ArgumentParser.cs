using Compression.Lib.Framework;
using Compression.Lib.Plugins;

namespace Compression.App.Parsing
{
    public class ArgumentParser
    {
        public static readonly string HelpText = @"
Encode a stream of bytes with the specified sequence of encoders.

--input, -i Input file (default: STDIN)
--output, -o Output file (default: STDOUT)
--encoders, -e Comma separated list of encoders
--help, -h Output help text

Example:

cli --input in.txt --output out.txt --encoders rle,other
";

        private enum ArgType
        {
            Input,
            Output,
            Encoders,
            Help,
            Value,
        }

        private readonly Dictionary<string, ICliEncoderPlugin> pluginByName;

        public ArgumentParser(ICliEncoderPlugin[] plugins)
        {
            pluginByName = plugins.Select(plugin => (plugin.Name, plugin)).ToDictionary();
        }

        public bool TryParse(string[] args, out PipelineOptions options)
        {
            options = PipelineOptions.Dummy;
            if (args.Length == 0)
            {
                return false;
            }

            string? inputFile = null;
            string? outputFile = null;
            IEncoderMiddleware[]? encoders = null;
            var didParseHelp = false;

            for (var i = 1; i < args.Length; i++)
            {
                var prevArg = args[i - 1];
                var arg = args[i];

                var prevArgType = ToArgType(prevArg);

                switch (prevArgType)
                {
                    case ArgType.Input:
                        inputFile = arg;
                        break;
                    case ArgType.Output:
                        outputFile = arg;
                        break;
                    case ArgType.Encoders:
                        encoders = ToEncoders(arg.Split(","));
                        break;
                    case ArgType.Help:
                        didParseHelp = true;
                        break;
                    case ArgType.Value:
                    default:
                        break;
                }
            }
            var last = ToArgType(args[args.Length - 1]);
            if (last == ArgType.Help)
            {
                didParseHelp = true;
            }

            if (didParseHelp || encoders == null)
            {
                return false;
            }

            options = new PipelineOptions(inputFile, outputFile, encoders);
            return true;
        }

        private static ArgType ToArgType(string arg)
        {
            var result = ArgType.Value;
            switch(arg)
            {
                case "--input":
                case "-i":
                    result = ArgType.Input;
                    break;
                case "--output":
                case "-o":
                    result = ArgType.Output;
                    break;
                case "--encoders":
                case "-e":
                    result = ArgType.Encoders;
                    break;
                case "--help":
                case "-h":
                    result = ArgType.Help;
                    break;
                default:
                    result = ArgType.Value;
                    break;
            }
            return result;
        }

        private IEncoderMiddleware[]? ToEncoders(string[] encoderIds)
        {
            var encoders = (IEncoderMiddleware[])encoderIds
                .Select(ToEncoder)
                .Where(x => x != null)
                .ToArray();
            return encoders.Length > 0 ? encoders : null;
        }

        private IEncoderMiddleware? ToEncoder(string encoderId)
        {
            IEncoderMiddleware? result = null;

            if (pluginByName.TryGetValue(encoderId, out var plugin))
            {
                result = plugin.CreateEncoder();
            }
            return result;
        }
    }
}
