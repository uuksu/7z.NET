using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenZipNET.Arguments
{
    /// <summary>
    /// The list of commands that can be used with 7za.
    /// </summary>
    public enum SevenZipCommands
    {
        /// <summary>
        /// Adds files to an archive.
        /// </summary>
        Add,
        /// <summary>
        /// Extracts files from an archive.
        /// </summary>
        Extract,
        /// <summary>
        /// Deletes files from an archive.
        /// </summary>
        Delete,
        /// <summary>
        /// Lists files from an archive.
        /// </summary>
        List,
        /// <summary>
        /// Tests and runs diagnostics on an archive.
        /// </summary>
        Test,
        /// <summary>
        /// Updates files inside of an archive.
        /// </summary>
        Update,
        /// <summary>
        /// Extracts files from an archive, retaining internal directory structure.
        /// </summary>
        ExtractWithStructure
    }

    /// <summary>
    /// A class to generate arguments and argument particles from input.
    /// </summary>
    public static partial class ArgumentFactory
    {
        /// <summary>
        /// Creates an ArgumentCommand from a valid 7z command and the archive for it to target.
        /// </summary>
        /// <param name="command">The command to use on the archive.</param>
        /// <param name="archive">The filename of the archive.</param>
        public static ArgumentCommand ToArgument(this SevenZipCommands command, string archive)
        {
            switch (command)
            {
                case SevenZipCommands.Add:
                    return new ArgumentCommand("a", archive);
                case SevenZipCommands.Extract:
                    return new ArgumentCommand("e", archive);
                case SevenZipCommands.Delete:
                    return new ArgumentCommand("d", archive);
                case SevenZipCommands.List:
                    return new ArgumentCommand("l", archive);
                case SevenZipCommands.Test:
                    return new ArgumentCommand("t", archive);
                case SevenZipCommands.Update:
                    return new ArgumentCommand("u", archive);
                case SevenZipCommands.ExtractWithStructure:
                    return new ArgumentCommand("x", archive);
            }
            return null;
        }
    }
}
