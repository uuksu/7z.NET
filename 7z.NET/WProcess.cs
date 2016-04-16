using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace SevenZipNET
{
    /// <summary>
    /// A wrapper for the <c>Process</c> class.
    /// </summary>
    /// <seealso cref="Process"/>
    internal class WProcess
    {
        private Process _process;

        public Process BaseProcess
        {
            get
            {
                return _process;
            }
        }

        public WProcess()
        {
            _process = new Process();
            _process.StartInfo.FileName = SevenZipBase.Path7za;
            
            _process.StartInfo.CreateNoWindow = true;
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.RedirectStandardOutput = true;
        }

        /// <param name="arguments">The string of arguments to start the process with.</param>
        public WProcess(string arguments) : this()
        {
            _process.StartInfo.Arguments = arguments;
        }
        
        /// <param name="builder">The argument builder to retrieve arguments from.</param>
        public WProcess(Arguments.ArgumentBuilder builder) : this(builder.ToString())
        {

        }
        
        public event SevenZipBase.ProgressUpdatedEventArgs ProgressUpdated;

        /// <summary>
        /// Fires the ProgressUpdated event.
        /// </summary>
        /// <param name="progress">The current progress.</param>
        protected void UpdateProgress(int progress)
        {
            SevenZipBase.ProgressUpdatedEventArgs invoke = ProgressUpdated;
            if (invoke != null)
            {
                invoke(progress);
            }
        }

        /// <summary>
        /// Starts a process and returns the stdout stream output.
        /// </summary>
        /// <returns>Output console data.</returns>
        public string[] Execute()
        {
            List<string> output = new List<string>();

            Regex r = new Regex(@"(\d{1,3})%");

            _process.OutputDataReceived += ((s, e) => {

                if (e.Data != null)
                {
                    output.Add(e.Data);
                
                    var match = r.Match(e.Data.Trim());
                    if (match.Success)
                    {
                        UpdateProgress(int.Parse(match.Groups[1].Value));
                    }
                }

            });
            
            _process.Start();
            _process.BeginOutputReadLine();

            _process.WaitForExit();

            return output.ToArray();
        }
    }
}
