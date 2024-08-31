using Compression.Lib.Encoders;
using Compression.Lib.Framework;

namespace Compression.Lib.Plugins
{
    public class RunLengthDecoderCliPlugin : ICliEncoderPlugin
    {
        public string Id => "rle-d";

        public string Name => "Run Length Decoder";

        public IEncoderMiddleware CreateEncoder()
        {
            return new RunLengthDecoder();
        }
    }
}
