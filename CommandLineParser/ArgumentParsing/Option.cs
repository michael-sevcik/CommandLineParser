using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgumentParsing
{
    /// <summary>
    /// Abstract base class implementation.
    /// </summary>
    public abstract class Option
    {


        /// <summary>
        /// Determines whether a given option must occur in a parsed command. 
        /// </summary>
        public bool IsMandatory { get; init; }

        /// <summary>
        /// Determines whether a given option may, or must have parameters.
        /// </summary>
        public bool IsParametrized { get; init; }

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
        /// <exception cref="NotImplementedException"></exception>
        public bool SetHelpString(string helpString)
        {
            throw new NotImplementedException();
        }

    }

    /// <summary>
    /// Instance class of options with no parameters.
    /// </summary>
    public class NoParameterOption : Option
    {
        Action action;

        /// <summary>
        /// Constructs instance of <see cref="NoParameterOption"/>.
        /// </summary>
        /// <param name="isMandatory">Determines whether a given option must occur in a parsed command.</param>
        /// <param name="action">Encapsulated method to call, when the option occurs in the parsed command.</param>
        public NoParameterOption(bool isMandatory, Action action)
        {
            this.IsMandatory = isMandatory;
            this.action = action;
        }

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
    public abstract class ParameterOption : Option
    {
        /// <summary>
        /// Determines whether an option requires parameter.
        /// </summary>
        public bool IsParameterRequired { get; init; }

        /// <summary>
        /// Parses the parameter.
        /// </summary>
        /// <param name="param">Parameter which followed the option.</param>
        /// <returns>True if parsing was successful, otherwise false.</returns>
        public abstract bool TryParse(string param);
    }

    
    public abstract class MultipleParameterOption : ParameterOption
    {
        /// <summary>
        /// Delimits multiple parameters entries. 
        /// </summary>
        public char Delimiter { get; init; }
    }


    /// <summary>
    /// This class represents option, which takes 0 to 1 int parameters based on isParameterRequired property.
    /// </summary>
    public class IntOption : ParameterOption
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


        public IntOption(Action<int?> action, bool isParameterRequired, bool isMandatory, char[] shortSynonyms, string[] longSynonyms)
        {
            this.saveAction = action;
            this.IsParameterRequired = IsParameterRequired;
            this.IsMandatory = isMandatory;
            this.shortSynonyms = shortSynonyms;
            this.longSynonyms = longSynonyms;

        }

        /// <summary>
        /// Parses the parameter.
        /// </summary>
        /// Accepts any string param that is parseable by Int32.TryPase method.
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
        /// <exception cref="NotImplementedException"></exception>
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
        public void SetUpperBound (int? upperBound)
        {
            throw new NotImplementedException();
        }

    }


    /// <summary>
    /// This class represents option, which takes 0-1(based on isParameterRequired property.) to unlimited int parameters.
    /// </summary>
    public class MultipleIntOption : MultipleParameterOption
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
        public MultipleIntOption(Action<int[]?> action, bool isParameterRequired, bool isMandatory,char[] shortSynonyms, string[] longSynonyms)
        {
            this.saveAction = action;
            this.IsParameterRequired = IsParameterRequired;
            this.IsMandatory = isMandatory;
            this.shortSynonyms = shortSynonyms;
            this.longSynonyms = longSynonyms;

        }

        /// <inheritdoc/>
        public override bool TryParse(string param)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Allows user set lower bound for Option-accepted parameter i. e. what 
        /// is the smallest number to accept.
        /// </summary>
        /// <param name="lowerBound">Smallest number to accept by option in its parameter. If no lower bound
        /// desired leave as null i. e. dont call this option at all.
        /// </param>     
        /// <exception cref="NotImplementedException"></exception>
        public void SetLowerBound(int? lowerBound)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Allows user set upper bound for Option-accepted parameter i. e. what 
        /// is the biggest number to accept.
        /// </summary>
        /// <param name="upperBound">Biggest number to accept by option in its parameter. If no biggest bound
        /// desired leave as null i. e. dont call this option at all.
        /// </param>     
        /// <exception cref="NotImplementedException"></exception>
        public void SetUpperBound(int? upperBound)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// This class represents option, which takes 0 to 1 string arguments based on isParameterRequired property..
    /// </summary>
    public class StringOption : ParameterOption
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
        public StringOption(Action<string?> action, bool isParameterRequired, bool isMandatory, char[] shortSynonyms, string[] longSynonyms)
        {
            this.saveAction = action;
            this.IsParameterRequired = IsParameterRequired;
            this.IsMandatory = isMandatory;
            this.shortSynonyms = shortSynonyms;
            this.longSynonyms = longSynonyms;

        }

        /// <inheritdoc/>
        public override bool TryParse(string param)
        {
            throw new NotImplementedException();
        }

    }
    /// <summary>
    /// This class represents option, which takes 0-1(based on isParameterRequired property.) to unlimited string options.
    /// </summary>
    public class MultipleStringOption : MultipleParameterOption
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
        public MultipleStringOption(Action<string[]?> action, bool isParameterRequired, bool isMandatory, char[] shortSynonyms, string[] longSynonyms)
        {
            this.saveAction = action;
            this.IsParameterRequired = IsParameterRequired;
            this.IsMandatory = isMandatory;
            this.shortSynonyms = shortSynonyms;
            this.longSynonyms = longSynonyms;

        }

        /// <inheritdoc/>
        public override bool TryParse(string param)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// This class represents option, which takes 0 to 1 bool arguments based on isParameterRequired property.
    /// </summary>
    class BoolOption : ParameterOption
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
        public BoolOption(Action<bool> action, bool isParameterRequired, bool isMandatory, char[] shortSynonyms, string[] longSynonyms)
        {
            this.saveAction = action;
            this.IsParameterRequired = IsParameterRequired;
            this.IsMandatory = isMandatory;
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
    class MultipleBoolOption : ParameterOption
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
        public MultipleBoolOption(Action<bool[]?> action, bool isParameterRequired, bool isMandatory, char[] shortSynonyms, string[] longSynonyms)
        {
            this.saveAction = action;
            this.IsParameterRequired = IsParameterRequired;
            this.IsMandatory = isMandatory;
            this.shortSynonyms = shortSynonyms;
            this.longSynonyms = longSynonyms;
        }
        /// <summary>
        /// This method is used for parsing command line parameters following the option.
        /// </summary>
        /// <param name="param">One of the possibly following parameters.</param>
        /// <returns>Returns true if parsing was succesful, that means the parameter was one of the following:  "1", "0", "true","false" ; ignorecase.
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
    /// <typeparam name="T">Enum type is used to specify matchable strings.</typeparam>
    public class EnumOption <T> : ParameterOption where T : Enum
    {
        Action<T?> saveAction;

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
        public EnumOption(Action<T?> action, bool isParameterRequired, bool isMandatory, char[] shortSynonyms, string[] longSynonyms)
        {
            this.saveAction = action;
            this.IsParameterRequired = IsParameterRequired;
            this.IsMandatory = isMandatory;
            this.shortSynonyms = shortSynonyms;
            this.longSynonyms = longSynonyms;
        }

        /// <summary>
        /// This method is used for parsing command line parameter following the option.
        /// </summary>
        /// <param name="param">Parameter that follows this option.</param>
        /// <returns>Returns true if parsing was succesful, that means the parameter matches one of the Enum´s option names.
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
    /// <typeparam name="T"></typeparam>
    public class MultipleEnumOption<T> : ParameterOption where T : Enum
    {
        Action<T[]?> saveAction;

        // TODO: Consider creating a synonyms struct.
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
        public MultipleEnumOption(Action<T[]?> action, bool isParameterRequired, bool isMandatory, char[] shortSynonyms, string[] longSynonyms)
        {
            this.saveAction = action;
            this.IsParameterRequired = IsParameterRequired;
            this.IsMandatory = isMandatory;
            this.shortSynonyms = shortSynonyms;
            this.longSynonyms = longSynonyms;
        }

        /// <summary>
        /// This method is used for parsing command line parameters following the option.
        /// </summary>
        /// <param name="param">one of the possibly following parameters.</param>
        /// <returns>Returns true if parsing was succesful, that means the parameter matches one of the Enum´s option names.
        /// Otherwise returns false.
        /// </returns>
        public override bool TryParse(string param)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// This class enables users create an instance of class based on their preferences
    /// i. e. if they want required/optional NoParameterOption/ParameterOption/MultipleParameterOption 
    /// and with the parametrized options of which types should parameters be (int,string,bool). Enum option must be created
    /// the casual way.
    /// </summary>
    public static class OptionFactory
    {
        /// <summary>
        /// Creates desired instance of Option, based on user preferences defined in OptionSpecifics parameter.
        /// </summary> 
        /// <param name="OptionSpecifics">
        /// Specifies what kind of option user desires. Enter string in following format:
        /// add ":m" if you want the option to be mandatory, otherwise it will be not. 
        /// add ":p" if you want the option to be parametrized, otherwise it will be not.
        /// if :p is present user must add one of the following: ":int" ":string" ":bool" which specifies what kind of 
        /// parameters should option take.
        /// if :p is present user can add ":r" to specify that the option must take at least one parameter,
        /// otherwise 0 parameter is valid for an option.
        /// </param>
        /// <returns>Returns adequate class for users desire based on the string that he provided.</returns>
        public static Option CreateOption(string OptionSpecifics)
        {
            throw new NotImplementedException();
        }

        
    }
}
