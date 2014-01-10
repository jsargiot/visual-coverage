//
// visual-coverage: HtmlReport.cs
//
// Author:
//   Joaquin Sargiotto (joaquinsargiotto@gmail.com)
// Contributor(s):
//
//
// Copyright (c) 2012 Joaquin Sargiotto
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
namespace VisualCoverage.Core
{
    using System;
    using System.Text;
    using System.IO;
    using System.Xml;
    using System.Xml.Xsl;
    using System.Reflection;
    using VisualCoverage.Core.Elements;

    public class HtmlReport : CloverReport
    {
        public HtmlReport () : base() {
            
        }
        
        public override String Execute ( ProjectElement project )
        {
            // load our transform file out of the embedded resources
            Stream xsltStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "VisualCoverage.Core.Resources.Clover2Html.xsl");
            if (xsltStream == null) {
                throw new Exception("Missing 'Clover2Html.xsl' Resource Stream");
            }
            
            String xmlString = base.Execute(project);
            StringBuilder sbuffer = new StringBuilder();

            XmlReader xmlreader = XmlReader.Create(new StringReader(xmlString));
            XmlReader xslreader = XmlReader.Create(xsltStream);
            XmlWriter xmlwriter = XmlWriter.Create(sbuffer);
            
            XslCompiledTransform xsltProc = new XslCompiledTransform();
            xsltProc.Load(xslreader);
            xsltProc.Transform(xmlreader, xmlwriter);

            return sbuffer.ToString();
        }
    }
}
