namespace Compression.Lib.Plugins
{
    public static class CliPluginHelpers
    {
        public static ICliEncoderPlugin[] GetDefaultPlugins()
        {
            return [
                new DummyEncoderCliPlugin(),
                new RunLengthEncoderCliPlugin(),
                new RunLengthDecoderCliPlugin(),
            ];
        }
    }
}
