using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using SevenZipNET.Arguments;
using System.Collections.ObjectModel;

namespace SevenZipNET
{
    /// <summary>
    /// Provides reading and extraction functionality from an already existing archive.
    /// </summary>
    public class SevenZipExtractor : SevenZipBase
    {
        private readonly string archive;
        private readonly string password;

        internal static Regex indexReg = new Regex(@"^(\d{2,4}-\d{2,4}-\d{2,4})\s+(\d{2}:\d{2}:\d{2})\s+(.{5})\s+(\d+)\s+(\d+)?\s+(.+)");
        
        /// <summary>
        /// Raised when progress of an operation has been updated.
        /// </summary>
        public event ProgressUpdatedEventArgs ProgressUpdated;

        /// <summary>
        /// Fires the ProgressUpdated event.
        /// </summary>
        /// <param name="progress">The current progress.</param>
        protected void UpdateProgress(int progress)
        {
            ProgressUpdatedEventArgs invoke = ProgressUpdated;
            if (invoke != null)
            {
                invoke(progress);
            }
        }

        /// <param name="filename">Path to the archive.</param>
        public SevenZipExtractor(string filename)
        {
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));

            if (!File.Exists(filename))
                throw new ArgumentException("File could not be found.", nameof(filename));

            archive = filename;

            _indexCache = new Lazy<ReadOnlyCollection<ArchiveFile>>(Index);
        }

        public SevenZipExtractor(string filename, string password) : this(filename)
        {
            this.password = password;
        }

        /// <summary>
        /// Returns a list of files inside the archive.
        /// </summary>
        /// <returns>The list of files inside the archive.</returns>
        protected ReadOnlyCollection<ArchiveFile> Index()
        {
            List<ArchiveFile> files = new List<ArchiveFile>();

            WProcess p = new WProcess(new ArgumentBuilder()
                .AddCommand(SevenZipCommands.List, archive)
                );

            string[] output = p.Execute();

            int dash = 0; //so it collects data only inbetween the lines made of dashes

            foreach (string line in output)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                else if (line.StartsWith("---"))
                {
                    dash++;
                }
                else if (indexReg.IsMatch(line) &&
                         dash == 1)
                {
                    ArchiveFile file = ArchiveFile.Parse(line);
                    files.Add(file);
                }
            }
            
            return files.AsReadOnly();
        }

        private Lazy<ReadOnlyCollection<ArchiveFile>> _indexCache;

        /// <summary>
        /// Information relating to each file stored inside the archive.
        /// </summary>
        public ReadOnlyCollection<ArchiveFile> Files
        {
            get
            {
                return _indexCache.Value;
            }
        }

        /// <summary>
        /// The total size of the contents of the archive when unpacked.
        /// </summary>
        public ulong UnpackedSize
        {
            get
            {
                return (ulong)Files.Sum(x => (float)x.UnpackedSize);
            }
        }

        /// <summary>
        /// Extracts everything in the archive to the specified destination.
        /// </summary>
        /// <param name="destination">The directory to extract archive contents to.</param>
        /// <param name="overwrite">Whether or not to overwrite existing files. Default false.</param>
        /// <param name="keepStructure">Whether or not to retain folder structure in the archive. Default true.</param>
        public void ExtractAll(string destination, bool overwrite = false, bool keepStructure = true)
        {
            ArgumentBuilder argumentBuilder = GetExtractArguments(destination, overwrite, keepStructure);

            if (!string.IsNullOrEmpty(password))
            {
                argumentBuilder.AddSwitch(new SwitchPassword(password));
            }

            WProcess p = new WProcess(argumentBuilder);

            p.ProgressUpdated += ProgressUpdated;

            p.Execute();
        }

        /// <summary>
        /// Extracts everything in the archive to the specified destination.
        /// </summary>
        /// <param name="destination">The directory to extract archive contents to.</param>
        /// <param name="wildcard">The wildcard(s) to extract.</param>
        /// <param name="overwrite">Whether or not to overwrite existing files. Default false.</param>
        /// <param name="keepStructure">Whether or not to retain folder structure in the archive. Default true.</param>
        public void ExtractWildcard(string destination, string wildcard, bool overwrite = false, bool keepStructure = true)
        {
            ArgumentBuilder argumentBuilder = GetExtractArguments(destination, overwrite, keepStructure);

            if (!string.IsNullOrEmpty(password))
            {
                argumentBuilder.AddSwitch(new SwitchPassword(password));
            }

            argumentBuilder.AddTarget(wildcard);

            WProcess p = new WProcess(argumentBuilder);

            p.ProgressUpdated += ProgressUpdated;

            p.Execute();
        }

        private ArgumentBuilder GetExtractArguments(string destination, bool overwrite = false, bool keepStructure = true)
        {
            return new ArgumentBuilder()
                .AddCommand(keepStructure ? SevenZipCommands.ExtractWithStructure : SevenZipCommands.Extract, archive)
                .AddSwitch(new SwitchOutputStream(OutputStream.Progress, OutputStreamState.Stdout))
                .AddSwitch(new SwitchOverwrite(overwrite))
                .AddSwitch(new SwitchDestination(destination));
        }

        /// <summary>
        /// Tests the archive to see if it is valid.
        /// </summary>
        /// <returns>If the archive is valid.</returns>
        public bool TestArchive()
        {
            ArgumentBuilder argumentBuilder = new ArgumentBuilder()
                .AddCommand(SevenZipCommands.Test, archive);

            if (!string.IsNullOrEmpty(password))
            {
                argumentBuilder.AddSwitch(new SwitchPassword(password));
            }

            WProcess p = new WProcess(argumentBuilder);

            p.ProgressUpdated += ProgressUpdated;

            string aggregate = string.Join(" ", p.Execute());

            return aggregate.Contains("Everything is Ok"); //a bit hacky but it's fine
        }
    }
}
