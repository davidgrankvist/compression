# compression

Encoding library and CLI.

## About

This is an encoding system with two main parts:
1. Compression.Lib is a library for building arbitrary pipelines of encoders
2. Compression.App is a CLI where you can run a specified pipeline

### Library

The library constructs a pipeline of encoder middleware that consumes one byte at a time.
All input and output buffering is handled within specific middlewares.

Middlewares that can be used within the CLI are exposed via a plugin interface.

### CLI

The CLI loads available encoders and allows the user to build a pipeline by specifying
a middleware sequence.

```
Encode a stream of bytes with the specified sequence of encoders.

--input, -i Input file (default: STDIN)
--output, -o Output file (default: STDOUT)
--encoders, -e Comma separated list of encoders
--list, -l List available encoders
--help, -h Output help text

Example:

cli --input in.txt --output out.txt --encoders rle,other
```

### Testing

The pipeline operates on input and output streams, which are set to [MemoryStream](https://learn.microsoft.com/en-us/dotnet/api/system.io.memorystream) instances in the tests.

To test encoding details like buffering behavior, there are special test encoder middlewares.
