//
// visual-coverage: ClassMetrics.cs
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
namespace VisualCoverage.Core.Metrics
{
    using System;

    public class ClassMetrics : BaseMetrics
    {
        public ClassMetrics()
        {}
    
        public ClassMetrics ( uint complexity, uint statements, uint covered_statements, uint conditionals, uint covered_conditionals, uint methods, uint covered_methods ) {
            SetMetric("complexity", complexity);
            SetMetric("conditionals", conditionals);
            SetMetric("coveredconditionals", covered_conditionals);
            SetMetric("statements", statements);
            SetMetric("coveredstatements", covered_statements);
            SetMetric("methods", methods);
            SetMetric("coveredmethods", covered_methods);
        }
        
        public uint Elements
        {
            get { return _metrics["conditionals"] + _metrics["statements"] + _metrics["methods"]; }
        }
        
        public uint CoveredElements
        {
            get { return _metrics["coveredconditionals"] + _metrics["coveredstatements"] + _metrics["coveredmethods"]; }
        }
        
        public override String ToXml ()
        {
            SetMetric("elements", Elements);
            SetMetric("coveredelements", CoveredElements);

            return base.ToXml();
        }
        
    }
}
