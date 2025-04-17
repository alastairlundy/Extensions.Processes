# AlastairLundy.ProcessInvoke.Abstractions
This package contains Process Running and handling abstractions as well as common types used by implementing classes.

For an implementing package, check out [``AlastairLundy.ProcessInvoke``](https://www.nuget.org/packages/AlastairLundy.ProcessInvoke/).

Key Abstractions:
* ``IProcessFactory``
* Piping:
  * ``IProcessPipeHandler``
* Builders:
  * ``IEnvironmentVariablesBuilder``
  * ``IProcessResourcePolicyBuilder``
  * ``IUserCredentialBuilder``

[![NuGet](https://img.shields.io/nuget/v/AlastairLundy.ProcessInvoke.Abstractions.svg)](https://www.nuget.org/packages/AlastairLundy.ProcessInvoke.Abstractions/)
[![NuGet](https://img.shields.io/nuget/dt/AlastairLundy.ProcessInvoke.Abstractions.svg)](https://www.nuget.org/packages/AlastairLundy.ProcessInvoke.Abstractions/)

## Table of Contents
* [Features](#features)
* [Installing ProcessInvoke.Abstractions](#how-to-install-and-use-processinvokeabstractions)
    * [Compatibility](#supported-platforms)
* [Examples](#examples)
* [Contributing to ProcessInvoke.Abstractions](#how-to-contribute-to-processextensions)
* [License](#license)
* [Acknowledgements](#acknowledgements)

## Features
* Easy to use safe Process Running classes and interfaces
* Models that help abstract away some of the complicated nature of Process objects
* Compatible with .NET Standard 2.0 and 2.1 ^1
* [SourceLink](https://learn.microsoft.com/en-us/dotnet/standard/library-guidance/sourcelink) support

^1 - [Polyfill](https://github.com/SimonCropp/Polyfill) is a dependency only required for .NET Standard 2.0 and 2.1 users. [Microsoft.Bcl.HashCode](https://www.nuget.org/packages/Microsoft.Bcl.HashCode) is a dependency only required for .NET Standard 2.0 users.

## How to install and use ProcessInvoke.Abstractions
ProcessInvoke.Abstractions is available on [Nuget](https://nuget.org).

### Installing ProcessInvoke.Abstractions
ProcessInvoke.Abstractions' packages can be installed via the .NET SDK CLI, Nuget via your IDE or code editor's package interface, or via the Nuget website.

| Package Name                       | Nuget Link                                                                                                | .NET SDK CLI command                                      |
|------------------------------------|-----------------------------------------------------------------------------------------------------------|-----------------------------------------------------------|
| AlastairLundy.ProcessInvoke.Abstractions | [AlastairLundy.ProcessInvoke.Abstractions Nuget](https://nuget.org/packages/AlastairLundy.ProcessInvoke.Abstractions) | ``dotnet add package AlastairLundy.ProcessInvoke.Abstractionss`` |

### Supported Platforms
ProcessInvoke.Abstractions can be added to any .NET Standard 2.0, .NET Standard 2.1, .NET 8, or .NET 9 supported project.

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
One of the main use cases for ProcessInvoke.Abstractions is intended to be [safe Process Running](#safe-process-running).

### Safe Process Running
ProcessInvoke.Abstractions offers safe abstractions around Process Running to avoid accidentally not disposing of Processes after they are executed.

``IProcessFactory`` provides for this in

#### ``IProcessFactory``
IProcessFactory is an interface for enabling easy Process Creation, Running, and Disposal depending on the methods used.

The ``From`` method and its overloads provide for easy standalone process creation.
The ``StartNew`` method provides for creating and starting new processes easily.

The ``ContinueWhenExitAsync`` and ``ContinueWhenExitBufferedAsync`` methods provide for safe process running, and disposal after a Process has exited. 

This example shows how it might be used:
```csharp
using AlastairLundy.ProcessInvoke.Abstractions;
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

## How to Contribute
Thank you in advance for considering contributing to ProcessInvoke.

Please see the [CONTRIBUTING.md file](https://github.com/alastairlundy/ProcessInvoke/blob/main/CONTRIBUTING.md) for code and localization contributions.

If you want to file a bug report or suggest a potential feature to add, please check out the [GitHub issues page](https://github.com/alastairlundy/ProcessInvoke/issues/) to see if a similar or identical issue is already open.
If there is not already a relevant issue filed, please [file one here](https://github.com/alastairlundy/ProcessInvoke/issues/new) and follow the respective guidance from the appropriate issue template.

Thanks.

## License
ProcessInvoke.Abstractions is licensed under the MPL 2.0 license. If you modify any of ProcessInvoke.Abstractions' files then the modified files must be licensed under the MPL 2.0.

If you use ProcessInvoke.Abstractions in your project, please make an exact copy of the contents of ProcessInvoke.Abstractions' [LICENSE.txt file](https://github.com/alastairlundy/ProcessInvoke/blob/main/LICENSE.txt) available either in your third party licenses txt file or as a separate txt file.

## Acknowledgements

### Projects
This project would like to thank the following projects for their work:
* [Microsoft.Bcl.HashCode](https://github.com/dotnet/maintenance-packages) for providing a backport of the HashCode class and static methods to .NET Standard 2.0

For more information, please see the [THIRD_PARTY_NOTICES file](https://github.com/alastairlundy/ProcessInvoke.Abstractionss/blob/main/THIRD_PARTY_NOTICES.txt).
