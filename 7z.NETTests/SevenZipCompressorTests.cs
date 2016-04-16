using Microsoft.VisualStudio.TestTools.UnitTesting;
using SevenZipNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SevenZipNET.Tests
{
    [DeploymentItem("test.7z")]
    [DeploymentItem("7za.exe")]
    [DeploymentItem("7za64.exe")]
    [TestClass()]
    public class SevenZipCompressorTests
    {
        [TestMethod()]
        public void CompressDirectoryTest()
        {
            string file = Environment.CurrentDirectory + @"\exp.7z";

            var c = new SevenZipCompressor(file);

            string dir = Environment.CurrentDirectory;

            c.ProgressUpdated += (p) =>
            {
                Trace.WriteLine("Progress: " + p + "%");
            };

            c.CompressDirectory(dir, CompressionLevel.Fast);

            Trace.WriteLine(100 * new FileInfo(file).Length /
                new DirectoryInfo(dir).GetFiles("*", SearchOption.AllDirectories).Where(x => x.FullName != file).Sum(x => x.Length)
                + "% ratio");
        }
    }
}