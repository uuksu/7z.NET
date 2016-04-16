using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SevenZipNET
{
    /// <summary>
    /// The attributes associated with a file inside an archive.
    /// </summary>
    [Flags]
    public enum Attributes
    {
        /// <summary>
        /// There are no attributes. This is the default value.
        /// </summary>
        None = 0,
        /// <summary>
        /// This subfile is a directory.
        /// </summary>
        Directory = 1,
        /// <summary>
        /// This subfile has been archived.
        /// </summary>
        Archived = 2,
        /// <summary>
        /// This subfile is read-only.
        /// </summary>
        ReadOnly = 4,
        /// <summary>
        /// This subfile is normally hidden.
        /// </summary>
        Hidden = 8,
        /// <summary>
        /// This subfile is a system file.
        /// </summary>
        System = 16
    }

    /// <summary>
    /// A class containing info about a file inside an archive.
    /// </summary>
    public class ArchiveFile
    {
        /// <summary>
        /// The name of the file.
        /// </summary>
        public string Filename
        {
            get;
            protected set;
        }

        /// <summary>
        /// The date and time the file was last modified.
        /// </summary>
        public DateTime LastModified
        {
            get;
            protected set;
        }

        /// <summary>
        /// The unpacked size (in bytes) of the file.
        /// </summary>
        public ulong UnpackedSize
        {
            get;
            protected set;
        }

        /// <summary>
        /// The attributes of the packed file.
        /// </summary>
        public Attributes Attributes
        {
            get;
            protected set;
        }
        
        /// <param name="filename">The name of the file.</param>
        /// <param name="modified">The date and time the file was last modified.</param>
        /// <param name="size">The unpacked size (in bytes) of the file.</param>
        /// <param name="attributes">The attributes associated with this file.</param>
        public ArchiveFile(string filename, DateTime modified, ulong size, Attributes attributes)
        {
            Filename = filename;
            LastModified = modified;
            UnpackedSize = size;
            Attributes = attributes;
        }

        /// <summary>
        /// Parses a line of output of the list command from 7z.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static ArchiveFile Parse(string s)
        {
            var matches = SevenZipExtractor.indexReg.Matches(s);
            List<string> groups = matches[0]
                .Groups.Cast<Group>()
                .Select(x => x.Value)
                .ToList();

            groups.RemoveAt(0); //remove the original string

            DateTime date = DateTime.Parse(groups[0] + " " + groups[1]);

            Attributes attr = Attributes.None;

            if (groups[2].Contains("A"))
                attr |= Attributes.Archived;

            if (groups[2].Contains("S"))
                attr |= Attributes.System;

            if (groups[2].Contains("R"))
                attr |= Attributes.ReadOnly;

            if (groups[2].Contains("D"))
                attr |= Attributes.Directory;

            if (groups[2].Contains("H"))
                attr |= Attributes.Hidden;

            ulong size = ulong.Parse(groups[3]);

            string name = groups[5];

            return new ArchiveFile(name, date, size, attr);
        }
    }
}
