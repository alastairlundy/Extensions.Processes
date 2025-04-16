﻿/*
    ProcessInvoke.Primitives 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.

    Based on Tyrrrz's CliWrap CommandResult.cs
    https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/CommandResult.cs

     Constructor signature and field declarations from CliWrap licensed under the MIT License except where considered Copyright Fair Use by law.
     See THIRD_PARTY_NOTICES.txt for a full copy of the MIT LICENSE.
 */

using System;

// ReSharper disable MemberCanBeProtected.Global

// ReSharper disable MemberCanBePrivate.Global

namespace AlastairLundy.ProcessInvoke.Primitives.Results
{
    /// <summary>
    /// A class that represents the results from an executed Process or Command.
    /// </summary>
    public class ProcessResult
    {
        /// <summary>
        /// Instantiates a ProcessResult with data about a Process' execution.
        /// </summary>
        /// <param name="executableFilePath">The file path of the file that was executed.</param>
        /// <param name="exitCode">The process' exit code.</param>
        /// <param name="startTime">The start time of the process.</param>
        /// <param name="exitTime">The exit time of the process.</param>
        public ProcessResult(string executableFilePath,
            int exitCode,
            DateTime startTime,
            DateTime exitTime)
        {
            ExitCode = exitCode;
            ExecutedFilePath = executableFilePath;
            StartTime = startTime;
            ExitTime = exitTime;
        }

        /// <summary>
        /// Whether the Command successfully exited.
        /// </summary>
        public bool WasSuccessful => ExitCode == 0;
        
        /// <summary>
        /// The exit code from the Command that was executed.
        /// </summary>
        public int ExitCode { get; }

        /// <summary>
        /// The file path of the file to be executed.
        /// </summary>
        public string ExecutedFilePath { get; }

        /// <summary>
        /// The Date and Time that the Command's execution started.
        /// </summary>
        public DateTime StartTime { get; }

        /// <summary>
        /// The Date and Time that the Command's execution finished.
        /// </summary>
        public DateTime ExitTime { get; }

        /// <summary>
        /// How long the Command took to execute represented as a TimeSpan.
        /// </summary>
        public TimeSpan RuntimeDuration => ExitTime.Subtract(StartTime);
    }
}
