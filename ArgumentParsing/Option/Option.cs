using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ArgumentParsing.Option
{

    /// <summary>
    /// Abstract base class implementation.
    /// </summary>
    internal abstract class Option : IOption
    {

        /// <summary>
        /// Determines whether a given option must occur in a parsed command. 
        /// </summary>
        public bool IsMandatory { get; init; }

        /// <summary>
        /// Determines whether a given option may, or must have parameters.
        /// </summary>
        public abstract bool IsParametrized { get; }

        /// <summary>
        /// Array of char identifiers that represent the Option; e.g. 'f' - short identifier is used as "-f" on command-line.
        /// </summary>
        public char[]? ShortSynonyms { get; init; }

        /// <summary>
        /// Array of string identifiers that represent the Option; e.g. "size" - long identifier is used as "--size on command-line".
        /// </summary>
        public string[]? LongSynonyms { get; init; }

        /// <summary>
        /// This method allow user to set the explanation string - string which is shown when someone uses -h/--help on command line.
        /// Empty explanation will be showed if user does not provide any helpString (does not call this method).
        /// </summary>
        /// <param name="helpString">Text to be shown next to the option in explanation string</param>
        /// <returns></returns>
        public bool SetHelpString(string helpString)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method to call when option occurs in the parsed command-line.
        /// </summary>
        public abstract void TakeAction();

        /// <inheritdoc/>
        public string HelpString { get; set; }

    }

    /// <summary>
    /// Instance class of options with no parameters.
    /// </summary>
    internal class NoParameterOption : Option
    {
        Action action;

        /// <summary>
        /// Constructs instance of <see cref="NoParameterOption"/>.
        /// </summary>
        /// <param name="action">Encapsulated method to call, when the option occurs in the parsed command.</param>
        /// <param name="isMandatory">Determines whether a given option must occur in a parsed command.</param>        /// 
        /// <param name="shortSynonyms"> Specifies what kind of short synonyms should option represent (e.g. "-v").</param>
        /// <param name="longSynonyms"> Specifies what kind of long synonyms should option represent. (e.g. "--version")</param>
        public NoParameterOption(Action action, bool isMandatory, char[]? shortSynonyms = null, string[]? longSynonyms = null)
        {
            IsMandatory = isMandatory;
            this.action = action;
            this.ShortSynonyms = shortSynonyms;
            this.LongSynonyms = longSynonyms;

        }

        /// <inheritdoc/>
        public override bool IsParametrized => false;

        /// <summary>
        /// Method to call when option occurs in the parsed command-line.
        /// </summary>
        public override void TakeAction() => action();
    }

    /// <summary>
    /// Abstract class for parametrized options.
    /// </summary>
    internal abstract class ParameterOption : Option, IParametrizedOption
    {
        
        /// <summary>
        /// Determines whether an option requires parameter.
        /// </summary>
        public bool IsParameterRequired { get; init; }

        /// <inheritdoc/>
        public override bool IsParametrized => true;
        
        /// <summary>
        /// Parses the parameter.
        /// </summary>
        /// <param name="param">Parameter which followed the option.</param>
        /// <returns>True if parsing was successful, otherwise false.</returns> // TODO: maybe redo comments.
        public abstract bool ProcessParameter(string param);


    }

    delegate bool ParseMethodDelegate<T>(string input, out T output );
    delegate bool ParseMethodDelegateMultipleOption<T>(string input, out T[]? output, char separator = ',');

    /// <summary>
    /// Generic class for parametrized options.
    /// </summary>
    internal class GenericParameterOption<T> : ParameterOption, IParametrizedOption
    {
        Action<T?> action;
        ParseMethodDelegate<T> parser;
        T? parserResult;

        /// <summary>
        /// Separator of multiple parameter entries. 
        /// </summary>
        public char Separator { get; init; }        

        public GenericParameterOption(
            ParseMethodDelegate<T> parser,
            Action<T?> action,
            bool isMandatory,
            char[]? shortSynonyms = null,
            string[]? longSynonyms = null,
            char separator = ','
            )
        {
            this.parser = parser;
            this.action = action;
            this.IsMandatory = isMandatory;
            this.ShortSynonyms = shortSynonyms;
            this.LongSynonyms = longSynonyms;
            this.Separator = separator;

        }

        /// <summary>
        /// Parses the parameter.
        /// </summary>
        /// <param name="param">Parameter which followed the option.</param>
        /// <returns>True if parsing was successful, otherwise false.</returns> // TODO: maybe redo comments.
        public override bool ProcessParameter(string param)
        {
            var wasSuccessful = parser(param, out T output);
            if (wasSuccessful) parserResult = output;
            return wasSuccessful;
        }

        public override void TakeAction() => action(parserResult);

    }

    internal class GenericMultipleParameterOption<T> : ParameterOption , IMultipleParameterOption
    {
        Action<T[]?> action;
        ParseMethodDelegateMultipleOption<T> parser;
        T[]? parserResult;

        /// <summary>
        /// Separator of multiple parameter entries. 
        /// </summary>
        public char Separator { get; init; }

        public GenericMultipleParameterOption(
            ParseMethodDelegateMultipleOption<T> parser,
            Action<T[]?> action,
            bool isMandatory,
            char[]? shortSynonyms = null,
            string[]? longSynonyms = null,
            char separator = ','
            )
        {
            this.parser = parser;
            this.action = action;
            this.IsMandatory = isMandatory;
            this.ShortSynonyms = shortSynonyms;
            this.LongSynonyms = longSynonyms;
            this.Separator = separator;

        }

        /// <summary>
        /// Parses the parameter.
        /// </summary>
        /// <param name="param">Parameter which followed the option.</param>
        /// <returns>True if parsing was successful, otherwise false.</returns> // TODO: maybe redo comments.
        public override bool ProcessParameter(string param)
        {
            var wasSuccessful = parser(param, out T[]? output);
            if (wasSuccessful) parserResult = output;
            return wasSuccessful;
        }

        /// <inheritdoc/>
        public override void TakeAction()
        {
            action(parserResult);
            parserResult = null;
        }
    }

}
