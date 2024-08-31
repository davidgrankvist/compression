using Compression.Lib.Framework;
using Compression.Lib.Encoders;

namespace Compression.Lib.Plugins
{
    public class DummyEncoderCliPlugin : ICliEncoderPlugin
    {
        public string Id => "dummy";

        public string Name => "Dummy encoder";

        public IEncoderMiddleware CreateEncoder()
        {
            return new DummyEncoder();
        }
    }
}
