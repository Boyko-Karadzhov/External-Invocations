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
        [TestInitialize]
        public void Initialize()
        {
            DynamicLibraryManager.Reset();

            var binPath = Path.Combine(CudaCompilerTests.GetSamplesPath(), "bin");
            if (Directory.Exists(binPath))
            {
                Directory.Delete(binPath, recursive: true);
            }

            Directory.CreateDirectory(binPath);
        }

        [TestMethod]
        public void Compile_DllWithCudaSource_ValidDll()
        {
            var samplesPath = CudaCompilerTests.GetSamplesPath();
            var targetDll = samplesPath + "bin\\vector_sum.dll";

            var compile = new CudaCompiler();
            var arguments = new CudaCompileArguments()
            {
                ComputeCapability = CudaComputeCapability.Cuda50,
                TargetProcessorArchitecture = Environment.Is64BitProcess ? ProcessorArchitecture.Amd64 : ProcessorArchitecture.X86,
                IsDll = true,
                Output = targetDll,
                Optimizations = CompileOptimization.MaximumOptimization
            };
            arguments.Files.Add(samplesPath + "vector_sum.cu");
            new CudaCompiler().Compile(arguments);

            Assert.IsTrue(File.Exists(targetDll));

            var size = 5;
            var a = new int[] { 1, 2, 3, 4, 5 };
            var b = new int[] { 6, 7, 8, 9, 10 };
            var c = new int[5];
            DynamicLibraryManager.Invoke<int>(targetDll, "addWithCuda", c, a, b, size);

            Assert.AreEqual(7, c[0]);
            Assert.AreEqual(9, c[1]);
            Assert.AreEqual(11, c[2]);
            Assert.AreEqual(13, c[3]);
            Assert.AreEqual(15, c[4]);
        }

        [TestMethod]
        public void Compile_DllWithCublas_ValidDll()
        {
            var samplesPath = CudaCompilerTests.GetSamplesPath();
            var targetDll = samplesPath + "bin\\mat_mul.dll";

            var compile = new CudaCompiler();
            var arguments = new CudaCompileArguments()
            {
                ComputeCapability = CudaComputeCapability.Cuda50,
                TargetProcessorArchitecture = Environment.Is64BitProcess ? ProcessorArchitecture.Amd64 : ProcessorArchitecture.X86,
                IsDll = true,
                Output = targetDll,
                Optimizations = CompileOptimization.MaximumOptimization
            };

            arguments.Files.Add(samplesPath + "mat_mul.cu");
            arguments.Files.Add("cublas.lib");
            arguments.Files.Add("cublas_device.lib");
            arguments.Files.Add("cudadevrt.lib");

            new CudaCompiler().Compile(arguments);

            Assert.IsTrue(File.Exists(targetDll));

            int m = 2; int n = 4; int k = 3;

            var a = new float[] { 1.0f, 4.0f, 2.0f, 5.0f, 3.0f, 6.0f };

            var b = new float[] { 1.0f, 5.0f, 9.0f, 2.0f, 6.0f, 10.0f, 3.0f, 7.0f, 11.0f, 4.0f, 8.0f, 12.0f };

            var c = new float[m * n];
            DynamicLibraryManager.Invoke<int>(System.Runtime.InteropServices.CallingConvention.Cdecl, targetDll, "matrixMul", m, n, k, a, b, c);

            Assert.AreEqual(38.0f, c[0], 0.0001f);
            Assert.AreEqual(83.0f, c[1], 0.0001f);
            Assert.AreEqual(44.0f, c[2], 0.0001f);
            Assert.AreEqual(98.0f, c[3], 0.0001f);
            Assert.AreEqual(50.0f, c[4], 0.0001f);
            Assert.AreEqual(113.0f, c[5], 0.0001f);
            Assert.AreEqual(56.0f, c[6], 0.0001f);
            Assert.AreEqual(128.0f, c[7], 0.0001f);
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
