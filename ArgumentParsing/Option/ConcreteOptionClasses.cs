

namespace ArgumentParsing.Option
{

    /// <summary>
    /// Abstract base class for options more than one possible parameters.
    /// </summary>
    internal abstract class MultipleParameterOption : ParameterOption
    {
        /// <summary>
        /// Separator of multiple parameter entries. 
        /// </summary>
        public char Separator { get; init; }
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
            this.ShortSynonyms = shortSynonyms;
            this.LongSynonyms = longSynonyms;

        }

        /// <summary>
        /// Parses the parameter.
        /// </summary>
        /// Accepts any string parameter that is parseable by Int32.TryPase method.
        /// <param name="param">Corresponding parameter</param>
        /// <returns>True if parseable parameter was passed, otherwise false.</returns>
        public override bool ProcessParameter(string param)
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

        // TODO:
        public override void TakeAction()
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
            this.ShortSynonyms = shortSynonyms;
            this.LongSynonyms = longSynonyms;

        }

        /// <summary>
        /// Parses the parameter.
        /// </summary>
        /// Accepts any string parameter that is  after splitting by the delimiter parseable by Int32.TryPase method.
        /// <param name="param">Corresponding parameter</param>
        /// <returns>True if parseable parameter was passed, otherwise false.</returns>
        public override bool ProcessParameter(string param)
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

        public override void TakeAction()
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
        string? parameter;

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
            this.ShortSynonyms = shortSynonyms;
            this.LongSynonyms = longSynonyms;

        }

        /// <inheritdoc/>
        public override bool ProcessParameter(string param)
        {
            parameter = param;
            return true;
        }

        public override void TakeAction()
        {
            saveAction(parameter);
            parameter = null;
        }
    }       //TODO Michael

    /// <summary>
    /// This class represents option, which takes 0-1(based on isParameterRequired property.) to unlimited string options.
    /// </summary>
    internal class MultipleStringOption : MultipleParameterOption
    {
        Action<string[]?> saveAction;
        string[]? parameters;

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
            this.ShortSynonyms = shortSynonyms;
            this.LongSynonyms = longSynonyms;

        }

        /// <inheritdoc/>
        public override bool ProcessParameter(string parameter)
        {
            parameters = parameter.Split(',');
            return true;
        }

        public override void TakeAction()
        {
            saveAction(parameters);
            parameters = null;
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
            this.ShortSynonyms = shortSynonyms;
            this.LongSynonyms = longSynonyms;
        }

        /// <summary>
        /// This method is used for parsing command line parameter following the option.
        /// </summary>
        /// <param name="param">Parameter that follows this option.</param>
        /// <returns>Returns true if the param was one of the following: "1", "0", "true","false" ; ignorecase.
        /// Otherwise returns false.
        /// </returns>
        public override bool ProcessParameter(string param)
        {
            throw new NotImplementedException();
        }

        public override void TakeAction()
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
            this.ShortSynonyms = shortSynonyms;
            this.LongSynonyms = longSynonyms;
        }
        /// <summary>
        /// This method is used for parsing command line parameters following the option.
        /// </summary>
        /// <param name="param">One of the possibly following parameters.</param>
        /// <returns>Returns true if parsing was successful, that means the parameter was one of the following:  "1", "0", "true","false" ; ignorecase.
        /// Otherwise returns false.
        /// </returns>
        public override bool ProcessParameter(string param)
        {
            throw new NotImplementedException();
        }

        public override void TakeAction()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Represents an option, which takes 0 to 1 string arguments that matches one of the Enum's option names.
    /// </summary>
    /// <typeparam name="TEnum">Enum type is used to specify matchable strings.</typeparam>
    internal class EnumOption<TEnum> : ParameterOption where TEnum : struct, Enum             //TODO Michael // TODO: After TryParse, take action must proceed,
                                                                                              //otherwise, some nullable mechanism needs to be implemented.
    {
        Action<TEnum?> saveAction;
        TEnum? parsedValue;


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
            this.ShortSynonyms = shortSynonyms;
            this.LongSynonyms = longSynonyms;
        }

        /// <summary>
        /// This method is used for parsing command line parameter following the option.
        /// </summary>
        /// <param name="param">Parameter that follows this option.</param>
        /// <returns>Returns true if parsing was successful, that means the parameter matches one of the Enum´s option names.
        /// Otherwise returns false.
        /// </returns>
        public override bool ProcessParameter(string param)
        {
            var wasSuccessful = Enum.TryParse<TEnum>(param, out var result); // TODO: should we ignore upper case, lower case?

            if (wasSuccessful)
            {
                parsedValue = result;
            }

            return wasSuccessful;
        }

        public override void TakeAction()
        {
            saveAction(parsedValue);
            parsedValue = null;
        }
    }

    /// <summary>
    /// Represents an option which takes 0/1 (based on isParameterRequired) string arguments that matches
    /// one of the user-defined Enum's option names. To use this class you need to create your own Enum type,
    /// which specifies what kind of string arguments the option accepts.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    internal class MultipleEnumOption<TEnum> : MultipleParameterOption where TEnum : struct, Enum
    {
        Action<TEnum[]?> saveAction;

        TEnum[]? parsedEnums;

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
            this.ShortSynonyms = shortSynonyms;
            this.LongSynonyms = longSynonyms;
        }

        /// <summary>
        /// This method is used for parsing command line parameters following the option.
        /// </summary>
        /// <param name="param">one of the possibly following parameters.</param>
        /// <returns>Returns true if parsing was successful, that means the parameter matches one of the Enum´s option names.
        /// Otherwise returns false.
        /// </returns>
        public override bool ProcessParameter(string param)
        {
            var splittedParams = param.Split(Separator);
            parsedEnums = new TEnum[splittedParams.Length];
            for (int i = 0; i < splittedParams.Length; i++)
            {
                if (!Enum.TryParse(splittedParams[i], out parsedEnums[i]))
                {
                    parsedEnums = null;
                    return false;
                }
            }

            return true;
        }

        public override void TakeAction()
        {
            saveAction(parsedEnums);
            parsedEnums = null;
        }
    }           //TODO Michael

}
