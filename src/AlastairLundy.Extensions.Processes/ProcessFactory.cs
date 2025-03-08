/*
    AlastairLundy.Extensions.Processes
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

using AlastairLundy.Extensions.IO.Files.Abstractions;

using AlastairLundy.Extensions.Processes.Abstractions;
using AlastairLundy.Extensions.Processes.Exceptions;
using AlastairLundy.Extensions.Processes.Internal.Localizations;
using AlastairLundy.Extensions.Processes.Piping.Abstractions;

#if NET5_0_OR_GREATER
#endif

// ReSharper disable UnusedType.Global

namespace AlastairLundy.Extensions.Processes;

public class ProcessFactory : IProcessFactory
{
    private readonly IProcessPipeHandler _processPipeHandler;
    private readonly IFilePathResolver _filePathResolver;
    
    public ProcessFactory(IProcessPipeHandler processPipeHandler, IFilePathResolver filePathResolver)
    {
        _processPipeHandler = processPipeHandler;
        _filePathResolver = filePathResolver;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="processStartInfo"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
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
    /// 
    /// </summary>
    /// <param name="startInfo"></param>
    /// <param name="credential"></param>
    /// <returns></returns>
    public Process From(ProcessStartInfo startInfo, UserCredential credential)
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
    /// 
    /// </summary>
    /// <param name="startInfo"></param>
    /// <param name="standardInput"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Process> FromAsync(ProcessStartInfo startInfo, Pipe standardInput, CancellationToken cancellationToken = default)
    {
        Process output = From(startInfo);
        
        output.StartInfo.RedirectStandardInput = true;
        
        await _processPipeHandler.PipeStandardInputAsync(standardInput, output, cancellationToken);
            
        return output;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="startInfo"></param>
    /// <param name="standardInput"></param>
    /// <param name="userCredential"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Process> FromAsync(ProcessStartInfo startInfo, Pipe standardInput, UserCredential userCredential,
        CancellationToken cancellationToken = default)
    {
        Process output = From(startInfo, userCredential);
        
        output.StartInfo.RedirectStandardInput = true;
        
        await _processPipeHandler.PipeStandardInputAsync(standardInput, output, cancellationToken);
            
        return output;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="startInfo"></param>
    /// <returns></returns>
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
    /// 
    /// </summary>
    /// <param name="startInfo"></param>
    /// <param name="credential"></param>
    /// <returns></returns>
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
    public Process StartNew(ProcessStartInfo startInfo, UserCredential credential)
    {
        Process process = From(startInfo, credential);
        
        process.Start();
        
        return process;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="startInfo"></param>
    /// <param name="resourcePolicy"></param>
    /// <returns></returns>
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
    public Process StartNew(ProcessStartInfo startInfo, ProcessResourcePolicy resourcePolicy)
    {
        Process process = From(startInfo);

        process.Start();
        
        process.SetResourcePolicy(resourcePolicy);
        
        return process;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="startInfo"></param>
    /// <param name="resourcePolicy"></param>
    /// <param name="credential"></param>
    /// <returns></returns>
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
    public Process StartNew(ProcessStartInfo startInfo, ProcessResourcePolicy resourcePolicy, UserCredential credential)
    {
        Process process = From(startInfo, credential);
        
        process.Start();
        
        process.SetResourcePolicy(resourcePolicy);

        return process;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="process"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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
    public async Task<ProcessResult> ContinueWhenExitAsync(Process process, CancellationToken cancellationToken = default)
    {
        return await ContinueWhenExitAsync(process, ProcessResultValidation.None, cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="process"></param>
    /// <param name="resultValidation"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ProcessNotSuccessfulException"></exception>
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
    public async Task<ProcessResult> ContinueWhenExitAsync(Process process, ProcessResultValidation resultValidation,
        CancellationToken cancellationToken = default)
    {
        await process.WaitForExitAsync(cancellationToken);

        if (process.ExitCode != 0 && resultValidation == ProcessResultValidation.ExitCodeZero)
        {
            throw new ProcessNotSuccessfulException(exitCode: process.ExitCode, process: process);
        }
        
        ProcessResult processResult = new ProcessResult(process.StartInfo.FileName, process.ExitCode, process.StartTime,
            process.ExitTime);
        
        process.Dispose();
        
        return processResult;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="processStartInfo"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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
    public async Task<BufferedProcessResult> ContinueWhenExitBufferedAsync(Process processStartInfo, CancellationToken cancellationToken = default)
    {
        return await ContinueWhenExitBufferedAsync(processStartInfo, ProcessResultValidation.None, cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="process"></param>
    /// <param name="resultValidation"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ProcessNotSuccessfulException"></exception>
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
    public async Task<BufferedProcessResult> ContinueWhenExitBufferedAsync(Process process, ProcessResultValidation resultValidation,
        CancellationToken cancellationToken = default)
    {
        await process.WaitForExitAsync(cancellationToken);
        
        if (process.ExitCode != 0 && resultValidation == ProcessResultValidation.ExitCodeZero)
        {
            throw new ProcessNotSuccessfulException(exitCode: process.ExitCode, process: process);
        }
        
        BufferedProcessResult processResult = new BufferedProcessResult(
            process.StartInfo.FileName, process.ExitCode,
            await process.StandardOutput.ReadToEndAsync(),  await process.StandardError.ReadToEndAsync(),
            process.StartTime, process.ExitTime);
        
        process.Dispose();
        
        return processResult;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="process"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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
    public async Task<PipedProcessResult> ContinueWhenExitPipedAsync(Process process, CancellationToken cancellationToken = default)
    {
        return await ContinueWhenExitPipedAsync(process, ProcessResultValidation.None, cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="process"></param>
    /// <param name="resultValidation"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ProcessNotSuccessfulException"></exception>
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
    public async Task<PipedProcessResult> ContinueWhenExitPipedAsync(Process process, ProcessResultValidation resultValidation,
        CancellationToken cancellationToken = default)
    {
        await process.WaitForExitAsync(cancellationToken);
        
        if (process.ExitCode != 0 && resultValidation == ProcessResultValidation.ExitCodeZero)
        {
            throw new ProcessNotSuccessfulException(exitCode: process.ExitCode, process: process);
        }

        Pipe standardOutput = new Pipe();
        Pipe standardError = new Pipe();

        await _processPipeHandler.PipeStandardOutputAsync(process, standardOutput, cancellationToken);
        await _processPipeHandler.PipeStandardErrorAsync(process, standardError, cancellationToken);
        
        PipedProcessResult processResult = new PipedProcessResult(
            process.StartInfo.FileName, process.ExitCode,
            process.StartTime, process.ExitTime,
            standardOutput,
            standardError);
        
        process.Dispose();
        
        return processResult;
    }
}