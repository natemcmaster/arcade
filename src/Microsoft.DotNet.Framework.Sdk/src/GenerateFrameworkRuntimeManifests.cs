// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Extensions.DependencyModel;

namespace Microsoft.DotNet.Framework.Sdk
{
#if NET472
    [LoadInSeparateAppDomain]
    public class GenerateFrameworkRuntimeManifests : AppDomainIsolatedTask
    {
        static GenerateFrameworkRuntimeManifests() => AssemblyResolution.Initialize();

#else
    public class GenerateFrameworkRuntimeManifests : Task
    {
#endif
        /// <summary>
        /// The path for the .deps.json file.
        /// </summary>
        [Required]
        public string DepsOutputPath { get; set; }

        /// <summary>
        /// The path for the PlatformManifest.txt file.
        /// </summary>
        [Required]
        public string PlatformManifestOutputPath { get; set; }

        /// <summary>
        /// The target framework moniker that the framework supports.
        /// </summary>
        [Required]
        public string TargetFrameworkMoniker { get; set; }

        /// <summary>
        /// The name of the shared framework
        /// </summary>
        [Required]
        public string FrameworkName { get; set; }

        /// <summary>
        /// The version of the shared framework
        /// </summary>
        [Required]
        public string FrameworkVersion { get; set; }

        /// <summary>
        /// A list of files, both managed and native, that are part of the runtime
        /// </summary>
        [Required]
        public ITaskItem[] RuntimeReferences { get; set; }

        /// <summary>
        /// The runtime identifier of the shared framework
        /// </summary>
        [Required]
        public string RuntimeIdentifier { get; set; }

        /// <summary>
        /// The name of the runtime pack
        /// </summary>
        [Required]
        public string RuntimePackageName { get; set; }

        public override bool Execute()
        {
#if NET472
            AssemblyResolution.Log = Log;
#endif
            try
            {
                ExecuteImpl();
                return !Log.HasLoggedErrors;
            }
            finally
            {
#if NET472
                AssemblyResolution.Log = null;
#endif
            }
        }

        private void ExecuteImpl()
        {
            var target = new TargetInfo(
                TargetFrameworkMoniker, 
                RuntimeIdentifier,
                // Earlier versions of .NET Core used a SHA1 algorithm, but as far as we can tell was never consumed by the runtime or anywhere else
                runtimeSignature: string.Empty,  
                isPortable: false);
            var runtimeFiles = new List<RuntimeFile>();
            var nativeFiles = new List<RuntimeFile>();
            var resourceAssemblies = new List<ResourceAssembly>();
            var platformManifest = new List<string>();

            foreach (var reference in RuntimeReferences)
            {
                var filePath = reference.ItemSpec;
                var fileName = Path.GetFileName(filePath);
                var fileVersion = FileUtilities.GetFileVersion(filePath)?.ToString() ?? string.Empty;
                var assemblyVersion = FileUtilities.TryGetAssemblyVersion(filePath);
                if (assemblyVersion == null)
                {
                    var nativeFile = new RuntimeFile(fileName, null, fileVersion);
                    nativeFiles.Add(nativeFile);
                    platformManifest.Add($"{fileName}|{FrameworkName}||{fileVersion}");
                }
                else
                {
                    var runtimeFile = new RuntimeFile(fileName,
                        fileVersion: fileVersion,
                        assemblyVersion: assemblyVersion.ToString());
                    runtimeFiles.Add(runtimeFile);
                    platformManifest.Add($"{fileName}|{FrameworkName}|{assemblyVersion}|{fileVersion}");
                }
            }

            var runtimeLibrary = new RuntimeLibrary("package",
               RuntimePackageName,
               FrameworkVersion,
               hash: string.Empty,
               runtimeAssemblyGroups: new[] { new RuntimeAssetGroup(string.Empty, runtimeFiles) },
               nativeLibraryGroups: new[] { new RuntimeAssetGroup(string.Empty, nativeFiles) },
               Enumerable.Empty<ResourceAssembly>(),
               Array.Empty<Dependency>(),
               hashPath: null,
               path: $"{RuntimePackageName.ToLowerInvariant()}/{FrameworkVersion}",
               serviceable: true);

            var context = new DependencyContext(target,
                CompilationOptions.Default,
                Enumerable.Empty<CompilationLibrary>(),
                new[] { runtimeLibrary },
                // TODO: support generating the RID graph for Microsoft.NETCore.App
                Enumerable.Empty<RuntimeFallbacks>());

            Directory.CreateDirectory(Path.GetDirectoryName(DepsOutputPath));
            Directory.CreateDirectory(Path.GetDirectoryName(PlatformManifestOutputPath));

            File.WriteAllText(
                PlatformManifestOutputPath,
                string.Join("\n", platformManifest.OrderBy(n => n)),
                Encoding.UTF8);

            try
            {
                using (var depsStream = File.Create(DepsOutputPath))
                {
                    new DependencyContextWriter().Write(context, depsStream);
                }
            }
            catch (Exception ex)
            {
                // If there is a problem, ensure we don't write a partially complete version to disk.
                if (File.Exists(DepsOutputPath))
                {
                    File.Delete(DepsOutputPath);
                }
                Log.LogErrorFromException(ex);
            }
        }
    }
}
