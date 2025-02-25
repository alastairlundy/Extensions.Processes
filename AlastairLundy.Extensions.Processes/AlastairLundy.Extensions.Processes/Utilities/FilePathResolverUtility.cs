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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputFilePath"></param>
    /// <param name="resolvedFilePath"></param>
    public void ResolveFilePath(string inputFilePath, out string resolvedFilePath)
    {
        int recursionNumber = 0;
        string newPath = string.Join(string.Empty, inputFilePath.Where(x => Path.GetInvalidPathChars().Contains(x) == false && Path.GetInvalidFileNameChars().Contains(x) == false)
            .ToArray()); 
            
        while (recursionNumber < 3)
        {
#if NET6_0_OR_GREATER || NETSTANDARD2_1
            if (Path.IsPathFullyQualified(newPath))
#else
            if(Path.IsPathRooted(newPath))
#endif
            {
                resolvedFilePath = newPath;
                return;
            }

#if NET6_0_OR_GREATER
            if (Path.Exists(Path.GetFullPath(inputFilePath)) == false)
#else
            if (File.Exists(Path.GetFullPath(inputFilePath)) == false)
#endif
            {
                string[] directoryComponents = Path.GetFullPath(inputFilePath).Split(Path.DirectorySeparatorChar);

                string lastDirectory = Directory.Exists(directoryComponents.Last())
                    ? directoryComponents.Last()
                    : directoryComponents.SkipLast(1).Last();
                
                string targetFileName = Path.GetFileName(newPath);
                
                if (Directory.Exists(Path.GetFullPath(lastDirectory)))
                {
                    string[] files = Directory.EnumerateFiles(Path.GetFullPath(inputFilePath)).ToArray();

                    foreach (string file in files)
                    {
                        FileInfo fileInfo = new(file);

                        if (fileInfo.FullName.Equals(Path.GetFullPath(lastDirectory)) ||
                            fileInfo.Name.Equals(targetFileName)){
                            resolvedFilePath = fileInfo.FullName;
                            return;
                        }
                    }
                }
                
                recursionNumber += 1;
            }
            else
            {
                resolvedFilePath = Path.GetFullPath(newPath);
                return;
            }
        }
        
        resolvedFilePath = newPath;
    }
}