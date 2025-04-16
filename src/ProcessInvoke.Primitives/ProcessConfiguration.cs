﻿/*
    Resyslib.Processes
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using AlastairLundy.ProcessInvoke.Primitives.Policies;
using AlastairLundy.ProcessInvoke.Primitives.Results;

// ReSharper disable MemberCanBePrivate.Global

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace AlastairLundy.ProcessInvoke.Primitives
{
    /// <summary>
    /// A class to store Process configuration information.
    /// </summary>
    public class ProcessConfiguration : IEquatable<ProcessConfiguration>, IDisposable
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
            if (credential != null && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))                    
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

            StandardInputEncoding = Encoding.Default;
            StandardOutputEncoding = Encoding.Default;
            StandardErrorEncoding = Encoding.Default;
        }
                
        /// <summary>
        /// The environment variables to be set.
        /// </summary>
        public IReadOnlyDictionary<string, string> EnvironmentVariables { get; }
        
        /// <summary>
        /// The timespan after which a Process should no longer be allowed to continue waiting to exit.
        /// </summary>
        public TimeSpan TimeoutThreshold { get; }
        
        /// <summary>
        /// The Process start information to be used.
        /// </summary>
        public ProcessStartInfo StartInfo { get; }
        
        /// <summary>
        /// The credential to be used when executing the Command.
        /// </summary>
        public UserCredential? Credential { get; }

        /// <summary>
        /// The result validation to apply to the Command when it is executed.
        /// </summary>
        public ProcessResultValidation ResultValidation { get; }

        /// <summary>
        /// The Standard Input source to redirect Standard Input to if configured.
        /// </summary>
        /// <remarks>Using Shell Execution whilst also Redirecting Standard Input will throw an Exception. This is a known issue with the System Process class.</remarks>
        /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.redirectstandarderror" />
        public StreamWriter? StandardInput { get; }

        /// <summary>
        /// The Standard Output target to redirect Standard Output to if configured.
        /// </summary>
        public StreamReader? StandardOutput { get; }

        /// <summary>
        /// The Standard Error target to redirect Standard Output to if configured.
        /// </summary>
        public StreamReader? StandardError { get; }

        /// <summary>
        /// The Process Resource Policy to be used for executing the Command.
        /// </summary>
        /// <remarks>Process Resource Policy objects enable configuring Processor Affinity and other resource settings to be applied to the Command if supported by the currently running operating system.
        /// <para>Not all properties of a Process Resource Policy support all operating systems. Check before configuring a property.</para></remarks>
        public ProcessResourcePolicy? ResourcePolicy { get; }
        
        /// <summary>
        /// The encoding to use for the Standard Input.
        /// </summary>
        /// <remarks>This is ignored on .NET Standard 2.0 as it is unsupported on that Target Framework's Process class.</remarks>
        public Encoding StandardInputEncoding { get; }
        
        /// <summary>
        /// The encoding to use for the Standard Output.
        /// </summary>
        public Encoding StandardOutputEncoding { get; }
        
        /// <summary>
        /// The encoding to use for the Standard Error.
        /// </summary>
        public Encoding StandardErrorEncoding { get; }

                
        /// <summary>
        /// Determines if a Process configuration is equal to another Process configuration.
        /// </summary>
        /// <param name="other">The other Process configuration to compare</param>
        /// <returns>True if both are equal to each other; false otherwise.</returns>
        public bool Equals(ProcessConfiguration? other)
        {
            if (other is null)
            { 
                return false;
            }

            if (Credential is not null &&
                other.Credential is not null &&
                StartInfo.FileName != other.StartInfo.FileName &&
                ResourcePolicy is not null)
            {

                if (StandardOutput is not null && StandardError is not null)
                {
                    return StartInfo.Equals(other.StartInfo) &&
                           EnvironmentVariables.Equals(other.EnvironmentVariables) &&
                           TimeoutThreshold.Equals(other.TimeoutThreshold) && StartInfo.Equals(other.StartInfo) &&
                           Credential.Equals(other.Credential)
                           && ResultValidation == other.ResultValidation &&
                           ResourcePolicy.Equals(other.ResourcePolicy) &&
                           StandardOutput.Equals(other.StandardOutput) &&
                           StandardError.Equals(other.StandardError) &&
                           StandardInputEncoding.Equals(other.StandardInputEncoding) &&
                           StandardOutputEncoding.Equals(other.StandardOutputEncoding) &&
                           StandardErrorEncoding.Equals(other.StandardErrorEncoding);   
                }
                else
                {
                    return StartInfo.Equals(other.StartInfo) &&
                           EnvironmentVariables.Equals(other.EnvironmentVariables) &&
                           TimeoutThreshold.Equals(other.TimeoutThreshold) && StartInfo.Equals(other.StartInfo) &&
                           Credential.Equals(other.Credential)
                           && ResultValidation == other.ResultValidation &&
                           ResourcePolicy.Equals(other.ResourcePolicy) &&
                           StandardInputEncoding.Equals(other.StandardInputEncoding) &&
                           StandardOutputEncoding.Equals(other.StandardOutputEncoding) &&
                           StandardErrorEncoding.Equals(other.StandardErrorEncoding);
                }
                                
            }
            else
            {
                return StartInfo.Equals(other.StartInfo) &&
                       EnvironmentVariables.Equals(other.EnvironmentVariables) &&
                       TimeoutThreshold.Equals(other.TimeoutThreshold) && StartInfo.Equals(other.StartInfo) 
                       && ResultValidation == other.ResultValidation &&
                       StandardInputEncoding.Equals(other.StandardInputEncoding) &&
                       StandardOutputEncoding.Equals(other.StandardOutputEncoding) &&
                       StandardErrorEncoding.Equals(other.StandardErrorEncoding);
            }
        }

        /// <summary>
        /// Determines if a Process configuration is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare against.</param>
        /// <returns>True if both are equal to each other; false otherwise.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is null)
            { 
                return false;
            }

            if (obj is ProcessConfiguration other)
            {
                return Equals(other);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the hash code for the current ProcessConfiguration.
        /// </summary>
        /// <returns>The hash code for the current ProcessConfiguration.</returns>
        public override int GetHashCode()
        {
            HashCode hashCode = new HashCode();
            hashCode.Add(EnvironmentVariables);
            hashCode.Add(TimeoutThreshold);
            hashCode.Add(StartInfo);
            hashCode.Add(Credential);
            hashCode.Add((int)ResultValidation);
            hashCode.Add(StandardInput);
            hashCode.Add(StandardOutput);
            hashCode.Add(StandardError);
            hashCode.Add(ResourcePolicy);
            hashCode.Add(StandardInputEncoding);
            hashCode.Add(StandardOutputEncoding);
            hashCode.Add(StandardErrorEncoding);
            return hashCode.ToHashCode();
        }

        /// <summary>
        /// Determines if a Process configuration is equal to another Process configuration.
        /// </summary>
        /// <param name="left">A Process configuration to be compared.</param>
        /// <param name="right">The other Process configuration to be compared.</param>
        /// <returns>True if both Process configurations are equal to each other; false otherwise.</returns>
        public static bool Equals(ProcessConfiguration? left, ProcessConfiguration? right)
        {
            if (left is null || right is null)
            {
                return false;
            }
            
            return left.Equals(right);
        }
        
        /// <summary>
        /// Determines if a Process configuration is equal to another Process configuration.
        /// </summary>
        /// <param name="left">A Process configuration to be compared.</param>
        /// <param name="right">The other Process configuration to be compared.</param>
        /// <returns>True if both Process configurations are equal to each other; false otherwise.</returns>
        public static bool operator ==(ProcessConfiguration? left, ProcessConfiguration? right)
        {
            if (left is null || right is null)
            {
                return false;
            }
            
            return Equals(left, right);
        }

        /// <summary>
        /// Determines if a Process Configuration is not equal to another Process configuration.
        /// </summary>
        /// <param name="left">A Process configuration to be compared.</param>
        /// <param name="right">The other Process configuration to be compared.</param>
        /// <returns>True if both Process configurations are not equal to each other; false otherwise.</returns>
        public static bool operator !=(ProcessConfiguration? left, ProcessConfiguration? right)
        {
            if (left is null || right is null)
            {
                return false;
            }
            
            return Equals(left, right) == false;
        }

        /// <summary>
        /// Disposes of the disposable properties in ProcessConfiguration.
        /// </summary>
        public void Dispose()
        {
            if (Credential is not null)
            {
                Credential.Dispose();
            }

            if (StandardInput is not null)
            { 
                StandardInput.Dispose();
            }

            if (StandardOutput is not null)
            {
                StandardOutput.Dispose();
            }

            if (StandardError is not null)
            {
                StandardError.Dispose();
            }
        }
    }
}