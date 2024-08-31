using Compression.Lib.Framework;
using Compression.Lib.Encoders;

namespace Compression.Lib.Plugins
{
    internal class DummyEncoderCliPlugin : ICliEncoderPlugin
    {
        public string Name => "dummy";

        public IEncoderMiddleware CreateEncoder()
        {
            return new DummyEncoder();
        }
    }
}
