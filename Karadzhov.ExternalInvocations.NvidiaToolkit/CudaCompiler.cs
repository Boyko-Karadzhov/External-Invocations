using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Karadzhov.ExternalInvocations.VisualStudioCommonTools;

namespace Karadzhov.ExternalInvocations.NvidiaToolkit
{
    /// <summary>
    /// Instances of this class invoke the NVIDIA (R) Cuda compiler driver.
    /// </summary>
    public sealed class CudaCompiler
    {
        /// <summary>
        /// Invokes the CUDA Compiler with the specified arguments.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <exception cref="CompilationException">Compilation failed. Check exception's Compiler Output property for details.</exception>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Keeping it consistent with the COptimizingCompiler.")]
        public void Compile(CudaCompileArguments arguments)
        {
            var commandArguments = CudaCompiler.GetCommandArguments(arguments);
            var startInfo = new ProcessStartInfo(CudaCompiler.CudaCompilerPath.Value, commandArguments)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            using (var compilerProcess = new Process())
            {
                compilerProcess.StartInfo = startInfo;
                compilerProcess.Start();
                compilerProcess.WaitForExit();

                if (0 != compilerProcess.ExitCode)
                    throw new CompilationException("Compilation failed. Check exception's Compiler Output property for details.", compilerProcess.StandardError.ReadToEnd());
            }
        }

        private static string GetCommandArguments(CudaCompileArguments arguments)
        {
            var argList = new List<string>();
            argList.Add(CudaCompiler.ComputeArgumentsMap[arguments.ComputeCapability]);
            argList.Add(string.Format(CultureInfo.InvariantCulture, "-ccbin \"{0}\"", Path.Combine(VisualStudioToolsLocatorBase.VCPath, "bin")));

            if (CompileOptimization.None == arguments.Optimizations)
                argList.Add("-O0");

            if (arguments.IsDll)
                argList.Add("-shared");

            argList.Add(string.Format(CultureInfo.InvariantCulture, "-o=\"{0}\"", arguments.Output));

            if (ProcessorArchitecture.Amd64 == arguments.TargetProcessorArchitecture)
                argList.Add("-m 64");
            else if (ProcessorArchitecture.X86 == arguments.TargetProcessorArchitecture)
                argList.Add("-m 32");
            else
                throw new InvalidOperationException("Only X86 and Amd64 architectures are supported.");

            argList.Add(string.Join(" ", arguments.Files.Select(f => string.Format(CultureInfo.InvariantCulture, "\"{0}\"", f))));

            var result = string.Join(" ", argList);
            return result;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "nvcc")]
        private static string FindCudaCompiler()
        {
            var cudaPathEnv = Environment.GetEnvironmentVariable("CUDA_PATH");
            if (string.IsNullOrEmpty(cudaPathEnv))
                throw new InvalidOperationException("Could not find the path to the NVIDIA GPU Toolkit. Make sure it is installed.");

            var compilerPath = cudaPathEnv + "\\bin\\nvcc.exe";
            if (false == File.Exists(compilerPath))
                throw new InvalidOperationException("Could not find the CUDA compiler (nvcc.exe).");

            return compilerPath;
        }

        private static readonly Lazy<string> CudaCompilerPath = new Lazy<string>(CudaCompiler.FindCudaCompiler);
        private static readonly Dictionary<CudaComputeCapability, string> ComputeArgumentsMap = new Dictionary<CudaComputeCapability, string>()
        {
            { CudaComputeCapability.Cuda11, "-arch=compute_11 -code=sm_11" },
            { CudaComputeCapability.Cuda12, "-arch=compute_12 -code=sm_12" },
            { CudaComputeCapability.Cuda13, "-arch=compute_13 -code=sm_13" },
            { CudaComputeCapability.Cuda20, "-arch=compute_20 -code=sm_20" },
            { CudaComputeCapability.Cuda21, "-arch=compute_20 -code=sm_21" },
            { CudaComputeCapability.Cuda30, "-arch=compute_30 -code=sm_30" },
            { CudaComputeCapability.Cuda32, "-arch=compute_32 -code=sm_32" },
            { CudaComputeCapability.Cuda35, "-arch=compute_35 -code=sm_35" },
            { CudaComputeCapability.Cuda50, "-arch=compute_50 -code=sm_50" },
            { CudaComputeCapability.Cuda52, "-arch=compute_52 -code=sm_52" }
        };
    }
}
