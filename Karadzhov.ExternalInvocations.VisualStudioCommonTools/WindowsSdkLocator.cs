using System;
using System.IO;
using Microsoft.Win32;

namespace Karadzhov.ExternalInvocations.VisualStudioCommonTools
{
    public static class WindowsSdkLocator
    {
        public static string Root
        {
            get
            {
                return WindowsSdkLocator.rootPath.Value;
            }
        }

        public static string Lib
        {
            get
            {
                return WindowsSdkLocator.libPath.Value;
            }
        }

        public static string Lib64
        {
            get
            {
                return WindowsSdkLocator.lib64Path.Value;
            }
        }

        public static string Include
        {
            get
            {
                return WindowsSdkLocator.includePath.Value;
            }
        }

        private static string FindRootPath()
        {
            var sdkRootPath = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Microsoft SDKs\\Windows\\v7.1A", "InstallationFolder", null) as string;
            if (null == sdkRootPath)
                throw new InvalidOperationException("Could not find Windows SDK 7.1A installation folder.");

            return sdkRootPath;
        }

        private static string FindLibPath()
        {
            return Path.Combine(WindowsSdkLocator.Root, "Lib");
        }

        private static string FindLib64Path()
        {
            return Path.Combine(WindowsSdkLocator.Root, "Lib\\x64");
        }

        private static string FindIncludePath()
        {
            return Path.Combine(WindowsSdkLocator.Root, "Include");
        }

        private static readonly Lazy<string> rootPath = new Lazy<string>(WindowsSdkLocator.FindRootPath);
        private static readonly Lazy<string> libPath = new Lazy<string>(WindowsSdkLocator.FindLibPath);
        private static readonly Lazy<string> lib64Path = new Lazy<string>(WindowsSdkLocator.FindLib64Path);
        private static readonly Lazy<string> includePath = new Lazy<string>(WindowsSdkLocator.FindIncludePath);
    }
}
