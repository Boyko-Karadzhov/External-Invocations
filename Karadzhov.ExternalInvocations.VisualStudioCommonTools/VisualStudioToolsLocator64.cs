using System;
using System.IO;

namespace Karadzhov.ExternalInvocations.VisualStudioCommonTools
{
    internal sealed class VisualStudioToolsLocator64 : VisualStudioToolsLocatorBase
    {
        public override string CL
        {
            get 
            { 
                return VisualStudioToolsLocator64.clPath.Value; 
            }
        }

        public override string Libraries
        {
            get 
            {
                return VisualStudioToolsLocator64.libPath.Value;
            }
        }

        public override string AtlMfcLib
        {
            get 
            {
                return VisualStudioToolsLocator64.atlMfcLibPath.Value;
            }
        }

        private static string FindClPath()
        {
            if (Environment.Is64BitOperatingSystem)
                return Path.Combine(VisualStudioToolsLocatorBase.VcPath, "bin\\amd64\\cl.exe");
            else
                return Path.Combine(VisualStudioToolsLocatorBase.VcPath, "bin\\x86_amd64\\cl.exe");
        }

        private static string FindLibPath()
        {
            return Path.Combine(VisualStudioToolsLocatorBase.VcPath, "lib\\amd64");
        }

        private static string FindAtlMfcLibPath()
        {
            return Path.Combine(VisualStudioToolsLocatorBase.VcPath, "atlmfc\\lib\\amd64");
        }

        private static readonly Lazy<string> clPath = new Lazy<string>(VisualStudioToolsLocator64.FindClPath);
        private static readonly Lazy<string> libPath = new Lazy<string>(VisualStudioToolsLocator64.FindLibPath);
        private static readonly Lazy<string> atlMfcLibPath = new Lazy<string>(VisualStudioToolsLocator64.FindAtlMfcLibPath);
    }
}
