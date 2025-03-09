# AlastairLundy.Extensions.Processes.Abstractions
This package contains Process Running and handling abstractions as well as common types used by implementing classes.

For an implementing package, check out [``AlastairLundy.Extensions.Processes``](https://www.nuget.org/packages/AlastairLundy.Extensions.Processes/).

Key Abstractions:
* ``IProcessFactory``
* Piping:
  * ``IProcessPipeHandler``
* Builders:
  * ``IEnvironmentVariablesBuilder``
  * ``IProcessResourcePolicyBuilder``
  * ``IUserCredentialBuilder``

Key Types:
* Process Results:
  * ``ProcessResult``
  * ``BufferedProcessResult``
* ``ProcessResourcePolicy``
* ``UserCredential``
* ``ProcessConfiguration``
* ``ProcessResultValidation``

[![NuGet](https://img.shields.io/nuget/v/AlastairLundy.Extensions.Processes.Abstractions.svg)](https://www.nuget.org/packages/AlastairLundy.Extensions.Processes.Abstractions/)
[![NuGet](https://img.shields.io/nuget/dt/AlastairLundy.Extensions.Processes.Abstractions.svg)](https://www.nuget.org/packages/AlastairLundy.Extensions.Processes.Abstractions/)

## Table of Contents
* [Features](#features)
* [Installing ProcessExtensions](#how-to-install-and-use-processextensions)
    * [Compatibility](#supported-platforms)
* [Examples](#examples)
* [Contributing to ProcessExtensions](#how-to-contribute-to-processextensions)
* [Roadmap](#processextensions-roadmap)
* [License](#license)
* [Acknowledgements](#acknowledgements)

## Features
* Easy to use safe Process Running classes and interfaces
* Models that help abstract away some of the complicated nature of Process objects
* Compatible with .NET Standard 2.0 and 2.1 ^1
* [SourceLink](https://learn.microsoft.com/en-us/dotnet/standard/library-guidance/sourcelink) support

^1 - [Polyfill](https://github.com/SimonCropp/Polyfill) is a dependency only required for .NET Standard 2.0 and 2.1 users. [Microsoft.Bcl.HashCode](https://www.nuget.org/packages/Microsoft.Bcl.HashCode) is a dependency only required for .NET Standard 2.0 users.

## How to install and use ProcessExtensions
ProcessExtensions is available on [Nuget](https://nuget.org).

### Installing ProcessExtensions
ProcessExtensions's packages can be installed via the .NET SDK CLI, Nuget via your IDE or code editor's package interface, or via the Nuget website.

| Package Name                       | Nuget Link                                                                                                | .NET SDK CLI command                                      |
|------------------------------------|-----------------------------------------------------------------------------------------------------------|-----------------------------------------------------------|
| AlastairLundy.Extensions.Processes | [AlastairLundy.Extensions.Processes Nuget](https://nuget.org/packages/AlastairLundy.Extensions.Processes) | ``dotnet add package AlastairLundy.Extensions.Processes`` |

### Supported Platforms
ProcessExtensions can be added to any .NET Standard 2.0, .NET Standard 2.1, .NET 8, or .NET 9 supported project.

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
One of the main use cases for ProcessExtensions is intended to be [safe Process Running](#safe-process-running).

### Safe Process Running
ProcessExtensions offers safe abstractions around Process Running to avoid accidentally not disposing of Processes after they are executed.

``IProcessFactory`` provides for this in

#### ``IProcessFactory``
IProcessFactory is an interface for enabling easy Process Creation, Running, and Disposal depending on the methods used.

The ``From`` method and its overloads provide for easy standalone process creation.
The ``StartNew`` method provides for creating and starting new processes easily.

The ``ContinueWhenExitAsync`` and ``ContinueWhenExitBufferedAsync`` methods provide for safe process running, and disposal after a Process has exited. 

This example shows how it might be used:
```csharp
using AlastairLundy.Extensions.Processes.Abstractions;
// Using namespaces for Dependency Injection code ommitted for clarity

      // Dependency Injection setup code ommitted for clarity

    IProcessFactory _processFactory = serviceProvider.GetRequiredService<IProcessFactory>();
    
    // Define processStartInfo here.
    
    Process process1 = _processFactory.From(processStartInfo);
    
    // This process that is returned is a Process that has been started.
    Process process2 = _processFactory.StartNew(processStartInfo);
    
    // Wait for the Process to finish before safely disposing of it.
   ProcessResult result = await processFactory.ContinueWhenExitAsync(process2);
    
        
    ProcessResult result = await _processRunner.ExecuteProcessAsync(process, ProcessResultValidation.None);
```

Asynchronous methods in ``IProcessFactory`` also allow for an optional CancellationToken parameter.

Some overloads for ``ContinueWhenExitAsync`` and ``ContinueWhenExitBufferedAsync`` allow for specifying ProcessResultValidation.
## How to Build ProcessExtensions' code

### Requirements
ProcessExtensions requires the latest .NET release SDK to be installed to target all supported TFM (Target Framework Moniker) build targets.

Currently, the required .NET SDK is .NET 9.

The current build targets include:
* .NET 8
* .NET 9
* .NET Standard 2.0
* .NET Standard 2.1

Any version of the .NET 9 SDK can be used, but using the latest version is preferred.

### Versioning new releases
ProcessExtensions aims to follow Semantic versioning with ```[Major].[Minor].[Build]``` for most circumstances and an optional ``.[Revision]`` when only a configuration change is made, or a new build of a preview release is made.

#### Pre-releases
Pre-release versions should have a suffix of -alpha, -beta, -rc, or -preview followed by a ``.`` and what pre-release version number they are. The number should be incremented by 1 after each release unless it only contains a configuration change, or another packaging, or build change. An example pre-release version may look like 1.1.0-alpha.2 , this version string would indicate it is the 2nd alpha pre-release version of 1.1.0 .

#### Stable Releases
Stable versions should follow semantic versioning and should only increment the Revision number if a release only contains configuration or build packaging changes, with no change in functionality, features, or even bug or security fixes.

Releases that only implement bug fixes should see the Build version incremented.

Releases that add new non-breaking changes should increment the Minor version. Minor breaking changes may be permitted in Minor version releases where doing so is necessary to maintain compatibility with an existing supported platform, or an existing piece of code that requires a breaking change to continue to function as intended.

Releases that add major breaking changes or significantly affect the API should increment the Major version. Major version releases should not be released with excessive frequency and should be released when there is a genuine need for the API to change significantly for the improvement of the project.

### Building for Testing
You can build for testing by building the desired project within your IDE or VS Code, or manually by entering the following command: ``dotnet build -c Debug``.

If you encounter any bugs or issues, please [report them](https://github.com/alastairlundy/Extensions.Processes/issues/new/) if an issue doesn't already exist for the bug(s).

### Building for Release
Before building a release build, ensure you apply the relevant changes to ``AlastairLundy.Extensions.Processes.csproj`` file corresponding to the package you are trying to build:
* Update the Package Version variable
* Update the project file's Changelog

You should ensure the project builds under debug settings before producing a release build.

#### Producing Release Builds
To manually build a project for release, enter ``dotnet build -c Release /p:ContinuousIntegrationBuild=true`` for a release with [SourceLink](https://github.com/dotnet/sourcelink) enabled or just ``dotnet build -c Release`` for a build without SourceLink.

Builds should generally always include Source Link and symbol packages if intended for wider distribution.


## How to Contribute to ProcessExtensions
Thank you in advance for considering contributing to ProcessExtensions.

Please see the [CONTRIBUTING.md file](https://github.com/alastairlundy/Extensions.Processes/blob/main/CONTRIBUTING.md) for code and localization contributions.

If you want to file a bug report or suggest a potential feature to add, please check out the [GitHub issues page](https://github.com/alastairlundy/Extensions.Processes/issues/) to see if a similar or identical issue is already open.
If there is not already a relevant issue filed, please [file one here](https://github.com/alastairlundy/Extensions.Processes/issues/new) and follow the respective guidance from the appropriate issue template.

Thanks.

## ProcessExtensions' Roadmap
ProcessExtensions aims to make working with processes easier and safer.

Whilst an initial set of features are available in version 1, there is room for more features, and for modifications of existing features in future updates.

That being said, all stable releases from 1.0 onwards must be stable and should not contain regressions.

Future updates should aim focus on one or more of the following:
* Improved ease of use
* Improved stability
* New features
* Enhancing existing features

## License
ProcessExtensions is licensed under the MPL 2.0 license. If you modify any of ProcessExtensions' files then the modified files must be licensed under the MPL 2.0 .

If you use ProcessExtensions in your project please make an exact copy of the contents of ProcessExtensions' [LICENSE.txt file](https://github.com/alastairlundy/Extensions.Processes/blob/main/LICENSE.txt) available either in your third party licenses txt file or as a separate txt file.

## Acknowledgements

### Projects
This project would like to thank the following projects for their work:
* [Polyfill](https://github.com/SimonCropp/Polyfill) for simplifying .NET Standard 2.0 & 2.1 support
* [Microsoft.Bcl.HashCode](https://github.com/dotnet/maintenance-packages) for providing a backport of the HashCode class and static methods to .NET Standard 2.0

For more information, please see the [THIRD_PARTY_NOTICES file](https://github.com/alastairlundy/Extensions.Processes/blob/main/THIRD_PARTY_NOTICES.txt).
