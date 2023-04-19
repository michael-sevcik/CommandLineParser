using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgumentParsing.Option
{

    /// <summary>
    /// Abstract base class implementation.
    /// </summary>
    internal abstract class Option
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
        public char[]? shortSynonyms { get; init; }

        /// <summary>
        /// Array of string identifiers that represent the Option; e.g. "size" - long identifier is used as "--size on command-line".
        /// </summary>
        public string[]? longSynonyms { get; init; }

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
            this.shortSynonyms = shortSynonyms;
            this.longSynonyms = longSynonyms;

        }

        /// <inheritdoc/>
        public override bool IsParametrized => false;

        /// <summary>
        /// Method to call when option occurs in the parsed command-line.
        /// </summary>
        public void TakeAction()
        {
            action();
        }
    }

    /// <summary>
    /// Abstract class for parametrized options.
    /// </summary>
    internal abstract class ParameterOption : Option
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
        /// <returns>True if parsing was successful, otherwise false.</returns>
        public abstract bool TryParse(string param);
    }

    /// <summary>
    /// Abstract base class for options more than one possible parameters.
    /// </summary>
    internal abstract class MultipleParameterOption : ParameterOption
    {
        /// <summary>
        /// Delimits multiple parameters entries. 
        /// </summary>
        public char Delimiter { get; init; }
    }

    /// <summary>
    /// This class represents option, which takes 0 to 1 int parameters based on isParameterRequired property.
    /// </summary>
    internal class IntOption : ParameterOption
    {
        Action<int?> saveAction;
        public int? LowerBound { get; set; }
        public int? UpperBound { get; set; }
        /// <summary>
        /// Constructs an instance of <see cref="IntOption"/>.
        /// </summary>
        /// <param name="action">Specifies action, which should be executed with int option parameter or null if 
        /// mandatory is set to false.
        /// </param>
        /// <param name="isParameterRequired"> Specifies whether the option requires at least one parameter present on
        /// command line.
        /// </param>
        /// <param name="isMandatory"> Specifies whether option is mandatory i. e. must be present on command line.</param>
        /// <param name="shortSynonyms"> Specifies what kind of short synonyms should option represent (e.g. "-v").</param>
        /// <param name="longSynonyms"> Specifies what kind of long synonyms should option represent. (e.g. "--version")</param>

        public IntOption(Action<int?> action, bool isParameterRequired, bool isMandatory, char[]? shortSynonyms = null, string[]? longSynonyms = null)
        {
            saveAction = action;
            IsParameterRequired = IsParameterRequired;
            IsMandatory = isMandatory;
            this.shortSynonyms = shortSynonyms;
            this.longSynonyms = longSynonyms;

        }

        /// <summary>
        /// Parses the parameter.
        /// </summary>
        /// Accepts any string parameter that is parseable by Int32.TryPase method.
        /// <param name="param">Corresponding parameter</param>
        /// <returns>True if parseable parameter was passed, otherwise false.</returns>
        public override bool TryParse(string param)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Allows user set lower bound for Option-accepted parameter i. e. what 
        /// is the smallest number to accept.
        /// </summary>
        /// <param name="lowerBound">Smallest number to accept by option in its parameter. If no lower bound
        /// desired leave as null i. e. don't call this option at all.
        /// </param>     
        public void SetLowerBound(int? lowerBound)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Allows user set upper bound for Option-accepted parameter i. e. what 
        /// is the biggest number to accept.
        /// </summary>
        /// <param name="upperBound">Biggest number to accept by option in its parameter. If no biggest bound
        /// desired leave as null i. e. don't call this option at all.
        /// </param>     
        public void SetUpperBound(int? upperBound)
        {
            throw new NotImplementedException();
        }

    }       //TODO Michal

    /// <summary>
    /// This class represents option, which takes 0-1(based on isParameterRequired property.) to unlimited int parameters.
    /// </summary>
    internal class MultipleIntOption : MultipleParameterOption
    {
        Action<int[]?> saveAction;
        /// <summary>
        /// Creates an instance of <see cref="MultipleIntOption"/>.
        /// </summary>
        /// <param name="action">Specifies action, which should be executed with int parameters or null if
        /// there were no parameters present on command line.
        /// </param>
        /// <param name="isParameterRequired"> Specifies whether the option requires at least one parameter present on
        /// command line.
        /// </param>
        /// <param name="isMandatory"> Specifies whether option is mandatory i. e. must be present on command line.</param>
        /// <param name="shortSynonyms"> Specifies what kind of short synonyms should option represent (e.g. "-v").</param>
        /// <param name="longSynonyms"> Specifies what kind of long synonyms should option represent. (e.g. "--version")</param>
        public MultipleIntOption(Action<int[]?> action, bool isParameterRequired, bool isMandatory, char[]? shortSynonyms = null, string[]? longSynonyms = null)
        {
            saveAction = action;
            IsParameterRequired = IsParameterRequired;
            IsMandatory = isMandatory;
            this.shortSynonyms = shortSynonyms;
            this.longSynonyms = longSynonyms;

        }

        /// <summary>
        /// Parses the parameter.
        /// </summary>
        /// Accepts any string parameter that is  after splitting by the delimiter parseable by Int32.TryPase method.
        /// <param name="param">Corresponding parameter</param>
        /// <returns>True if parseable parameter was passed, otherwise false.</returns>
        public override bool TryParse(string param)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Allows user set lower bound for Option-accepted parameter i. e. what 
        /// is the smallest number to accept.
        /// </summary>
        /// <param name="lowerBound">Smallest number to accept by option in its parameter. If no lower bound
        /// desired leave as null i. e. don't call this option at all.
        /// </param>     
        public void SetLowerBound(int? lowerBound)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Allows user set upper bound for Option-accepted parameter i. e. what 
        /// is the biggest number to accept.
        /// </summary>
        /// <param name="upperBound">Biggest number to accept by option in its parameter. If no biggest bound
        /// desired leave as null i. e. don't call this option at all.
        /// </param>     
        /// <exception cref="NotImplementedException"></exception>
        public void SetUpperBound(int? upperBound)
        {
            throw new NotImplementedException();
        }
    }       //TODO Michal

    /// <summary>
    /// This class represents option, which takes 0 to 1 string arguments based on isParameterRequired property..
    /// </summary>
    internal class StringOption : ParameterOption
    {
        Action<string?> saveAction;

        /// <summary>
        /// Creates an instance of <see cref="StringOption"/>.
        /// </summary>
        /// <param name="action">Specifies action, which should be executed with string parameter
        /// or null if there was no parameter present on command line.
        /// </param>
        /// <param name="isParameterRequired"> Specifies whether the option requires at least one parameter present on
        /// command line.
        /// </param>
        /// <param name="isMandatory"> Specifies whether option is mandatory i. e. must be present on command line.</param>
        /// <param name="shortSynonyms"> Specifies what kind of short synonyms should option represent (e.g. "-v").</param>
        /// <param name="longSynonyms"> Specifies what kind of long synonyms should option represent. (e.g. "--version")</param>
        public StringOption(Action<string?> action, bool isParameterRequired, bool isMandatory, char[]? shortSynonyms = null, string[]? longSynonyms = null)
        {
            saveAction = action;
            IsParameterRequired = IsParameterRequired;
            IsMandatory = isMandatory;
            this.shortSynonyms = shortSynonyms;
            this.longSynonyms = longSynonyms;

        }

        /// <inheritdoc/>
        public override bool TryParse(string param)
        {
            throw new NotImplementedException();
        }

    }       //TODO Michael

    /// <summary>
    /// This class represents option, which takes 0-1(based on isParameterRequired property.) to unlimited string options.
    /// </summary>
    internal class MultipleStringOption : MultipleParameterOption
    {
        Action<string[]?> saveAction;
        /// <summary>
        /// Creates an instance of <see cref="MultipleStringOption"/>.
        /// </summary>
        /// <param name="action">Specifies action, which should be executed with string parameters or null if
        /// there were no parameters present on command line.
        /// </param>
        /// <param name="isParameterRequired"> Specifies whether the option requires at least one parameter present on
        /// command line.
        /// </param>
        /// <param name="isMandatory"> Specifies whether option is mandatory i. e. must be present on command line.</param>
        /// <param name="shortSynonyms"> Specifies what kind of short synonyms should option represent (e.g. "-v").</param>
        /// <param name="longSynonyms"> Specifies what kind of long synonyms should option represent. (e.g. "--version")</param>
        public MultipleStringOption(Action<string[]?> action, bool isParameterRequired, bool isMandatory, char[]? shortSynonyms = null, string[]? longSynonyms = null)
        {
            saveAction = action;
            IsParameterRequired = IsParameterRequired;
            IsMandatory = isMandatory;
            this.shortSynonyms = shortSynonyms;
            this.longSynonyms = longSynonyms;

        }

        /// <inheritdoc/>
        public override bool TryParse(string param)
        {
            throw new NotImplementedException();
        }
    }       //TODO Michael

    /// <summary>
    /// This class represents option, which takes 0 to 1 bool arguments based on isParameterRequired property.
    /// </summary>
    internal class BoolOption : ParameterOption
    {
        private Action<bool> saveAction;
        /// <summary>
        /// Creates an instance of <see cref="BoolOption"/>.
        /// </summary>
        /// <param name="action">Specifies action, which should be executed with bool parameter
        /// or null if there was no parameter present on command line.
        /// </param>
        /// <param name="isParameterRequired"> Specifies whether the option requires at least one parameter present on
        /// command line.
        /// </param>
        /// <param name="isMandatory"> Specifies whether option is mandatory i. e. must be present on command line.</param>
        /// <param name="shortSynonyms"> Specifies what kind of short synonyms should option represent (e.g. "-v").</param>
        /// <param name="longSynonyms"> Specifies what kind of long synonyms should option represent. (e.g. "--version")</param>
        public BoolOption(Action<bool> action, bool isParameterRequired, bool isMandatory, char[]? shortSynonyms = null, string[]? longSynonyms = null)
        {
            saveAction = action;
            IsParameterRequired = IsParameterRequired;
            IsMandatory = isMandatory;
            this.shortSynonyms = shortSynonyms;
            this.longSynonyms = longSynonyms;
        }

        /// <summary>
        /// This method is used for parsing command line parameter following the option.
        /// </summary>
        /// <param name="param">Parameter that follows this option.</param>
        /// <returns>Returns true if the param was one of the following: "1", "0", "true","false" ; ignorecase.
        /// Otherwise returns false.
        /// </returns>
        public override bool TryParse(string param)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// This class represents option, which takes 0-1(based on isParameterRequired field) to unlimited bool options.
    /// </summary>
    internal class MultipleBoolOption : ParameterOption
    {
        private Action<bool[]?> saveAction;
        /// <summary>
        /// Creates an instance of <see cref="MultipleBoolOption"/>.
        /// </summary>
        /// <param name="action">Specifies action, which should be executed with bool parameters or null if
        /// there were no parameters present on command line.
        /// </param>
        /// <param name="isParameterRequired"> Specifies whether the option requires at least one parameter present on
        /// command line.
        /// </param>
        /// <param name="isMandatory"> Specifies whether option is mandatory i. e. must be present on command line.</param>
        /// <param name="shortSynonyms"> Specifies what kind of short synonyms should option represent (e.g. "-v").</param>
        /// <param name="longSynonyms"> Specifies what kind of long synonyms should option represent. (e.g. "--version")</param>
        public MultipleBoolOption(Action<bool[]?> action, bool isParameterRequired, bool isMandatory, char[]? shortSynonyms = null, string[]? longSynonyms = null)
        {
            saveAction = action;
            IsParameterRequired = IsParameterRequired;
            IsMandatory = isMandatory;
            this.shortSynonyms = shortSynonyms;
            this.longSynonyms = longSynonyms;
        }
        /// <summary>
        /// This method is used for parsing command line parameters following the option.
        /// </summary>
        /// <param name="param">One of the possibly following parameters.</param>
        /// <returns>Returns true if parsing was successful, that means the parameter was one of the following:  "1", "0", "true","false" ; ignorecase.
        /// Otherwise returns false.
        /// </returns>
        public override bool TryParse(string param)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Represents an option, which takes 0 to 1 string arguments that matches one of the Enum's option names.
    /// </summary>
    /// <typeparam name="TEnum">Enum type is used to specify matchable strings.</typeparam>
    internal class EnumOption<TEnum> : ParameterOption where TEnum : struct, Enum             //TODO Michael
    {
        Action<TEnum?> saveAction;

        /// <summary>
        /// Creates an instance of <see cref="EnumOption{T}"/>.
        /// </summary>
        /// <param name="action">Specifies action, which should be executed with enum parameter or null if
        /// there were no parameters present on command line.</param>
        /// <param name="isParameterRequired"> Specifies whether the option requires at least one parameter present on
        /// command line.
        /// </param>
        /// <param name="isMandatory"> Specifies whether option is mandatory i. e. must be present on command line.</param>
        /// <param name="shortSynonyms"> Specifies what kind of short synonyms should option represent (e.g. "-v").</param>
        /// <param name="longSynonyms"> Specifies what kind of long synonyms should option represent. (e.g. "--version")</param>
        public EnumOption(Action<TEnum?> action, bool isParameterRequired, bool isMandatory, char[]? shortSynonyms = null, string[]? longSynonyms = null)
        {
            saveAction = action;
            IsParameterRequired = IsParameterRequired;
            IsMandatory = isMandatory;
            this.shortSynonyms = shortSynonyms;
            this.longSynonyms = longSynonyms;
        }

        /// <summary>
        /// This method is used for parsing command line parameter following the option.
        /// </summary>
        /// <param name="param">Parameter that follows this option.</param>
        /// <returns>Returns true if parsing was successful, that means the parameter matches one of the Enum´s option names.
        /// Otherwise returns false.
        /// </returns>
        public override bool TryParse(string param)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Represents an option which takes 0/1 (based on isParameterRequired) string arguments that matches
    /// one of the user-defined Enum's option names. To use this class you need to create your own Enum type,
    /// which specifies what kind of string arguments the option accepts.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    internal class MultipleEnumOption<TEnum> : ParameterOption where TEnum : Enum
    {
        Action<TEnum[]?> saveAction;

        /// <summary>
        /// Creates an instance of <see cref="MultipleEnumOption{T}"/>.
        /// </summary>
        /// <param name="action">Specifies action, which should be executed with enum parameters or null if
        /// there were no parameters present on command line.</param>
        /// <param name="isParameterRequired">Specifies whether the option requires at least one parameter present on
        /// command line.</param>
        /// <param name="isMandatory"> Specifies whether option is mandatory i. e. must be present on command line.</param>
        /// <param name="shortSynonyms"> Specifies what kind of short synonyms should option represent (e.g. "-v").</param>
        /// <param name="longSynonyms"> Specifies what kind of long synonyms should option represent. (e.g. "--version")</param>
        public MultipleEnumOption(Action<TEnum[]?> action, bool isParameterRequired, bool isMandatory, char[]? shortSynonyms = null, string[]? longSynonyms = null)
        {
            saveAction = action;
            IsParameterRequired = IsParameterRequired;
            IsMandatory = isMandatory;
            this.shortSynonyms = shortSynonyms;
            this.longSynonyms = longSynonyms;
        }

        /// <summary>
        /// This method is used for parsing command line parameters following the option.
        /// </summary>
        /// <param name="param">one of the possibly following parameters.</param>
        /// <returns>Returns true if parsing was successful, that means the parameter matches one of the Enum´s option names.
        /// Otherwise returns false.
        /// </returns>
        public override bool TryParse(string param)
        {
            throw new NotImplementedException();
        }
    }           //TODO Michael


}
