// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Microsoft.DotNet.Framework.Sdk
{
    internal static class FileUtilities
    {
        public static Version GetFileVersion(string sourcePath)
        {
            var fvi = FileVersionInfo.GetVersionInfo(sourcePath);

            return fvi != null
                ? new Version(fvi.FileMajorPart, fvi.FileMinorPart, fvi.FileBuildPart, fvi.FilePrivatePart)
                : null;
        }

        private static readonly HashSet<string> _assemblyExtensions = new HashSet<string>(new[] { ".dll", ".exe", ".winmd" }, StringComparer.OrdinalIgnoreCase);

        public static Version TryGetAssemblyVersion(string sourcePath)
        {
            var extension = Path.GetExtension(sourcePath);

            return _assemblyExtensions.Contains(extension)
                ? GetAssemblyVersion(sourcePath)
                : null;
        }

        private static Version GetAssemblyVersion(string sourcePath)
        {
            try
            {
                return AssemblyName.GetAssemblyName(sourcePath)?.Version;
            }
            catch (BadImageFormatException)
            {
                // If an .dll file cannot be read, it may be a native .dll which would not have an assembly version.
                return null;
            }
        }
    }
}
