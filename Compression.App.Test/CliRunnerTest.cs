using Compression.App.Running;
using Compression.App.Test.Helpers;
using Compression.Lib.Plugins;

namespace Compression.App.Test
{
    [TestClass]
    public class CliRunnerTest
    {
        [TestMethod]
        public void ShouldCallDefaultActionWhenHelpIsSet()
        {
            var args = new[] { "--help" };
            var argsAbbrev = new[] { "-h" };

            var numCalls = 0;
            var defaultAction = () =>
            {
                numCalls++;
            };

            RunPipeline(args, defaultAction, []);
            RunPipeline(argsAbbrev, defaultAction, []);

            Assert.AreEqual(2, numCalls);
        }

        [TestMethod]
        public void ShouldCallDefaultActionIfParsingFails()
        {
            var args = new[] { "2p98hg", "---135135135" };

            var numCalls = 0;
            var defaultAction = () =>
            {
                numCalls++;
            };

            RunPipeline(args, defaultAction, []);

            Assert.AreEqual(1, numCalls);
        }

        [TestMethod]
        public void ShouldRunDummyEncoder()
        {
            var args = new[] { "--encoders", "dummy" };
            var argsAbbrev = new[] { "-e", "dummy" };

            var input = new byte[] { 1, 2, 3 };

            CheckPipeline(args, input, input);
            CheckPipeline(argsAbbrev, input, input);
        }

        [TestMethod]
        public void ShouldRunRle()
        {
            var args = new[] { "--encoders", "rle" };
            var argsAbbrev = new[] { "-e", "rle" };

            var input = new byte[] { 1, 1, 1, 2, 2, 2, 3, 3, 3 };
            var expected = new byte[] { 3, 1, 3, 2, 3, 3 };

            CheckPipeline(args, input, expected);
            CheckPipeline(argsAbbrev, input, expected);
        }

        [TestMethod]
        public void ShouldRunRleDecoder()
        {
            var args = new[] { "--encoders", "rle-d" };
            var argsAbbrev = new[] { "-e", "rle-d" };

            var input = new byte[] { 3, 1, 3, 2, 3, 3 };
            var expected = new byte[] { 1, 1, 1, 2, 2, 2, 3, 3, 3 };

            CheckPipeline(args, input, expected);
            CheckPipeline(argsAbbrev, input, expected);
        }

        [TestMethod]
        public void ShouldRunRleEncoderDecoder()
        {
            var args = new[] { "--encoders", "rle,rle-d" };
            var argsAbbrev = new[] { "-e", "rle,rle-d" };

            var input = new byte[] { 1, 1, 1, 2, 2, 2, 3, 3, 3 };

            CheckPipeline(args, input, input);
            CheckPipeline(argsAbbrev, input, input);
        }

        private static void CheckPipeline(string[] args, Action? defaultAction, byte[] input, byte[] expectedOutput)
        {
            var output = RunPipeline(args, defaultAction, input, expectedOutput.Length);
            Assert.IsTrue(expectedOutput.SequenceEqual(output));
        }

        private static void CheckPipeline(string[] args, byte[] input, byte[] expectedOutput)
        {
            CheckPipeline(args, null, input, expectedOutput);
        }

        private static byte[] RunPipeline(string[] args, Action? defaultAction, byte[] input, int? outputSize = null)
        {
            var outputBuffer = new byte[outputSize ?? input.Length];
            var streamProvider = new MemoryStreamProvider(input, outputBuffer);

            var cliRunner = new CliRunner(CliPluginHelpers.GetDefaultPlugins());
            cliRunner.Run(args, streamProvider, defaultAction);

            return outputBuffer;
        }
    }
}
