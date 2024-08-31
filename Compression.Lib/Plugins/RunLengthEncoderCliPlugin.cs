using Compression.Lib.Encoders;
using Compression.Lib.Framework;

namespace Compression.Lib.Plugins
{
    public class RunLengthEncoderCliPlugin : ICliEncoderPlugin
    {
        public string Id => "rle";

        public string Name => "Run Length Encoder";

        public IEncoderMiddleware CreateEncoder()
        {
            return new RunLengthEncoder();
        }
    }
}
