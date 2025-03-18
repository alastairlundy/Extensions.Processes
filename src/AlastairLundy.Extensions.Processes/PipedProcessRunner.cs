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
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

using AlastairLundy.Extensions.Processes.Abstractions.Piping;
using AlastairLundy.Extensions.Processes.Abstractions.Utilities;
using AlastairLundy.Extensions.Processes.Exceptions;
using AlastairLundy.Extensions.Processes.Internal;

namespace AlastairLundy.Extensions.Processes;

/// <summary>
/// A Process Runner-esque class for Piping output after Executing processes.
/// </summary>
[Obsolete(DeprecationMessages.ClassDeprecationV2UseProcessFactoryInstead, false)]
public class PipedProcessRunner : Abstractions.IPipedProcessRunner
{
    private readonly IProcessPipeHandler _processPipeHandler;
    
    private readonly IProcessRunnerUtility _processRunnerUtils;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="processRunnerUtils">The process runner utility service to use.</param>
    /// <param name="processPipeHandler">The process pipe handler service to use.</param>
    public PipedProcessRunner(IProcessRunnerUtility processRunnerUtils, IProcessPipeHandler processPipeHandler)
    {
        _processRunnerUtils = processRunnerUtils;
        _processPipeHandler = processPipeHandler;
    }

    /// <summary>
    /// Runs the process asynchronously, waits for exit, and safely disposes of the Process before returning.
    /// </summary>
    /// <param name="process">The process to be run.</param>
    /// <param name="processResultValidation">The process result validation to be used.</param>
    /// <param name="processResourcePolicy">The process resource policy to be set if it is not null.</param>
    /// <param name="cancellationToken">A token to cancel the operation if required.</param>
    /// <returns>The Process Results from the running the process with the Piped Standard Output and Standard Error.</returns>
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
    public async Task<(
        Processes.Abstractions.ProcessResult processResult, Stream standardOutput, Stream standardError)> ExecuteProcessWithPipingAsync(Process process,
        Processes.Abstractions.ProcessResultValidation processResultValidation, Processes.Abstractions.ProcessResourcePolicy? processResourcePolicy = null, CancellationToken cancellationToken = default)
    {
        await _processRunnerUtils.ExecuteAsync(process, Processes.Abstractions.ProcessResultValidation.None, processResourcePolicy, cancellationToken);
       
        if (processResultValidation == Processes.Abstractions.ProcessResultValidation.ExitCodeZero && process.ExitCode != 0)
        {
            throw new ProcessNotSuccessfulException(process: process, exitCode: process.ExitCode);
        }

        Stream standardOutput = Stream.Null;
        Stream standardError = Stream.Null;
        
        // Pipe Standard Output and Error
        await _processPipeHandler.PipeStandardOutputAsync(process, standardOutput);
        await _processPipeHandler.PipeStandardErrorAsync(process, standardError);
        
        Processes.Abstractions.ProcessResult processResult = await _processRunnerUtils.GetResultAsync(process, true);
       
        return (processResult, standardOutput, standardError);
    }

    /// <summary>
    /// Runs the process asynchronously, waits for exit, and safely disposes of the Process before returning.
    /// </summary>
    /// <param name="process">The process to be run.</param>
    /// <param name="processResultValidation">The process result validation to be used.</param>
    /// <param name="processResourcePolicy">The process resource policy to be set if it is not null.</param>
    /// <param name="cancellationToken">A token to cancel the operation if required.</param>
    /// <returns>The Buffered Process Results from running the process with the Piped Standard Output and Standard Error.</returns>
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
    public async Task<(Processes.Abstractions.BufferedProcessResult processResult, Stream standardOutput, Stream standardError)>
        ExecuteBufferedProcessWithPipingAsync(Process process, Processes.Abstractions.ProcessResultValidation processResultValidation,
            Processes.Abstractions.ProcessResourcePolicy? processResourcePolicy = null,
            CancellationToken cancellationToken = default)
    {
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        
        await _processRunnerUtils.ExecuteAsync(process, Processes.Abstractions.ProcessResultValidation.None, processResourcePolicy, cancellationToken);
        
        if (processResultValidation == Processes.Abstractions.ProcessResultValidation.ExitCodeZero && process.ExitCode != 0)
        {
            throw new ProcessNotSuccessfulException(process: process, exitCode: process.ExitCode);
        }

        Stream standardOutput = Stream.Null;
        Stream standardError = Stream.Null;
        
        // Pipe Standard Output and Error
        await _processPipeHandler.PipeStandardOutputAsync(process, standardOutput);
        await _processPipeHandler.PipeStandardErrorAsync(process, standardError);
        
        Processes.Abstractions.BufferedProcessResult output = await _processRunnerUtils.GetBufferedResultAsync(process, true);

        return (output, standardOutput, standardError);
    }
}