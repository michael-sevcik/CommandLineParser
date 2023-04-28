using ArgumentParsing.Option;
using System.Threading.Channels;
using System.Xml;

namespace ArgumentParsing
{
    /// <summary>
    /// The <see cref="OptionBuilder"/> Enables creating of options using fluent syntax.
    /// </summary>
    public class OptionBuilder      //TODO Michal 
    {
        int? lowerBound, upperBound;
        bool requiresParameter = false;
        bool isMandatory = false;
        char[]? shortSynonyms = null;
        string[]? longSynonyms= null;
        char separator = ',';


        Type actionType;
        object action;
        (Type actionType, object action) actionTuple;

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
            return this;
        }

        /// <summary>
        /// Calling this method will determine that the option will be Parametrized and take 0 to 1 parameters, as specified
        /// in the IParametrizedOption interface. Also allows you to specify action to be called with the parsed parameter.
        /// </summary>
        /// <typeparam name="TArgument">Determines the type of which the parameters accepted by this option should be. Accepted types are bool, string, int, enum</typeparam>
        /// <param name="action">Specifies action, which is called, when option is present on command line. If isMandatory is set to true,
        /// action is called with one parameter of type T, else it is called with 0(null) to 1 parameters of type T according to the number of parameters
        /// provided on command line.
        /// </param>
        /// <returns>Object that builds the desired option</returns>
        /// <exception cref="InvalidOperationException">Thrown when wrong <typeparamref name="TArgument"/> is chosen. Accepted types are bool, string, int, enum.</exception>
        public OptionBuilder WithParametrizedAction<TArgument> (Action<TArgument?> action) // TODO: TArgument is from the supported types, throw exception.
        {
            actionTuple = (typeof(TArgument), action);
            //if (typeof(TArgument).IsSubclassOf(typeof(Enum)))
            if (typeof(TArgument).IsSubclassOf(typeof(Enum)))
            {

            }
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Specifies that the option is mandatory, i.e. must be present on command line.
        /// </summary>
        /// <returns>Object that builds the desired option</returns>
        /// <remarks>Options are optional by default.</remarks>
        public OptionBuilder SetAsMandatory()
        {
            this.isMandatory= true;
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
            this.separator= separator;
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lets you set the lower bound for the int option. Otherwise it has no effect on the option creation.
        /// </summary>
        /// <param name="lowerBound">Lower bound of int parameter</param>
        /// <returns>Object that builds the desired option.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public OptionBuilder WithLowerBound(int lowerBound)
        {
            this.lowerBound= lowerBound;
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
        public OptionBuilder WithBounds(int lowerBound,int upperBound)
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
            (Type actionType, object action) = actionTuple;
            if (actionTuple.actionType == typeof(Action))       //No parameter option
            {
                parser.Add( new NoParameterOption((Action)action,isMandatory,shortSynonyms,longSynonyms));                
            }
            else if (actionType == typeof(Action<string?>))     //String Option
            {
                parser.Add(new GenericClassParameterOption<string>(parseString, (Action<string?>)action, isMandatory, shortSynonyms, longSynonyms));
            }
            else if (actionType == typeof(Action<string[]?>))     //String Option multiple parameters
            {
                parser.Add(new GenericClassParameterOption<string[]>(parseStringMultipleParameters, (Action<string[]?>)action, isMandatory, shortSynonyms, longSynonyms,separator));
            }
            else if (actionType == typeof(Action<int?>))     //Int Option
            {
                parser.Add(new GenericStructParameterOption<int>(parseInt, (Action<int?>)action, isMandatory, shortSynonyms, longSynonyms));
            }
            else if (actionType == typeof(Action<int[]?>))     //Int Option multiple parameters
            {
                parser.Add(new GenericClassParameterOption<int[]>(parseIntMultipleParameters, (Action<int[]?>)action, isMandatory, shortSynonyms, longSynonyms, separator));
            }
            else if (actionType == typeof(Action<bool?>))     //Bool Option
            {
                parser.Add(new GenericStructParameterOption<bool>(parseBool, (Action<bool?>)action, isMandatory, shortSynonyms, longSynonyms));
            }
            else if (actionType == typeof(Action<bool[]?>))     //Bool Option multiple parameters
            {
                parser.Add(new GenericClassParameterOption<bool[]>(parseBoolMultipleParameters, (Action<bool[]?>)action, isMandatory, shortSynonyms, longSynonyms, separator));
            }

            else if (actionType == typeof(Action<Enum>))
            return false;
        }


        bool parseString(string input,out string output)
        {
            output = input;
            return true;
        }

        bool parseStringMultipleParameters(string input, out string[] output)
        {
            output = input.Split(separator);
            return true;
        }

        bool parseInt(string input, out int output) => Int32.TryParse(input, out output);
        
        bool parseIntMultipleParameters(string input, out int[] output)
        {
            var inputParts = input.Split(separator);
            var result = new int[inputParts.Length];
            output = new int[] { };
            for(int i = 0; i < result.Length; i++)
            {
                if (!Int32.TryParse(inputParts[i], out result[i])) return false;
            }
            output = result;
            return true;
        }

        bool parseBool(string input, out bool output) => bool.TryParse(input, out output);

        bool parseBoolMultipleParameters(string input, out bool[] output)
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


        bool parseEnumMultipleParameters(string input, out string output)
        {
            throw new NotImplementedException();
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
