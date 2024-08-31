using Compression.Lib.Framework;

namespace Compression.Lib.Plugins
{
    public interface ICliEncoderPlugin
    {
        public string Id { get; }

        public string Name { get; }

        public IEncoderMiddleware CreateEncoder();
    }
}
