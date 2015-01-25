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
        /// Gets or sets the target CPU architecture.
        /// </summary>
        /// <value>
        /// The target CPU architecture.
        /// </value>
        public ProcessorArchitecture TargetCPUArchitecture { get; set; }

        /// <summary>
        /// Specify the name of the NVIDIA GPU to compile for.
        /// </summary>
        /// <value>
        /// The GPU compute capability.
        /// </value>
        public CudaComputeCapability ComputeCapability { get; set; }
    }
}
