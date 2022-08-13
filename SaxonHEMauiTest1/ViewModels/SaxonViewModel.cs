using System.ComponentModel;
using net.sf.saxon.s9api;

namespace SaxonHEMauiTest1.ViewModels
{
    public class SaxonViewModel : INotifyPropertyChanged
    {
        private readonly SaxonSingleton saxonModel = SaxonSingleton.Instance;

        public event PropertyChangedEventHandler PropertyChanged;

        public SaxonViewModel()
        {
            TestCurrentDateTimeValue = new Command(() => { XPathCurrentDateTimeTest = TestCurrentDateTime(); });
            TestXdmAtomicValue = new Command(() => { XdmAtomicValueTest = new XdmAtomicValue(DateTime.Now.ToLongTimeString()).getStringValue(); });
            TestCallTemplateTransformationResult = new Command(() => { XdmTransformationResult = RunTestCallTemplate(); });
        }

        public string SaxonProductTitle
        {
            get
            {
                return $"Saxon {saxonModel.SaxonProcessor.getSaxonEdition()} {saxonModel.SaxonProcessor.getSaxonProductVersion()}";
            }
        }

        private string TestCurrentDateTime()
        {
            return XdmFunctionItem.getSystemFunction(saxonModel.SaxonProcessor, new QName("http://www.w3.org/2005/xpath-functions", "current-dateTime"), 0).call(saxonModel.SaxonProcessor, new XdmValue[] { } ).itemAt(0).getStringValue();
        }

        private string currentDateTimeValue = "Not tested!";
        public string XPathCurrentDateTimeTest
        {
            get
            {
                return currentDateTimeValue;
            }
            set
            {
                if (currentDateTimeValue != value)
                {
                    currentDateTimeValue = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(XPathCurrentDateTimeTest)));
                }
            }
        }

        private string xdmAtomicValue = "Not tested!";
        public string XdmAtomicValueTest
        {
            get
            {
                return xdmAtomicValue;
                ;
            }
            set
            {
                if (xdmAtomicValue != value)
                {
                    xdmAtomicValue = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(XdmAtomicValueTest)));
                }
            }
        }

        private string xdmTransformationResult = "Not tested!";
        public string XdmTransformationResult
        {
            get
            {
                return xdmTransformationResult;
            }
            set
            {
                if (xdmTransformationResult != value)
                {
                    xdmTransformationResult = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(XdmTransformationResult)));
                }
            }
        }

        public Command TestCurrentDateTimeValue { get; }
        public Command TestXdmAtomicValue { get; }
        public Command TestCallTemplateTransformationResult { get; }

        private string RunTestCallTemplate()
        {
            XsltCompiler xsltCompiler = saxonModel.SaxonProcessor.newXsltCompiler();

            XsltExecutable xsltExecutable;

            string xsltCode = @"<xsl:stylesheet xmlns:xsl='http://www.w3.org/1999/XSL/Transform' version='3.0' expand-text='yes'>
  <xsl:template name='xsl:initial-template'>
    <Test>Run with {system-property('xsl:product-name')} {system-property('xsl:product-version')}</Test>
  </xsl:template>
</xsl:stylesheet>";

            //using (var stringReader = new StringReader(xsltCode))
            //{
            //xsltExecutable = xsltCompiler.Compile(stringReader);
            xsltExecutable = xsltCompiler.compile(new javax.xml.transform.stream.StreamSource(new java.io.StringReader(xsltCode)));
            //}

            using (var resultWriter = new java.io.StringWriter())
            {
                
                var xslt30Transformer = xsltExecutable.load30();
                xslt30Transformer.callTemplate(null, saxonModel.SaxonProcessor.newSerializer(resultWriter));
                return resultWriter.toString();
            }
        }

    }
}
