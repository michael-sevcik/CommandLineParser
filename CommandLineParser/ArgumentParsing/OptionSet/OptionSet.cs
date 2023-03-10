namespace ArgumentParsing.OptionSet
{
    using Option;

    /// <summary>
    /// The OptionSet class is used to define the set of options that can be parsed by the Parser class.
    /// </summary>
    /// 
    /// <Remarks>
    /// If two or more options with the same identifier are added to the OptionSet, Find method could behave nondeterministically.
    /// </Remarks>
    internal class OptionSet
    {
        private Dictionary<string, Option> longOptions = new();
        private Dictionary<char, Option> shortOptions = new();

        /// <summary>
        /// Adds the option to the OptionSet.
        /// </summary>
        /// <param name="option"><see cref="Option"> instance to be added.</param>
        /// <returns>Returns true if there were no problems adding an option to the OptionSet,
        /// returns false if an error occurred, such as synonyms colliding with already added options, no short options and
        /// no long options at the same time and other undefined behavior.
        /// </returns>
        public bool Add(Option option)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Looks for option that was added to the OptionSet and has the specified identifier.
        /// </summary>
        /// <param name="shortIdentifier">Short identifier of an option.</param>
        /// <returns>Returns an option if corresponding one was found, otherwise null.</returns>
        public Option? Find(char shortIdentifier)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Looks for option that was added to the OptionSet and has the specified identifier.
        /// </summary>
        /// <param name="longIdentifier">Long identifier of an option.</param>
        /// <returns>Returns an option if corresponding one was found, otherwise null.</returns>
        public Option? Find(string longIdentifier)
        {
            throw new NotImplementedException();
        }
    }
}
