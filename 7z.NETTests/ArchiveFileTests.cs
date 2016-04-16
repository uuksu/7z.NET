using Microsoft.VisualStudio.TestTools.UnitTesting;
using SevenZipNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenZipNET.Tests
{
    [TestClass()]
    public class ArchiveFileTests
    {
        [TestMethod()]
        public void ParseTest()
        {
            string line = @"2000-01-01 00:00:00 ....A            0            0  directory\archive space.txt";

            ArchiveFile file = ArchiveFile.Parse(line);

            Assert.AreEqual(@"directory\archive space.txt", file.Filename);

            Assert.AreEqual(file.Attributes, Attributes.Archived);
        }
    }
}