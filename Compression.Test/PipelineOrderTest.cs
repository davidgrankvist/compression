using Compression.Lib.Framework;
using Compression.Test.Helpers;
using Compression.Test.TestEncoders;

namespace Compression.Test
{
    [TestClass]
    public class PipelineOrderTest
    {
        [TestMethod]
        public void ShouldApplyMiddlewareInOrder()
        {
            var input = new byte[] { 1, 2, 3 };
            var middleware = new ConstantEncoder(4,
                new SpyEncoder(input.Length,
                    new ConstantEncoder(5,
                        new SpyEncoder(input.Length,
                            new ConstantEncoder(6,
                                new SpyEncoder(input.Length)
                            )
                        )
                    )
                )
            );
            var pipeline = new EncoderPipeline(middleware);

            PipelineTestHelpers.RunPipeline(pipeline, input);

            AssertConstantEncodingAppliedInOrder(pipeline, input.Length);
        }

        private static void AssertConstantEncodingAppliedInOrder(EncoderPipeline pipeline, int spyBufferSize)
        {
            var constantEncoders = PipelineTestHelpers.GetMiddleWares(pipeline.Middleware).OfType<ConstantEncoder>().ToArray();
            var spies = PipelineTestHelpers.GetMiddleWares(pipeline.Middleware).OfType<SpyEncoder>().ToArray();

            Assert.AreEqual(constantEncoders.Length, spies.Length);

            for (var i = 0; i < constantEncoders.Length; i++)
            {
                var c = constantEncoders[i].Value;
                var expectedSpyBuffer = Enumerable.Repeat(c, spyBufferSize);
                AssertExtensions.SequenceEqual(expectedSpyBuffer, spies[i].Buffer);
            }
        }

        [TestMethod]
        public void ShouldApplyMiddlewareInOrderWhenUsingBuilder()
        {
            var input = new byte[] { 1, 2, 3 };
            var pipeline = new EncoderPipelineBuilder()
                .Add(new ConstantEncoder(4))
                .Add(new SpyEncoder(input.Length))
                .Add(new ConstantEncoder(5))
                .Add(new SpyEncoder(input.Length))
                .Add(new ConstantEncoder(6))
                .Add(new SpyEncoder(input.Length))
                .Build();

            PipelineTestHelpers.RunPipeline(pipeline, input);

            AssertConstantEncodingAppliedInOrder(pipeline, input.Length);
        }
    }
}
