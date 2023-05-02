﻿using ArgumentParsing.Option;
using System;
using System.Net;
using System.Reflection;
using System.Threading.Channels;
using System.Xml;

namespace ArgumentParsing
{
    /// <summary>
    /// The <see cref="OptionBuilder"/> Enables creating of options using fluent syntax.
    /// </summary>
    public class OptionBuilder      //TODO Michal 
    {
        int? lowerBound = null;
        int? upperBound = null;
        bool requiresParameter = false;
        bool isMandatory = false;
        char[]? shortSynonyms = null;
        string[]? longSynonyms = null;
        char separator = ',';
        string? helpString = null;


        bool multipleParameterOption = false;

        (Type actionType, Delegate action) actionTuple;
        Type multipleParameterOptionType;

        /// <summary>
        /// Lets you define short synonyms for the option being built.
        /// </summary>
        /// <param name="shortSynonyms">Array of short synonyms accepted for option.</param>
        /// <returns>Object that builds the desired option</returns>
        public OptionBuilder WithShortSynonyms(params char[]? shortSynonyms)
        {
            this.shortSynonyms = shortSynonyms;
            return this;
        }

        /// <summary>
        /// Lets you define short synonyms for the option being built.
        /// </summary>
        /// <param name="longSynonyms">Array of long synonyms accepted for option.</param>
        /// <returns>Object that builds the desired option</returns>
        public OptionBuilder WithLongSynonyms(params string[]? longSynonyms)
        {
            this.longSynonyms = longSynonyms;
            return this;
        }

        //Following three methods determine what kind of option (implementing one of the 3 option interfaces) it will be.
        //When called mutually, we take the last call as defining.

        /// <summary>
        /// Lets you define encapsulated method to call, when the option occurs in the parsed command.
        /// This determines that the option will be parameterless.
        /// </summary>
        /// <param name="action">method to call, when the option occurs in the parsed command</param>
        /// <returns>Object that builds the desired option</returns>
        public OptionBuilder WithAction(Action action)
        {
            actionTuple = (typeof(Action), action);
            multipleParameterOption = false;
            return this;
        }

        /// <summary>
        /// Calling this method will determine that the option will be Parametrized and take 0 to 1 parameters, as specified
        /// in the IParametrizedOption interface. Also allows you to specify action to be called with the parsed parameter.
        /// </summary>
        /// <typeparam name="TArgument">Determines the type of which the parameters accepted by this option should be. Accepted types are bool?, string, int?, Enum? and its descendants
        /// Also note that the TArgument should not be declared as nullable.</typeparam>
        /// <param name="action">Specifies action, which is called, when option is present on command line. If isMandatory is set to true,
        /// action is called with one parameter of type T, else it is called with 0(null) to 1 parameters of type T according to the number of parameters
        /// provided on command line.
        /// </param>
        /// <returns>Object that builds the desired option</returns>
        /// <exception cref="InvalidOperationException">Thrown when wrong <typeparamref name="TArgument"/> is chosen. Accepted types are bool?, string, int?,
        /// Enum? and its descendants.</exception>
        public OptionBuilder WithParametrizedAction<TArgument>(Action<TArgument?> action) // TODO: TArgument is from the supported types, throw exception.
        {
            if (!isOfSupportedTypesForParametrizedOption(typeof(TArgument)))throw new InvalidOperationException();
            actionTuple = (typeof(TArgument), action);
            multipleParameterOption = false;
            return this;
        }

        /// <summary>
        /// Calling this method will determine that the option will be MultipleParameter and take 0 to unlimited parameters, as specified
        /// in the IMultipleParameterOption interface. Also allows you to specify action to be called with the parsed parameter(s).
        /// </summary>
        /// <typeparam name="TArgument">Determines the type of which the parameters accepted by this option should be. Accepted types are bool, string, int, enum.</typeparam>
        /// <param name="action">Specifies action, which is called, when option is present on command line. It is called
        /// with 0(null) to unlimited number of parameters of type T according to the number of parameters provided on command line.
        /// If isMandatory is true, there must be at least one parameter present on command line.
        /// </param>
        /// <returns>Object that builds the desired option</returns>
        /// <exception cref="InvalidOperationException">Thrown when wrong <typeparamref name="TArgument"/> is chosen. Accepted types are bool, string, int, enum.</exception>
        public OptionBuilder WithMultipleParametersAction<TArgument>(Action<TArgument[]?> action)
        {
            if (!isOfSupportedTypesForMultipleParamOption(typeof(TArgument)))throw new InvalidOperationException();
            multipleParameterOption = true;
            actionTuple = (typeof(TArgument), action);
            multipleParameterOptionType = typeof(TArgument[]);
            return this;
        }

