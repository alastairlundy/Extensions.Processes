/*
    AlastairLundy.Extensions.Processes  
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Diagnostics;

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#else
using OperatingSystem = Polyfills.OperatingSystemPolyfill;
using System.Runtime.InteropServices;
#endif

namespace AlastairLundy.Extensions.Processes;

/// <summary>
/// A class that defines a Process' resource configuration.
/// </summary>
public class ProcessResourcePolicy
{
    /// <summary>
    /// Instantiates the ProcessResourcePolicy with default values unless specified parameters are provided.
    /// </summary>
    /// <param name="processorAffinity">The processor affinity to be used for the Process.</param>
    /// <param name="minWorkingSet">The Minimum Working Set Size for the Process.</param>
    /// <param name="maxWorkingSet">The Maximum Working Set Size for the Process.</param>
    /// <param name="priorityClass">The priority class to assign to the Process.</param>
    /// <param name="enablePriorityBoost">Whether to enable Priority Boost if the process window enters focus.</param>
    public ProcessResourcePolicy(IntPtr? processorAffinity = null,
        nint? minWorkingSet = null, 
        nint? maxWorkingSet = null,
        ProcessPriorityClass priorityClass = ProcessPriorityClass.Normal,
        bool enablePriorityBoost = true)
    {
        if (processorAffinity == null)
        {
            processorAffinity = new IntPtr(0x0001);  
        }

        if (OperatingSystem.IsWindows() || OperatingSystem.IsMacOS() || OperatingSystem.IsFreeBSD())
        {
            MinWorkingSet = minWorkingSet;
            MaxWorkingSet = maxWorkingSet;
        }

#if NET5_0_OR_GREATER
        if (OperatingSystem.IsWindows() || OperatingSystem.IsLinux())
#else
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ||
           RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
#endif
        {
            ProcessorAffinity = processorAffinity;
        }
        
        PriorityClass = priorityClass;
        EnablePriorityBoost = enablePriorityBoost;
    }

    /// <summary>
    /// The cores and threads to assign to the Process.
    /// </summary>
    /// <remarks>Process objects only support Processor Affinity on Windows and Linux operating systems.</remarks>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("linux")]
#endif
    public IntPtr? ProcessorAffinity { get; }
    
    /// <summary>
    /// The priority class to assign to the Process.
    /// </summary>
    public ProcessPriorityClass PriorityClass { get; }

    /// <summary>
    /// Whether to enable Priority Boost if/when the main window of the Process enters focus.
    /// </summary>
    public bool EnablePriorityBoost { get; }
    
    /// <summary>
    /// The Minimum Working Set size to be used for the Process.
    /// </summary>
    /// <remarks>Not supported on Linux based operating systems.</remarks>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("maccatalyst")]
    [SupportedOSPlatform("freebsd")]
    [UnsupportedOSPlatform("linux")]
    [UnsupportedOSPlatform("android")]
#endif
    public nint? MinWorkingSet { get; }
    
    /// <summary>
    /// Maximum Working Set size to be used for the Process.
    /// </summary>
    /// <remarks>Not supported on Linux based operating systems.</remarks>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("maccatalyst")]
    [SupportedOSPlatform("freebsd")]
    [UnsupportedOSPlatform("linux")]
    [UnsupportedOSPlatform("android")]
#endif
    public nint? MaxWorkingSet { get; }
    
    /// <summary>
    /// Creates a ProcessResourcePolicy with a default configuration.
    /// </summary>
    public static ProcessResourcePolicy Default { get; } = new ProcessResourcePolicy();
}