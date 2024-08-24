namespace Compression.Lib.Framework
{
    public class EncoderPipeline
    {
        public IEncoderMiddleware Middleware { get; }

        public EncoderPipeline(IEncoderMiddleware middleware)
        {
            Middleware = middleware;
        }

        public void Process(Stream input, Stream output)
        {
            int received;
            while ((received = input.ReadByte()) != -1)
            {
                if (Middleware.Encode((byte)received, out var encoded) && encoded.HasValue)
                {
                    output.WriteByte(encoded.Value);
                }
            }

            while (Middleware.Flush(out var encoded))
            {
                if (encoded.HasValue)
                {
                    output.WriteByte(encoded.Value);
                }
            }
        }
    }
}
