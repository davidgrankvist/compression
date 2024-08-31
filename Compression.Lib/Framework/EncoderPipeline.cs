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
                ConsumeInputByte((byte)received, output);
                FlushPendingOutput(output);
            }

            FlushBufferedInput(output);
        }

        private void ConsumeInputByte(byte received, Stream output)
        {
            if (Middleware.Encode(received, out var encoded) && encoded.HasValue)
            {
                output.WriteByte(encoded.Value);
            }
        }

        private void FlushPendingOutput(Stream output)
        {
            var shouldFlush = false;
            do
            {
                shouldFlush = Middleware.FlushOutput(out var flushed);
                if (flushed.HasValue)
                {
                    output.WriteByte(flushed.Value);
                }
            } while (shouldFlush);
        }

        private void FlushBufferedInput(Stream output)
        {
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
