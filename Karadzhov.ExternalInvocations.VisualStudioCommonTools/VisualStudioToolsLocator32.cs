using System;
using System.IO;

namespace Karadzhov.ExternalInvocations.VisualStudioCommonTools
{
    internal sealed class VisualStudioToolsLocator32 : VisualStudioToolsLocatorBase
    {
        public override string CL
        {
            get
            {
                return VisualStudioToolsLocator32.clPath.Value;
            }
        }

        public override string Libraries
        {
            get
            {
                return VisualStudioToolsLocator32.libPath.Value;
            }
        }

        public override string AtlMfcLib
        {
            get
            {
                return VisualStudioToolsLocator32.atlMfcLibPath.Value;
            }
        }

        private static string FindClPath()
        {
            if (Environment.Is64BitOperatingSystem)
                return Path.Combine(VisualStudioToolsLocatorBase.VcPath, "bin\\amd64_x86\\cl.exe");
            else
                return Path.Combine(VisualStudioToolsLocatorBase.VcPath, "bin\\cl.exe");
        }

        private static string FindLibPath()
        {
            return Path.Combine(VisualStudioToolsLocatorBase.VcPath, "lib");
        }

        private static string FindAtlMfcLibPath()
        {
            return Path.Combine(VisualStudioToolsLocatorBase.VcPath, "atlmfc\\lib");
        }

        private static readonly Lazy<string> clPath = new Lazy<string>(VisualStudioToolsLocator32.FindClPath);
        private static readonly Lazy<string> libPath = new Lazy<string>(VisualStudioToolsLocator32.FindLibPath);
        private static readonly Lazy<string> atlMfcLibPath = new Lazy<string>(VisualStudioToolsLocator32.FindAtlMfcLibPath);
    }
}
