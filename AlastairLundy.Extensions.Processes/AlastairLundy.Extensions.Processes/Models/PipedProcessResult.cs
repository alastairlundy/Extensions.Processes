/*
    AlastairLundy.Extensions.Processes 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.IO.Pipelines;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace AlastairLundy.Extensions.Processes;

public class PipedProcessResult : ProcessResult
{
    public PipedProcessResult(string executableFilePath,
        int exitCode,
        DateTime startTime,
        DateTime exitTime, Pipe standardOutput,
        Pipe standardError) : base(executableFilePath,
        exitCode,
        startTime,
        exitTime)
    {
        StandardOutput = standardOutput;
        StandardError = standardError;
    }
    
    /// <summary>
    /// 
    /// </summary>
    public Pipe StandardOutput { get; init; }
    
    /// <summary>
    /// 
    /// </summary>
    public Pipe StandardError { get; init; }
}