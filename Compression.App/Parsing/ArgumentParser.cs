using Compression.Lib.Encoders;
using Compression.Lib.Framework;

namespace Compression.App.Parsing
{
    internal static class ArgumentParser
    {
        private static readonly PipelineOptions DummyOptions = new PipelineOptions(null, null, []);

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

        public static bool TryParse(string[] args, out PipelineOptions options)
        {
            options = DummyOptions;

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

        private static IEncoderMiddleware[]? ToEncoders(string[] encoderIds)
        {
            var encoders = (IEncoderMiddleware[])encoderIds
                .Select(ToEncoder)
                .Where(x => x != null)
                .ToArray();
            return encoders.Length > 0 ? encoders : null;
        }

        private static IEncoderMiddleware? ToEncoder(string encoderId)
        {
            // TODO(improvement): load encoders dynamically from lib and show options in help text
            IEncoderMiddleware? result = null;
            switch (encoderId)
            {
                case "rle":
                    result = new RunLengthEncoder();
                    break;
                case "rle-d":
                    result = new RunLengthDecoder();
                    break;
                default:
                    result = null;
                    break;
            }
            return result;
        }
    }
}
