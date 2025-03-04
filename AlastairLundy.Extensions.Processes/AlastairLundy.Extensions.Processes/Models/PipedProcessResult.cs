using System;
using System.IO.Pipelines;

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