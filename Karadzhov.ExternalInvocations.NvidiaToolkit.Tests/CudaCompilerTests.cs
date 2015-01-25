using System;
using System.IO;
using System.Reflection;
using Karadzhov.ExternalInvocations.VisualStudioCommonTools;
using Karadzhov.Interop.DynamicLibraries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Karadzhov.ExternalInvocations.NvidiaToolkit.Tests
{
    [TestClass]
    public class CudaCompilerTests
    {
        [TestMethod]
        public void Compile_DllWithCudaSource_ValidDll()
        {
            var samplesPath = CudaCompilerTests.GetSamplesPath();
            string targetDll;
            if (Environment.Is64BitProcess)
                targetDll = samplesPath + "x64\\vector_sum.dll";
            else
                targetDll = samplesPath + "bin\\vector_sum.dll";

            var compile = new CudaCompiler();
            var arguments = new CudaCompileArguments()
            {
                ComputeCapability = CudaComputeCapability.Cuda21,
                TargetCPUArchitecture = Environment.Is64BitProcess ? ProcessorArchitecture.Amd64 : ProcessorArchitecture.X86,
                IsDll = true,
                Output = targetDll,
                Optimizations = CompileOptimization.MaximumOptimization
            };
            arguments.Files.Add(samplesPath + "vector_sum.cu");

            Assert.IsTrue(File.Exists(targetDll));

            var size = 5;
            var a = new int[] { 1, 2, 3, 4, 5 };
            var b = new int[] { 6, 7, 8, 9, 10 };
            var c = DynamicLibraryManager.Invoke<int[]>(targetDll, "addWithCuda", a, b, size);

            Assert.IsNotNull(c);
            Assert.AreEqual(5, c.Length);
            Assert.AreEqual(7, c[0]);
            Assert.AreEqual(9, c[1]);
            Assert.AreEqual(11, c[2]);
            Assert.AreEqual(13, c[3]);
            Assert.AreEqual(15, c[4]);
        }

        private static string TestProjectPath()
        {
            var location = typeof(CudaCompilerTests).Assembly.Location;
            var path = Path.GetDirectoryName(location);
            return path + "\\..\\..\\";
        }

        private static string GetSamplesPath()
        {
            return CudaCompilerTests.TestProjectPath() + "Samples\\";
        }
    }
}
