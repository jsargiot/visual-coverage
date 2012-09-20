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
* Microsoft .NET Framework version 4.0.


Usage
-----

VisualCoverage.exe -i file.coverage [--html report.html] [--clover report.xml]

Options:
  --html {file.html}         Html report output file (*.html).

  --clover {file.xml}       Clover report output file (*.xml).

  -i, --input    Required. Visual studio coverage (*.coverage) input file.

  --help         Display this help screen.

HINT: In order to be able to read the coverage file, the images (a.k.a libraries, executables) that were executed during testing must be accesible and in the directory where they were executed.


Examples
--------

    # Generates an html report of the coverage
    VisualCoverage.exe --input file.coverage --html report.html

    # Generates a clover report from the coverage file
    VisualCoverage.exe --input somefile.coverage --clover clover.xml

    # Generates both reports from a single input file
    VisualCoverage.exe --input otherfile.coverage --html report.html --clover clover.xml


License
-------

Copyright (c) 2012 Joaquin Sargiotto

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.