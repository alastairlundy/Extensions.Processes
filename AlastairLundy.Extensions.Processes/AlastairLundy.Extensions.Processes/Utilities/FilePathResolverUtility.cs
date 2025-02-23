/*
    AlastairLundy.Extensions.Processes  
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System.IO;
using System.Linq;

using AlastairLundy.Extensions.Processes.Utilities.Abstractions;

namespace AlastairLundy.Extensions.Processes.Utilities;

public class FilePathResolverUtility : IFilePathResolverUtility
{
    public void ResolveFilePath(string inputFilePath, out string resolvedFilePath)
    {
#if NET6_0_OR_GREATER || NETSTANDARD2_1
        if (Path.IsPathFullyQualified(inputFilePath) == true)
        {
            resolvedFilePath = inputFilePath;
            return;
        }
#endif
        
        char[] tempPath = inputFilePath.Where(x => Path.GetInvalidPathChars().Contains(x) == false && Path.GetInvalidFileNameChars().Contains(x) == false)
            .ToArray();
        
        string newPath = string.Join(string.Empty, tempPath);
        
        resolvedFilePath = Path.GetFullPath(newPath);
    }
}