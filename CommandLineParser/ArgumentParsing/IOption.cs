using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgumentParsing
{
    // TODO: Add descriptions of interfaces.
    /// <summary>
    /// 
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
        public char[]? shortSynonyms { get; }

        /// <summary>
        /// Array of string identifiers that represent the Option; e.g. "size" - long identifier is used as "--size on command-line".
        /// </summary> 
        public string[]? longSynonyms { get; }

        /// <summary>
        /// This method allow user to set the explanation string - string which is shown when someone uses -h/--help on command line.
        /// Empty explanation will be showed if user does not provide any helpString (does not call this method).
        /// </summary>
        /// <param name="helpString">Text to be shown next to the option in explanation string</param>
        /// <returns></returns>
        public bool SetHelpString(string helpString);

        /// <summary>
        /// Method to call when option occurs in the parsed command-line.
        /// </summary>
        public void TakeAction();

        /// <summary>
        /// Creates an instance of <see cref="IOption"/> with desired properties.
        /// </summary>
        /// <param name="action">Encapsulated method to call, when the option occurs in the parsed command.</param>
        /// <param name="isMandatory">Determines whether a given option must occur in a parsed command.</param>        /// 
        /// <param name="shortSynonyms"> Specifies what kind of short synonyms should option represent (e.g. "-v").</param>
        /// <param name="longSynonyms"> Specifies what kind of long synonyms should option represent. (e.g. "--version")</param>
        public static IOption CreateNoParameterOption(
            Action action,
            bool isMandatory,
            char[]? shortSynonyms = null,
            string[]? longSynonyms = null
            )
        {
            throw new NotImplementedException();
        }

    }

    public interface IParametrizedOption : IOption
    {
        /// <summary>
        /// Determines whether an option requires parameter.
        /// </summary>
        public bool IsParameterRequired { get; }

        /// <summary>
        /// Method to call with string parameter corresponding to the option.
        /// </summary>
        /// <param name="parameter">Parameter corresponding to the option.</param>
        /// <returns>True if no error occurred during processing, otherwise false.</returns>
        public bool ProcessParameter(string parameter);

        /// <summary>
        /// Constructs an instance of <see cref="IParametrizedOption"/> that takes 0 to 1 parameters (based on isParameterRequired) of type T .
        /// </summary>
        /// <param name="action"> Specifies action, which is called, when option is present on command line. If isMandatory is set to true,
        /// action is called with one parameter of type T, else it is called with 0(null) to 1 parameters of type T according to the number of parameters
        /// provided on command line.
        /// </param>
        /// <param name="isParameterRequired"> Specifies whether the option requires at least one parameter present on
        /// command line.
        /// </param>
        /// <param name="isMandatory"> Specifies whether option is mandatory i. e. must be present on command line.</param>
        /// <param name="shortSynonyms"> Specifies what kind of short synonyms should option represent (e.g. "-v").</param>
        /// <param name="longSynonyms"> Specifies what kind of long synonyms should option represent. (e.g. "--version")</param>
        public static IParametrizedOption CreateParameterOption<T>(
            Action<T?> action,
            bool isMandatory,
            bool isParameterRequired = false,
            char[]? shortSynonyms = null,
            string[]? longSynonyms = null
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates an object that represents plain argument, that should stand alone on the command line.
        /// </summary>
        /// <typeparam name="T">Specifies of what type this plain argument should be</typeparam>
        /// <param name="action"> Specifies what action should be taking with the parsed plain argument.</param>
        /// <param name="isMandatory"> Specfies whether this plain argument must be present on the command line (user must provide it)</param>
        /// <returns>Object satisfying conditions above</returns>
        public static IParametrizedOption CreateMultipleParametersPlainArgument<T>(
           Action<T?> action,
           bool isMandatory       
           )
        {
            throw new NotImplementedException();
        }

    }

    public interface IMultipleParameterOption : IParametrizedOption
    {
        /// <summary>
        /// Separates multiple parameters entries. 
        /// </summary>
        public char Separator { get; }

        /// <summary>
        /// Creates an instance of <see cref="IMultipleParameterOption"/>.
        /// </summary>
        /// <param name="action">Specifies action, which is called, when option is present on command line. It is called
        /// with 0(null) to unlimited number of parameters of type T according to the number of parameters provided on command line.
        /// If isMandatory is true, there must be at least one parameter present on command line.
        /// </param>
        /// <param name="isParameterRequired"> Specifies whether the option requires at least one parameter present on
        /// command line.
        /// </param>
        /// <param name="isMandatory"> Specifies whether option is mandatory i. e. must be present on command line.</param>
        /// <param name="shortSynonyms"> Specifies what kind of short synonyms should option represent (e.g. "-v").</param>
        /// <param name="longSynonyms"> Specifies what kind of long synonyms should option represent. (e.g. "--version")</param>
        /// <param name="separator"> Specifies what char is used to separate multiple parameter entries. (e.g. "--version")</param>

        public static IMultipleParameterOption CreateMulitipleParameterOption<T>(
           Action<T[]?> action,
           bool isMandatory,
           bool isParameterRequired = false,
           char[]? shortSynonyms = null,
           string[]? longSynonyms = null,
           char separator = ','
           )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates an object that represents plain argument, that should represent multiple plain arguments separated by
        /// non-white-space separator. I. e. if you want to take multiple plain arguments of same type you choose this object.
        /// </summary>
        /// <typeparam name="T">Specifies of what type this plain argument should be</typeparam>
        /// <param name="action"> Specifies what action should be taking with the parsed plain arguments.</param>
        /// <param name="isMandatory"> Specfies whether these plain arguments must be present on the command line (user must provide them)</param>
        /// <param name="separator"> Specifies by what char should be arguments separated.</param>
        /// <returns>Object satisfying conditions above</returns>
        public static IMultipleParameterOption CreateMultipleParametersPlainArgument<T>(
           Action<T[]?> action,
           bool isMandatory,
           char separator = ','
           )
        {
            throw new NotImplementedException();
        }

    }
}
