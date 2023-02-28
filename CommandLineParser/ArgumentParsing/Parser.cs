using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgumentParsing
{
    /// <summary>
    /// The Parser class enables parsing of command-line inputs.
    /// </summary>
    public class Parser
    {
        private List<string> _plainParamethers = new List<string>();
        protected OptionSet options;

        /// <summary>
        /// Creates instance of Parser that parses given options.
        /// </summary>
        /// <param name="optionSet">Set of options used for parsing.</param>
        public Parser(OptionSet optionSet)
        {
            this.options = optionSet;
        }

        /// <summary>
        /// Parses a given command.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>True when parsing was successful, otherwise false.</returns>
        public bool ParseCommandLine(string [] args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets parameters that are not options and are not belonging to a option.
        /// </summary>
        /// <returns>List of plain parameters.</returns>
        public List<string> GetPlainParameters() 
            => _plainParamethers;

        /// <summary>
        /// Method allows user to get the help text to be shown when client uses -h/--help on command line.
        /// To work correctly, user must specify at each option, which hints or explanations to be showed.
        /// </summary>
        /// <returns>Returns string to be shown when client uses -h/--help on command line</returns>
        public string GetHelpString()
        {
            throw new NotImplementedException();
        }
    }

    

    /// <summary>
    /// The OptionSet class is used to define the set of options that can be parsed by the Parser class.
    /// </summary>
    /// 
    /// <Remarks>
    /// If two or more options with the same identifier are added to the OptionSet, Find method could behave nondeterministically.
    /// </Remarks>
    public class OptionSet
    {
        private Dictionary<string, Option> longOptions = new();
        private Dictionary<char, Option> shortOptions = new();

        /// <summary>
        /// Adds the option to the OptionSet.
        /// </summary>
        /// <param name="option"><see cref="Option"> instance to be added.</param>
        /// <returns>True </returns>
        public bool Add(Option option)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Looks for option that was added to the OptionSet and has the specified identifier.
        /// </summary>
        /// <param name="shortIdentifier">Short identifier of an option.</param>
        /// <returns>Returns an option if corresponding one was found, otherwise null.</returns>
        public Option? Find(char shortIdentifier)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Looks for option that was added to the OptionSet and has the specified identifier.
        /// </summary>
        /// <param name="longIdentifier">Long identifier of an option.</param>
        /// <returns>Returns an option if corresponding one was found, otherwise null.</returns>
        public Option? Find(string longIdentifier)
        {
            throw new NotImplementedException();
        }
    }



}
