using Compression.Lib.Framework;

namespace Compression.Lib.Encoders
{
    public class RunLengthDecoder : EncoderMiddlewareBase
    {
        private byte currentCount;
        private byte currentValue;

        private Queue<byte> inputQueue;

        private State state;

        private enum State
        {
            ReadCount,
            ReadValue,
            OutputValue,
        }

        public RunLengthDecoder()
        {
            inputQueue = new Queue<byte>();
        }

        protected override byte? EncodeByte(byte input)
        {
            byte? result = null;
            switch (state)
            {
                case State.ReadCount:
                    result = HandleReadCount(input);
                    break;
                case State.ReadValue:
                    result = HandleReadValue(input);
                    break;
                case State.OutputValue:
                    result = HandleOutputValue(input);
                    break;
                default:
                    throw new InvalidOperationException($"Unexpected state {state} when encoding");
            }

            return result;
        }

        private byte? HandleReadCount(byte input)
        {
            var currentInput = input;
            if (inputQueue.Count > 0)
            {
                currentInput = inputQueue.Dequeue();
                inputQueue.Enqueue(input);
            }

            return ReadCount(currentInput);
        }

        private byte? ReadCount(byte input)
        {
            currentCount = input;
            state = State.ReadValue;

            return null;
        }

        private byte? HandleReadValue(byte input)
        {
            var currentInput = input;
            if (inputQueue.Count > 0)
            {
                currentInput = inputQueue.Dequeue();
                inputQueue.Enqueue(input);
            }

            return ReadValue(currentInput);
        }

        private byte? ReadValue(byte input)
        {
            currentValue = input;
            state = State.OutputValue;

            return null;
        }

        private byte? HandleOutputValue(byte input)
        {
            inputQueue.Enqueue(input);
            return OutputValue();
        }

        private byte? OutputValue()
        {
            byte? result = null;
            if (currentCount > 0)
            {
                currentCount--;
                result = currentValue;

                if (currentCount == 0)
                {
                    state = State.ReadCount;
                }
            }
            else
            {
                state = State.ReadCount;
            }

            return result;
        }

        protected override byte? FlushByte()
        {
            byte? result = null;

            // make multiple state transitions at once, since the flush ends when returning null

            if (state == State.ReadCount && inputQueue.Count > 0)
            {
                result = FlushOutputByte();
            }

            if (state == State.ReadValue && inputQueue.Count > 0)
            {
                result = FlushOutputByte();
            }

            if (state == State.OutputValue && currentCount > 0)
            {
                result = FlushOutputByte();
            }
            return result;
        }

        private byte? FlushImmediate(byte input)
        {
            byte? result = null;
            switch (state)
            {
                case State.ReadCount:
                    result = ReadCount(input);
                    break;
                case State.ReadValue:
                    result = ReadValue(input);
                    break;
                default:
                    throw new InvalidOperationException($"Unexpected state {state} when flushing byte");
            }

            return result;
        }

        protected override byte? FlushOutputByte()
        {
            byte? result = null;
            if (state == State.OutputValue && currentCount > 0)
            {
                result = OutputValue();
            }
            else if (inputQueue.Count > 0)
            {
                var input = inputQueue.Dequeue();
                result = FlushImmediate(input);
            }

            return result;
        }

        public override bool HasPendingOutput()
        {
            return inputQueue.Count > 0 || (state == State.OutputValue && currentCount > 0);
        }
    }
}
