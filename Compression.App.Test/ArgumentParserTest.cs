using Compression.App.Parsing;
using Compression.Lib.Encoders;
using Compression.Lib.Plugins;

namespace Compression.App.Test
{
    [TestClass]
    public class ArgumentParserTest
    {
        [TestMethod]
        public void ShouldNotParseEmpty()
        {
            var input = Array.Empty<string>();

            CheckParsing(input, false);
        }

        [TestMethod]
        public void ShouldNotParseUnsupported()
        {
            var input = new[] { "068094876", "--9831513", "-26246", "135153", "1535" };

            CheckParsing(input, false);
        }

        [TestMethod]
        public void ShouldNotParseEmptyEncoderList()
        {
            var input = new[] { "--encoders" };
            var inputAbbrev = new[] { "-e" };

            CheckParsing(input, false);
            CheckParsing(inputAbbrev, false);
        }

        [TestMethod]
        public void ShouldParseSupportedEncoder()
        {
            var input = new[] { "--encoders", "dummy" };
            var inputAbbrev = new[] { "-e", "dummy" };

            var expectedOptions = new PipelineOptions(null, null, [new DummyEncoder()]);
            var expectedOutputMode = ParserOutputMode.Encode;

            CheckParsing(input, true, expectedOptions, expectedOutputMode);
            CheckParsing(inputAbbrev, true, expectedOptions, expectedOutputMode);
        }

        [TestMethod]
        public void ShouldParseSupportedEncoderMultiple()
        {
            var input = new[] { "--encoders", "dummy,dummy,dummy" };
            var inputAbbrev = new[] { "-e", "dummy,dummy,dummy" };

            var expectedOptions = new PipelineOptions(null, null, [new DummyEncoder(), new DummyEncoder(), new DummyEncoder()]);
            var expectedOutputMode = ParserOutputMode.Encode;

            CheckParsing(input, true, expectedOptions, expectedOutputMode);
            CheckParsing(inputAbbrev, true, expectedOptions, expectedOutputMode);
        }

        [TestMethod]
        public void ShouldParseInputFile()
        {
            var fileName = "input.txt";
            var input = new[] { "--input", fileName, "--encoders", "dummy" };
            var inputAbbrev = new[] { "-i", fileName, "-e", "dummy" };

            var expectedOptions = new PipelineOptions(fileName, null, [new DummyEncoder()]);
            var expectedOutputMode = ParserOutputMode.Encode;

            CheckParsing(input, true, expectedOptions, expectedOutputMode);
            CheckParsing(inputAbbrev, true, expectedOptions, expectedOutputMode);
        }

        [TestMethod]
        public void ShouldParseOutputFile()
        {
            var fileName = "output.txt";
            var input = new[] { "--output", fileName, "--encoders", "dummy" };
            var inputAbbrev = new[] { "-o", fileName, "-e", "dummy" };

            var expectedOptions = new PipelineOptions(null, fileName, [new DummyEncoder()]);
            var expectedOutputMode = ParserOutputMode.Encode;

            CheckParsing(input, true, expectedOptions, expectedOutputMode);
            CheckParsing(inputAbbrev, true, expectedOptions, expectedOutputMode);
        }

        [TestMethod]
        public void ShouldParseInputOutputFiles()
        {
            var inputFileName = "input.txt";
            var outputFileName = "output.txt";
            var input = new[] { "--input", inputFileName, "--output", outputFileName, "--encoders", "dummy" };
            var inputAbbrev = new[] { "-i", inputFileName, "-o", outputFileName, "-e", "dummy" };

            var expectedOptions = new PipelineOptions(inputFileName, outputFileName, [new DummyEncoder()]);
            var expectedOutputMode = ParserOutputMode.Encode;

            CheckParsing(input, true, expectedOptions, expectedOutputMode);
            CheckParsing(inputAbbrev, true, expectedOptions, expectedOutputMode);
        }

        [TestMethod]
        public void ShouldNotParseIfHelpIsSet()
        {
            var inputFileName = "input.txt";
            var outputFileName = "output.txt";
            var input = new[] { "--help", "--input", inputFileName, "--output", outputFileName, "--encoders", "dummy" };
            var inputAbbrev = new[] { "-h", "-i", inputFileName, "-o", outputFileName, "-e", "dummy" };

            CheckParsing(input, false);
            CheckParsing(inputAbbrev, false);
        }

        [TestMethod]
        public void ShouldOutputList()
        {
            var input = new[] { "--list" };
            var inputAbbrev = new[] { "-l" };

            CheckParsing(input, false, null, ParserOutputMode.List);
            CheckParsing(inputAbbrev, false, null, ParserOutputMode.List);
        }

        [TestMethod]
        public void ShouldNotParseIfListIsSet()
        {
            var inputFileName = "input.txt";
            var outputFileName = "output.txt";
            var input = new[] { "--list", "--input", inputFileName, "--output", outputFileName, "--encoders", "dummy" };
            var inputAbbrev = new[] { "-l", "-i", inputFileName, "-o", outputFileName, "-e", "dummy" };

            CheckParsing(input, false, null, ParserOutputMode.List);
            CheckParsing(inputAbbrev, false, null, ParserOutputMode.List);
        }

        [TestMethod]
        public void ShouldOutputHelpIfHelpAndListAreSet()
        {
            var input = new[] { "--help", "--list" };
            var inputAbbrev = new[] { "-h", "-l" };

            CheckParsing(input, false);
            CheckParsing(inputAbbrev, false);
        }

        private static void CheckParsing(string[] input, bool expectedDidParse, PipelineOptions? expectedOptions = null, ParserOutputMode? expectedOutputmode = ParserOutputMode.Help)
        {
            var parser = new ArgumentParser(CliPluginHelpers.GetDefaultPlugins());
            var didParse = parser.TryParse(input, out var result);

            Assert.AreEqual(expectedDidParse, didParse);
            Assert.AreEqual(expectedOutputmode, result.Mode);

            var expectedOpts = expectedOptions ?? PipelineOptions.Dummy;
            var options = result.Options;
            AssertEqualOptions(expectedOpts, options);
        }

        private static void AssertEqualOptions(PipelineOptions expected, PipelineOptions actual)
        {
            Assert.AreEqual(expected.InputFile, actual.InputFile);
            Assert.AreEqual(expected.OutputFile, actual.OutputFile);

            Assert.AreEqual(expected.Encoders.Length, actual.Encoders.Length);
            for (var i = 0; i < expected.Encoders.Length; i++)
            {
                var expectedEncoder = expected.Encoders[i];
                var resultEncoder = actual.Encoders[i];

                // TODO(improvement): consider encoder parameters
                Assert.AreEqual(expectedEncoder.GetType(), resultEncoder.GetType());
            }
        }
    }
}