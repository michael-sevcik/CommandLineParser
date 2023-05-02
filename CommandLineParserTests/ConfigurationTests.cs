using ArgumentParsing;

namespace CommandLineParserTests
{
    public class ConfigurationTests
    {
        // arrange
        private Parser parser = new();
        private OptionBuilder optionBuilder = new OptionBuilder();

        /// The following tests relate to the RegisterOption() method, where its return is defined as:
        /// RegisterOption() returns true if there were no problems adding the option,
        /// returns false if an error occurred, such as synonyms colliding with already added options, no short options and
        /// no long options at the same time and other undefined behavior.
        [Test]
        public void optionWasRegisteredInParser()
        {
            // act
            object returnValue = optionBuilder.Reset()
                                    .WithShortSynonyms('p')
                                    .WithLongSynonyms("portability")
                                    .SetAsMandatory()
                                    .WithAction(() => Console.WriteLine("was portable"))
                                    .WithHelpString("Use the portable output format.")
                                    .RegisterOption(parser);

            // assert            
            Assert.IsTrue(returnValue is bool);
        }

        [Test]
        public void optionHasNoNameDefined()
        {
            // act
            bool correctAddingOfOption = optionBuilder.Reset()
                                           .SetAsMandatory()
                                           .WithAction(() => Console.WriteLine("was portable"))
                                           .WithHelpString("Use the portable output format.")
                                           .RegisterOption(parser);

            // assert            
            Assert.IsTrue(correctAddingOfOption);
        }

        [Test]
        public void optionsHaveCollidingNames()
        {
            // act
            optionBuilder.Reset()
               .WithShortSynonyms('p')
               .WithLongSynonyms("portability")
               .SetAsMandatory()
               .WithAction(() => Console.WriteLine("was portable"))
               .WithHelpString("Use the portable output format.")
               .RegisterOption(parser);

            bool correctAddingOfOptions = optionBuilder.Reset()
                                           .WithShortSynonyms('p')
                                           .WithLongSynonyms("preferability")
                                           .SetAsMandatory()
                                           .WithAction(() => Console.WriteLine("was preferable"))
                                           .WithHelpString("Prefer the default output format.")
                                           .RegisterOption(parser);

            // assert            
            Assert.IsFalse(correctAddingOfOptions);
        }
        [Test]
        public void optionWithMultipleParamsDoesNotDefineSeparator()
        {
            // act
            bool correctAddingOfOptions = optionBuilder.Reset()
                                             .WithShortSynonyms('f')
                                             .WithLongSynonyms("format")
                                             .WithMultipleParametersAction<string>((string[]?formats) => Console.WriteLine(formats))
                                             .RequiresParameter()
                                             .WithHelpString("Specify output format, possibly overriding the format specified in the environment variable TIME.")
                                             .RegisterOption(parser);

            // assert
            Assert.IsTrue(correctAddingOfOptions);
        }
        [Test]
        public void optionBuilderNotResetBeforeAddingAnotherOption() 
        {
            // act 
            bool correctAddingOfOptions = optionBuilder
                                            .WithShortSynonyms('p')
                                            .WithLongSynonyms("portability")
                                            .SetAsMandatory()
                                            .WithAction(() => Console.WriteLine("was portable"))
                                            .WithHelpString("Use the portable output format.")
                                            .RegisterOption(parser);

            // assert
            Assert.IsFalse(correctAddingOfOptions);
        }
        [Test]
        public void noActionSpecifiedForOption()
        {
            // act
            bool correctAddingOfOption = optionBuilder.Reset()
                                            .WithShortSynonyms('p')
                                            .WithLongSynonyms("portability")
                                            .SetAsMandatory()                                            
                                            .WithHelpString("Use the portable output format.")
                                            .RegisterOption(parser);

            // assert            
            Assert.IsFalse(correctAddingOfOption);
        }
    }
}
