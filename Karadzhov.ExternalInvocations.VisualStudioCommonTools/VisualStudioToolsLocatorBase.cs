using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace Karadzhov.ExternalInvocations.VisualStudioCommonTools
{
    /// <summary>
    /// Utility class used for locating Visual Studio tools.
    /// </summary>
    public abstract class VisualStudioToolsLocatorBase : IVisualStudioToolsLocator
    {
        /// <summary>
        /// Gets the file name of the C/C++ Optimizing Compiler (cl.exe).
        /// </summary>
        public abstract string CL { get; }

        /// <summary>
        /// Gets the path to the lib and obj files that are distributed with Visual Studio.
        /// </summary>
        public abstract string Libraries { get; }

        /// <summary>
        /// Gets the path to the MFC and ATL libraries.
        /// </summary>
        public abstract string AtlMfcLib { get; }

        /// <summary>
        /// Gets the path to the header files that are distributed with Visual Studio.
        /// </summary>
        public string Include
        { 
            get
            {
                return VisualStudioToolsLocatorBase.includePath.Value;
            }
        }

        /// <summary>
        /// Gets the path to the MFC and ATL header files.
        /// </summary>
        public string AtlMfcInclude 
        { 
            get
            {
                return VisualStudioToolsLocatorBase.atlMfcIncludePath.Value;
            }
        }

        /// <summary>
        /// Gets the VC path.
        /// </summary>
        /// <value>
        /// The VC path.
        /// </value>
        public static string VCPath
        {
            get
            {
                return VisualStudioToolsLocatorBase.vcPath.Value;
            }
        }

        /// <summary>
        /// Gets the IDE path.
        /// </summary>
        public string IDE
        {
            get 
            {
                return VisualStudioToolsLocatorBase.ide.Value;
            }
        }

        private static string FindVcPath()
        {
            var fromEnv = VisualStudioToolsLocatorBase.PathFromEnvironment();
            return Path.Combine(fromEnv, "..\\..\\VC");
        }

        private static string FindIdePath()
        {
            var fromEnv = VisualStudioToolsLocatorBase.PathFromEnvironment();
            return Path.Combine(fromEnv, "..\\IDE");
        }

        private static string PathFromEnvironment()
        {
            var envVariables = Environment.GetEnvironmentVariables();
            if (envVariables.Contains(VisualStudioToolsLocatorBase.Vs2013Env))
            {
                return (string)envVariables[VisualStudioToolsLocatorBase.Vs2013Env];
            }
            else if (envVariables.Contains(VisualStudioToolsLocatorBase.Vs2012Env))
            {
                return (string)envVariables[VisualStudioToolsLocatorBase.Vs2012Env];
            }
            else if (envVariables.Contains(VisualStudioToolsLocatorBase.Vs2010Env))
            {
                return (string)envVariables[VisualStudioToolsLocatorBase.Vs2010Env];
            }
            else
            {
                throw new InvalidOperationException("Could not find environmental variable containing Visual Studio Tools path.");
            }
        }

        private static string FindAtlMfcIncludePath()
        {
            return Path.Combine(VisualStudioToolsLocatorBase.vcPath.Value, "atlmfc\\include");
        }

        private static string FindIncludePath()
        {
            return Path.Combine(VisualStudioToolsLocatorBase.vcPath.Value, "include");
        }

        private const string Vs2013Env = "VS120COMNTOOLS";
        private const string Vs2012Env = "VS110COMNTOOLS";
        private const string Vs2010Env = "VS100COMNTOOLS";
        private static readonly Lazy<string> vcPath = new Lazy<string>(VisualStudioToolsLocatorBase.FindVcPath);
        private static readonly Lazy<string> includePath = new Lazy<string>(VisualStudioToolsLocatorBase.FindIncludePath);
        private static readonly Lazy<string> atlMfcIncludePath = new Lazy<string>(VisualStudioToolsLocatorBase.FindAtlMfcIncludePath);
        private static readonly Lazy<string> ide = new Lazy<string>(VisualStudioToolsLocatorBase.FindIdePath);
    }
}
