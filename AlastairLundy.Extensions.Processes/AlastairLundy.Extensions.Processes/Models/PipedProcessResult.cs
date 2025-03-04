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

/// <summary>
/// A ProcessResult extended with Standard Output and Standard Error Pipes.
/// </summary>
public class PipedProcessResult : ProcessResult, IEquatable<PipedProcessResult>
{
    /// <summary>
    /// Instantiates the PipedProcessResult
    /// </summary>
    /// <param name="executableFilePath"></param>
    /// <param name="exitCode"></param>
    /// <param name="startTime"></param>
    /// <param name="exitTime"></param>
    /// <param name="standardOutput"></param>
    /// <param name="standardError"></param>
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
    /// The Standard Output from a Process or Command as a Pipe.
    /// </summary>
    public Pipe StandardOutput { get; init; }
    
    /// <summary>
    /// The Standard Error from a Process or Command as a Pipe.
    /// </summary>
    public Pipe StandardError { get; init; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(PipedProcessResult? other)
    {
        if (other is null)
        {
            return false;
        }
        
        return StandardOutput.Equals(other.StandardOutput) &&
               StandardError.Equals(other.StandardError) &&
               ExitCode.Equals(other.ExitCode)
               && StartTime.Equals(other.StartTime)
               && ExitTime.Equals(other.ExitTime)
               && ExecutedFilePath.Equals(other.ExecutedFilePath);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }
    
        return Equals((PipedProcessResult)obj);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool Equals(PipedProcessResult? left, PipedProcessResult? right)
    {
        if (left != null && right != null)
        {
            return left.Equals(right);
        }

        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(StandardOutput, StandardError);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(PipedProcessResult? left, PipedProcessResult? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(PipedProcessResult? left, PipedProcessResult? right)
    {
        return Equals(left, right) == false;
    }
}