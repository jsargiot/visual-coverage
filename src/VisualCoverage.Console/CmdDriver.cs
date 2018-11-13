//
// visual-coverage: CmdDriver.cs
//
// Author:
//   Joaquin Sargiotto (joaquinsargiotto@gmail.com)
// Contributor(s):
//   Martin M Reed
//
//
// Copyright (c) 2013 Martin M Reed
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
namespace VisualCoverage.Console
{
    using System;
    using System.Collections;
    using VisualCoverage.Core;
    using VisualCoverage.Core.Util;
    using VisualCoverage.Core.Elements;
    using System.IO;
    using CommandLine;
    using CommandLine.Text;

    class CmdDriver
    {
        private sealed class Options : CommandLineOptionsBase
        {
            private ArrayList __inputFiles = new ArrayList();
            private ArrayList __filesExcludes = new ArrayList();
            private ArrayList __filesIncludes = new ArrayList();
            private ArrayList __namesIncludes = new ArrayList();
            private ArrayList __namesExcludes = new ArrayList();
            
            [Option(null, "html", Required = false, HelpText = "Html report output file (*.html).")]
            public string HtmlOutput {get; set;}

            [Option(null, "clover", Required = false, HelpText = "Clover report output file (*.xml).")]
            public string CloverOutput { get; set; }

            [OptionArray("i", "input", Required = true, HelpText = "Visual studio coverage (*.coverage) input file. Can be specified multiple times.")]
            public string[] InputFiles
            {
                get
                {
                    return (string[])__inputFiles.ToArray(typeof(string));
                }
                set
                {
                    __inputFiles.AddRange(value);
                }
            }
            
            [OptionArray(null, "include-namespace", HelpText = "Includes a namespace in the report. If no namespace is added, all namespaces are included. This value can be a regular expression. Can be specified multiple times.")]
            public string[] IncludedNamespaces
            { 
                get {
                    return (string[])__namesIncludes.ToArray(typeof(string));
                }
                set {
                    __namesIncludes.AddRange(value);
                }
            }
            
            [OptionArray(null, "exclude-namespace", HelpText = "Exclude a namespace from the report. This value can be a regular expression, in that case, all matching namespaces will be excluded. Can be specified multiple times.")]
            public string[] ExcludedNamespaces
            { 
                get {
                    return (string[])__namesExcludes.ToArray(typeof(string));
                }
                set {
                    __namesExcludes.AddRange(value);
                }
            }
            
            [OptionArray(null, "include-file", HelpText = "Includes a file in the report. This value can be a regular expression. If no files are added, all files are included in the report. This argument can be specified multiple times to specify multiple files.")]
            public string[] IncludedFiles
            { 
                get {
                    return (string[])__filesIncludes.ToArray(typeof(string));
                }
                set {
                    __filesIncludes.AddRange(value);
                }
            }
            
            [OptionArray(null, "exclude-file", HelpText = "Excludes a file from the report. This value can be a regular expression, in that case, all files matching this value will be excluded from the report. This argument can be specified multiple times.")]
            public string[] ExcludedFiles
            { 
                get {
                    return (string[])__filesExcludes.ToArray(typeof(string));
                }
                set {
                    __filesExcludes.AddRange(value);
                }
            }

            [Option("s", "shortReport", Required = false, DefaultValue = false, HelpText = "create only a short report (excluding line details)")]
            public bool shortReport { get; set; }
            // <---- end
            
            [HelpOption]
            public string GetUsage()
            {
                return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            }
        }
        
        static void Main(string[] args)
        {
            // Parse arguments
            var options = new Options();
            var parser = new CommandLineParser(new CommandLineParserSettings(Console.Error));
            if (!parser.ParseArguments(args, options))
                Environment.Exit(1);
            
            // Coverage file parser
            MikeParser mikeParser = new MikeParser();
            
            foreach(String s in options.IncludedNamespaces)
            {
                mikeParser.IncludeNamespace(s);
            }
            
            foreach(String s in options.ExcludedNamespaces)
            {
                mikeParser.ExcludeNamespace(s);
            }
            
            foreach(String s in options.IncludedFiles)
            {
                mikeParser.IncludeFile(s);
            }
            
            foreach(String s in options.ExcludedFiles)
            {
                mikeParser.ExcludeFile(s);
            }
            
            // Parse coverage file
            ProjectElement pe = mikeParser.Parse(options.InputFiles);
            
            // Generate clover report
            if (options.CloverOutput != null)
            {
                CloverReport cloverreport = new CloverReport();
                using (StreamWriter outfile = new StreamWriter(options.CloverOutput))
                {
                    cloverreport.DirectWrite(outfile, pe, options.shortReport);
                }
            }
            // Generate html report
            if (options.HtmlOutput != null)
            {
                HtmlReport htmlreport = new HtmlReport();
                using (StreamWriter outfile = new StreamWriter(options.HtmlOutput))
                {
                    outfile.Write(htmlreport.Execute(pe));
                }
            }
        }
    }
}
