using Microsoft.VisualStudio.TestTools.UnitTesting;
using SevenZipNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenZipNET.Tests
{
    [DeploymentItem("test.7z")]
    [DeploymentItem("7za.exe")]
    [DeploymentItem("7za64.exe")]
    [TestClass()]
    public class SevenZipExtractorTests
    {
        [TestMethod()]
        public void IndexTest()
        {
            var ext = new SevenZipExtractor("test.7z");

            Assert.AreEqual(6, ext.Files.Count);
        }

        [TestMethod()]
        public void ExtractAllTest()
        {
            var ext = new SevenZipExtractor("test.7z");

            string dir = Environment.CurrentDirectory + @"\ext\";

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            ext.ExtractAll(dir, true);

            Assert.IsTrue(Directory.EnumerateFiles(dir).Count() == 6);
        }

        [TestMethod()]
        public void ExtractWildcardTest()
        {
            var ext = new SevenZipExtractor("test.7z");

            string dir = Environment.CurrentDirectory + @"\extw\";

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            ext.ExtractWildcard(dir, "*.cs", true);

            Assert.IsTrue(Directory.EnumerateFiles(dir).Count() == 4);

            foreach (string s in Directory.EnumerateFiles(dir))
                File.Delete(s);

            ext.ExtractWildcard(dir, "7z*", true);

            Assert.IsTrue(Directory.EnumerateFiles(dir).Count() == 2);

            foreach (string s in Directory.EnumerateFiles(dir))
                File.Delete(s);

            ext.ExtractWildcard(dir, "Core*.cs", true);

            Assert.IsTrue(Directory.EnumerateFiles(dir).Count() == 1);

            foreach (string s in Directory.EnumerateFiles(dir))
                File.Delete(s);
        }
    }
}