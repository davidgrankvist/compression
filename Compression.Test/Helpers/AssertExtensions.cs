namespace Compression.Test.Helpers
{
    internal static class AssertExtensions
    {
        public static void SequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            Assert.IsTrue(expected.SequenceEqual(actual));
        }
    }
}
