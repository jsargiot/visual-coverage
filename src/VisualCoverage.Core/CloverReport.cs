//
// visual-coverage: CloverReport.cs
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
    using VisualCoverage.Core.Elements;

    public class CloverReport
    {
        public CloverReport () {
            
        }
        
        public virtual String Execute ( ProjectElement project ) {
            
            StringBuilder buffer = new StringBuilder();

            Int32 timestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            
            buffer.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            buffer.Append(String.Format("\n<coverage generated=\"{0}\" clover=\"3.0.2\" xmlns=\"http://schemas.atlassian.com/clover3/report\">", timestamp));
            buffer.Append(String.Format("\n<project name=\"\" timestamp=\"{0}\">", timestamp));
            buffer.Append(String.Format("\n  {0}", project.Metrics.ToXml()));
            
            foreach (PackageElement pe in project.GetPackages())
            {
                buffer.Append(String.Format("\n  <package name=\"{0}\">", System.Security.SecurityElement.Escape(pe.Name)));
                buffer.Append(String.Format("\n    {0}", pe.Metrics.ToXml()));

                foreach (ClassElement ce in pe.GetClasses())
                {
                    buffer.Append(String.Format("\n    <class name=\"{0}\">", System.Security.SecurityElement.Escape(ce.Name)));
                    buffer.Append(String.Format("\n      {0}", ce.Metrics.ToXml()));

                    foreach (MethodElement me in ce.GetMethods())
                    {
                        buffer.Append(String.Format("\n      <method name=\"{0}\">", System.Security.SecurityElement.Escape(me.Name)));
                        buffer.Append(String.Format("\n        {0}", me.Metrics.ToXml()));
                        buffer.Append("\n      </method>");
                    }
                    buffer.Append("\n    </class>");
                }
                buffer.Append("\n  </package>");
            }
            buffer.Append("</project>");
            buffer.Append("</coverage>");
            
            return buffer.ToString();
        }
    }
}
