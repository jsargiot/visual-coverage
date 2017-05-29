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
            ProjectElement project = new ProjectElement();

            // Open file
            using (CoverageInfo info = JoinCoverageFiles(inputfiles))
            {
                CoverageDS dataSet = info.BuildDataSet();
                
                Dictionary<String, PackageElement> packages = new Dictionary<String, PackageElement>();
                Dictionary<uint, string> files = new Dictionary<uint, string>();
                
                // Namespaces
                DataTable namespacesTable = dataSet.Tables["NameSpaceTable"];
                DataTable classesTable = dataSet.Tables["Class"];
                DataTable filesTable = dataSet.Tables["SourceFileNames"];
                
                CreateIndexPackages(project, namespacesTable, packages);
                CreateIndexFiles(filesTable, files);
                
                foreach(DataRow iclass in classesTable.Rows)
                {
                    string className = System.Security.SecurityElement.Escape((string)iclass["ClassName"]);
                    ClassElement ce = new ClassElement(className);

                    DataRow[] methodRows = iclass.GetChildRows("Class_Method");
                    
                    foreach(DataRow imethod in methodRows)
                    {
                        // Get First Line in class
                        DataRow[] lineRows = imethod.GetChildRows("Method_Lines");
                        bool includeFile = lineRows.Length < 1 ? false : files.ContainsKey(lineRows[0].Field<uint>("SourceFileID"));

                        if (includeFile)
                        {
                            string methodName = (string)imethod["MethodName"]; // System.Security.SecurityElement.Escape((string)imethod["MethodName"]);
                            MethodElement me = new MethodElement(methodName);

                            uint coveredBlocks = (uint)imethod["BlocksCovered"];
                            uint totalBlocks = coveredBlocks + (uint)imethod["BlocksNotCovered"];
                            uint coveredLines = (uint)imethod["LinesCovered"];
                            uint totalLines = coveredLines + (uint)imethod["LinesNotCovered"] + (uint)imethod["LinesPartiallyCovered"];
                            me.Metrics = new MethodMetrics(totalBlocks, coveredBlocks, totalLines, coveredLines);

                            ce.AddMethod(me);
                        }
                    }

                    if (packages.ContainsKey((string)iclass["NamespaceKeyName"]))
                    {
                        PackageElement pe = packages[(string)iclass["NamespaceKeyName"]];
                        pe.AddClass(ce);
                    }
                }
            }

            return project;
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
                String nsname = (string)row["NamespaceName"];
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
        
        private void CreateIndexFiles ( DataTable origin, Dictionary<uint, string> dest )
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
                    FileInfo info = new FileInfo(fname);
                    dest.Add((uint)row["SourceFileID"], info.FullName);
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
