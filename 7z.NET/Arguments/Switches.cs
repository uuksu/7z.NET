using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenZipNET.Arguments
{
    /// <summary>
    /// A switch to indicate which compression to use when archiving.
    /// </summary>
    public class SwitchCompression : ArgumentSwitch
    {
        /// <param name="c">The compression level to use.</param>
        public SwitchCompression(CompressionLevel c) : base("mx", ((int)c).ToString())
        {
            Separator = "";
        }
    }

    /// <summary>
    /// A switch to indicate the output directory to use when extracting an archive.
    /// </summary>
    public class SwitchDestination : ArgumentSwitch
    {
        /// <param name="destination">The path to the directory to extract to.</param>
        public SwitchDestination(string destination) : base("o", "\"" + destination + "\"")
        {
            Separator = "";
        }
    }

    /// <summary>
    /// A switch to enable multithreaded processing.
    /// </summary>
    public class SwitchMultithread : ArgumentSwitch
    {
        /// <summary>
        /// 
        /// </summary>
        public SwitchMultithread() : base("mmt")
        {

        }
    }

    /// <summary>
    /// The streams that are outputted by 7z when executing.
    /// </summary>
    public enum OutputStream
    {
        /// <summary>
        /// The current progress.
        /// </summary>
        Progress,
        /// <summary>
        /// The general output.
        /// </summary>
        Output,
        /// <summary>
        /// Any error information.
        /// </summary>
        Error
    }

    /// <summary>
    /// The states of which each stream can be.
    /// </summary>
    public enum OutputStreamState
    {
        /// <summary>
        /// This stream will not be outputted at all.
        /// </summary>
        Disabled = 0,
        /// <summary>
        /// This stream will be redirected to stdout.
        /// </summary>
        Stdout = 1,
        /// <summary>
        /// This stream will be redirected to stderr.
        /// </summary>
        Stderr = 2
    }

    /// <summary>
    /// A switch to change output stream behaviour.
    /// </summary>
    public class SwitchOutputStream : ArgumentSwitch
    {
        /// <param name="stream">The stream of which the state will be set.</param>
        /// <param name="state">The state to set the stream to.</param>
        public SwitchOutputStream(OutputStream stream, OutputStreamState state) : base("bs")
        {
            Separator = "";

            switch(stream)
            {
                case OutputStream.Progress:
                    strings.Add("p" + ((int)state).ToString());
                    break;
                case OutputStream.Output:
                    strings.Add("o" + ((int)state).ToString());
                    break;
                case OutputStream.Error:
                    strings.Add("e" + ((int)state).ToString());
                    break;
            }
        }
    }

    /// <summary>
    /// A switch to indicate if files being extracted will overwrite any existing.
    /// </summary>
    public class SwitchOverwrite : ArgumentSwitch
    {
        /// <param name="overwrite">Wheter or not to overwrite any files.</param>
        public SwitchOverwrite(bool overwrite) : base("ao", overwrite ? "a" : "s")
        {
            Separator = "";
        }
    }
}
