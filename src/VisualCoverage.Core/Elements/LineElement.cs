//
// visual-coverage: LineElement.cs
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
    using VisualCoverage.Core.Metrics;

    public class LineElement
    {
        private uint _num = 0;
        private string _type = "";
        private string _signature = "";
    
        public LineElement ( uint num, string type, string signature ) {
            _num = num;
            _type = type;
            _signature = signature;
        }
        
        public uint Number
        {
            get { return _num; }
            set { _num = value; }
        }
        
        public String Type
        {
            get { return _type; }
            set { _type = value; }
        }
        
        public String Signature
        {
            get { return _signature; }
            set { _signature = value; }
        }
    }
}

