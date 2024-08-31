using Compression.Lib.Encoders;
using Compression.Lib.Framework;
using Compression.Test.Helpers;

namespace Compression.Test
{
    [TestClass]
    public class RunLengthEncoderTest
    {
        [TestMethod]
        public void ShouldEncodeRepetitions()
        {
            var input = new byte[] { 1, 1, 1, 2, 2, 2, 3, 3, 3 };
            var expected = new byte[] { 3, 1, 3, 2, 3, 3 };

            var pipeline = new EncoderPipeline(new RunLengthEncoder());

            PipelineTestHelpers.CheckPipeline(pipeline, input, expected);
        }

        [TestMethod]
        public void ShouldEncodeUnique()
        {
            var input = new byte[] { 2, 3, 4 };
            var expected = new byte[] { 1, 2, 1, 3, 1, 4 };

            var pipeline = new EncoderPipeline(new RunLengthEncoder());

            PipelineTestHelpers.CheckPipeline(pipeline, input, expected);
        }

        [TestMethod]
        public void ShouldDecodeRepetitions()
        {
            var input = new byte[] { 3, 1, 3, 2, 3, 3 };
            var expected = new byte[] { 1, 1, 1, 2, 2, 2, 3, 3, 3 };

            var pipeline = new EncoderPipeline(new RunLengthDecoder());

            PipelineTestHelpers.CheckPipeline(pipeline, input, expected);
        }

        [TestMethod]
        public void ShouldDecodeUnique()
        {
            var input = new byte[] { 1, 2, 1, 3, 1, 4 };
            var expected = new byte[] { 2, 3, 4 };

            var pipeline = new EncoderPipeline(new RunLengthDecoder());

            PipelineTestHelpers.CheckPipeline(pipeline, input, expected);
        }

        [TestMethod]
        public void ShouldEncodeDecodeRepetitions()
        {
            var input = new byte[] { 1, 1, 1, 2, 2, 2, 3, 3, 3 };

            var pipeline = new EncoderPipelineBuilder()
                .Add(new RunLengthEncoder())
                .Add(new RunLengthDecoder())
                .Build();

            PipelineTestHelpers.CheckPipeline(pipeline, input, input);
        }

        [TestMethod]
        public void ShouldEncodeDecodeUnique()
        {
            var input = new byte[] { 2, 3, 4 };

            var pipeline = new EncoderPipelineBuilder()
                .Add(new RunLengthEncoder())
                .Add(new RunLengthDecoder())
                .Build();

            PipelineTestHelpers.CheckPipeline(pipeline, input, input);
        }
    }
}
