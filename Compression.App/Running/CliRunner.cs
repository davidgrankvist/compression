using Compression.App.Parsing;
using Compression.Lib.Plugins;

namespace Compression.App.Running
{
    public class CliRunner
    {
        private readonly ArgumentParser parser;
        private readonly ICliEncoderPlugin[] plugins;

        public CliRunner(ICliEncoderPlugin[] plugins)
        {
            this.plugins = plugins;
            parser = new ArgumentParser(plugins);
        }

        public void Run(string[] args, IPipelineStreamProvider streamProvider, Action? defaultAction = null, Action<ICliEncoderPlugin[]>? listEncoders = null)
        {
            if (parser.TryParse(args, out var result))
            {
                PipelineRunner.Run(result.Options, streamProvider);
            }
            else
            {
                switch (result.Mode)
                {
                    case ParserOutputMode.List:
                        listEncoders?.Invoke(plugins);
                        break;
                    default:
                        defaultAction?.Invoke();
                        break;
                }
            }
        }
    }
}
