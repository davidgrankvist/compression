namespace Compression.Lib.Framework
{
    public class EncoderPipelineBuilder
    {
        private IEncoderMiddleware? head;
        private IEncoderMiddleware? tail;

        public EncoderPipelineBuilder Add(IEncoderMiddleware middleware)
        {
            if (head == null)
            {
                head = middleware;
                tail = middleware;
            }
            else
            {
                if (tail is not EncoderMiddlewareBase tailBase)
                {
                    throw new InvalidOperationException($"Unable to compose encoders. The previous encoder does not inherit {nameof(EncoderMiddlewareBase)}.");
                }

                tailBase.Next = middleware;
                tail = middleware;
            }

            return this;
        }

        public EncoderPipeline Build()
        {
            if (head == null)
            {
                throw new InvalidOperationException("No middleware has been added.");
            }
            return new EncoderPipeline(head);
        }
    }
}
