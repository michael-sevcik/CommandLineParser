namespace ArgumentParsing
{
    /// <summary>
    /// The <see cref="OptionBuilder"/> Enables creating of options using fluent syntax.
    /// </summary>
    public class OptionBuilder
    {
        /// <summary>
        /// Allows you to start creating new option.
        /// </summary>
        /// <returns>Object that builds the desired option</returns>
        public static OptionBuilder CreateOption()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lets you define short synonyms for the option being built.
        /// </summary>
        /// <param name="shortSynonyms">Array of short synonyms accepted for option.</param>
        /// <returns>Object that builds the desired option</returns>
        public OptionBuilder WithShortSynonyms(params char[]? shortSynonyms)
        {
            throw new NotImplementedException(); 
        }

        /// <summary>
        /// Lets you define long synonyms for the option being built.
        /// </summary>
        /// <param name="longSynonyms">Array of long synonyms accepted for option.</param>
        /// <returns>Object that builds the desired option</returns>
        public OptionBuilder WithLongSynonyms(params string[]? longSynonyms)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calling this method will determine that the option will be Parametrized and take 0 to 1 parameters, as specified
        /// in the IParametrizedOption interface.
        /// </summary>
        /// <typeparam name="TArgument">Determines the type of which the parameters accepted by this option should be. Accepted types are bool, string, int, enum</typeparam>
        /// <param name="action">Specifies action, which is called, when option is present on command line. If isMandatory is set to true,
        /// action is called with one parameter of type T, else it is called with 0(null) to 1 parameters of type T according to the number of parameters
        /// provided on command line.
        /// </param>
        /// <returns>Object that builds the desired option</returns>
        /// <exception cref="InvalidOperationException">Thrown when wrong <typeparamref name="TArgument"/> is chosen. Accepted types are bool, string, int, enum.</exception>
        public OptionBuilder WithParametrizedAction<TArgument> (Action<TArgument?> action)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calling this method will determine that the option will be MultipleParameter and take 0 to unlimited parameters, as specified
        /// in the IMultipleParameterOption interface.
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Specifies whether the option requires at least one parameter present on
        /// command line.
        /// </summary>
        /// <returns>Object that builds the desired option</returns>
        public OptionBuilder RequiresParameter() 
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Specifies by what char should be possible arguments separated.
        /// </summary>
        /// <param name="separator">Separator character.</param>
        /// <returns>Object that builds the desired option.</returns>
        public OptionBuilder WithSeparator(char separator = ',')
        {
            throw new NotImplementedException();
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
        /// Adds the configured option to the specified Parser.
        /// </summary>
        /// <param name="parser">Parser object to which the configured option should be added. </param>
        /// <returns>Returns true if there were no problems adding the option,
        /// returns false if an error occurred, such as synonyms colliding with already added options, no short options and
        /// no long options at the same time and other undefined behavior.
        /// </returns>
        public bool RegisterOption(Parser parser)
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
            throw new NotImplementedException();
        }
    }
}
