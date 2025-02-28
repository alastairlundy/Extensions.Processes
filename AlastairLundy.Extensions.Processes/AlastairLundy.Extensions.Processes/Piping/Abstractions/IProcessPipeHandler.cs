/*
    AlastairLundy.Extensions.Processes  
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace AlastairLundy.Extensions.Processes.Piping.Abstractions;

/// <summary>
/// An interface to allow for a standardized way of Process pipe handling.
/// </summary>
public interface IProcessPipeHandler
{
    /// <summary>
    /// Asynchronously copies the Stream to the process' standard input.
    /// </summary>
    /// <param name="source">The Pipe to be copied from.</param>
    /// <param name="destination">The process to be copied to</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The destination process with the copied stream.</returns>
    Task PipeStandardInputAsync(PipeReader source, Process destination, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously copies the process' Standard Output to a Stream.
    /// </summary>
    /// <param name="source">The process to be copied from.</param>
    /// <param name="destination">The Pipe to be copied to.</param>
    /// <param name="cancellationToken"></param>
    Task PipeStandardOutputAsync(Process source, PipeWriter destination, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously copies the process' Standard Error to a Stream.
    /// </summary>
    /// <param name="source">The process to be copied from.</param>
    /// <param name="destination">The Pipe to copied to.</param>
    /// <param name="cancellationToken"></param>
    Task PipeStandardErrorAsync(Process source, PipeWriter destination, CancellationToken cancellationToken = default);
}