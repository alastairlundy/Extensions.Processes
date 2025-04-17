# AlastairLundy.ProcessInvoke
In this readme, the package AlastairLundy.ProcessInvoke is referred to as ProcessInvoke for brevity.

ProcessInvoke adds much-needed abstractions and extensions to Processes and Process Running, to make it easier and safer.

ProcessInvoke started as part of CliInvoke but is now a separate package, helping other projects besides CliInvoke to run processes easily and safely.

[![NuGet](https://img.shields.io/nuget/v/AlastairLundy.ProcessInvoke.svg)](https://www.nuget.org/packages/AlastairLundy.ProcessInvoke/)
[![NuGet](https://img.shields.io/nuget/dt/AlastairLundy.ProcessInvoke.svg)](https://www.nuget.org/packages/AlastairLundy.ProcessInvoke/)

## Table of Contents
* [Features](#features)
* [Installing ProcessInvoke](#how-to-install-and-use-ProcessInvoke)
    * [Compatibility](#supported-platforms)
* [Examples](#examples)
* [Contributing to ProcessInvoke](#how-to-contribute-to-ProcessInvoke)
* [Roadmap](#ProcessInvoke-roadmap)
* [License](#license)
* [Acknowledgements](#acknowledgements)

## Features
* Easy to use safe Process Running classes and interfaces
* Models that help abstract away some of the complicated nature of Process objects
* Compatible with .NET Standard 2.0 and 2.1 ^1
* [SourceLink](https://learn.microsoft.com/en-us/dotnet/standard/library-guidance/sourcelink) support

^1 - [Polyfill](https://github.com/SimonCropp/Polyfill) is a dependency only required for .NET Standard 2.0 and 2.1 users. [Microsoft.Bcl.HashCode](https://www.nuget.org/packages/Microsoft.Bcl.HashCode) is a dependency only required for .NET Standard 2.0 users.

## When should I use [ProcessInvoke](https://github.com/alastairlundy/ProcessInvoke) vs [CliInvoke](https://github.com/alastairlundy/CliInvoke)?

Use _CliInvoke_ **IF any** of the following apply:
- You want to programatically execute a Cli Application
- You want to get the output of a Cli Application.
- You want to Pipe the Input or Output of a Cli Application.

Use _ProcessInvoke_ **IF ALL** the following apply:
- You don't care about getting a Process's Standard Output.
- You're focussed more on starting and/or stopping Processes.
- You don't need Argument Building or Escaping capabilities

**Note**: Whilst ProcessInvoke is capable of doing most of what CliInvoke is capable of, they are intended for different use cases.

CliInvoke internally uses ProcessInvoke but provides additional abstractions around programatically executing programs and is designed specifically for this task.


## How to install and use ProcessInvoke
ProcessInvoke is available on [Nuget](https://nuget.org).

### Installing ProcessInvoke
ProcessInvoke's packages can be installed via the .NET SDK CLI, Nuget via your IDE or code editor's package interface, or via the Nuget website.

| Package Name                       | Nuget Link                                                                                                | .NET SDK CLI command                                      |
|------------------------------------|-----------------------------------------------------------------------------------------------------------|-----------------------------------------------------------|
| AlastairLundy.ProcessInvoke | [AlastairLundy.ProcessInvoke Nuget](https://nuget.org/packages/AlastairLundy.ProcessInvoke) | ``dotnet add package AlastairLundy.ProcessInvoke`` |

### Supported Platforms
ProcessInvoke can be added to any .NET Standard 2.0, .NET Standard 2.1, .NET 8, or .NET 9 supported project.

The following table details which target platforms are supported for running Processes.

| Operating System | Support Status                     | Notes                                                                           |
|------------------|------------------------------------|---------------------------------------------------------------------------------|
| Windows          | Fully Supported :white_check_mark: |                                                                                 |
| macOS            | Fully Supported :white_check_mark: |                                                                                 |
| Mac Catalyst     | Untested Platform :warning:        | Support for this platform has not been tested but should theoretically work.    |
| Linux            | Fully Supported :white_check_mark: |                                                                                 |
| FreeBSD          | Fully Supported :white_check_mark: |                                                                                 |
| Android          | Untested Platform :warning:        | Support for this platform has not been tested but should theoretically work.    |
| IOS              | Not Supported :x:                  | Not supported due to ``Process.Start()`` not supporting IOS. ^2                 | 
| tvOS             | Not Supported :x:                  | Not supported due to ``Process.Start()`` not supporting tvOS ^2                 |
| watchOS          | Not Supported :x:                  | Not supported due to ``Process.Start()`` not supporting watchOS ^3              |
| Browser          | Not Supported and Not Planned :x:  | Not supported due to not being a valid target Platform for executing processes. |

^2 - See the [Process class documentation](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.process.start?view=net-9.0#system-diagnostics-process-start) for more info.

^3 - Lack of watchOS support is implied by lack of IOS support since [watchOS is based on IOS](https://en.wikipedia.org/wiki/WatchOS).


**Note:** This library has not been tested on Android or Tizen.


## Examples
One of the main use cases for ProcessInvoke is intended to be [safe Process Running](#safe-process-running).

### Safe Process Running
ProcessInvoke offers safe abstractions around Process Running to avoid accidentally not disposing of Processes after they are executed.

If directly executing the process and receiving a ``ProcessResult`` or ``BufferedProcessResult`` object is desirable you should use ``IProcessRunner`` as a service.

If you don't wish to immediately dispose of the Process after executing it, but plan to dispose of it later then ``IProcessRunnerUtility`` provides more flexibility.

#### ``IProcessRunner``
**Note**: ``IProcessRunner`` and it's implementation class's execution methods do require a ``ProcessResultValidation`` argument to be passed, that configures whether validation should be performed on the Process' exit code.
A default value for the parameter is intentionally not provided, as it is up to the user to decide whether they require exit code validation.

This example shows how it might be used:
```csharp
using AlastairLundy.ProcessInvoke;
// Using namespaces for Dependency Injection code ommitted for clarity

//Namespace and classs code ommitted for clarity 

      // ServiceProvider and Dependency Injection code ommitted for clarity

    IProcessRunner _processRunner = serviceProvider.GetRequiredService<IProcessRunner>();
    

    ProcessResult result = await _processRunner.ExecuteProcessAsync(process, ProcessResultValidation.None);
```

Asynchronous methods in ``IProcessRunner`` do provide an optional CancellationToken parameter.

Synchronous methods are available in ``IProcessRunner`` but should be used as a last resort, in situations where using async and await are not possible.

##### ``IProcessRunnerUtility``
The naming of ``IProcessRunnerUtility`` is deliberately similar to ``IProcessRunner`` as it is the utility interface (and corresponding implementing class) that ``IProcessRunner`` uses behind the scenes for functionality.

Usage of ``IProcessRunnerUtility`` is most appropriate when greater flexibility is required than what ``IProcessRunner`` provides.

For instance, you can keep a Process object alive for as long as needed, and then dispose of it later.

```csharp
using AlastairLundy.ProcessInvoke;
using AlastairLundy.ProcessInvoke.Utilities.Abstractions;

// Using namespaces for Dependency Injection code ommitted for clarity

//Namespace and classs code ommitted for clarity 

      // ServiceProvider and Dependency Injection code ommitted for clarity

    IProcessRunnerUtility _processRunnerUtility = serviceProvider.GetRequiredService<IProcessRunnerUtility>();
    
    // Result Validation and Cancellation token are optional parameters.
    int exitCode = await _processRunnerUtility.ExecuteAsync(process);
    
    // Getting the result afterwards is done manually.
    ProcessResult = await _processRunnerUtility.GetResultAsync(process);
    
    // Code continuing to use process object ommitted.
    
    
    // Dispose of Process when no longer needed.
    _processRunnerUtility.DisposeOfProcess(process);
```

Some synchronous methods are available in ``IProcessRunnerUtility`` but should be used as a last resort, in situations where using async and await are not possible.

## How to Build ProcessInvoke' code

### Requirements
ProcessInvoke requires the latest .NET release SDK to be installed to target all supported TFM (Target Framework Moniker) build targets.

Currently, the required .NET SDK is .NET 9.

The current build targets include:
* .NET 8
* .NET 9
* .NET Standard 2.0
* .NET Standard 2.1

Any version of the .NET 9 SDK can be used, but using the latest version is preferred.

### Versioning new releases
ProcessInvoke aims to follow Semantic versioning with ```[Major].[Minor].[Build]``` for most circumstances and an optional ``.[Revision]`` when only a configuration change is made, or a new build of a preview release is made.

#### Pre-releases
Pre-release versions should have a suffix of -alpha, -beta, -rc, or -preview followed by a ``.`` and what pre-release version number they are. The number should be incremented by 1 after each release unless it only contains a configuration change, or another packaging, or build change. An example pre-release version may look like 1.1.0-alpha.2 , this version string would indicate it is the 2nd alpha pre-release version of 1.1.0 .

#### Stable Releases
Stable versions should follow semantic versioning and should only increment the Revision number if a release only contains configuration or build packaging changes, with no change in functionality, features, or even bug or security fixes.

Releases that only implement bug fixes should see the Build version incremented.

Releases that add new non-breaking changes should increment the Minor version. Minor breaking changes may be permitted in Minor version releases where doing so is necessary to maintain compatibility with an existing supported platform, or an existing piece of code that requires a breaking change to continue to function as intended.

Releases that add major breaking changes or significantly affect the API should increment the Major version. Major version releases should not be released with excessive frequency and should be released when there is a genuine need for the API to change significantly for the improvement of the project.

### Building for Testing
You can build for testing by building the desired project within your IDE or VS Code, or manually by entering the following command: ``dotnet build -c Debug``.

If you encounter any bugs or issues, please [report them](https://github.com/alastairlundy/ProcessInvoke/issues/new/) if an issue doesn't already exist for the bug(s).

### Building for Release
Before building a release build, ensure you apply the relevant changes to ``AlastairLundy.ProcessInvoke.csproj`` file corresponding to the package you are trying to build:
* Update the Package Version variable
* Update the project file's Changelog

You should ensure the project builds under debug settings before producing a release build.

#### Producing Release Builds
To manually build a project for release, enter ``dotnet build -c Release /p:ContinuousIntegrationBuild=true`` for a release with [SourceLink](https://github.com/dotnet/sourcelink) enabled or just ``dotnet build -c Release`` for a build without SourceLink.

Builds should generally always include Source Link and symbol packages if intended for wider distribution.


## How to Contribute to ProcessInvoke
Thank you in advance for considering contributing to ProcessInvoke.

Please see the [CONTRIBUTING.md file](https://github.com/alastairlundy/ProcessInvoke/blob/main/CONTRIBUTING.md) for code and localization contributions.

If you want to file a bug report or suggest a potential feature to add, please check out the [GitHub issues page](https://github.com/alastairlundy/ProcessInvoke/issues/) to see if a similar or identical issue is already open.
If there is not already a relevant issue filed, please [file one here](https://github.com/alastairlundy/ProcessInvoke/issues/new) and follow the respective guidance from the appropriate issue template.

Thanks.

## ProcessInvoke' Roadmap
ProcessInvoke aims to make working with processes easier and safer.

Whilst an initial set of features are available in version 1, there is room for more features, and for modifications of existing features in future updates.

That being said, all stable releases from 1.0 onwards must be stable and should not contain regressions.

Future updates should aim focus on one or more of the following:
* Improved ease of use
* Improved stability
* New features
* Enhancing existing features

## License
ProcessInvoke is licensed under the MPL 2.0 license. If you modify any of ProcessInvoke' files then the modified files must be licensed under the MPL 2.0 .

If you use ProcessInvoke in your project please make an exact copy of the contents of ProcessInvoke' [LICENSE.txt file](https://github.com/alastairlundy/ProcessInvoke/blob/main/LICENSE.txt) available either in your third party licenses txt file or as a separate txt file.

## Acknowledgements

### Projects
This project would like to thank the following projects for their work:
* [Polyfill](https://github.com/SimonCropp/Polyfill) for simplifying .NET Standard 2.0 & 2.1 support
* [Microsoft.Bcl.HashCode](https://github.com/dotnet/maintenance-packages) for providing a backport of the HashCode class and static methods to .NET Standard 2.0

For more information, please see the [THIRD_PARTY_NOTICES file](https://github.com/alastairlundy/ProcessInvoke/blob/main/THIRD_PARTY_NOTICES.txt).
