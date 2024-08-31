using Compression.App.Parsing;
using Compression.App.Running;
using Compression.App.Test.Helpers;
using Compression.Lib.Encoders;
using Compression.Test.Encoders;

namespace Compression.App.Test
{
    [TestClass]
    public class PipelineRunnerTest
    {
        [TestMethod]
        public void ShouldUseGivenFileStreams()
        {
            var inputFile = "input.txt";
            var outputFile = "output.txt";
            var options = new PipelineOptions(inputFile, outputFile, [new DummyEncoder()]);
            var input = new byte[1];
            var output = new byte[1];
            var streamProvider = new MemoryStreamProvider(input, output);

            PipelineRunner.Run(options, streamProvider);

            Assert.AreEqual(inputFile, streamProvider.InputFile);
            Assert.AreEqual(outputFile, streamProvider.OutputFile);
        }

        [TestMethod]
        public void ShouldRunDummyEncoder()
        {
            var options = new PipelineOptions(null, null, [new DummyEncoder()]);
            var input = new byte[] { 1, 2, 3 };

            CheckPipeline(options, input, input);
        }


        [TestMethod]
        public void ShouldRunMulipleDummyEncoders()
        {
            var options = new PipelineOptions(null, null, [new DummyEncoder(), new DummyEncoder(), new DummyEncoder()]);
            var input = new byte[] { 1, 2, 3 };

            CheckPipeline(options, input, input);
        }

        [TestMethod]
        public void ShouldRunRleEncoder()
        {
            var options = new PipelineOptions(null, null, [new RunLengthEncoder()]);
            var input = new byte[] { 1, 1, 1, 2, 2, 2, 3, 3, 3 };
            var expected = new byte[] { 3, 1, 3, 2, 3, 3 };

            CheckPipeline(options, input, expected);
        }

        private static void CheckPipeline(PipelineOptions options, byte[] input, byte[] expectedOutput)
        {
            var output = RunPipeline(options, input, expectedOutput.Length);
            Assert.IsTrue(expectedOutput.SequenceEqual(output));
        }

        private static byte[] RunPipeline(PipelineOptions options, byte[] input, int? outputSize)
        {
            var outputBuffer = new byte[outputSize ?? input.Length];
            var streamProvider = new MemoryStreamProvider(input, outputBuffer);

            PipelineRunner.Run(options, streamProvider);

            return outputBuffer;
        }
    }
}
