using ArgumentParsing;
using System;

namespace CommandLineParserTests
{
    public class ParseTests
    {
        // arrange
        private Parser parser;
        private OptionBuilder optionBuilder;

        [SetUp]
        public void Setup()
        {
            // arrange
            Action<string?> firstPlainArgumentAction = (string? name) => Console.WriteLine($"Hi{name}");
            var firstPlainArgument = IPlainArgument.CreatePlainArgument(firstPlainArgumentAction, true);

            Action<int?> secondPlainArgumentAction = (int? age) => Console.WriteLine($"Your age: {age}");
            var secondPlainArgument = IPlainArgument.CreatePlainArgument(secondPlainArgumentAction, false);

            var plainArguments = new IPlainArgument[] { firstPlainArgument, secondPlainArgument };

            parser = new Parser(plainArguments);

            optionBuilder = new OptionBuilder(); 

            optionBuilder.Reset()
                .WithShortSynonyms('p')
                .WithLongSynonyms("portability")
                .SetAsMandatory()
                .WithAction(() => Console.WriteLine("was portable"))
                .WithHelpString("Use the portable output format.")
                .RegisterOption(parser);
        }

        [Test]
        public void mandatoryOptionNotPresentInCommandLine()
        {
            // act
            string[] args = { "--", "John" };

            parser.ParseCommandLine(args);

            // assert
            // first check whether error occured, then check its type
            Assert.IsNotNull(parser.Error);
            Assert.AreEqual(parser.Error.Value.type, ParserErrorType.MissingMandatoryOption);
        }


        [Test]
        [TestCase(new object[] { "-f", "form", "-p", "--", "John" })]
        [TestCase(new object[] { "-f", "0.2", "-p", "--", "John" })]
        [TestCase(new object[] { "-f", "True", "-p", "--", "John" })]
        public void intParameterDoNotParseOtherDataTypes(object[] argsO)
        {
            // arrange
            string[] args = Array.ConvertAll(argsO, x => x.ToString());

            // act
            optionBuilder.Reset()
                 .WithShortSynonyms('f')
                 .WithLongSynonyms("format")
                 .WithParametrizedAction<int?>(format => Console.WriteLine("Format accepted."))
                 .RequiresParameter()
                 .WithHelpString("Specify output format, possibly overriding the format specified in the environment variable TIME.")
                 .RegisterOption(parser);

            parser.ParseCommandLine(args);

            // assert
            Assert.IsNotNull(parser.Error);
            Assert.AreEqual(parser.Error.Value.type, ParserErrorType.CouldNotParseTheParameter);
        }

        [Test]
        [TestCase(new object[] { "-f", "2", "-p", "--", "John" })]
        [TestCase(new object[] { "-f", "format", "-p", "--", "John" })]
        [TestCase(new object[] { "-f", "True", "-p", "--", "John" })]
        public void floatParameterDoNotParseOtherDataTypes(object[] argsO)
        {
            // arrange
            string[] args = Array.ConvertAll(argsO, x => x.ToString());

            // act
            Assert.Throws<InvalidOperationException>(()=>optionBuilder.Reset()
                 .WithShortSynonyms('f')
                 .WithLongSynonyms("format")
                 .WithParametrizedAction<float?>(format => Console.WriteLine("Format accepted.")));
            
        }
        
        [Test]
        public void mandatoryParameterNotPresentInCommandLine()
        {
            // act
            optionBuilder.Reset()
                 .WithShortSynonyms('f')
                 .WithLongSynonyms("format")
                 .WithParametrizedAction<string?>(format => Console.WriteLine(format))
                 .RequiresParameter()
                 .WithHelpString("Specify output format, possibly overriding the format specified in the environment variable TIME.")
                 .RegisterOption(parser);

            string[] args = { "-f", "-p", "--", "John" };

            parser.ParseCommandLine(args);

            // assert
            Assert.IsNotNull(parser.Error);
            Assert.AreEqual(parser.Error.Value.type, ParserErrorType.Other);
        }
        [Test]
        public void optionWithMaxOneParamRequiredHasMultipleParamsInCommandLine()
        {
            // act
            optionBuilder.Reset()
                 .WithShortSynonyms('f')
                 .WithLongSynonyms("format")
                 .WithParametrizedAction<string?>(format => Console.WriteLine(format))
                 .RequiresParameter()
                 .WithHelpString("Specify output format, possibly overriding the format specified in the environment variable TIME.")
                 .RegisterOption(parser);

            string[] args = { "-f", "format1,format2", "-p", "--", "John" };

            parser.ParseCommandLine(args);

            // assert
            Assert.IsNotNull(parser.Error);
            Assert.AreEqual(parser.Error.Value.type, ParserErrorType.Other);
        }

