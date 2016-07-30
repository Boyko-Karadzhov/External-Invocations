
namespace Karadzhov.ExternalInvocations.NvidiaToolkit
{
    /// <summary>
    /// Enumeration of supported compute capability options. Check here [https://developer.nvidia.com/cuda-gpus] for more information.
    /// </summary>
    public enum CudaComputeCapability
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Cuda11,
        Cuda12,
        Cuda13,
        Cuda20,
        Cuda21,
        Cuda30,
        Cuda32,
        Cuda35,
        Cuda50,
        Cuda52
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
