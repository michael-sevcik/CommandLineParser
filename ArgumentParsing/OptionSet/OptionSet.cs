namespace ArgumentParsing.OptionSet
{
    using Option;
    internal static class DictionaryExtensions
    {
        public static bool ContainsOneOfKeys<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey[]? keys) where TKey : notnull
        {
            if (keys is null)
            {
                return false;
            }

            foreach (var shortSynonym in keys)
            {
                if (dictionary.ContainsKey(shortSynonym))
                {
                    return true;
                }
            }

            return false;
        }
    }

    /// <summary>
    /// The OptionSet class is used to define the set of options that can be parsed by the Parser class.
    /// </summary>
    /// 
    /// <Remarks>
    /// If two or more options with the same identifier are added to the OptionSet, Find method could behave nondeterministically.
    /// </Remarks>
    internal class OptionSet
    {
        private Dictionary<string, IOption> optionsByLongSynonyms = new();
        private Dictionary<char, IOption> optionsByShortSynonyms = new();
        private List<IOption> mandatoryOptions = new();

        /// <summary>
        /// Gets the list of options that were added to the OptionSet and are mandatory.
        /// </summary>
        public IReadOnlyList<IOption> MandatoryOptions => mandatoryOptions;


        /// <summary>
        /// Adds the option to the OptionSet.
        /// </summary>
        /// <param name="option"><see cref="Option"> instance to be added.</param>
        /// <returns>Returns true if there were no problems adding an option to the OptionSet,
        /// returns false if an error occurred, such as synonyms colliding with already added options, no short options and
        /// no long options at the same time and other undefined behavior.
        /// </returns>
        public bool Add(IOption option)
        {
            if (!IsOptionAddable(option))
            {
                return false;
            }

            RegisterOption(option);
            return true;
        }

        /// <summary>
        /// Looks for option that was added to the OptionSet and has the specified identifier.
        /// </summary>
        /// <param name="shortIdentifier">Short identifier of an option.</param>
        /// <returns>Returns an option if corresponding one was found, otherwise null.</returns>
        public IOption? Find(char shortIdentifier)
        => optionsByShortSynonyms.GetValueOrDefault(shortIdentifier);

        /// <summary>
        /// Looks for option that was added to the OptionSet and has the specified identifier.
        /// </summary>
        /// <param name="longIdentifier">Long identifier of an option.</param>
        /// <returns>Returns an option if corresponding one was found, otherwise null.</returns>
        public IOption? Find(string longIdentifier)
        => optionsByLongSynonyms.GetValueOrDefault(longIdentifier);

        private bool IsOptionAddable(IOption option)
        {
            if (optionsByShortSynonyms.ContainsOneOfKeys(option.ShortSynonyms)) 
            {
                return false;
            }

            if (optionsByLongSynonyms.ContainsOneOfKeys(option.LongSynonyms))
            {
                return false;
            }

            return true;
        }

        private void RegisterOption(IOption option)
        {
            if (option.IsMandatory)
            {
                mandatoryOptions.Add(option);
            }

            if (option.ShortSynonyms is not null)
            {
                foreach (var synonym in option.ShortSynonyms)
                {
                    optionsByShortSynonyms.Add(synonym, option);
                }
            }
            
            if (option.LongSynonyms is not null)
            {
                foreach (var synonym in option.LongSynonyms)
                {
                    optionsByLongSynonyms.Add(synonym, option);
                }
            }
        }

        
        
    }
}
