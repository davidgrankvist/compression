using Compression.Lib.Framework;
using Compression.Lib.Encoders;
using Compression.Test.Helpers;

namespace Compression.Test
{
    [TestClass]
    public class PipelineBuilderTest
    {
        [TestMethod]
        public void ShouldThrowIfBuildingWithoutAddingMiddleware()
        {
            var builder = new EncoderPipelineBuilder();

            Assert.ThrowsException<InvalidOperationException>(builder.Build);
        }

        [TestMethod]
        public void ShouldThrowIfComposingUncomposable()
        {
            var builder = new EncoderPipelineBuilder();
            Assert.ThrowsException<InvalidOperationException>(
                () => builder
                        .Add(new StubEncoder())
                        .Add(new StubEncoder())
            );
        }

        [TestMethod]
        public void ShouldComposeInAscendingOrder()
        {
            var dummy1 = new DummyEncoder();
            var dummy2 = new DummyEncoder();
            var dummy3 = new DummyEncoder();

            var builder = new EncoderPipelineBuilder();
            var pipeline = builder
                .Add(dummy1)
                .Add(dummy2)
                .Add(dummy3)
                .Build();
            var added = PipelineTestHelpers.GetMiddleWares(pipeline.Middleware);

            AssertExtensions.SequenceEqual([dummy1, dummy2, dummy3], added);
        }
    }
}
