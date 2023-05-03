using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArgumentParsing.OptionSet;

namespace ArgumentParsing;

/// <summary>
/// The Parser class enables parsing of command-line inputs.
/// </summary>
public sealed partial class Parser
{

    /// <summary>
    /// Specifies what kind of error occurred during the parsing of command line.
    /// </summary>
    public enum ParserErrorType : byte
    {
        /// <summary>
        /// There was no option with the specified identifier.
        /// </summary>
        InvalidOptionIdentifier,

        /// <summary>
        /// Option could not parse the parameter belonging to her.
        /// </summary>
        CouldNotParseTheParameter,

        /// <summary>
        /// There is Mandatory option missing on the command line.
        /// </summary>
        MissingMandatoryOption,

        /// <summary>
        /// There are not enough plain arguments to satisfy number of the mandatory plain arguments.
        /// </summary>
        MissingMandatoryPlainArgument,

        /// <summary>
        /// Argument for an option with a required argument was not passed.
        /// </summary>
        MissingOptionParameter,

        /// <summary>
        /// A given option occurred on the command-line more than once. // TODO: Add to documentation.
        /// </summary>
        RepeatedOccurenceOfOption,

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
        /// Option related to the error. Null if there is no option related
        /// </summary>
        public readonly IOption? optionInError;

        /// <summary>
        /// PlainArgument related to the error. Null if there is no plain argument related.
        /// </summary>
        public readonly IPlainArgument? plainArgumentInError;

        /// <summary>
        /// Description of the error that has occurred.
        /// </summary>
        public readonly string message;

        /// <summary>
        /// Creates instance of <see cref="ParserError"/> with a specified type, option and additional info message.
        /// </summary>
        public ParserError(ParserErrorType type, string message, IOption? optionInError = null, IPlainArgument? plainArgumentInError = null)
        {
            this.type = type;
            this.optionInError = optionInError;
            this.plainArgumentInError = plainArgumentInError;
            this.message = message;
        }
    }

    partial class ArgumentProcessor { }

    // TODO: Custom types of options and plain arguments should be also immutable. -  documentation

    private readonly IPlainArgument[]? plainArguments;
    private readonly string? plainArgumentsHelpMessage;
    private readonly OptionSet.OptionSet options = new();
    private readonly IPlainArgument[]? mandatoryPlainArguments;

    // TODO: add the null info to the documentation
    /// <summary>
    /// Gets remaining plain arguments, which were excessive in relation to the number of IPlainArguments passed in constructor. Null if there were no excessive plain arguments.
    /// </summary>
    public string[]? RemainingPlainArguments { get; private set; } = null;

    /// <summary>
    /// If an error occurs during parsing it gets a instance of <see cref="ParserError"/> that is describing the problem, otherwise null.
    /// </summary>
    public ParserError? Error { get; private set; } = null;

    /// <summary>
    /// Creates instance of <see cref="Parser"/> without specified types of plain parameters.
    /// </summary>
    public Parser() { }

    /// <summary>
    /// Creates instance of <see cref="Parser"/> with specified types of plain parameters.
    /// </summary>
    /// <param name="plainArguments"> Array of <see cref="IParametrizedOption"/>, where mandatory plain arguments should come before the non-mandatory 
    /// plain arguments. To create instances of these plain arguments user can use static method CreatePlainArgument of Interface IParametrized,
    /// when he wants to define just one plain argument that should stand alone, or static method CreateMultipleParametersPlainArgument of Interface
    /// IMultipleParameterOption when he wants to process plain arguments separated by the separator. Or he can create them manually and non-necessary
    /// fields and properties are mentioned next to the mentioned methods.
    /// </param>
    public Parser(IPlainArgument[] plainArguments, string? plainArgumentsHelpMessage = null) // Todo: Add parameter to documentation.
    {
        this.plainArguments = plainArguments;
        mandatoryPlainArguments = this.plainArguments.Where(plainArgument => plainArgument.IsMandatory).ToArray();
        this.plainArgumentsHelpMessage = plainArgumentsHelpMessage;
    } 
    // TODO: Add to documentation that the plainArguments (especially the default library types) had to be unique,
    // otherwise the parser will fail on repeated plainArgument occurrence OR WE CAN JUST RISK LOSING SOME DATA.

    /// <summary>
    /// Adds the option to the OptionSet.
    /// </summary>
    /// <param name="option"><see cref="IOption"> instance to be added.</param>
    /// <returns>Returns true if there were no problems adding the option,
    /// returns false if an error occurred, such as synonyms colliding with already added options, no short options and
    /// no long options at the same time and other undefined behavior.
    /// </returns>
    public bool Add(IOption option) => options.Add(option);


    /// <summary>
    /// Parses a given command.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    /// <returns>True when parsing was successful, otherwise false.</returns>
    public bool ParseCommandLine(string[] args) // TODO: We will probably need to add a method used to restore the IParametrizedOption instance, when the parsing fails.
    {
        ArgumentProcessor argumentProcessor = new(options, plainArguments, mandatoryPlainArguments);

        for (int i = 0; i < args.Length; i++)
        {
            if (!argumentProcessor.ProcessArgument(args[i]))
            {
                Error = argumentProcessor.Error;
                return false;
            }
        }

        if (!argumentProcessor.FinalizeProcessing(out var result))
        {
            Error = argumentProcessor.Error;
            return false;
        }
        RemainingPlainArguments = result.excessivePlainArgumentsEntries;

        foreach (var option in result.optionsToTakeAction)
        {
            option.TakeAction();
        }

        foreach (var plainArgument in result.plainArgumentsToTakeAction)
        {
            plainArgument.TakeAction();
        }

        return true;
    }

    /// <summary>
    /// Method allows user to get the help text to be shown when client uses -h/--help on command line.
    /// To work correctly, user must specify at each option, which hints or explanations to be showed.
    /// </summary>
    /// <returns>Returns string to be shown when client uses -h/--help on command line</returns>
    public string GetHelpString()
    {
        string helpString = "Options: \n";
        helpString += options.GetHelpString();

        helpString += "Plain arguments: \n";
        helpString += plainArgumentsHelpMessage;
        return helpString;
    }

    /// <summary>
    /// Allows user set help string for "--", which is shown when -h/--help is present on command line.
    /// </summary>
    /// <param name="PAHelpString">Help string to be shown next to -- in help page.</param>

    //public void SetPlainArgumentHelpString(string PAHelpString) // TODO: delete this from documentation and example programs.


}
