/*
    AlastairLundy.Extensions.Processes 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */


using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AlastairLundy.Extensions.Processes
{
    public static class SanitizeProcessNamesExtensions
    {
        /// <summary>
        /// Sanitizes a Process Name.
        /// </summary>
        /// <param name="process">The process to sanitize the name of.</param>
        /// <param name="excludeFileExtension">Whether to remove the file extension from the Process when sanitizing the process name.</param>
        /// <returns>the sanitized process names.</returns>
        public static string SanitizeProcessName(this Process process, bool excludeFileExtension = true)
        {
#if NET8_0_OR_GREATER
        return SanitizeProcessNames([process], excludeFileExtension).First();
#else
            return SanitizeProcessNames(new Process[]{process}, excludeFileExtension).First();
#endif
        }

        /// <summary>
        /// Sanitizes Process Names from a list of Process objects.
        /// </summary>
        /// <param name="processNames">The list of Processes to sanitize the names of.</param>
        /// <param name="excludeFileExtensions">Whether to remove the file extension from the Process when sanitizing the process name.</param>
        /// <returns>the sanitized process names.</returns>
        public static IEnumerable<string> SanitizeProcessNames(this IEnumerable<Process> processNames, bool excludeFileExtensions = true)
        {
            List<string> output;
        
            if (excludeFileExtensions)
            {
                Process[] enumerable = processNames as Process[] ?? processNames.ToArray();
            
                output = enumerable.Select(x => x.ProcessName.Replace(Path.GetExtension(x.ProcessName), string.Empty))
                    .Select(x => x.Replace("System.Diagnostics.Process (", string.Empty)
                        .Replace(")", string.Empty)).ToList();
            }
            else
            {
                output = processNames.Select(x => x.ProcessName.Replace("System.Diagnostics.Process (", string.Empty)
                    .Replace(")", string.Empty)).ToList();
            }
        
            return output;
        }
    }
}