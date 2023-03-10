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
    /// Specifies what kind of error occurred during the parsing of command line.
    /// </summary>
    public enum ParserErrorType : byte 
    {
        /// <summary>
        /// Occurs when there is on the command line -{InvalidIdentifier} or --{InvalidIdentifier} before the plain arguments separator --.
        /// </summary>
        InvalidOptionIdentifier,

        /// <summary>
        /// Occurs when the option could not parse the parameter belonging to her.
        /// </summary>
        CouldNotParseTheParameter,

        /// <summary>
        /// Occurs when there is Mandatory option missing on the command line.
        /// </summary>
        MissingMandatoryOption,

        /// <summary>
        /// Occurs when there is not enough plain arguments to satisfy number of the mandatory plain arguments.
        /// </summary>
        MissingMandatoryPlainArgument,

        /// <summary>
        /// When other errors occur.
        /// </summary>
        Other

    }

    /// <summary>
    /// Object encapsulating information about an error in <see cref="Parser"/> class instance.
    /// </summary>
    public readonly struct ParserError
    {
        /// <summary>
        /// Type of an error that has occurred.
        /// </summary>
        public readonly ParserErrorType type;
        
        /// <summary>
        /// Description of the error that has occurred.
        /// </summary>
        public readonly string message;        
    }

    /// <summary>
    /// The Parser class enables parsing of command-line inputs.
    /// </summary>
    public sealed class Parser
    {
        private IParametrizedOption[]? _plainArguments;
        private OptionSet.OptionSet _options = new();

        /// <summary>
        /// Gets parser error if one occurred, otherwise null.
        /// </summary>
        public ParserError? Error { get; private set; }

        /// <summary>
        /// Creates instance of <see cref="Parser"/> without specified types of plain parameters.
        /// </summary>
        public Parser ()
        {
            _plainArguments = null;
        }

        /// <summary>
        /// Creates instance of <see cref="Parser"/> with specified types of plain parameters.
        /// </summary>
        /// <param name="plainArguments"> Array of <see cref="IParametrizedOption"/>, where mandatory plain arguments should come before the non-mandatory 
        /// plain arguments. To create instances of these plain arguments user can use static method CreatePlainArgument of Interface IParametrized,
        /// when he wants to define just one plain argument that should stand alone, or static method CreateMultipleParametersPlainArgument of Interface
        /// IMultipleParameterOption when he wants to process plain arguments separated by the separator. Or he can create them manually and non-necessary
        /// fields and properties are mentioned next to the mentioned methods.
        /// </param>
        public Parser(IParametrizedOption[] plainArguments)
        {
            _plainArguments = plainArguments;
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
