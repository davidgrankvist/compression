namespace Compression.Lib.Framework
{
    public class EncoderPipeline
    {
        private readonly IEncoderMiddleware middleware;

        public EncoderPipeline(IEncoderMiddleware middleware)
        {
            this.middleware = middleware;
        }

        public void Process(Stream input, Stream output)
        {
            int received;
            while ((received = input.ReadByte()) != -1)
            {
                if (middleware.Encode((byte)received, out var encoded) && encoded.HasValue)
                {
                    output.WriteByte(encoded.Value);
                }
            }

            while (middleware.Flush(out var encoded))
            {
                if (encoded.HasValue)
                {
                    output.WriteByte(encoded.Value);
                }
            }
        }
    }
}
