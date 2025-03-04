/*
    AlastairLundy.Extensions.Processes
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System.Diagnostics;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace AlastairLundy.Extensions.Processes.Abstractions;

public interface IProcessFactory
{
    public Process From(ProcessStartInfo startInfo);
    public Process From(ProcessStartInfo startInfo, UserCredential credential);
    
    public Task<Process> FromAsync(ProcessStartInfo startInfo, Pipe standardInput, CancellationToken cancellationToken = default);
    public Task<Process> FromAsync(ProcessStartInfo startInfo, Pipe standardInput, UserCredential userCredential, CancellationToken cancellationToken = default);

    
    public Process StartNew(ProcessStartInfo startInfo);
    public Process StartNew(ProcessStartInfo startInfo, UserCredential credential);
    public Process StartNew(ProcessStartInfo startInfo, ProcessResourcePolicy resourcePolicy);
    public Process StartNew(ProcessStartInfo startInfo, ProcessResourcePolicy resourcePolicy, UserCredential credential);


    public Task<ProcessResult> ContinueWhenExitAsync(Process process, CancellationToken cancellationToken = default);
    public Task<ProcessResult> ContinueWhenExitAsync(Process process, ProcessResultValidation resultValidation, CancellationToken cancellationToken = default);
    
    public Task<BufferedProcessResult> ContinueWhenExitBufferedAsync(Process process, CancellationToken cancellationToken = default);
    public Task<BufferedProcessResult> ContinueWhenExitBufferedAsync(Process process, ProcessResultValidation resultValidation, CancellationToken cancellationToken = default);

    public Task<PipedProcessResult> ContinueWhenExitPipedAsync(Process process, CancellationToken cancellationToken = default);
    public Task<PipedProcessResult> ContinueWhenExitPipedAsync(Process process, ProcessResultValidation resultValidation, CancellationToken cancellationToken = default);
}