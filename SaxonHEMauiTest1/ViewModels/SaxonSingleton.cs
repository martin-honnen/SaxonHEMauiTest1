using net.sf.saxon.s9api;

namespace SaxonHEMauiTest1.ViewModels
{
    public sealed class SaxonSingleton
    {
        private static readonly Lazy<SaxonSingleton> lazy =
            new Lazy<SaxonSingleton>(() => new SaxonSingleton());

        public static SaxonSingleton Instance { get { return lazy.Value; } }

        private SaxonSingleton()
        {
            SaxonProcessor = new Processor(false);
        }

        public Processor SaxonProcessor { get; }
    }
}
