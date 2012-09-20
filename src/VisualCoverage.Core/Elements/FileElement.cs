//
// visual-coverage: FileElement.cs
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
    using System.IO;
    using VisualCoverage.Core.Metrics;

    public class FileElement
    {
        private string _name = "";
        private string _path = "";
        private List<ClassElement> _classes = new List<ClassElement>();
        private List<LineElement> _lines = new List<LineElement>();
    
        public FileElement ( string name, string path ) {
            _name = name;
            _path = path;
        }
        
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }
        
        public String Path
        {
            get { return _path; }
            set { _path = value; }
        }
        
        public FileMetrics Metrics
        {
            get {
                // File metrics are calculated based on the classes
                // inside this file. Add all the metrics from the classes
                FileMetrics fm = new FileMetrics(0, 0, 0, 0, 0, 0, 0, (uint)_classes.Count, (uint)_lines.Count, (uint)_lines.Count);
                foreach ( ClassElement ce in GetClasses())
                    fm.Add(ce.Metrics);
                // Complexity is fixed, we shouldn't add this metric.
                fm.SetMetric("complexity", 1);
                return fm;
            }
        }
        
        public void AddClass ( ClassElement e )
        {
            _classes.Add(e);
        }
        
        public void AddLine ( LineElement e )
        {
            _lines.Add(e);
        }
        
        public List<ClassElement> GetClasses()
        {
            return _classes;
        }
        
        public List<LineElement> GetLines()
        {
            return _lines;
        }
    }
}

