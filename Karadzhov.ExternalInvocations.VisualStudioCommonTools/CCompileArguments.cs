using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Karadzhov.ExternalInvocations.VisualStudioCommonTools
{
    /// <summary>
    /// Instances of this class contain compilation arguments for the C/C++ compiler and linker.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1722:IdentifiersShouldNotHaveIncorrectPrefix", Justification = "C here stands for the C language.")]
    public class CCompileArguments
    {
        /// <summary>
        /// The name of one or more source files, .obj files, or libraries. CL compiles source files and passes the names of the .obj files and libraries to the linker.
        /// </summary>
        public IList<string> Files
        {
            get
            {
                return this.files;
            }
        }

        /// <summary>
        /// Gets or sets the output file name.
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// Gets or sets the optimization level of the compilation.
        /// </summary>
        public CompileOptimization Optimizations { get; set; }

        /// <summary>
        /// Builds a DLL if set to true.
        /// </summary>
        public bool IsDll { get; set; }

        private IList<string> files = new List<string>();
    }
}
