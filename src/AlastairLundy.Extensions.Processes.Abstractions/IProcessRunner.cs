﻿using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AlastairLundy.Extensions.Processes.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProcessRunner
    {
        /// <summary>
        /// Runs the process asynchronously, waits for exit, and safely disposes of the Process before returning.
        /// </summary>
        /// <param name="process">The process to be run.</param>
        /// <param name="processConfiguration"></param>
        /// <param name="cancellationToken">A token to cancel the operation if required.</param>
        /// <returns>The Process Results from the running the process.</returns>
        public Task<ProcessResult> ExecuteProcessAsync(Process process, 
            ProcessConfiguration processConfiguration,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Runs the process asynchronously, waits for exit, and safely disposes of the Process before returning.
        /// </summary>
        /// <param name="process">The process to be run.</param>
        /// <param name="processResultValidation">The process result validation to be used.</param>
        /// <param name="processResourcePolicy">The process resource policy to be set if it is not null.</param>
        /// <param name="cancellationToken">A token to cancel the operation if required.</param>
        /// <returns>The Process Results from the running the process.</returns>
        public Task<ProcessResult> ExecuteProcessAsync(Process process, ProcessResultValidation processResultValidation,
            ProcessResourcePolicy? processResourcePolicy = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Runs the process asynchronously, waits for exit, and safely disposes of the Process before returning.
        /// </summary>
        /// <param name="process">The process to be run.</param>
        /// <param name="processConfiguration"></param>
        /// <param name="cancellationToken">A token to cancel the operation if required.</param>
        /// <returns>The Buffered Process Results from running the process.</returns>
        public Task<BufferedProcessResult> ExecuteBufferedProcessAsync(Process process,
            ProcessConfiguration processConfiguration,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Runs the process asynchronously, waits for exit, and safely disposes of the Process before returning.
        /// </summary>
        /// <param name="process">The process to be run.</param>
        /// <param name="processResultValidation">The process result validation to be used.</param>
        /// <param name="processResourcePolicy">The process resource policy to be set if it is not null.</param>
        /// <param name="cancellationToken">A token to cancel the operation if required.</param>
        /// <returns>The Buffered Process Results from running the process.</returns>
        public Task<BufferedProcessResult> ExecuteBufferedProcessAsync(Process process,
            ProcessResultValidation processResultValidation,
            ProcessResourcePolicy? processResourcePolicy = null,
            CancellationToken cancellationToken = default);
    }
}