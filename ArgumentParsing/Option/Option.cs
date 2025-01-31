﻿using System;
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
        /// Method to call when option occurs in the parsed command-line.
        /// </summary>
        public abstract void TakeAction();

        /// <inheritdoc/>
        public abstract void Restore();

        /// <inheritdoc/>
        public string HelpString { get; set; } = string.Empty;

    }

    /// <summary>
    /// Instance class of options with no parameters.
    /// </summary>
    internal class NoParameterOption : Option
    {
        readonly Action action;

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

        /// <inheritdoc/>
        public override void Restore() { }

        /// <summary>
        /// Method to call when option occurs in the parsed command-line.
        /// </summary>
        public override void TakeAction() => action();
    }

    /// <summary>
    /// Abstract class for parametrized options.
    /// </summary>
    internal abstract class ParameterOption : Option, IParametrizedOption, IPlainArgument
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

    // delegates which we pass to Generic Option classes, which are used for parsing the parameters on command line
    // decreases number of classes needed to be implemented
    delegate bool ParseMethodDelegate<T>(string input, out T output );
    delegate bool ParseMethodDelegateMultipleOption<T>(string input, out T[]? output, char separator = ',');

    /// <summary>
    /// Generic class for parametrized options.
    /// </summary>
    /// /// <typeparam name="T"> Type of parameter</typeparam>
    internal class GenericParameterOption<T> : ParameterOption, IParametrizedOption , IPlainArgument
    {
        readonly Action<T?> action;
        readonly ParseMethodDelegate<T> parser;
        T? parsingResult;

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
            bool requiresParameter = false,
            char separator = ','
            
            )
        {
            this.parser = parser;
            this.action = action;
            this.IsMandatory = isMandatory;
            this.ShortSynonyms = shortSynonyms;
            this.LongSynonyms = longSynonyms;
            this.IsParameterRequired = requiresParameter;
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
            if (wasSuccessful) parsingResult = output;
            return wasSuccessful;
        }

        /// <summary>
        /// Calls the user-provided action, which returns parsed parameter to the user
        /// </summary>
        public override void TakeAction() => action(parsingResult);

        /// <inheritdoc/>
        public override void Restore()
        {
            parsingResult = default;
        }
    }

    /// <summary>
    /// Generic class for Multiple parameter options.
    /// </summary>
    /// <typeparam name="T"> Type of parameters</typeparam>
    internal class GenericMultipleParameterOption<T> : ParameterOption , IMultipleParameterOption , IPlainArgument
    {
        readonly Action<T[]?> action;
        readonly ParseMethodDelegateMultipleOption<T> parser;
        T[]? parsingResult;

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
            bool requiresParameter = false,
            char separator = ','
            )
        {
            this.parser = parser;
            this.action = action;
            this.IsMandatory = isMandatory;
            this.ShortSynonyms = shortSynonyms;
            this.LongSynonyms = longSynonyms;
            this.IsParameterRequired = requiresParameter;
            this.Separator = separator;

        }

        /// <summary>
        /// Parses the parameter.
        /// </summary>
        /// <param name="param">Parameter which followed the option.</param>
        /// <returns>True if parsing was successful, otherwise false.</returns> // TODO: maybe redo comments.
        public override bool ProcessParameter(string param)
        {
            var wasSuccessful = parser(param, out T[]? output,Separator);
            if (wasSuccessful) parsingResult = output;
            return wasSuccessful;
        }

        /// <inheritdoc/>
        public override void TakeAction()
        {
            action(parsingResult);
            parsingResult = null;
        }

        public override void Restore()
        {
            parsingResult = default;
        }
    }

}
