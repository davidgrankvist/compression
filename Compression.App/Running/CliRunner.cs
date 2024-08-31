using Compression.App.Parsing;
using Compression.Lib.Plugins;

namespace Compression.App.Running
{
    public class CliRunner
    {
        private readonly ArgumentParser parser;

        public CliRunner(ICliEncoderPlugin[] plugins)
        {
            parser = new ArgumentParser(plugins);
        }

        public void Run(string[] args, IPipelineStreamProvider streamProvider, Action? defaultAction = null)
        {
            if (parser.TryParse(args, out var options))
            {
                PipelineRunner.Run(options, streamProvider);
            }
            else
            {
                defaultAction?.Invoke();
            }
        }
    }
}
