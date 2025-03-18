using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AlastairLundy.Extensions.Processes.Abstractions
{
    /// <summary>
    /// An interface for running processes efficiently.
    /// </summary>
    public interface IProcessRunner
    {
        Task<ProcessResult> ExecuteProcessConfigAsync(ProcessConfiguration processConfiguration,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Runs the process asynchronously, waits for exit, and safely disposes of the Process before returning.
        /// </summary>
        /// <param name="process">The process to be run.</param>
        /// <param name="processConfiguration">The process configuration to use when running the Process.</param>
        /// <param name="cancellationToken">A token to cancel the operation if required.</param>
        /// <returns>The Process Results from the running the process.</returns>
         Task<ProcessResult> ExecuteProcessAsync(Process process, 
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
         Task<ProcessResult> ExecuteProcessAsync(Process process,
            ProcessResultValidation processResultValidation,
            ProcessResourcePolicy? processResourcePolicy = null,
            CancellationToken cancellationToken = default);
        
        
        Task<PipedProcessResult> ExecutePipedProcessConfigAsync(
            ProcessConfiguration processConfiguration,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Runs the process asynchronously, waits for exit, and safely disposes of the Process before returning.
        /// </summary>
        /// <param name="process">The process to be run.</param>
        /// <param name="processConfiguration">The process configuration to use when running the Process.</param>
        /// <param name="cancellationToken">A token to cancel the operation if required.</param>
        /// <returns>The Piped Process Results from running the process.</returns>
        Task<PipedProcessResult> ExecutePipedProcessAsync(Process process, 
            ProcessConfiguration processConfiguration,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Runs the process asynchronously, waits for exit, and safely disposes of the Process before returning.
        /// </summary>
        /// <param name="process">The process to be run.</param>
        /// <param name="processResultValidation">The process result validation to be used.</param>
        /// <param name="processResourcePolicy">The process resource policy to be set if it is not null.</param>
        /// <param name="cancellationToken">A token to cancel the operation if required.</param>
        /// <returns>The Piped Process Results from running the process.</returns>
        Task<PipedProcessResult> ExecutePipedProcessAsync(Process process, ProcessResultValidation processResultValidation,
            ProcessResourcePolicy? processResourcePolicy = null,
            CancellationToken cancellationToken = default);
        
        Task<BufferedProcessResult> ExecuteBufferedProcessConfigAsync(ProcessConfiguration processConfiguration,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Runs the process asynchronously, waits for exit, and safely disposes of the Process before returning.
        /// </summary>
        /// <param name="process">The process to be run.</param>
        /// <param name="processConfiguration">The process configuration to use when running the Process.</param>
        /// <param name="cancellationToken">A token to cancel the operation if required.</param>
        /// <returns>The Buffered Process Results from running the process.</returns>
         Task<BufferedProcessResult> ExecuteBufferedProcessAsync(Process process,
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
         Task<BufferedProcessResult> ExecuteBufferedProcessAsync(Process process,
            ProcessResultValidation processResultValidation,
            ProcessResourcePolicy? processResourcePolicy = null,
            CancellationToken cancellationToken = default);
    }
}