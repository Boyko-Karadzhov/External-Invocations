using System.Diagnostics.CodeAnalysis;

namespace Karadzhov.ExternalInvocations.VisualStudioCommonTools
{
    /// <summary>
    /// Implementations of this interface should provide file locations of Visual Studio tools.
    /// </summary>
    public interface IVisualStudioToolsLocator
    {
        /// <summary>
        /// Gets the file name of the C/C++ Optimizing Compiler (cl.exe).
        /// </summary>
        string CL { get; }

        /// <summary>
        /// Gets the path to the lib and obj files that are distributed with Visual Studio.
        /// </summary>
        string Libraries { get; }

        /// <summary>
        /// Gets the path to the header files that are distributed with Visual Studio.
        /// </summary>
        string Include { get; }

        /// <summary>
        /// Gets the path to the MFC and ATL libraries.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Atl")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mfc")]
        string AtlMfcLib { get; }
        
        /// <summary>
        /// Gets the path to the MFC and ATL header files.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Atl")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mfc")]
        string AtlMfcInclude { get; }
    }
}
