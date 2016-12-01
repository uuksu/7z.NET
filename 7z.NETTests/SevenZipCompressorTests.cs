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
            
            Assert.IsTrue(File.Exists(file));

            Trace.WriteLine(100 * new FileInfo(file).Length /
                new DirectoryInfo(dir).GetFiles("*", SearchOption.AllDirectories).Where(x => x.FullName != file).Sum(x => x.Length)
                + "% ratio");
        }

        [TestMethod()]
        public void CompressDirectoryWithPasswordTest()
        {
            string file = Environment.CurrentDirectory + @"\exp_with_password.7z";

            var c = new SevenZipCompressor(file, "123");

            string dir = Environment.CurrentDirectory;

            c.ProgressUpdated += (p) =>
            {
                Trace.WriteLine("Progress: " + p + "%");
            };

            c.CompressDirectory(dir, CompressionLevel.Fast);

            Assert.IsTrue(File.Exists(file));

            Trace.WriteLine(100 * new FileInfo(file).Length /
                new DirectoryInfo(dir).GetFiles("*", SearchOption.AllDirectories).Where(x => x.FullName != file).Sum(x => x.Length)
                + "% ratio");
        }

        [TestMethod()]
        public void CompressDirectoryWithPasswordAndEncryptedHeadersTest()
        {
            string file = Environment.CurrentDirectory + @"\exp_with_password_encrypted_headers.7z";

            var c = new SevenZipCompressor(file, "123", true);

            string dir = Environment.CurrentDirectory;

            c.ProgressUpdated += (p) =>
            {
                Trace.WriteLine("Progress: " + p + "%");
            };

            c.CompressDirectory(dir, CompressionLevel.Fast);

            Assert.IsTrue(File.Exists(file));

            Trace.WriteLine(100 * new FileInfo(file).Length /
                new DirectoryInfo(dir).GetFiles("*", SearchOption.AllDirectories).Where(x => x.FullName != file).Sum(x => x.Length)
                + "% ratio");
        }
    }
}