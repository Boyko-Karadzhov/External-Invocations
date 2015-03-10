using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Karadzhov.ExternalInvocations.VisualStudioCommonTools
{
    /// <summary>
    /// Instances of this class invoke the Visual Studio C/C++ Optimizing Compiler.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1722:IdentifiersShouldNotHaveIncorrectPrefix", Justification = "C here stands for the C language.")]
    public sealed class COptimizingCompiler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="COptimizingCompiler"/> class.
        /// </summary>
        public COptimizingCompiler() : this(Environment.Is64BitProcess ? ProcessorArchitecture.Amd64 : ProcessorArchitecture.X86)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="COptimizingCompiler"/> class.
        /// </summary>
        /// <param name="targetArchitecture">The target architecture.</param>
        /// <exception cref="ArgumentException">Only Amd64 and X86 architectures are supported.</exception>
        public COptimizingCompiler(ProcessorArchitecture targetArchitecture)
        {
            if (ProcessorArchitecture.Amd64 == targetArchitecture)
                this.toolsLocator = new VisualStudioToolsLocator64();
            else if (ProcessorArchitecture.X86 == targetArchitecture)
                this.toolsLocator = new VisualStudioToolsLocator32();
            else
                throw new ArgumentException("Only Amd64 and X86 architectures are supported.");

            this.targetArchitecture = targetArchitecture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="COptimizingCompiler"/> class.
        /// </summary>
        /// <param name="locator">The Visual Studio Tools locator.</param>
        public COptimizingCompiler(IVisualStudioToolsLocator locator)
        {
            this.toolsLocator = locator;
        }

        /// <summary>
        /// Invokes the compiler with the specified arguments.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <exception cref="Karadzhov.ExternalInvocations.VisualStudioCommonTools.CompilationException">Compilation failed. Check exception's Compiler Output property for details.</exception>
        public void Compile(CCompileArguments arguments)
        {
            var commandArguments = this.CommandArguments(arguments);

            var startInfo = new ProcessStartInfo(this.toolsLocator.CL, commandArguments)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            if (ProcessorArchitecture.X86 == this.targetArchitecture)
                startInfo.EnvironmentVariables["PATH"] = this.toolsLocator.IDE + ";" + startInfo.EnvironmentVariables["PATH"];

            using (var compileProcess = new Process())
            {
                compileProcess.StartInfo = startInfo;
                compileProcess.Start();
                compileProcess.WaitForExit();

                if (0 != compileProcess.ExitCode)
                    throw new CompilationException("Compilation failed. Check exception's Compiler Output property for details.", compileProcess.StandardOutput.ReadToEnd(), compileProcess.StandardError.ReadToEnd());
            }
        }

        private string CommandArguments(CCompileArguments arguments)
        {
            var commandParts = new List<string>(15);

            if (CompileOptimization.None == arguments.Optimizations)
                commandParts.Add("/Od");
            else
                commandParts.Add("/Ox");

            commandParts.Add("/nologo");

            commandParts.Add(string.Format(CultureInfo.InvariantCulture, "/I \"{0}\"", this.toolsLocator.Include));
            commandParts.Add(string.Format(CultureInfo.InvariantCulture, "/I \"{0}\"", this.toolsLocator.AtlMfcLib));
            commandParts.Add(string.Format(CultureInfo.InvariantCulture, "/I \"{0}\"", WindowsSdkLocator.Include));

            commandParts.AddRange(arguments.Files.Select(f => "\"" + f + "\""));
            commandParts.Add("/link");

            if (arguments.IsDll)
            {
                commandParts.Add("/DLL");
            }

            commandParts.Add("/NOLOGO");
            commandParts.Add("/OUT:" + arguments.Output);

            commandParts.Add(string.Format(CultureInfo.InvariantCulture, "/LIBPATH:\"{0}\"", this.toolsLocator.Libraries));
            commandParts.Add(string.Format(CultureInfo.InvariantCulture, "/LIBPATH:\"{0}\"", this.toolsLocator.AtlMfcLib));

            string windowsSdkLib;
            if (this.targetArchitecture == ProcessorArchitecture.Amd64)
                windowsSdkLib = WindowsSdkLocator.Lib64;
            else
                windowsSdkLib = WindowsSdkLocator.Lib;

            commandParts.Add(string.Format(CultureInfo.InvariantCulture, "/LIBPATH:\"{0}\"", windowsSdkLib));

            var result = string.Join(" ", commandParts);
            return result;
        }

        private readonly IVisualStudioToolsLocator toolsLocator;
        private readonly ProcessorArchitecture targetArchitecture;
    }
}