        /// <summary>
        /// Specifies that the option is mandatory, i.e. must be present on command line.
        /// </summary>
        /// <returns>Object that builds the desired option</returns>
        /// <remarks>Options are optional by default.</remarks>
        public OptionBuilder SetAsMandatory()
        {
            this.isMandatory = true;
            return this;
        }

        /// <summary>
        /// Specifies whether the option requires at least one parameter present on
        /// command line.
        /// </summary>
        /// <returns>Object that builds the desired option</returns>
        public OptionBuilder RequiresParameter()
        {
            this.requiresParameter = true;
            return this;
        }

        /// <summary>
        /// Specifies by what char should be possible arguments separated in multiple parameters options.
        /// </summary>
        /// <param name="separator">Separator character.</param>
        /// <returns>Object that builds the desired option.</returns>
        public OptionBuilder WithSeparator(char separator = ',')
        {
            this.separator = separator;
            return this;
        }

        /// <summary>
        /// Sets explanation string - string which is shown when someone uses -h/--help on command line.
        /// Empty explanation will be showed if user does not provide any helpString (does not call this method).
        /// </summary>
        /// <param name="helpString">Actual help string</param>
        /// <returns>Object that builds the desired option.</returns>
        public OptionBuilder WithHelpString(string helpString)
        {
            this.helpString = helpString;
            return this;
        }

        /// <summary>
        /// Lets you set the lower bound for the int option. Otherwise it has no effect on the option creation.
        /// </summary>
        /// <param name="lowerBound">Lower bound of int parameter</param>
        /// <returns>Object that builds the desired option.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public OptionBuilder WithLowerBound(int lowerBound)
        {
            this.lowerBound = lowerBound;
            return this;
        }
        /// <summary>
        /// Lets you set the upper bound for the int option. Otherwise it has no effect on the option creation.
        /// </summary>
        /// <param name="upperBound">Upper bound of int parameter</param>
        /// <returns>Object that builds the desired option.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public OptionBuilder WithUpperBound(int upperBound)
        {
            this.upperBound = upperBound;
            return this;
        }
        /// <summary>
        /// Lets you set the lower upper bound for the int option. Otherwise it has no effect on the option creation.
        /// </summary>
        /// <param name="lowerBound">Lower bound of int parameter.</param>
        /// <param name="upperBound">Upper bound of int parameter.</param>
        /// <returns>Object that builds the desired option.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public OptionBuilder WithBounds(int lowerBound, int upperBound)
        {
            this.lowerBound = lowerBound;
            this.upperBound = upperBound;
            return this;
        }

        /// <summary>
        /// Adds the configured option to the specified Parser.
        /// </summary>
        /// <param name="parser">Parser object to which the configured option should be added. </param>
        /// <returns>Returns true if there were no problems adding the option,
        /// returns false if an error occurred, such as synonyms colliding with already added options, no short options and
        /// no long options at the same time and other undefined behavior.
        /// </returns>
        public bool RegisterOption(Parser parser)
        {
            var option = CreateParticularOptionForRegistration();
            option.HelpString = helpString;
            return parser.Add(option);           
        }

