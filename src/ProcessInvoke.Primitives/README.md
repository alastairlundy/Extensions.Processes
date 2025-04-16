# ProcessInvoke.Primitives
A library that adds useful Process related primitives, like BufferedProcessResult, ProcessResultValidation, and ProcessConfiguration.

Some primitives added include:

## Result Types
* ``ProcessResult`` - For basic process result information
* ``BufferedProcessResult`` - String copies of Standard Output and Standard Error + basic process result information.
* ``PipedProcessResult`` - Process result information + Standard output and Standard Error pipes for more advanced piping scenarios.

### Other Primitives
* ``ProcessResultValidation`` - An enum representing whether process result validation should be performed or not.
* ``ProcessConfiguration`` - A model class for representing Process configurations.

[![NuGet](https://img.shields.io/nuget/v/AlastairLundy.ProcessInvoke.Primitives.svg)](https://www.nuget.org/packages/AlastairLundy.ProcessInvoke.Primitives/)
[![NuGet](https://img.shields.io/nuget/dt/AlastairLundy.ProcessInvoke.Primitives.svg)](https://www.nuget.org/packages/AlastairLundy.ProcessInvoke.Primitives/)

## Table of Contents
* [Using ProcessInvoke.Primitives](#how-to-use-the-project)
* [How to Build the Code](#how-to-build-the-code)
* [Roadmap](#roadmap)
* [Acknowledgements](#acknowledgements)

## How to Use the Project
Get the package from the [Official Nuget Gallery](https://nuget.org/) [here](https://www.nuget.org/packages/AlastairLundy.ProcessInvoke.Primitives).

If you use Visual Studio, Jetbrains Rider, or an IDE with a Nuget interface built in, you can search for the package through there instead.

## How to build the code

### Part 1
**From the Command Line**:
1. Open a terminal application
2. In the terminal application, navigate to the directory containing the source code of this project.
3. Enter the command ``dotnet build -c Release`` if you intend to build the project for release, use ``dotnet build -c Debug`` otherwise.

**From an IDE**:
1. Change the Build Configuration in your IDE to ``Release`` if you intend to distribute the built package or ``Debug`` otherwise.
2. Right-click on the project
3. Select ``Build Selected Projects``, or something similar if it appears differently in your IDE

### Part 2
Regardless of whether you used an IDE or the Terminal to build your package, if it built successfully you'll find the resulting .nupkg and .snupkg files and other dll files in the following locations:
* The ```/bin/Release``` directory inside the source code directory if you built the project using the Release configuration
* or the ``/bin/Debug`` directory inside the source code directory if you built the project using the Debug configuration.

## How to contribute to the project
If you want to add features or make a change to the code:
1. Fork the project if you haven't already done so.
2. Create a new branch in your fork for working on the change.
3. Test the changes to ensure the project still builds.
4. Create a Pull Request (PR) in this project's repo to make the changes, explaining what your changes do and why they should be added in case it's not obvious.
5. A maintainer reviews your PR and checks to see if it can be safely added to the library. If your PR is safe to add the maintainer should accept it and merge it.
6. Hopefully your PR is accepted and merged, if not discuss with the maintainer how you can get your changes approved so that they can be merged.

Thanks in advance for contributing to this project!

## Roadmap
Future versions will aim to add more Process related primitives as well as possibly tweak the behaviour of some primitives in the library.

## Acknowledgements

### Projects
This project would like to thank the following projects for their work:
* [System.IO.Pipelines](https://www.nuget.org/packages/System.IO.Pipelines) for simplifying Piping support.
* [Microsoft.Bcl.HashCode](https://github.com/dotnet/maintenance-packages) for providing a backport of the HashCode class and static methods to .NET Standard 2.0
