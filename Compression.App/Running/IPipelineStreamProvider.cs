namespace Compression.App.Running
{
    /// <summary>
    /// Creates a stream for the given file or provides a default stream.
    /// </summary>
    public interface IPipelineStreamProvider
    {
        Stream CreateInputStream(string? inputFile = null);
        Stream CreateOutputStream(string? outputFile = null);
    }
}
