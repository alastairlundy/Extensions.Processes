﻿/*
    AlastairLundy.Extensions.Processes  
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using AlastairLundy.Extensions.Processes.Exceptions;
using AlastairLundy.Extensions.Processes.Internal;
using AlastairLundy.Extensions.Processes.Internal.Localizations;

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

using IProcessRunnerUtility = AlastairLundy.Extensions.Processes.Abstractions.Utilities.IProcessRunnerUtility;

namespace AlastairLundy.Extensions.Processes;

/// <summary>
/// The default implementation of IProcessRunner, a safer way to execute processes.
/// </summary>
public class ProcessRunner : Processes.Abstractions.IProcessRunner
{
    private readonly IProcessRunnerUtility _processRunnerUtils;
    
    [Obsolete(DeprecationMessages.InterfaceDeprecationV2)]
    public ProcessRunner(IProcessRunnerUtility processRunnerUtils)
    {
        _processRunnerUtils = processRunnerUtils;
    }
    
    /// <summary>
    /// Runs the process synchronously, waits for exit, and safely disposes of the Process before returning.
    /// </summary>
    /// <remarks>Use the Async version of this method where possible to avoid UI freezes and other potential issues.</remarks>
    /// <param name="process">The process to be run.</param>
    /// <param name="processResultValidation">The process result validation to be used.</param>
    /// <param name="processResourcePolicy">The process resource policy to be set if it is not null.</param>
    /// <returns>The Process Results from the running the process.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file, with the file name of the process to be executed, is not found.</exception>
    /// <exception cref="ProcessNotSuccessfulException">Thrown if the result validation requires the process to exit with exit code zero and the process exits with a different exit code.</exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("freebsd")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("maccatalyst")]
    [UnsupportedOSPlatform("ios")]
    [SupportedOSPlatform("android")]
    [UnsupportedOSPlatform("tvos")]
    [UnsupportedOSPlatform("browser")]
#endif
    [Obsolete(DeprecationMessages.InterfaceDeprecationV2)]
    public Processes.Abstractions.ProcessResult ExecuteProcess(Process process, Processes.Abstractions.ProcessResultValidation processResultValidation,
        Processes.Abstractions.ProcessResourcePolicy? processResourcePolicy = null)
    {
        if (File.Exists(process.StartInfo.FileName) == false)
        {
            throw new FileNotFoundException(Resources.Exceptions_IO_FileNotFound.Replace("{file}", process.StartInfo.FileName));
        }

        _processRunnerUtils.Execute(process, processResultValidation, processResourcePolicy);

        if (processResultValidation == Abstractions.ProcessResultValidation.ExitCodeZero && process.ExitCode != 0)
        {
            throw new ProcessNotSuccessfulException(process: process, exitCode: process.ExitCode);
        }

        return _processRunnerUtils.GetResult(process, disposeOfProcess: true);
    }

    /// <summary>
    /// Runs the process synchronously, waits for exit, and safely disposes of the Process before returning.
    /// </summary>
    /// <remarks>Use the Async version of this method where possible to avoid UI freezes and other potential issues.</remarks>
    /// <param name="process">The process to be run.</param>
    /// <param name="processResultValidation">The process result validation to be used.</param>
    /// <param name="processResourcePolicy">The process resource policy to set if it is not null.</param>
    /// <returns>The Buffered Process Results from running the process.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file, with the file name of the process to be executed, is not found.</exception>
    /// <exception cref="ProcessNotSuccessfulException">Thrown if the result validation requires the process to exit with exit code zero and the process exits with a different exit code.</exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("freebsd")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("maccatalyst")]
    [UnsupportedOSPlatform("ios")]
    [SupportedOSPlatform("android")]
    [UnsupportedOSPlatform("tvos")]
    [UnsupportedOSPlatform("browser")]
#endif
    [Obsolete(DeprecationMessages.InterfaceDeprecationV2)]
    public Processes.Abstractions.BufferedProcessResult ExecuteBufferedProcess(Process process,
        Processes.Abstractions.ProcessResultValidation processResultValidation,
        Processes.Abstractions.ProcessResourcePolicy? processResourcePolicy = null)
    {
        if (File.Exists(process.StartInfo.FileName) == false)
        {
            throw new FileNotFoundException(Resources.Exceptions_IO_FileNotFound.Replace("{file}", process.StartInfo.FileName));
        }
        
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        
        _processRunnerUtils.Execute(process, processResultValidation, processResourcePolicy);
       
        if (processResultValidation == Abstractions.ProcessResultValidation.ExitCodeZero && process.ExitCode != 0)
        {
            throw new ProcessNotSuccessfulException(process: process, exitCode: process.ExitCode);
        }
        
        return _processRunnerUtils.GetBufferedResult(process, disposeOfProcess: true);
    }

    /// <summary>
    /// Runs the process synchronously, waits for exit, and safely disposes of the Process before returning.
    /// </summary>
    /// <param name="process">The process to be run.</param>
    /// <param name="processConfiguration">The configuration to use for the process.</param>
    /// <param name="cancellationToken">A token to cancel the operation if required.</param>
    /// <returns>The Process Results from running the process.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file, with the file name of the process to be executed, is not found.</exception>
    /// <exception cref="ProcessNotSuccessfulException">Thrown if the result validation requires the process to exit with exit code zero and the process exits with a different exit code.</exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("freebsd")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("maccatalyst")]
    [UnsupportedOSPlatform("ios")]
    [SupportedOSPlatform("android")]
    [UnsupportedOSPlatform("tvos")]
    [UnsupportedOSPlatform("browser")]
#endif
    public async Task<Abstractions.ProcessResult> ExecuteProcessAsync(Process process, Abstractions.ProcessConfiguration processConfiguration,
        CancellationToken cancellationToken = default)
    {
        if (File.Exists(process.StartInfo.FileName) == false)
        {
            throw new FileNotFoundException(Resources.Exceptions_IO_FileNotFound.Replace("{file}", process.StartInfo.FileName));
        }
        
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        
        await _processRunnerUtils.ExecuteAsync(process, processConfiguration.ResultValidation, processConfiguration.ResourcePolicy, cancellationToken);
       
        if (processConfiguration.ResultValidation == Abstractions.ProcessResultValidation.ExitCodeZero && process.ExitCode != 0)
        {
            throw new ProcessNotSuccessfulException(process: process, exitCode: process.ExitCode);
        }
        
        return await _processRunnerUtils.GetBufferedResultAsync(process, disposeOfProcess: true);
    }

    /// <summary>
    /// Runs the process asynchronously, waits for exit, and safely disposes of the Process before returning.
    /// </summary>
    /// <param name="process">The process to be run.</param>
    /// <param name="processResultValidation">The process result validation to be used.</param>
    /// <param name="processResourcePolicy">The process resource policy to be set if not null.</param>
    /// <param name="cancellationToken">A token to cancel the operation if required.</param>
    /// <returns>The Process Results from the running the process.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file, with the file name of the process to be executed, is not found.</exception>
    /// <exception cref="ProcessNotSuccessfulException">Thrown if the result validation requires the process to exit with exit code zero and the process exits with a different exit code.</exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("freebsd")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("maccatalyst")]
    [UnsupportedOSPlatform("ios")]
    [SupportedOSPlatform("android")]
    [UnsupportedOSPlatform("tvos")]
    [UnsupportedOSPlatform("browser")]
#endif
    public async Task<Processes.Abstractions.ProcessResult> ExecuteProcessAsync(Process process,
        Processes.Abstractions.ProcessResultValidation processResultValidation,
        Processes.Abstractions.ProcessResourcePolicy? processResourcePolicy = null,
        CancellationToken cancellationToken = default)
    {
        await _processRunnerUtils.ExecuteAsync(process, processResultValidation, processResourcePolicy , cancellationToken);
       
        return await _processRunnerUtils.GetResultAsync(process, disposeOfProcess: true);
    }

    /// <summary>
    /// Runs the process asynchronously, waits for exit, and safely disposes of the Process before returning.
    /// </summary>
    /// <param name="process">The process to be run.</param>
    /// <param name="processConfiguration">The configuration to use for the process.</param>
    /// <param name="cancellationToken">A token to cancel the operation if required.</param>
    /// <returns>The Buffered Process Results from running the process.</returns>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("freebsd")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("maccatalyst")]
    [UnsupportedOSPlatform("ios")]
    [SupportedOSPlatform("android")]
    [UnsupportedOSPlatform("tvos")]
    [UnsupportedOSPlatform("browser")]
#endif
    public async Task<Abstractions.BufferedProcessResult> ExecuteBufferedProcessAsync(Process process,
        Abstractions.ProcessConfiguration processConfiguration,
        CancellationToken cancellationToken = default)
    {
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        
        await _processRunnerUtils.ExecuteAsync(process, processConfiguration.ResultValidation,
            processConfiguration.ResourcePolicy,
            cancellationToken);
        
        return await _processRunnerUtils.GetBufferedResultAsync(process, disposeOfProcess: true);
    }


    /// <summary>
    /// Runs the process asynchronously, waits for exit, and safely disposes of the Process before returning.
    /// </summary>
    /// <param name="process">The process to be run.</param>
    /// <param name="processResultValidation">The process result validation to be used.</param>
    /// <param name="processResourcePolicy">The resource policy to be set if not null.</param>
    /// <param name="cancellationToken">A token to cancel the operation if required.</param>
    /// <returns>The Buffered Process Results from running the process.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file, with the file name of the process to be executed, is not found.</exception>
    /// <exception cref="ProcessNotSuccessfulException">Thrown if the result validation requires the process to exit with exit code zero and the process exits with a different exit code.</exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("freebsd")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("maccatalyst")]
    [UnsupportedOSPlatform("ios")]
    [SupportedOSPlatform("android")]
    [UnsupportedOSPlatform("tvos")]
    [UnsupportedOSPlatform("browser")]
#endif
    public async Task<Processes.Abstractions.BufferedProcessResult> ExecuteBufferedProcessAsync(Process process,
        Processes.Abstractions.ProcessResultValidation processResultValidation,
        Processes.Abstractions.ProcessResourcePolicy? processResourcePolicy = null,
        CancellationToken cancellationToken = default)
    {
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        
        await _processRunnerUtils.ExecuteAsync(process, processResultValidation, processResourcePolicy, cancellationToken);
        
        return await _processRunnerUtils.GetBufferedResultAsync(process, disposeOfProcess: true);
    }
}