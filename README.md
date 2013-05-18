Visual Coverage
===============

What is it? 
-----------

Visual Studio coverage converter tool. Converts Visual Studio Coverage files (*.coverage) to clover or html format.

This tool uses the excelent Command Line Parser Library.
(check at: https://github.com/gsscoder/commandline).


Prerequisites
-------------
To run VisualCoverage, you will need the following components:

* Windows XP or Windows 7.
* Microsoft .NET Framework version 4.5.
* Visual Studio 2012 or Nant 0.90.


Usage
-----

VisualCoverage.exe -i file.coverage [--html report.html] [--clover report.xml] 
    [--include-namespace pattern] [--exclude-namespace pattern]
    [--include-file pattern] [--exclude-file pattern]

Options:

  --help                Display this help screen.

  --html                Html report output file (*.html).

  --clover              Clover report output file (*.xml).

  -i, --input           Required. Visual studio coverage (*.coverage) input
                        file. Can be specified multiple times.

  --include-namespace   Includes a namespace in the report. If no namespace is
                        added, all namespaces are included. This value can be
                        a regular expression. Can be specified multiple times.

  --exclude-namespace   Exclude a namespace from the report. This value can be
                        a regular expression, in that case, all matching
                        namespaces will be excluded. Can be specified multiple
                        times.

  --include-file        Includes a file in the report. This value can be a
                        regular expression. If no files are added, all files
                        are included in the report. This argument can be
                        specified multiple times to specify multiple files.

  --exclude-file        Excludes a file from the report. This value can be a
                        regular expression, in that case, all files matching
                        this value will be excluded from the report. This
                        argument can be specified multiple times.

  --help                Display this help screen.




HINT: In order to be able to read the coverage file, the images (a.k.a libraries, executables) that were executed during testing must be accesible and in the directory where they were executed.


Examples
--------

    # Generates an html report of the coverage
    VisualCoverage.exe --input file.coverage --html report.html

    # Generates a clover report from the coverage file
    VisualCoverage.exe --input somefile.coverage --clover clover.xml

    # Generates both reports from a single input file
    VisualCoverage.exe --input otherfile.coverage --html report.html --clover clover.xml

    # Generates html report excluding all tests files
    VisualCoverage.exe --input file.coverage --html report.html --exclude-file .*test.*
    
    # Generates html report excluding namespace1 and namespace2
    VisualCoverage.exe --input file.coverage --html report.html --exclude-namespace namespace1 -- exclude-namespace namespace2

License
-------

Copyright (c) 2012 Joaquin Sargiotto

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
