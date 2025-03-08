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
using System.IO.Pipelines;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using AlastairLundy.Extensions.Processes.Piping.Abstractions;

namespace AlastairLundy.Extensions.Processes.Piping;

/// <summary>
/// 
/// </summary>
public class ProcessPipeHandler : IProcessPipeHandler
{
    /// <summary>
    /// Asynchronously copies the Stream to the process' standard input.
    /// </summary>
    /// <param name="source">The Stream to be copied from.</param>
    /// <param name="destination">The process to be copied to</param>
    /// <param name="cancellationToken"></param>
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
    public async Task PipeStandardInputAsync(Stream source, Process destination,
        CancellationToken cancellationToken = default)
    {
        if (destination.StartInfo.RedirectStandardInput && destination.StandardInput != StreamWriter.Null)
        {
            await destination.StandardInput.FlushAsync(cancellationToken);
            destination.StandardInput.BaseStream.Position = 0;
            await source.CopyToAsync(destination.StandardInput.BaseStream, cancellationToken); 
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    /// <param name="cancellationToken"></param>
    public async Task PipeStandardInputAsync(Pipe source, Process destination,
        CancellationToken cancellationToken = default)
    {
        if (destination.StartInfo.RedirectStandardInput && destination.StandardInput != StreamWriter.Null)
        {
            await destination.StandardInput.FlushAsync(cancellationToken);
         
            await source.Reader.CopyToAsync(destination.StandardInput.BaseStream, cancellationToken);
        }
    }

    /// <summary>
    /// Asynchronously copies the process' Standard Output to a Stream.
    /// </summary>
    /// <param name="source">The process to be copied from.</param>
    /// <param name="destination">The Stream to be copied to</param>
    /// <param name="cancellationToken"></param>
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
    public async Task PipeStandardOutputAsync(Process source, Stream destination,
        CancellationToken cancellationToken = default)
    {
        if (source.StartInfo.RedirectStandardOutput)
        {
            if (source.StandardOutput != StreamReader.Null)
            {
                await source.StandardOutput.BaseStream.CopyToAsync(destination, cancellationToken);
            }
        }
    }

    /// <summary>
    /// Asynchronously copies the Pipe to the process' standard input.
    /// </summary>
    /// <param name="source">The Pipe to be copied from.</param>
    /// <param name="destination">The process to be copied to</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="Exception"></exception>
    public async Task PipeStandardOutputAsync(Process source, Pipe destination,
        CancellationToken cancellationToken = default)
    {
        if (source.StartInfo.RedirectStandardOutput)
        {
            if (source.StandardOutput != StreamReader.Null)
            {
                const int minimumBufferSize = 512;

                while (true)
                {
                    Memory<byte> memory = destination.Writer.GetMemory(minimumBufferSize);

                    try
                    {
                        int bytesRead = await source.StandardOutput.BaseStream.ReadAsync(memory, cancellationToken);

                        if (bytesRead == 0)
                        {
                            break;
                        }

                        destination.Writer.Advance(bytesRead);
                    }
                    catch(Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                   
                    FlushResult flushResult = await destination.Writer.FlushAsync(cancellationToken);

                    if (flushResult.IsCompleted)
                    {
                        break;
                    }
                }
               
                await destination.Writer.CompleteAsync();
            }
        }
    }

    /// <summary>
    /// Asynchronously copies the process' Standard Output to a Pipe.
    /// </summary>
    /// <param name="source">The process to be copied from.</param>
    /// <param name="destination">The Pipe to be copied to.</param>
    /// <param name="cancellationToken"></param>
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
    public async Task PipeStandardErrorAsync(Process source, Stream destination,
        CancellationToken cancellationToken = default)
    {
        if (source.StartInfo.RedirectStandardError)
        {
            if (source.StandardError != StreamReader.Null)
            {
                await source.StandardError.BaseStream.CopyToAsync(destination, cancellationToken);
            }
        }
    }

    /// <summary>
    /// Asynchronously copies the process' Standard Error to a Pipe.
    /// </summary>
    /// <param name="source">The process to be copied from.</param>
    /// <param name="destination">The Pipe to copied to.</param>
    /// <param name="cancellationToken"></param>
    public async Task PipeStandardErrorAsync(Process source, Pipe destination,
        CancellationToken cancellationToken = default)
    {
        if (source.StartInfo.RedirectStandardError)
        {
            if (source.StandardError != StreamReader.Null)
            {
                const int minimumBufferSize = 512;

                while (true)
                {
                    Memory<byte> memory = destination.Writer.GetMemory(minimumBufferSize);

                    try
                    {
                        int bytesRead = await source.StandardError.BaseStream.ReadAsync(memory, cancellationToken);

                        if (bytesRead == 0)
                        {
                            break;
                        }

                        destination.Writer.Advance(bytesRead);
                    }
                    catch(Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                   
                    FlushResult flushResult = await destination.Writer.FlushAsync(cancellationToken);

                    if (flushResult.IsCompleted)
                    {
                        break;
                    }
                }
               
                await destination.Writer.CompleteAsync();
            }
        }
    }
}