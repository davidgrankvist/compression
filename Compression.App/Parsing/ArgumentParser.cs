namespace Compression.App.Parsing
{
    internal static class ArgumentParser
    {
        private static readonly PipelineOptions DummyOptions = new PipelineOptions(null, null, []);

        public static readonly string HelpText = @"
Encode a stream of bytes with the specified sequence of encoders.

--input, -i Input file (default: STDIN)
--outout, -o Output file (default: STDOUT)
--encoders, -e Comma separated list of encoders
--help, -h Output help text

Example:

cli --input in.txt --output out.txt --encoders rle,other
";

        public static bool TryParse(string[] args, out PipelineOptions options)
        {
            // TODO(incomplete)
            options = DummyOptions;
            return false;
        }
    }
}
