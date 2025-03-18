/*
    AlastairLundy.Extensions.Processes.Abstractions  
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

namespace AlastairLundy.Extensions.Processes.Abstractions.Piping
{
    /// <summary>
    /// An interface to allow for a standardized way of Process pipe handling.
    /// </summary>
    public interface IProcessPipeHandler
    {
        /// <summary>
        /// Asynchronously copies the Stream to the process' standard input.
        /// </summary>
        /// <param name="source">The Stream to be copied from.</param>
        /// <param name="destination">The process to be copied to</param>
        /// <param name="cancellationToken"></param>
        Task PipeStandardInputAsync(Stream source, Process destination,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously copies the Pipe to the process' standard input.
        /// </summary>
        /// <param name="source">The Pipe to be copied from.</param>
        /// <param name="destination">The process to be copied to</param>
        /// <param name="cancellationToken"></param>
        Task PipeStandardInputAsync(Pipe source, Process destination,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously copies the process' Standard Output to a Stream.
        /// </summary>
        /// <param name="source">The process to be copied from.</param>
        /// <param name="destination">The Stream to be copied to</param>
        /// <param name="cancellationToken"></param>
        Task PipeStandardOutputAsync(Process source, Stream destination,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously copies the process' Standard Output to a Pipe.
        /// </summary>
        /// <param name="source">The process to be copied from.</param>
        /// <param name="destination">The Pipe to be copied to</param>
        /// <param name="cancellationToken"></param>
        Task PipeStandardOutputAsync(Process source, Pipe destination,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously copies the process' Standard Error to a Stream.
        /// </summary>
        /// <param name="source">The process to be copied from.</param>
        /// <param name="destination">The Stream to be copied to</param>
        /// <param name="cancellationToken"></param>
        Task PipeStandardErrorAsync(Process source, Stream destination,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously copies the process' Standard Error to a Pipe.
        /// </summary>
        /// <param name="source">The process to be copied from.</param>
        /// <param name="destination">The Pipe to be copied to</param>
        /// <param name="cancellationToken"></param>
        Task PipeStandardErrorAsync(Process source, Pipe destination,
            CancellationToken cancellationToken = default);
    }
}