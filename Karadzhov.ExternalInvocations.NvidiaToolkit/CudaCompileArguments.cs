using System;
using System.Reflection;
using Karadzhov.ExternalInvocations.VisualStudioCommonTools;

namespace Karadzhov.ExternalInvocations.NvidiaToolkit
{
    /// <summary>
    /// Instances of this class contain compilation arguments for the CUDA Compiler.
    /// </summary>
    public class CudaCompileArguments : CCompileArguments
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CudaCompileArguments"/> class.
        /// </summary>
        public CudaCompileArguments()
        {
            this.TargetProcessorArchitecture = Environment.Is64BitProcess ? ProcessorArchitecture.Amd64 : ProcessorArchitecture.X86;
        }

        /// <summary>
        /// Gets or sets the target CPU architecture.
        /// </summary>
        /// <value>
        /// The target CPU architecture.
        /// </value>
        public ProcessorArchitecture TargetProcessorArchitecture { get; set; }

        /// <summary>
        /// Specify the name of the NVIDIA GPU to generate code for.
        /// </summary>
        /// <value>
        /// The GPU compute capability.
        /// </value>
        public CudaComputeCapability ComputeCapability { get; set; }
    }
}
