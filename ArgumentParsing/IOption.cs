
using System;

namespace ArgumentParsing
{
    /// <summary>
    /// Defines a generalized object representing an option with identifiers (synonyms). 
    /// </summary>
    public interface IOption
    {

        /// <summary>
        /// Determines whether a given option must occur in a parsed command. 
        /// </summary>
        public bool IsMandatory { get; }

        /// <summary>
        /// Array of char identifiers that represent the Option; e.g. 'f' - short identifier is used as "-f" on command-line.
        /// </summary>     
        public char[]? ShortSynonyms { get; }

        /// <summary>
        /// Array of string identifiers that represent the Option; e.g. "size" - long identifier is used as "--size on command-line".
        /// </summary>
        public string[]? LongSynonyms { get; }

        /// <summary>
        /// Gets or sets explanation string - string which is shown when someone uses -h/--help on command line.
        /// Empty explanation will be showed if user does not provide any helpString (does not call this method).
        /// </summary>
        /// <param name="helpString">Text to be shown next to the option in explanation string</param>
        public string HelpString { get; set; }

        /// <summary>
        /// Method to call when option occurs in the parsed command-line.
        /// </summary>
        public void TakeAction();

    }

    /// <summary>
    /// Defines a generalized object representing an option that satisfies the <see cref="IOption"/> interface and also takes a parameter. 
    /// </summary>
    public interface IParametrizedOption : IOption
    {
        /// <summary>
        /// Determines whether an option requires parameter.
        /// </summary>
        public bool IsParameterRequired { get; }

        /// <summary>
        /// Method to call with string corresponding to the option. 
        /// </summary>
        /// <param name="parameter">Parameter corresponding to the option.</param>
        /// <returns>True if no error occurred during processing, otherwise false.</returns>
        public bool ProcessParameter(string parameter);

    }

    /// <summary>
    /// Defines a generalized object representing an option that satisfies the <see cref="IParametrizedOption"/> interface and adds factory methods for options whose parameter can be separated to multiple parameters. 
    /// </summary>
    public interface IMultipleParameterOption : IParametrizedOption
    {
        /// <summary>
        /// Separates the parameters following and option on command line. Must be non-white space.
        /// </summary>
        public char Separator { get; }

    }
    /// <summary>
    /// On the command line can occur 2 types of plain arguments:
    ///    - Simple plain argument -> one word representing one plain argument, for example: "1" or "hello"
    ///    - Multiple parameters plain argument -> one string representing multiple plain arguments(similarly to multiple parameter option). 
    ///    For example: "1,2,3" . This can be interpreted as 3 int numbers separated by ',' separator.NOTE: the separator must be non-white-space.
    /// Instance implementing this interface represents either Simple plain argument or the Multiple parameters plain argument.
    /// These two are different in the implementation of ProcessParameter and Take action, the differences are described below.
    /// </summary>
    public interface IPlainArgument
    {
        /// <summary>
        /// Determines whether a given plain argument must occur in a parsed command. If set to true, then if it is not present the method
        /// ParseCommandLine in Parser will return false. Remember that mandatory plain arguments must come before the non mandatory
        /// plain arguments in the array passed to the <see cref="Parser"/>.
        /// </summary>
        public bool IsMandatory { get; }

        /// <summary>
        /// Method to call when plain argument occurs in the parsed command-line. If it is the instance of derived Interface.
        /// Our default implementation will basically call the Action provided in the factory method with parsed plain argument.
        /// In case of the Multiple parameters plain argument the action will take array of parsed arguments.
        /// <see cref="IMultipleParameterOption"/>, then this method is called after the parsing method ProcessParameter.
        /// </summary>
        public void TakeAction();

        /// <summary>
        /// Method to call with string corresponding to the option. In the case of the simple plain argument we should just parse the string
        /// In the case of Multiple parameters plain argument, we should (it is recommended) first split the <paramref name="parameter"/> according to the
        /// separator. When user implements this by himself he knows what kind of the separator should be present on the command line. When using our classes,
        /// provided by the factory methods, the separator is specified in the parameters of the CreateMultipleParametersPlainArgument method.
        /// </summary>
        /// <param name="parameter">Parameter corresponding to the option.</param>
        /// <returns>True if no error occurred during processing, otherwise false.</returns>
        public bool ProcessParameter(string parameter);

        /// <summary>
        /// Creates an object that represents plain argument, that should stand alone on the command line. It is similar to 
        /// <see cref="IParametrizedOption"/> and it's derived classes objects, but long and short synonyms are omitted, as in the plain arguments
        /// we only consider the parameters. (There are none options in the plain arguments). Also isParameterRequired is not necessary as isMandatory
        /// property replaces it.
        /// </summary>
        /// <typeparam name="TArgument">Specifies of what type this plain argument should be. Accepted types are bool, string, int, enum.</typeparam>
        /// <param name="action"> Specifies what action should be taking with the parsed plain argument.</param>
        /// <param name="isMandatory"> Specifies whether this plain argument must be present on the command line (user must provide it)</param>
        /// <returns>Object satisfying conditions above</returns>
        /// <exception cref="InvalidOperationException">Thrown when wrong <typeparamref name="TArgument"/> is chosen. Accepted types are bool, string, int, enum.</exception>

        public static IPlainArgument CreatePlainArgument<TArgument>(
           Action<TArgument?> action,
           bool isMandatory
           )
        {

            var builder = new OptionBuilder();
            builder.WithParametrizedAction<TArgument>( action );

            if (isMandatory) builder.SetAsMandatory();

            return (IPlainArgument)builder.CreateParticularOptionForRegistration();

        }

        /// <summary>
        /// Creates an object that represents multiple plain arguments separated by non-white-space separator. This object is similar to <see cref="IOption"/> and its derived
        /// classes objects, but some non-necessary details (mention in IParametrizedOption) are omitted.
        /// I. e. if you want to take multiple plain arguments of same type you choose this object.
        /// Note that you do not define synonyms or names for this object, you just define what kind of parameters should this "option" take.
        /// Note that if you want to implement this interface by yourself you need to consider what kind of separator will be expected.
        /// </summary>
        /// <typeparam name="TArgument">Specifies of what type this plain argument should be</typeparam>
        /// <param name="action"> Specifies what action should be taking with the parsed plain arguments.</param>
        /// <param name="isMandatory"> Specifies whether these plain arguments must be present on the command line (user must provide them)</param>
        /// <param name="separator"> Specifies by what char should be arguments separated.</param>
        /// <returns>Object satisfying conditions above</returns>
        /// <exception cref="InvalidOperationException">Thrown when wrong <typeparamref name="TArgument"/> is chosen. Accepted types are bool, string, int, enum.</exception>
        public static IPlainArgument CreateMultipleParametersPlainArgument<TArgument>(
           Action<TArgument[]?> action,
           bool isMandatory,
           char separator = ','
           )
        {
            var builder = new OptionBuilder();
            builder.
                WithMultipleParametersAction<TArgument>(action).
                WithSeparator(separator);

            if (isMandatory) builder.SetAsMandatory();

            return (IPlainArgument)builder.CreateParticularOptionForRegistration();
        }

    }
}
