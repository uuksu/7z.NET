using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenZipNET
{
    /// <summary>
    /// The base methods and settings for all SevenZip classes.
    /// </summary>
    public abstract class SevenZipBase
    {
        /// <summary>
        /// Event arguments which indicate the current progress of an operation.
        /// </summary>
        /// <param name="progress">The percentage of which the progress of the operation is at.</param>
        public delegate void ProgressUpdatedEventArgs(int progress);

        private static string _path = "";

        /// <summary>
        /// The path to the 7za.exe binary.
        /// </summary>
        public static string Path7za
        {
            get
            {
                return string.IsNullOrEmpty(_path) ? 
                    Environment.CurrentDirectory + @"\7za.exe" : 
                    _path;
            }
            set
            {
                _path = value;
            }
        }
    }
}
