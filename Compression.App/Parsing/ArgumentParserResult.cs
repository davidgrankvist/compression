namespace Compression.App.Parsing
{
    public class ArgumentParserResult
    {
        public PipelineOptions Options { get; }

        public ParserOutputMode Mode { get; }

        public ArgumentParserResult(PipelineOptions options, ParserOutputMode outputMode)
        {
            Options = options;
            Mode = outputMode;
        }
    }
}
