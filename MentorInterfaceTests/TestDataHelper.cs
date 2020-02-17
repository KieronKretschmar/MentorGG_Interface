using Microsoft.DotNet.PlatformAbstractions;
using System;
using System.IO;
using System.Linq;

namespace MenterInterfaceTests
{

    public static class TestDataHelper
    {
        public static readonly string TestDataRoot = "TestData";

        public static string FilePath(string fileName)
        {
            return Path.Combine(AbsoluteRootPath(), fileName);
        }
        public static string AbsoluteRootPath()
        {
            string startupPath = ApplicationEnvironment.ApplicationBasePath;
            var pathItems = startupPath.Split(Path.DirectorySeparatorChar);
            var pos = pathItems.Reverse().ToList().FindIndex(x => string.Equals("bin", x));
            string projectPath = String.Join(Path.DirectorySeparatorChar.ToString(), pathItems.Take(pathItems.Length - pos - 1));
            return Path.Combine(projectPath, TestDataRoot);
        }
    }
}