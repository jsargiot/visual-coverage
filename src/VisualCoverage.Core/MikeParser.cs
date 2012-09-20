//
// visual-coverage: MikeParser.cs
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
namespace VisualCoverage.Core.Util
{
    using System;
    using System.IO;
    using System.Data;
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.Coverage.Analysis;
    using VisualCoverage.Core.Elements;
    using VisualCoverage.Core.Metrics;

    public class MikeParser
    {
        public MikeParser () {
        }
        
        public ProjectElement Parse ( String inputfile ) {
            ProjectElement PROJECT = new ProjectElement("new_project", 123323230);
            // Open file
            using (CoverageInfo info = CoverageInfo.CreateFromFile(inputfile))
            {
                CoverageDS dataSet = info.BuildDataSet();
                //CoverageDS dataSet = new CoverageDS();
                //dataSet.ReadXml(@"C:\jsargiot\PERSONAL\CODE\VisualCoverage\example\intermediate.xml");
                
                Dictionary<String, PackageElement> packages = new Dictionary<String, PackageElement>();
                Dictionary<uint, FileElement> files = new Dictionary<uint, FileElement>();
                
                // Namespaces
                DataTable namespacesTable = dataSet.Tables["NameSpaceTable"];
                DataTable classesTable = dataSet.Tables["Class"];
                DataTable filesTable = dataSet.Tables["SourceFileNames"];
                
                CreateIndexPackages(PROJECT, namespacesTable, packages);
                CreateIndexFiles(filesTable, files);
                
                foreach(DataRow iclass in classesTable.Rows)
                {
                    DataRow[] childRows = iclass.GetChildRows("Class_Method");

                    // Since it's one class per file (at least) declare
                    // file here and add it to the class.
                    uint fileid = 0;
                    uint classlocs = 0;
                    uint covered_methods = 0;
                    FileElement fe = null;
                    
                    foreach(DataRow imethod in childRows)
                    {
                        // Method starting line
                        uint linenum = 0;
                        // Get First Line in class
                        DataRow[] childRows2 = imethod.GetChildRows("Method_Lines");
                        //if(childRows2.Length > 0)
                        foreach(DataRow iline in childRows2)
                        {
                            LineElement le = null;
                            if (linenum == 0)
                            {
                                fileid = iline.Field<uint>("SourceFileID");
                                linenum = iline.Field<uint>("LnStart");
                                le = new LineElement(linenum, "method", System.Security.SecurityElement.Escape((string)imethod["MethodName"]));
                            } else {
                                linenum = iline.Field<uint>("LnStart");
                                le = new LineElement(linenum, "stmt", "");
                            }
                            fe = files[fileid];
                            fe.AddLine(le);
                        }
                        
                        // Count how many methods covered we have
                        if ((uint)imethod["LinesCovered"] > 0) covered_methods++;
                    }
                    
                    ClassElement ce = new ClassElement (System.Security.SecurityElement.Escape((string)iclass["ClassName"]));
                    uint totallines = (uint)iclass["LinesCovered"] + (uint)iclass["LinesNotCovered"] + (uint)iclass["LinesPartiallyCovered"];
                    ClassMetrics cm = new ClassMetrics(1, totallines, (uint)iclass["LinesCovered"], 0, 0, (uint)childRows.Length, covered_methods);
                    ce.Metrics = cm;
                    
                    if (fe != null)
                    {
                        fe.AddClass(ce);
                        PackageElement pe = packages[(string)iclass["NamespaceKeyName"]];
                        pe.AddFile(fe);
                    }
                }
            }
            return PROJECT;
        }
        
        private void CreateIndexPackages ( ProjectElement project, DataTable origin, Dictionary<String, PackageElement> dest )
        {
            foreach(DataRow row in origin.Rows)
            {
                PackageElement pe = new PackageElement ((string)row["NamespaceName"]);
                dest.Add((string)row["NamespaceKeyName"], pe);
                project.AddPackage(pe);
            }
        }
        
        private void CreateIndexFiles ( DataTable origin, Dictionary<uint, FileElement> dest )
        {
            foreach(DataRow row in origin.Rows)
            {
                FileInfo info = new FileInfo((string)row["SourceFileName"]);
                FileElement fe = new FileElement (info.Name, info.FullName);
                dest.Add((uint)row["SourceFileID"], fe);
            }
        }
        
    }
}
