using Compression.Lib.Framework;
using Compression.Test.Encoders;
using Compression.Test.Helpers;

namespace Compression.Test
{
    [TestClass]
    public class DummyEncoderPipelineTest
    {
        [TestMethod]
        public void ShouldEncodeAsSameData()
        {
            var dummyEncoder = new DummyEncoder();
            var pipeline = new EncoderPipeline(dummyEncoder);

            var input = new byte[] { 1, 2, 3 };
            PipelineTestHelpers.CheckPipeline(pipeline, input, input);
        }

        [TestMethod]
        public void ShouldEncodeAsSameDataWhenMultiple()
        {
            var multiDummyEncoder = new DummyEncoder(new DummyEncoder(new DummyEncoder()));
            var pipeline = new EncoderPipeline(multiDummyEncoder);

            var input = new byte[] { 1, 2, 3 };
            PipelineTestHelpers.CheckPipeline(pipeline, input, input);
        }
    }
}
