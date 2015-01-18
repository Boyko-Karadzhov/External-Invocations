using System;
using System.IO;

namespace Karadzhov.ExternalInvocations.VisualStudioCommonTools
{
    internal abstract class VisualStudioToolsLocatorBase : IVisualStudioToolsLocator
    {
        public abstract string CL { get; }
        public abstract string Libraries { get; }
        public abstract string AtlMfcLib { get; }

        public string Include
        { 
            get
            {
                return VisualStudioToolsLocatorBase.includePath.Value;
            }
        }

        public string AtlMfcInclude 
        { 
            get
            {
                return VisualStudioToolsLocatorBase.atlMfcIncludePath.Value;
            }
        }

        protected static string VcPath
        {
            get
            {
                return VisualStudioToolsLocatorBase.vcPath.Value;
            }
        }

        private static string FindVcPath()
        {
            var fromEnv = VisualStudioToolsLocatorBase.PathFromEnvironment();
            return Path.Combine(fromEnv, "..\\..\\VC");
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
    }
}
