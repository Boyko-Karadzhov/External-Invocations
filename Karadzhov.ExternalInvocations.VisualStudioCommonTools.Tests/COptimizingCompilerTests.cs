using System;
using System.IO;
using System.Reflection;
using Karadzhov.Interop.DynamicLibraries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Karadzhov.ExternalInvocations.VisualStudioCommonTools.Tests
{
    [TestClass]
    public class COptimizingCompilerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            DynamicLibraryManager.Reset();

            var binPath = Path.Combine(Utilities.TestProjectPath(), "Samples\\bin");
            if (Directory.Exists(binPath))
            {
                Directory.Delete(binPath, recursive: true);
            }

            Directory.CreateDirectory(binPath);
        }

        [TestMethod]
        public void Compile_SumMethodIntoDll_ValidDll()
        {
            var samplesPath = Path.Combine(Utilities.TestProjectPath(), "Samples");

            var compiler = new COptimizingCompiler(Environment.Is64BitProcess ? ProcessorArchitecture.Amd64 : ProcessorArchitecture.X86);
            var arguments = new CCompileArguments()
            {
                Optimizations = CompileOptimization.None,
                IsDll = true
            };
            arguments.Files.Add(Path.Combine(samplesPath, "sum.c"));

            var dllPath = Path.Combine(samplesPath, "bin\\sum.dll");
            arguments.Output = dllPath;

            compiler.Compile(arguments);

            Assert.IsTrue(File.Exists(dllPath));

            var result = DynamicLibraryManager.Invoke<int>(dllPath, "sum", 5, 6);
            Assert.AreEqual(11, result);
        }

        [TestMethod]
        [ExpectedException(typeof(CompilationException))]
        public void Compile_SyntaxErrorInSource_ThrowsException()
        {
            var samplesPath = Path.Combine(Utilities.TestProjectPath(), "Samples");

            var compiler = new COptimizingCompiler(Environment.Is64BitProcess ? ProcessorArchitecture.Amd64 : ProcessorArchitecture.X86);
            var arguments = new CCompileArguments()
            {
                Optimizations = CompileOptimization.MaximumOptimization,
                IsDll = true
            };
            arguments.Files.Add(Path.Combine(samplesPath, "sum_comp_error.c"));
            
            var dllPath = Path.Combine(samplesPath, "bin\\sum_comp_error.dll");
            arguments.Output = dllPath;

            compiler.Compile(arguments);
        }
    }
}
