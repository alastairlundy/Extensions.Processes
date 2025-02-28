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
using System.Net.Sockets;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

using AlastairLundy.Extensions.Processes.Piping.Abstractions;

namespace AlastairLundy.Extensions.Processes.Piping;

/// <summary>
/// A class to allow for piping Streams into and out of Process objects.
/// </summary>
public class ProcessPipeHandler : IProcessPipeHandler
{
    /// <summary>
    /// Asynchronously copies the Stream to the process' standard input.
    /// </summary>
    /// <param name="source">The Stream to be copied from.</param>
    /// <param name="destination">The process to be copied to</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The destination process with the copied stream.</returns>
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
    public async Task PipeStandardInputAsync(PipeReader source, Process destination,
        CancellationToken cancellationToken = default)
    {
        if (destination.StartInfo.RedirectStandardInput && destination.StandardInput != StreamWriter.Null)
        {
            await destination.StandardInput.FlushAsync(cancellationToken);
         
            await source.CopyToAsync(destination.StandardInput.BaseStream, cancellationToken);
        }
    }

    /// <summary>
    /// Asynchronously copies the process' Standard Output to a Stream.
    /// </summary>
    /// <param name="source">The process to be copied from.</param>
    /// <param name="destination"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The copied Standard Output stream if it was successfully copied; null otherwise.</returns>
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
    public async Task PipeStandardOutputAsync(Process source, PipeWriter destination,
        CancellationToken cancellationToken = default)
    {
        if (source.StartInfo.RedirectStandardOutput)
        {
            if (source.StandardOutput != StreamReader.Null)
            {
               const int minimumBufferSize = 512;

               while (true)
               {
                   Memory<byte> memory = destination.GetMemory(minimumBufferSize);

                   try
                   {
                       int bytesRead = await source.StandardOutput.BaseStream.ReadAsync(memory, cancellationToken);

                       if (bytesRead == 0)
                       {
                           break;
                       }

                       destination.Advance(bytesRead);
                   }
                   catch(Exception ex)
                   {
                       throw new Exception(ex.Message);
                   }
                   
                   FlushResult flushResult = await destination.FlushAsync(cancellationToken);

                   if (flushResult.IsCompleted)
                   {
                       break;
                   }
               }
               
               await destination.CompleteAsync();
            }
        }
    }

    /// <summary>
    /// Asynchronously copies the process' Standard Error to a Stream.
    /// </summary>
    /// <param name="source">The process to be copied from.</param>
    /// <param name="destination"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The copied Standard Error stream if it was successfully copied; null otherwise.</returns>
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
    public async Task PipeStandardErrorAsync(Process source, PipeWriter destination,
        CancellationToken cancellationToken = default)
    {
        Stream output = Stream.Null;
        
        if (source.StartInfo.RedirectStandardError)
        {
            if (source.StandardError != StreamReader.Null)
            {
                const int minimumBufferSize = 512;

                while (true)
                {
                    Memory<byte> memory = destination.GetMemory(minimumBufferSize);

                    try
                    {
                        int bytesRead = await source.StandardError.BaseStream.ReadAsync(memory, cancellationToken);

                        if (bytesRead == 0)
                        {
                            break;
                        }

                        destination.Advance(bytesRead);
                    }
                    catch(Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                   
                    FlushResult flushResult = await destination.FlushAsync(cancellationToken);

                    if (flushResult.IsCompleted)
                    {
                        break;
                    }
                }
               
                await destination.CompleteAsync();
            }
        }
    }
}