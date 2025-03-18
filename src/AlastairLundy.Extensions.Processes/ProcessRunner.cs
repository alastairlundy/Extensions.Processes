/*
    AlastairLundy.Extensions.Processes  
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System.Diagnostics;
using System.IO;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

using AlastairLundy.Extensions.Processes.Abstractions;

using AlastairLundy.Extensions.Processes.Exceptions;

namespace AlastairLundy.Extensions.Processes;

/// <summary>
/// The default implementation of IProcessRunner, a safer way to execute processes.
/// </summary>
public class ProcessRunner : Abstractions.IProcessRunner
{
    private readonly IProcessFactory _processFactory;
    
    public ProcessRunner(IProcessFactory processFactory)
    {
        _processFactory = processFactory;
    }


    public async Task<ProcessResult> ExecuteProcessConfigAsync(ProcessConfiguration processConfiguration,
        CancellationToken cancellationToken = new CancellationToken())
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="process"></param>
    /// <param name="processConfiguration"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ProcessResult> ExecuteProcessAsync(Process process, 
        ProcessConfiguration processConfiguration,
        CancellationToken cancellationToken = default)
    {
        Process actualProcess = _processFactory.StartNew(processConfiguration);        
        
        actualProcess.SetResourcePolicy(processConfiguration.ResourcePolicy);
        
        return await _processFactory.ContinueWhenExitAsync(actualProcess, processConfiguration.ResultValidation,
            cancellationToken);
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
    public async Task<ProcessResult> ExecuteProcessAsync(Process process,
        ProcessResultValidation processResultValidation,
        ProcessResourcePolicy? processResourcePolicy = null,
        CancellationToken cancellationToken = default)
    {
        Process actualProcess;
        
        if (processResourcePolicy is not null)
        {
            actualProcess = _processFactory.StartNew(process.StartInfo, processResourcePolicy);
        }
        else
        {
            actualProcess = _processFactory.StartNew(process.StartInfo);
        }
        
        return await _processFactory.ContinueWhenExitAsync(actualProcess, processResultValidation,
                cancellationToken);
    }

    public async Task<BufferedProcessResult> ExecuteBufferedProcessConfigAsync(ProcessConfiguration processConfiguration,
        CancellationToken cancellationToken = new CancellationToken())
    {
        
    }

    public async Task<PipedProcessResult> ExecutePipedProcessConfigAsync(ProcessConfiguration processConfiguration,
        CancellationToken cancellationToken = new CancellationToken())
    {
        
    }

    public async Task<PipedProcessResult> ExecutePipedProcessAsync(Process process,
        ProcessConfiguration processConfiguration,
        CancellationToken cancellationToken = default)
    {
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;

        Process actualProcess = _processFactory.StartNew(processConfiguration);

        return await _processFactory.ContinueWhenExitPipedAsync(actualProcess, processConfiguration.ResultValidation,
            cancellationToken);
    }

    public async Task<PipedProcessResult> ExecutePipedProcessAsync(Process process,
        ProcessResultValidation processResultValidation,
        ProcessResourcePolicy? processResourcePolicy = null, CancellationToken cancellationToken = default)
    {
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;

        Process actualProcess;
        
        if (processResourcePolicy is not null)
        {
           actualProcess  = _processFactory.StartNew(process.StartInfo, processResourcePolicy);
        }
        else
        {
            actualProcess = _processFactory.StartNew(process.StartInfo);
        }

        return await _processFactory.ContinueWhenExitPipedAsync(actualProcess, processResultValidation,
            cancellationToken);
    }

    public async Task<BufferedProcessResult> ExecuteBufferedProcessAsync(Process process,
        ProcessConfiguration processConfiguration,
        CancellationToken cancellationToken = default)
    {
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        
        Process actualProcess = _processFactory.StartNew(processConfiguration);
        
        return await _processFactory.ContinueWhenExitBufferedAsync(actualProcess, processConfiguration.ResultValidation,
            cancellationToken);
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
    public async Task<BufferedProcessResult> ExecuteBufferedProcessAsync(Process process,
        ProcessResultValidation processResultValidation,
        ProcessResourcePolicy? processResourcePolicy = null,
        CancellationToken cancellationToken = default)
    {
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;

        Process actualProcess;

        if (processResourcePolicy is not null)
        {
            actualProcess = _processFactory.StartNew(process.StartInfo, processResourcePolicy);
        }
        else
        {
            actualProcess = _processFactory.StartNew(process.StartInfo);
        }
        
        return await _processFactory.ContinueWhenExitBufferedAsync(actualProcess, processResultValidation,
            cancellationToken);
    }
}