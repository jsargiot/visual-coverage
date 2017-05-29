//
// visual-coverage: ClassElement.cs
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
namespace VisualCoverage.Core.Elements
{
    using System;
    using System.Collections.Generic;
    using VisualCoverage.Core.Metrics;

    public class ClassElement
    {
        private string _name = "";
        private List<MethodElement> _elements = new List<MethodElement>();

        public ClassElement(string name) {
            _name = name;
        }
        
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public ClassMetrics Metrics
        {
            get
            {
                // File metrics are calculated based on the classes
                // inside this file.
                ClassMetrics pm = new ClassMetrics(0, 0, 0, 0, (uint)_elements.Count);
                foreach (MethodElement me in GetMethods())
                {
                    pm.Add(me.Metrics);
                }
                return pm;
            }
        }

        public void AddMethod(MethodElement e)
        {
            _elements.Add(e);
        }

        public List<MethodElement> GetMethods()
        {
            return _elements;
        }
    }
}