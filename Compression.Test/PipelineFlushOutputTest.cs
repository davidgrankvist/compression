using Compression.Lib.Framework;
using Compression.Lib.Encoders;
using Compression.Test.Helpers;
using Compression.Test.TestEncoders;

namespace Compression.Test
{
    [TestClass]
    public class PipelineFlushOutputTest
    {
        [TestMethod]
        public void ShouldOutputValuesInOrder()
        {
            var input = new byte[] { 1, 2, 3 };
            var expected = new byte[] { 1, 1, 2, 2, 3, 3 };

            var pipeline = new EncoderPipeline(new DummyOutputBufferEncoder());

            PipelineTestHelpers.CheckPipeline(pipeline, input, expected);
        }

        [TestMethod]
        public void ShouldFlushDelegatedOutput()
        {
            var input = new byte[] { 1, 2, 3 };
            var expected = new byte[] { 1, 1, 2, 2, 3, 3 };

            var pipeline = new EncoderPipelineBuilder()
                .Add(new DummyEncoder())
                .Add(new DummyOutputBufferEncoder())
                .Build();

            PipelineTestHelpers.CheckPipeline(pipeline, input, expected);
        }

        [TestMethod]
        public void ShouldCombineWithInputFlush()
        {
            byte target = 1;
            var input = new byte[] { 1, 2, 3 };
            var expected = new byte[] { 2, 2, 3, 3, 1, 1 };

            var pipeline = new EncoderPipelineBuilder()
                .Add(new DummyOutputBufferEncoder())
                .Add(new DummyBufferEncoder(target))
                .Build();

            PipelineTestHelpers.CheckPipeline(pipeline, input, expected);
        }
    }
}