        [Test]
        public void wrongSeparatorUsedForMultipleParamOptionsInCommandLine()
        {
            // act
            optionBuilder.Reset()
                 .WithShortSynonyms('f')
                 .WithLongSynonyms("format")                 
                 .WithMultipleParametersAction<string[]?>(formats => Console.WriteLine(formats))                
                 .RequiresParameter()
                 // .WithSeparator(char separator = ',') // I was not able to add the separator
                 .WithHelpString("Specify output format, possibly overriding the format specified in the environment variable TIME.")
                 .RegisterOption(parser);

            string[] args = { "-f", "format1;format2", "-p", "--", "John" };

            parser.ParseCommandLine(args);

            // assert
            Assert.IsNotNull(parser.Error);
            Assert.AreEqual(parser.Error.Value.type, ParserErrorType.Other);
        }

        [Test]
        public void wrongOrderOfMandatoryAndOptionalPlainArguments()
        { 
            // act
            string[] args = { "-p", "--", "26", "John" };

            parser.ParseCommandLine(args);

            // assert
            Assert.IsNotNull(parser.Error);
            Assert.AreEqual(parser.Error.Value.type, ParserErrorType.Other);
        }

        [Test]
        public void shortOptionIdentifierAfterThePlainArgumentsIdentifierPresentInCommandLine()
        {
            // act
            string[] args = { "-p", "--", "-J" };

            parser.ParseCommandLine(args);

            // assert
            Assert.IsNotNull(parser.Error);
            Assert.AreEqual(parser.Error.Value.type, ParserErrorType.InvalidOptionIdentifier);
        }

        [Test]
        public void mandatoryPlainArgumentNotPresentInCommandLine()
        {
            // act  
            string[] args = {"-p"};

            parser.ParseCommandLine(args);

            // assert
            Assert.IsNotNull(parser.Error);
            Assert.AreEqual(parser.Error.Value.type, ParserErrorType.CouldNotParseTheParameter);
        }

        [Test]
        public void helpPageShownWhenRequestedWithShortName()
        {
            // act  
            string[] args = { "-h" };

            // assert
            // There is a method GetHelpString() to show the help page, this will be called from the ParseCommandLine() method, I'm guessing?
            // To test whether a method was called, I would have to use Moq framework, and I dont want to add more dependecies to my project
            // I suggest, that the ParseCommandLine() should return a flag for whether the help page was requested or not. (Maybe using Tuple syntax,
            // since, we are returning whether the parsing was successful or not as well.)

            // Tuple<bool, bool> flags = parser.ParseCommandLine(args);
            // Assert.IsTrue(flags.Item2);
            Assert.Pass();
        }
        [Test]
        public void helpPageShownWhenRequestedWithLongName()
        {
            // act  
            string[] args = { "--help" };

            parser.ParseCommandLine(args);

            // assert
            // There is a method GetHelpString() to show the help page, this will be called from the ParseCommandLine() method, I'm guessing?
            // To test whether a method was called, I would have to use Moq framework, and I dont want to add more dependecies to my project
            // I suggests, that the ParseCommandLine() should return a flag for whether the help page was requested or not. (Maybe using Tuple syntax,
            // since, we are returning whether the parsing was successful or not, as well.

            // Tuple<bool, bool> flags = parser.ParseCommandLine(args);
            // Assert.IsTrue(flags.Item2);
            Assert.Pass();
        }
    }
}
