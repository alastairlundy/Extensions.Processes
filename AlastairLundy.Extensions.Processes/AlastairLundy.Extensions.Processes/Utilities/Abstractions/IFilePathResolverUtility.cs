/*
    AlastairLundy.Extensions.Processes  
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

namespace AlastairLundy.Extensions.Processes.Utilities.Abstractions;

public interface IFilePathResolverUtility
{
    /// <summary>
    /// Resolves the file path if the file path is a PATH environment variable.
    /// </summary>
    /// <param name="inputFilePath">The input file path to resolve.</param>
    /// <param name="resolvedFilePath">The resolved file path if the file path is in the PATH environment variable; the original input file path otherwise.</param>
    void ResolveFilePath(string inputFilePath, out string resolvedFilePath);
}