/*
    AlastairLundy.Extensions.Processes 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AlastairLundy.Extensions.Processes
{
    public static class IsProcessRunningExtensions
    {
        /// <summary>
        /// Check to see if a specified process is running or not.
        /// </summary>
        /// <param name="process">The process to be checked.</param>
        /// <returns>True if the specified process is running; returns false otherwise.</returns>
        public static bool IsRunning(this Process process)
        {
            if (process.HasStarted())
            {
                try
                {
                    return process.HasExited == false;
                }
                catch
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        
        /// <summary>
        /// Check to see if a specified process is running or not.
        /// </summary>
        /// <param name="processName">The name of the process to be checked.</param>
        /// <param name="sanitizeProcessName"></param>
        /// <returns>true if the specified process is running; returns false otherwise.</returns>
        public static bool IsProcessRunning(this string processName, bool sanitizeProcessName = true)
        {
            string[] processes;

            string tempProcessName = processName;
            
            if (sanitizeProcessName)
            {
                tempProcessName = Path.GetFileNameWithoutExtension(processName);
                processes = Process.GetProcesses().SanitizeProcessNames(excludeFileExtensions: true).ToArray();
            }
            else
            {
                processes = Process.GetProcesses().Select(x => x.ProcessName).ToArray();
            }
            
            processes = processes.Where(x => x.Contains(tempProcessName)).ToArray();
            
            return processes.Any(x => x.ToLower().Equals(tempProcessName.ToLower()));
        }
    }
}