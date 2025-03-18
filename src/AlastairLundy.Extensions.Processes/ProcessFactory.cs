/*
    AlastairLundy.Extensions.Processes
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using AlastairLundy.Extensions.IO.Files.Abstractions;
using AlastairLundy.Extensions.Processes.Exceptions;
using AlastairLundy.Extensions.Processes.Internal.Localizations;

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
using System.IO;
#endif

// ReSharper disable UnusedType.Global

namespace AlastairLundy.Extensions.Processes;

/// <summary>
/// 
/// </summary>
public class ProcessFactory : Processes.Abstractions.IProcessFactory
{
    private readonly IFilePathResolver _filePathResolver;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePathResolver"></param>
    public ProcessFactory(IFilePathResolver filePathResolver)
    {
        _filePathResolver = filePathResolver;
    }
    
    /// <summary>
    /// Creates a process from the specified start info.
    /// </summary>
    /// <param name="processStartInfo">The start information to use for the Process.</param>
    /// <returns>The newly created Process.</returns>
    /// <exception cref="ArgumentException">Thrown if the process start info FileName is empty.</exception>
    public Process From(ProcessStartInfo processStartInfo)
    {
        if (string.IsNullOrEmpty(processStartInfo.FileName))
        {
            throw new ArgumentException(Resources.Process_FileName_Empty);
        }
        
#if NET5_0_OR_GREATER
        if (Path.IsPathFullyQualified(processStartInfo.FileName) == false)
        {
            _filePathResolver.ResolveFilePath(processStartInfo.FileName, out string resolvedFilePath);
            processStartInfo.FileName = resolvedFilePath;
        }
#else
          _filePathResolver.ResolveFilePath(processStartInfo.FileName, out string resolvedFilePath);
            processStartInfo.FileName = resolvedFilePath;
#endif

        Process output = new Process
        {
            StartInfo = processStartInfo,
        };
            
        return output;
    }

    /// <summary>
    /// Creates a process from the specified start info and UserCredential.
    /// </summary>
    /// <param name="startInfo">The start information to use for the Process.</param>
    /// <param name="credential">The credential to use when creating the Process.</param>
    /// <returns>The newly created Process.</returns>
    public Process From(ProcessStartInfo startInfo, Processes.Abstractions.UserCredential credential)
    {
        Process output = From(startInfo);

        if (credential.IsSupportedOnCurrentOS())
        {
#pragma warning disable CA1416
            output.AddUserCredential(credential);
#pragma warning restore CA1416
        }

        return output;
    }

    /// <summary>
    /// Creates a process from the specified process configuration.
    /// </summary>
    /// <param name="configuration">The configuration information to use to configure the Process.</param>
    /// <returns>The newly created Process with the configuration.</returns>
    public Process From(Processes.Abstractions.ProcessConfiguration configuration)
    {
        Process output;
        
        // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
        if (configuration.Credential != null)
        {
            output = From(configuration.StartInfo, configuration.Credential);
        }
        else
        {
            output = From(configuration.StartInfo);
        }

        return output; 
    }

    /// <summary>
    /// Creates and starts a new Process with the specified Process Start Info.
    /// </summary>
    /// <param name="startInfo">The start info to use when creating and starting the new Process.</param>
    /// <returns>The newly created and started Process with the start info.</returns>
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
    public Process StartNew(ProcessStartInfo startInfo)
    {
        Process process = From(startInfo);
        
        process.Start();
        
        return process;
    }

    /// <summary>
    /// Creates and starts a new Process with the specified Process Start Info and credential.
    /// </summary>
    /// <param name="startInfo">The start info to use when creating and starting the new Process.</param>
    /// <param name="credential">The credential to use when creating and starting the Process.</param>
    /// <returns>The newly created and started Process with the start info and credential.</returns>
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
    public Process StartNew(ProcessStartInfo startInfo,
        Processes.Abstractions.UserCredential credential)
    {
        Process process = From(startInfo, credential);
        
        process.Start();
        
        return process;
    }

    /// <summary>
    /// Creates and starts a new Process with the specified Process Start Info and Process Resource policy.
    /// </summary>
    /// <param name="startInfo">The start info to use when creating and starting the new Process.</param>
    /// <param name="resourcePolicy">The process resource policy to use when creating and starting the new Process.</param>
    /// <returns>The newly created and started Process with the start info and Process Resource Policy.</returns>
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
    public Process StartNew(ProcessStartInfo startInfo,
        Processes.Abstractions.ProcessResourcePolicy resourcePolicy)
    {
        Process process = From(startInfo);

        process.Start();
        
        process.SetResourcePolicy(resourcePolicy);
        
        return process;
    }

    /// <summary>
    /// Creates and starts a new Process with the specified Process Start Info, credential, and Process Resource policy.
    /// </summary>
    /// <param name="startInfo">The start info to use when creating and starting the new Process.</param>
    /// <param name="resourcePolicy">The process resource policy to use when creating and starting the new Process.</param>
    /// <param name="credential">The credential to use when creating and starting the Process.</param>
    /// <returns>The newly created and started Process with the start info and Process Resource Policy.</returns>
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
    public Process StartNew(ProcessStartInfo startInfo, 
        Processes.Abstractions.ProcessResourcePolicy resourcePolicy,
        Processes.Abstractions.UserCredential credential)
    {
        Process process = From(startInfo, credential);
        
        process.Start();
        
        process.SetResourcePolicy(resourcePolicy);

        return process;
    }

    /// <summary>
    /// Creates and starts a new Process with the specified configuration.
    /// </summary>
    /// <param name="configuration">The configuration to use when creating and starting the process.</param>
    /// <returns>The newly created and started Process with the specified configuration.</returns>
    public Process StartNew(Processes.Abstractions.ProcessConfiguration configuration)
    {
        Process process = From(configuration);

        if (configuration.StandardInput is not null)
        {
            process.StartInfo.RedirectStandardInput = true;
            configuration.StandardInput.BaseStream.CopyTo(process.StandardInput.BaseStream);
        }
        
        process.Start();
        
        if (configuration.ResourcePolicy != null)
        {
            process.SetResourcePolicy(configuration.ResourcePolicy);
        }
        
        return process;
    }

    /// <summary>
    /// Creates a Task that returns a ProcessResult when the specified process exits.
    /// </summary>
    /// <param name="process">The process to continue and wait for exit.</param>
    /// <param name="cancellationToken">The cancellation token to use in case cancellation is requested.</param>
    /// <returns>The task and processResult that are returned upon completion of the task.</returns>
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
    public async Task<Processes.Abstractions.ProcessResult> ContinueWhenExitAsync(Process process, CancellationToken cancellationToken = default)
    {
        return await ContinueWhenExitAsync(process, Processes.Abstractions.ProcessResultValidation.None, cancellationToken);
    }

    /// <summary>
    /// Creates a Task that returns a ProcessResult when the specified process exits.
    /// </summary>
    /// <param name="process">The process to continue and wait for exit.</param>
    /// <param name="resultValidation">Whether to perform Result validation on the process' exit code.</param>
    /// <param name="cancellationToken">The cancellation token to use in case cancellation is requested.</param>
    /// <returns>The task and ProcessResult that are returned upon completion of the task.</returns>
    /// <exception cref="ProcessNotSuccessfulException">Thrown if the process exit code is not zero AND exit code validation is performed.</exception>
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
    public async Task<Processes.Abstractions.ProcessResult> ContinueWhenExitAsync(Process process, Processes.Abstractions.ProcessResultValidation resultValidation,
        CancellationToken cancellationToken = default)
    {
        await process.WaitForExitAsync(cancellationToken);

        if (process.ExitCode != 0 && resultValidation == Processes.Abstractions.ProcessResultValidation.ExitCodeZero)
        {
            throw new ProcessNotSuccessfulException(exitCode: process.ExitCode, process: process);
        }
        
        Processes.Abstractions.ProcessResult processResult = new Processes.Abstractions.ProcessResult(process.StartInfo.FileName, process.ExitCode, process.StartTime,
            process.ExitTime);
        
        process.Dispose();
        
        return processResult;
    }

    /// <summary>
    /// Creates a Task that returns a BufferedProcessResult when the specified process exits.
    /// </summary>
    /// <param name="process">The process to continue and wait for exit.</param>
    /// <param name="cancellationToken">The cancellation token to use in case cancellation is requested.</param>
    /// <returns>The task and BufferedProcessResult that are returned upon completion of the task.</returns>
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
    public async Task<Processes.Abstractions.BufferedProcessResult> ContinueWhenExitBufferedAsync(Process process, CancellationToken cancellationToken = default)
    {
        return await ContinueWhenExitBufferedAsync(process, Processes.Abstractions.ProcessResultValidation.None, cancellationToken);
    }

    /// <summary>
    /// Creates a Task that returns a BufferedProcessResult when the specified process exits.
    /// </summary>
    /// <param name="process">The process to continue and wait for exit.</param>
    /// <param name="resultValidation">Whether to perform Result validation on the process' exit code.</param>
    /// <param name="cancellationToken">The cancellation token to use in case cancellation is requested.</param>
    /// <returns>The task and BufferedProcessResult that are returned upon completion of the task.</returns>
    /// <exception cref="ProcessNotSuccessfulException">Thrown if the process exit code is not zero AND exit code validation is performed.</exception>
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
    public async Task<Processes.Abstractions.BufferedProcessResult> ContinueWhenExitBufferedAsync(Process process,
        Processes.Abstractions.ProcessResultValidation resultValidation,
        CancellationToken cancellationToken = default)
    {
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        
        await process.WaitForExitAsync(cancellationToken);
        
        if (process.ExitCode != 0 && resultValidation == Processes.Abstractions.ProcessResultValidation.ExitCodeZero)
        {
            throw new ProcessNotSuccessfulException(exitCode: process.ExitCode, process: process);
        }
        
        Processes.Abstractions.BufferedProcessResult processResult = new Processes.Abstractions.BufferedProcessResult(
            process.StartInfo.FileName, process.ExitCode,
            await process.StandardOutput.ReadToEndAsync(cancellationToken),  await process.StandardError.ReadToEndAsync(),
            process.StartTime, process.ExitTime);
        
        process.Dispose();
        
        return processResult;
    }
}