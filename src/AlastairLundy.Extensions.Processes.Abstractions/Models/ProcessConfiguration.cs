/*
    AlastairLundy.Extensions.Processes
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace AlastairLundy.Extensions.Processes.Abstractions
{
        /// <summary>
        /// A class to store Process configuration information.
        /// </summary>
        public class ProcessConfiguration
        {
                /// <summary>
                /// Instantiates the Process Configuration class with a ProcessStartInfo and other optional parameters.
                /// </summary>
                /// <param name="processStartInfo"></param>
                /// <param name="environmentVariables">The environment variables to be set (if specified).</param>
                /// <param name="credential">The credential to be used (if specified).</param>
                /// <param name="resultValidation">Whether to perform Result Validation and exception throwing if the Command exits with an exit code other than 0.</param>
                /// <param name="timeOutThreshold"></param>
                /// <param name="standardInput">The standard input source to be used (if specified).</param>
                /// <param name="standardOutput">The standard output destination to be used (if specified).</param>
                /// <param name="standardError">The standard error destination to be used (if specified).</param>
                /// <param name="processResourcePolicy">The process resource policy to be used (if specified).</param>
                public ProcessConfiguration(ProcessStartInfo processStartInfo,
                        IReadOnlyDictionary<string, string>? environmentVariables = null,
                        UserCredential? credential = null,
                        ProcessResultValidation resultValidation = ProcessResultValidation.ExitCodeZero,
                        TimeSpan timeOutThreshold = default,
                        StreamWriter? standardInput = null,
                        StreamReader? standardOutput = null,
                        StreamReader? standardError = null,
                        ProcessResourcePolicy? processResourcePolicy = null)
                {
                        StartInfo = processStartInfo;
                        EnvironmentVariables = environmentVariables ?? new Dictionary<string, string>();
                
                        Credential = credential ?? UserCredential.Null;
            
                        ResourcePolicy = processResourcePolicy ?? ProcessResourcePolicy.Default;

                        ResultValidation = resultValidation;

                        StandardInput = standardInput ?? StreamWriter.Null;
                        StandardOutput = standardOutput ?? StreamReader.Null;
                        StandardError = standardError ?? StreamReader.Null;

                        TimeoutThreshold = timeOutThreshold == default ? TimeSpan.FromMinutes(30) : timeOutThreshold;

#if NET5_0_OR_GREATER
                        if (credential != null && OperatingSystem.IsWindows())
#else
                                
#endif
                        {
#pragma warning disable CA1416
                                if (credential.UserName != null)
                                {
                                        processStartInfo.Domain = credential.Domain;
                                }
                                
                                if (credential.Domain != null)
                                {
                                        processStartInfo.UserName = credential.UserName;
                                }

                                if (credential.Password != null)
                                {
                                        processStartInfo.Password = credential.Password;
                                }

                                if (credential.LoadUserProfile != null)
                                { 
                                        processStartInfo.LoadUserProfile = (bool)credential.LoadUserProfile;
                                }
#pragma warning restore CA1416
                        }
                }
        
                /// <summary>
                /// The environment variables to be set.
                /// </summary>
                public IReadOnlyDictionary<string, string> EnvironmentVariables { get; protected set; }
        
                /// <summary>
                /// The timespan after which a Process should no longer be allowed to continue waiting to exit.
                /// </summary>
                public TimeSpan TimeoutThreshold { get; private set; }
        
                /// <summary>
                /// The Process start information to be used.
                /// </summary>
                public ProcessStartInfo StartInfo { get; private set; }
        
                /// <summary>
                /// The credential to be used when executing the Command.
                /// </summary>
                public UserCredential? Credential { get; protected set; }

                /// <summary>
                /// The result validation to apply to the Command when it is executed.
                /// </summary>
                public ProcessResultValidation ResultValidation { get; protected set; }

                /// <summary>
                /// The Standard Input source to redirect Standard Input to if configured.
                /// </summary>
                /// <remarks>Using Shell Execution whilst also Redirecting Standard Input will throw an Exception. This is a known issue with the System Process class.</remarks>
                /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.redirectstandarderror" />
                public StreamWriter? StandardInput { get; protected set; }

                /// <summary>
                /// The Standard Output target to redirect Standard Output to if configured.
                /// </summary>
                public StreamReader? StandardOutput { get; protected set; }

                /// <summary>
                /// The Standard Error target to redirect Standard Output to if configured.
                /// </summary>
                public StreamReader? StandardError { get; protected set; }

                /// <summary>
                /// The Process Resource Policy to be used for executing the Command.
                /// </summary>
                /// <remarks>Process Resource Policy objects enable configuring Processor Affinity and other resource settings to be applied to the Command if supported by the currently running operating system.
                /// <para>Not all properties of a Process Resource Policy support all operating systems. Check before configuring a property.</para></remarks>
                public ProcessResourcePolicy? ResourcePolicy { get; protected set; }
        }
}