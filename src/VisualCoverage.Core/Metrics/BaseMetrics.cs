//
// visual-coverage: BaseMetrics.cs
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
    using System.Text;
    using System.Collections.Generic;

    public class BaseMetrics
    {
        protected Dictionary<String, uint> _metrics = new Dictionary<String, uint>();

        public BaseMetrics ()
        {}
        
        public void SetMetric ( String name, uint value )
        {
            if (!_metrics.ContainsKey(name))
                _metrics.Add(name, value);
            else
                _metrics[name] = value;
        }
        
        public void Add(BaseMetrics bm)
        {
            foreach (var key in bm._metrics.Keys)
            {
                var value = bm._metrics[key];

                if (_metrics.ContainsKey(key))
                {
                    value += _metrics[key];
                }

                SetMetric(key, value);
            }
        }
        
        public virtual String ToXml ()
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append("<metrics ");
            foreach( KeyValuePair<string, uint> kvp in _metrics )
            {
                buffer.Append(String.Format(" {0}=\"{1}\"", kvp.Key, kvp.Value));
            }
            buffer.Append(" />");
            return buffer.ToString();
        }
        
    }
}
