using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenZipNET.Arguments
{
    #region Base Classes
    /// <summary>
    /// A particle of a whole argument list.
    /// </summary>
    public class ArgumentParticle
    {
        /// <summary>
        /// The separator to use to join the strings list when compiling. 
        /// </summary>
        protected string Separator = " ";

        /// <summary>
        /// The list of strings that this particle consists of.
        /// </summary>
        protected List<string> strings = new List<string>();
        
        /// <param name="s">The initial string of this particle.</param>
        public ArgumentParticle(string s)
        {
            strings.Add(s);
        }

        /// <summary>
        /// Compiles the internal list of strings into a valid string.
        /// </summary>
        /// <returns>The string representation of this argument particle.</returns>
        public virtual string Compile()
        {
            return string.Join(Separator, strings);
        }

        /// <summary>
        /// Returns the string representation of this object.
        /// </summary>
        /// <returns>The string representation of this object.</returns>
        public override string ToString() => Compile();
    }

    /// <summary>
    /// A switch for when creating arguments.
    /// </summary>
    public class ArgumentSwitch : ArgumentParticle
    {
        /// <summary>
        /// The prefix for this switch.
        /// </summary>
        protected string Prefix = "-";
        
        /// <param name="s">The inital string of this switch.</param>
        /// <param name="extras">The following parts of this switch.</param>
        public ArgumentSwitch(string s, params string[] extras) : base(s)
        {
            strings.AddRange(extras);
        }

        /// <summary>
        /// Compiles the internal list of strings into a valid string.
        /// </summary>
        /// <returns>The string representation of this argument particle.</returns>
        public override string Compile()
        {
            if (!strings[0].StartsWith(Prefix))
            {
                strings[0] = Prefix + strings[0];
            }
            
            return base.Compile();
        }
    }

    /// <summary>
    /// A command for when creating arguments.
    /// </summary>
    public class ArgumentCommand : ArgumentSwitch
    {
        /// <param name="s">The inital string of this command.</param>
        /// <param name="extras">The following parts of this command.</param>
        public ArgumentCommand(string s, params string[] extras) : base(s, extras.Select(x => "\"" + x + "\"").ToArray())
        {
            Prefix = "";
        }
    }
    #endregion

    /// <summary>
    /// A builder to combine argument particles into one string to be passed as arguments to a program.
    /// </summary>
    public class ArgumentBuilder
    {
        private List<ArgumentParticle> particles = new List<ArgumentParticle>();

        /// <summary>
        /// Adds a particle to be compiled.
        /// </summary>
        /// <param name="p">The particle to add.</param>
        public ArgumentBuilder Add(ArgumentParticle p)
        {
            particles.Add(p);
            return this;
        }

        /// <summary>
        /// Adds a command to be compiled.
        /// </summary>
        /// <param name="c">The command to add.</param>
        public ArgumentBuilder AddCommand(ArgumentCommand c)
        {
            particles.Insert(0, c);
            return this;
        }

        /// <summary>
        /// Adds a command to be compiled.
        /// </summary>
        /// <param name="c">The specific command to be used.</param>
        /// <param name="archive">The archive that is targeted.</param>
        public ArgumentBuilder AddCommand(SevenZipCommands c, string archive)
        {
            return AddCommand(c.ToArgument(archive));
        }

        /// <summary>
        /// Adds a switch to be compiled.
        /// </summary>
        /// <param name="s">The switch to add.</param>
        public ArgumentBuilder AddSwitch(ArgumentSwitch s)
        {
            particles.Insert(1, s);
            return this;
        }

        /// <summary>
        /// Adds a file target to be compiled.
        /// </summary>
        /// <param name="s">The file to target.</param>
        public ArgumentBuilder AddTarget(string s)
        {
            particles.Add(new ArgumentParticle("\"" + s + "\""));
            return this;
        }

        /// <summary>
        /// Adds multiple file targets to be compiled.
        /// </summary>
        /// <param name="s">The files to target.</param>
        public ArgumentBuilder AddTargets(string[] s)
        {
            foreach (string str in s)
                AddTarget(str);
            return this;
        }

        /// <summary>
        /// Compiles the internal list of particles into a valid argument string.
        /// </summary>
        /// <returns>The string representation of all added particles.</returns>
        public string Compile()
        {
            return string.Join(" ",
                particles.Select(x => x.Compile())
                );
        }

        /// <summary>
        /// Returns the string representation of this object.
        /// </summary>
        /// <returns>The string representation of this object.</returns>
        public override string ToString()
        {
            return Compile();
        }
    }
}
