# ProcessExtensions
In this readme, the package AlastairLundy.Extensions.Processes is referred to as ProcessExtensions for brevity.

This package enables easier setup of ProcessExtensions' interfaces for dependency injection when using Microsoft.Extensions.DependencyInjection.

[![NuGet](https://img.shields.io/nuget/v/AlastairLundy.Extensions.Processes.svg)](https://www.nuget.org/packages/AlastairLundy.Extensions.Processes/)
[![NuGet](https://img.shields.io/nuget/dt/AlastairLundy.Extensions.Processes.svg)](https://www.nuget.org/packages/AlastairLundy.Extensions.Processes/)

## Table of Contents
* [Installing ProcessExtensions](#how-to-install-and-use-alastairlundyextensionsprocessesdependencyinjection)
    * [Compatibility](#supported-platforms)
* [Usage](#usage)
* [Contributing to ProcessExtensions](#how-to-contribute-to-processextensions)
* [License](#license)


## How to install and use AlastairLundy.Extensions.Processes.DependencyInjection
ProcessExtensions is available on [Nuget](https://nuget.org).

### Installing ProcessExtensions
ProcessExtensions's packages can be installed via the .NET SDK CLI, Nuget via your IDE or code editor's package interface, or via the Nuget website.

| Nuget Link                                                                                                                                        | .NET SDK CLI command                                                          |
|---------------------------------------------------------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------|
| [AlastairLundy.Extensions.Processes.DependencyInjection Nuget](https://nuget.org/packages/AlastairLundy.Extensions.Processes.DependencyInjection) | ``dotnet add package AlastairLundy.Extensions.Processes.DependencyInjection`` |

### Supported Platforms
ProcessExtensions can be added to any .NET Standard 2.0, .NET Standard 2.1, .NET 8, or .NET 9 supported project.

The following table details which target platforms are supported for running Processes.

| Operating System | Support Status                    | Notes                                                                           |
|------------------|-----------------------------------|---------------------------------------------------------------------------------|
| Windows          | Fully Supported :white_check_mark: |                                                                                 |
| macOS            | Fully Supported :white_check_mark: |                                                                                 |
| Mac Catalyst     | Untested Platform :warning:       | Support for this platform has not been tested but should theoretically work.    |
| Linux            | Fully Supported :white_check_mark: |                                                                                 |
| FreeBSD          | Fully Supported :white_check_mark: |                                                                                 |
| Android          | Untested Platform :warning:       | Support for this platform has not been tested but should theoretically work.    |
| IOS              | Not Supported :x:                 | Not supported due to ``Process.Start()`` not supporting IOS. ^2                 | 
| tvOS             | Not Supported :x:                 | Not supported due to ``Process.Start()`` not supporting tvOS ^2                 |
| watchOS          | Not Supported :x:                 | Not supported due to ``Process.Start()`` not supporting watchOS ^3              |
| Browser          | Not Supported and Not Planned :x: | Not supported due to not being a valid target Platform for executing processes. |

^2 - See the [Process class documentation](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.process.start?view=net-9.0#system-diagnostics-process-start) for more info.

^3 - Lack of watchOS support is implied by lack of IOS support since [watchOS is based on IOS](https://en.wikipedia.org/wiki/WatchOS).


**Note:** This library has not been tested on Android or Tizen.


## Usage
To use the Dependency Injection extension method call the IServiceCollection ``AddProcessExtensions`` extension method before the Services are built.

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
Before building a release build, ensure you apply the relevant changes to ``AlastairLundy.Extensions.Processes.DependencyInjection.csproj`` file corresponding to the package you are trying to build:
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

## License
ProcessExtensions is licensed under the MPL 2.0 license. If you modify any of ProcessExtensions' files then the modified files must be licensed under the MPL 2.0 .

If you use ProcessExtensions in your project please make an exact copy of the contents of ProcessExtensions' [LICENSE.txt file](https://github.com/alastairlundy/Extensions.Processes/blob/main/LICENSE.txt) available either in your third party licenses txt file or as a separate txt file.
