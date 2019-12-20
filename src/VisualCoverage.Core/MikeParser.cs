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
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Text.RegularExpressions;
    using Microsoft.VisualStudio.Coverage.Analysis;
    using VisualCoverage.Core.Elements;
    using VisualCoverage.Core.Metrics;

    public class MikeParser
    {
        StringCollection __includeNamespaces = new StringCollection();
        StringCollection __excludeNamespaces = new StringCollection();
        StringCollection __includeFiles = new StringCollection();
        StringCollection __excludeFiles = new StringCollection();
        
        public MikeParser () {
        }
        
        public void IncludeNamespace ( String ns ) {
            this.__includeNamespaces.Add(ns);
        }
        
        public void IncludeFile ( String filename ) {
            this.__includeFiles.Add(filename);
        }
        
        public void ExcludeNamespace ( String ns ) {
            this.__excludeNamespaces.Add(ns);
        }
        
        public void ExcludeFile ( String filename ) {
            this.__excludeFiles.Add(filename);
        }

        private static CoverageInfo JoinCoverageFiles(String[] files)
        {
            CoverageInfo result = null;

            try
            {
                foreach (String file in files)
                {
                    CoverageInfo current = CoverageInfo.CreateFromFile(file);

                    if (result == null)
                    {
                        result = current;
                        continue;
                    }

                    CoverageInfo joined = null;
                    
                    try
                    {
                        joined = CoverageInfo.Join(result, current);
                    }
                    finally
                    {
                        current.Dispose();
                        result.Dispose();
                    }

                    result = joined;
                }
            }
            catch (Exception)
            {
                if (result != null)
                {
                    result.Dispose();
                }
                throw;
            }

            return result;
        }

        public ProjectElement Parse(String[] inputfiles) {
            ProjectElement PROJECT = new ProjectElement("new_project", 123323230);
            // Open file
            using (CoverageInfo info = JoinCoverageFiles(inputfiles))
            {
                CoverageDS dataSet = info.BuildDataSet();
                
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
                    // uint classlocs = 0;
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
                            uint coverage = iline.Field<uint>("Coverage");
                            if (linenum == 0)
                            {
                                fileid = iline.Field<uint>("SourceFileID");
                                string methodname = System.Security.SecurityElement.Escape((string)imethod["MethodName"]);
                                linenum = iline.Field<uint>("LnStart");
                                le = new LineElement(linenum, "method", methodname, coverage);
                            } else {
                                linenum = iline.Field<uint>("LnStart");
                                le = new LineElement(linenum, "stmt", "", coverage);
                            }
                            
                            // If the file doesn't exists in our report, we'll
                            // just ignore this information
                            if (files.ContainsKey(fileid))
                            {
                                fe = files[fileid];
                                fe.AddLine(le);
                            }
                        }
                        
                        // Count how many methods covered we have
                        if ((uint)imethod["LinesCovered"] > 0) covered_methods++;
                    }
                    
                    uint totallines = (uint)iclass["LinesCovered"] + (uint)iclass["LinesNotCovered"] + (uint)iclass["LinesPartiallyCovered"];
                    uint complexity = 1;
                    uint methods = (uint)childRows.Length;
                    uint statements = totallines - methods;
                    uint covered_statements = (uint)iclass["LinesCovered"] - covered_methods;
                    uint conditionals = 0;
                    uint covered_conditionals = 0;
                    
                    ClassElement ce = new ClassElement (System.Security.SecurityElement.Escape((string)iclass["ClassName"]));
                    ce.Metrics = new ClassMetrics(complexity, statements, covered_statements, conditionals, covered_conditionals, methods, covered_methods);
                    
                    if (fe != null)
                    {
                        if (!fe.GetClasses().Contains(ce))
                        {
                            fe.AddClass(ce);
                        }

                        if (packages.ContainsKey((string)iclass["NamespaceKeyName"]))
                        {
                            PackageElement pe = packages[(string)iclass["NamespaceKeyName"]];
                            if (!pe.GetFiles().Contains(fe))
                            {
                                pe.AddFile(fe);
                            }
                        }
                    }
                }
            }
            return PROJECT;
        }
        
        private void CreateIndexPackages ( ProjectElement project, DataTable origin, Dictionary<String, PackageElement> dest )
        {
            foreach(DataRow row in origin.Rows)
            {
                bool include = false;
                // If there is no include... include all (.*)
                if (__includeNamespaces.Count < 1)
                    __includeNamespaces.Add(".*");
                // Check if the namespace is included

                String nsname = (string)row.ItemArray[5];
                foreach (string entry in __includeNamespaces)
                {
                    if (TestRegex(nsname, entry)) 
                    {
                        include = true;
                        break;
                    }
                }
                // Check if the namespace is excluded
                foreach (string entry in __excludeNamespaces)
                {
                    if (TestRegex(nsname, entry)) 
                    {
                        include = false;
                        break;
                    }
                }
                // If the namespace passed all the filters, then we must
                // add it to the project
                if (include)
                {
                    PackageElement pe = new PackageElement (nsname);
                    dest.Add((string)row["NamespaceKeyName"], pe);
                    project.AddPackage(pe);
                }
            }
        }
        
        private void CreateIndexFiles ( DataTable origin, Dictionary<uint, FileElement> dest )
        {
            foreach(DataRow row in origin.Rows)
            {
                bool include = false;
                // If there is no include... include all (.*)
                if (__includeFiles.Count < 1)
                    __includeFiles.Add(".*");
                // Check if the filename is included
                String fname = (string)row["SourceFileName"];
                foreach (string entry in __includeFiles)
                {
                    if (TestRegex(fname, entry)) 
                    {
                        include = true;
                        break;
                    }
                }
                // Check if the namespace is excluded
                foreach (string entry in __excludeFiles)
                {
                    if (TestRegex(fname, entry)) 
                    {
                        include = false;
                        break;
                    }
                }
                // If the file passed all the filters, then we must
                // add it to the dictionary
                if (include)
                {
                    try
                    {
                        FileInfo info = new FileInfo(fname);
                        FileElement fe = new FileElement(info.Name, info.FullName);
                        dest.Add((uint)row["SourceFileID"], fe);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error reading file {0}. Message = {1}", fname, e.Message);
                    }
                    // <---- end
                }
            }
        }
        
        private bool TestRegex ( string s, string pattern ) {
            
            RegexOptions regexOptions = RegexOptions.Compiled;
            Regex r = new Regex(pattern, regexOptions);
            
            return r.IsMatch(s);
        }
        
    }
}
