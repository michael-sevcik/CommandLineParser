using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArgumentParsing.OptionSet;

namespace ArgumentParsing
{
    /// <summary>
    /// The Parser class enables parsing of command-line inputs.
    /// </summary>
    public class Parser
    {
        private IParametrizedOption[]? _plainParamethers;
        private OptionSet.OptionSet _options = new();

        /// <summary>
        /// Creates instance of <see cref="Parser"/> without specified types of plain parameters.
        /// </summary>
        public Parser ()
        {
            _plainParamethers = null;
        }

        /// <summary>
        /// Creates instance of <see cref="Parser"/> with specified types of plain parameters.
        /// </summary>
        /// <param name="plainParamethers"></param> // TODO: finish description.
        public Parser(IParametrizedOption[] plainParameters)
        {
            _plainParamethers = plainParameters;
        }

        /// <summary>
        /// Adds the option to the OptionSet.
        /// </summary>
        /// <param name="option"><see cref="IOption"> instance to be added.</param>
        /// <returns>Returns true if there were no problems adding the option,
        /// returns false if an error occurred, such as synonyms colliding with already added options, no short options and
        /// no long options at the same time and other undefined behavior.
        /// </returns>
        public bool Add(IOption option)
        {
            throw new NotImplementedException();
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
        /// Method allows user to get the help text to be shown when client uses -h/--help on command line.
        /// To work correctly, user must specify at each option, which hints or explanations to be showed.
        /// </summary>
        /// <returns>Returns string to be shown when client uses -h/--help on command line</returns>
        public string GetHelpString()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Allows user set help string for "--", which is shown when -h/--help is present on command line.
        /// </summary>
        /// <param name="PAHelpString">Help string to be shown next to -- in help page.</param>

        public void SetPlainArgumentHelpString(string PAHelpString)
        {
            throw new NotImplementedException();
        }
    }

    





}