        public IOption CreateParticularOptionForRegistration()
        {
            (Type actionType, Delegate actionObject) = actionTuple;
            if (actionType == typeof(Action))       //No parameter option
            {
                return new NoParameterOption((Action)actionObject, isMandatory, shortSynonyms, longSynonyms);
            }
            else if (actionType == typeof(string))     //String Options
            {
                if (!multipleParameterOption)
                    return new GenericParameterOption<string>(parseString, (Action<string?>)actionObject, isMandatory, shortSynonyms, longSynonyms,requiresParameter);

                return new GenericMultipleParameterOption<string>(
                    parseStringMultipleParameters,
                    (Action<string[]?>)actionObject,
                    isMandatory,
                    shortSynonyms,
                    longSynonyms,
                    requiresParameter,
                    separator
                    );
            }
            else if (actionType == typeof(int?) || actionType == typeof(int))     //Int Options
            {
                if (!multipleParameterOption)
                    return new GenericParameterOption<int?>(parseInt, (Action<int?>)actionObject, isMandatory, shortSynonyms, longSynonyms,requiresParameter);

                return new GenericMultipleParameterOption<int>(parseIntMultipleParameters, (Action<int[]?>)actionObject, isMandatory, shortSynonyms, longSynonyms, requiresParameter, separator);

            }
            else if (actionType == typeof(bool?) || actionType == typeof(bool))    //Bool Option
            {

                if (!multipleParameterOption)
                    return new GenericParameterOption<bool?>(parseBool, (Action<bool?>)actionObject, isMandatory, shortSynonyms, longSynonyms,requiresParameter);

                return new GenericMultipleParameterOption<bool>(
                    parseBoolMultipleParameters,
                    (Action<bool[]?>)actionObject,
                    isMandatory,
                    shortSynonyms,
                    longSynonyms,
                    requiresParameter,
                    separator
                    );
                    
            }
            else // we check that it is one of the supported types in action registration methods. -> Enum options
            {
                if (multipleParameterOption)
                {
                    var method = typeof(OptionBuilder).GetMethod(nameof(parseEnumMultipleParameters), BindingFlags.Static | BindingFlags.NonPublic)!.MakeGenericMethod(actionType); // TODO: Can this ever fail?
                    var parseMethodDelegateType = typeof(ParseMethodDelegateMultipleOption<>).MakeGenericType(actionType);
                    var generalDelegate = Delegate.CreateDelegate(parseMethodDelegateType, method);
                    var parseMethodDelegate = Convert.ChangeType(generalDelegate, parseMethodDelegateType);

                    var actionT = typeof(Action<>).MakeGenericType(multipleParameterOptionType);
                    var action = Convert.ChangeType(actionObject, actionT);

                    var instance = Activator.CreateInstance(
                        typeof(GenericMultipleParameterOption<>).MakeGenericType(actionType),
                        parseMethodDelegate,
                        action,
                        isMandatory,
                        shortSynonyms,
                        longSynonyms,
                        requiresParameter,
                        separator
                        
                        );
                    return (IOption)instance;
                }
                else
                {
                    var method = typeof(OptionBuilder).GetMethod(nameof(parseEnum), BindingFlags.Static | BindingFlags.NonPublic)!.MakeGenericMethod(actionType); // TODO: Can this ever fail?
                    var parseMethodDelegateType = typeof(ParseMethodDelegate<>).MakeGenericType(actionType);
                    var generalDelegate = Delegate.CreateDelegate(parseMethodDelegateType, method);
                    var parseMethodDelegate = Convert.ChangeType(generalDelegate, parseMethodDelegateType);
                    var actionT = typeof(Action<>).MakeGenericType(actionType);
                    var action = Convert.ChangeType(actionObject, actionT);

                    var instance = Activator.CreateInstance(
                        typeof(GenericParameterOption<>).MakeGenericType(actionType),
                        parseMethodDelegate,
                        action,
                        isMandatory,
                        shortSynonyms,
                        longSynonyms,
                        requiresParameter,
                        separator
                        );
                    return (IOption)instance;
                }
            }
            
        }
        static bool isOfSupportedTypesForParametrizedOption(Type type)
        {
            if(type == typeof(string) || type == typeof(int?) || type == typeof(bool?) ||
                (Nullable.GetUnderlyingType(type) is not null && Nullable.GetUnderlyingType(type).IsEnum))
                return true;
            return false;
        }
        static bool isOfSupportedTypesForMultipleParamOption(Type type)
        {
            if (type == typeof(int) || type == typeof(bool) || type == typeof(string) || type.IsSubclassOf(typeof(Enum))) return true;
            return false;
        }
        static bool parseString(string input, out string output)
        {
            output = input;
            return true;
        }

