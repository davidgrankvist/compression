using Compression.App.Parsing;

namespace Compression.App.Running
{
    public static class CliRunner
    {
        public static void Run(string[] args, IPipelineStreamProvider streamProvider, Action? defaultAction = null)
        {
            if (ArgumentParser.TryParse(args, out var options))
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
