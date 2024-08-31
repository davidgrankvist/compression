using Compression.Lib.Framework;

namespace Compression.Lib.Encoders
{
    public class RunLengthEncoder : EncoderMiddlewareBase
    {
        private const byte MaxCount = byte.MaxValue;

        private byte count;
        private byte? current;

        private Queue<byte> inputQueue;
        private byte? countResult;
        private byte? valueResult;

        private enum State
        {
            Counting,
            OutputCount,
            OutputValue,
            Flush,
            Done,
        }

        private State state;

        public RunLengthEncoder(IEncoderMiddleware? next = null) : base(next)
        {
            current = null;
            inputQueue = new Queue<byte>();
            countResult = null;
            valueResult = null;
        }

        protected override byte? EncodeByte(byte input)
        {
            byte? result = null;
            switch (state)
            {
                case State.Counting:
                    result = HandleCounting(input);
                    break;
                case State.OutputCount:
                    result = HandleOutputCount(input);
                    break;
                case State.OutputValue:
                    result = HandleOutputValue(input);
                    break;
                default:
                    throw new InvalidOperationException($"Unexpected state {state} while encoding");
            }

            return result;
        }

        private byte? HandleCounting(byte input)
        {
            var currentInput = input;
            if (inputQueue.Count > 0)
            {
                currentInput = inputQueue.Dequeue();
                inputQueue.Enqueue(input);
            }

            if (!current.HasValue)
            {
                current = input;
            }

            if (current == currentInput && count < MaxCount)
            {
                count++;
                return null;
            }

            countResult = count;
            valueResult = current;

            current = currentInput;
            count = 1;
            state = State.OutputCount;

            return null;
        }

        private byte? HandleOutputCount(byte input)
        {
            inputQueue.Enqueue(input);

            return OutputCount();
        }

        private byte? OutputCount()
        {
            var result = countResult;
            countResult = null;
            state = State.OutputValue;

            return result;
        }

        private byte? HandleOutputValue(byte input)
        {
            inputQueue.Enqueue(input);

            return OutputValue();
        }

        private byte? OutputValue()
        {
            var result = valueResult;
            valueResult = null;
            state = State.Counting;

            return result;
        }

        protected override byte? FlushByte()
        {
            if (state == State.Done)
            {
                return null;
            }

            if (state == State.Flush)
            {
                state = State.Done;
                return current;
            }

            // consume input queue
            byte? result = FlushOutputByte();
            if (result.HasValue)
            {
                return result;
            }

            // interrupt ongoing count (if any)
            if (state == State.Counting)
            {
                result = count;
                state = State.Flush;
            }

            return result;
        }

        protected override byte? FlushOutputByte()
        {
            if (state == State.OutputCount)
            {
                return OutputCount();
            }
            if (state == State.OutputValue)
            {
                return OutputValue();
            }

            byte? result = null;
            if (inputQueue.Count > 0)
            {
                var currentInput = inputQueue.Dequeue();
                result = EncodeByte(currentInput);
            }
            return result;
        }

        public override bool HasPendingOutput()
        {
            return state == State.OutputCount || state == State.OutputValue;
        }
    }
}