        static bool parseStringMultipleParameters(string input, out string[] output, char separator = ',')
        {
            output = input.Split(separator);
            return true;
        }

        public bool parseInt(string input, out int? output)
        {
            
            ParseMethodDelegate<int?> parseInt2 = (string input, out int? output) =>
            {
                int result;
                var success = Int32.TryParse(input, out result);
                var x = this.lowerBound;

                if(success)
                {
                    if (lowerBound is not null) success = result >= lowerBound;
                    if (upperBound is not null) success = success && (result <= upperBound);
                }
                
                if (!success) output = null; //make the output null if the operation was not succesful
                else output = result;
                return success;

            };
            
            return parseInt2(input, out output);  

        }

        /// <summary>
        /// Invalidates output of this lambda function however it is not necessary as we return false from that function.
        /// It just prevents some garbage to be stored in the output variable.
        /// </summary>
        /// <param name="output">Output parameter where the result of lambda function is stored.</param>
        /// <returns>False</returns>
        bool invalidateParseIntMultipleParametersOutput(out int[]? output)
        {
            output = null;
            return false;
        }
        bool parseIntMultipleParameters(string input, out int[]? output, char separator = ',')
        {
            ParseMethodDelegate<int[]?> parseIntMultipleParametersLambda = (string input, out int[]? output) =>
            {
                var inputParts = input.Split(separator);
                var result = new int[inputParts.Length];
                output = new int[] { };
                for (int i = 0; i < result.Length; i++)
                {
                    if (!Int32.TryParse(inputParts[i], out result[i])) return invalidateParseIntMultipleParametersOutput(out output);                   
                    else
                    {
                        if (lowerBound is not null && result[i] < lowerBound) return invalidateParseIntMultipleParametersOutput(out output);                      
                        if (upperBound is not null && result[i] > upperBound) return invalidateParseIntMultipleParametersOutput(out output);
                    }
                }
                output = result;
                return true;
            };
            return parseIntMultipleParametersLambda(input,out output); 
            
        }

        static bool parseBool(string input, out bool? output)
        {
            bool result;
            var success = bool.TryParse(input, out result);
            if (success)
            {
                output = result;
                return true;
            }
            output = null;
            return false;
        }

        static bool parseBoolMultipleParameters(string input, out bool[] output, char separator = ',')
        {
            var inputParts = input.Split(separator);
            var result = new bool[inputParts.Length];
            output = new bool[] { };
            for (int i = 0; i < result.Length; i++)
            {
                if (!bool.TryParse(inputParts[i], out result[i])) return false;
            }
            output = result;
            return true;
        }
        static bool parseEnum<TEnum>(string input, out TEnum output) 
        {
            var type = Nullable.GetUnderlyingType(typeof(TEnum));
            var names = Enum.GetNames(type);
            if (names.Contains(input))
            {
                output = (TEnum)Enum.Parse(type, input);
                return true;
            }
            output = default(TEnum);
            return false;
            

        }

        static bool parseEnumMultipleParameters<TEnum>(string input, out TEnum[] output, char separator = ',') 
        {

            var names = Enum.GetNames(typeof(TEnum));
            var splittedParams = input.Split(separator);
            output = new TEnum[splittedParams.Length];
            for (int i = 0; i < splittedParams.Length; i++)
            {
                if (names.Contains(splittedParams[i]))
                {
                    output[i] = (TEnum)Enum.Parse(typeof(TEnum), splittedParams[i]);

                }
                else return false;

            }
            return true;

        }
        static TEnum[] parseEnumMultipleParametersInternal<TEnum>(string input, char separator = ',') where TEnum : struct, Enum
        {
            
            var splittedParams = input.Split(separator);
            var output = new TEnum[splittedParams.Length];
            for (int i = 0; i < splittedParams.Length; i++)
            {
                if (!Enum.TryParse(splittedParams[i], out output[i])) throw new InvalidOperationException();

            }            
            return output;
        }

        /// <summary>
        /// Resets the current option configuration.
        /// </summary>
        /// <returns>
        /// Object that builds the desired option.
        /// </returns>
        public OptionBuilder Reset()
        {
            return new OptionBuilder();
        }
    }
}
