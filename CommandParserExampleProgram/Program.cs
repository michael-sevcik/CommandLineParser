using ArgumentParsing;
namespace ExampleProgram
{
    enum Format : byte
    {
        format1,
        format2,
        format3
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create Parser.
            Parser parser = new();
            
            //Create option builder object
            var optionBuilder = new OptionBuilder();

            // Create options via cool builder.

            optionBuilder
                .WithShortSynonyms('f')
                .WithLongSynonyms("format")
                .WithParametrizedAction<Format?>(format => Console.WriteLine(format))
                .RequiresParameter()
                .WithHelpString("Specify output format, possibly overriding the format specified in the environment variable TIME.")
                .RegisterOption(parser);             

            optionBuilder.Reset()
                .WithShortSynonyms('p')
                .WithLongSynonyms("portability")
                .WithAction(() => Console.WriteLine("was portable"))
                .WithHelpString("Use the portable output format.")
                .RegisterOption(parser);


            optionBuilder.Reset()
                .WithShortSynonyms('o')
                .WithLongSynonyms("output")
                .WithParametrizedAction<string?>((string? OutputFileName) => Console.WriteLine())
                .RequiresParameter()
                .WithHelpString("Do not send the results to stderr, but overwrite the specified file.")
                .RegisterOption(parser);

            Action markAppend = () => Console.WriteLine("was append");
            optionBuilder.Reset()
                .WithShortSynonyms('a')
                .WithLongSynonyms("append")
                .WithAction(() => Console.WriteLine("was append"))
                .WithHelpString("(Used together with -o.) Do not overwrite but append.")
                .RegisterOption(parser);

            Action verboseAction = () => Console.WriteLine("Verbose output");
            optionBuilder.Reset()
                .WithShortSynonyms('V')
                .WithLongSynonyms("verbose")
                .WithAction(verboseAction)
                .WithHelpString("Give very verbose output about all the program knows about.")
                .RegisterOption(parser);

            Action markHelp = () => Console.WriteLine("was Helped");
            optionBuilder.Reset()
                .WithLongSynonyms("help")
                .WithAction(markHelp)
                .WithHelpString("(Used together with -o.) Do not overwrite but append.")
                .RegisterOption(parser);

            // Parse command-line input.
            parser.ParseCommandLine(args);

            if (!parser.ParseCommandLine(args))
            {
                Console.WriteLine(parser.Error?.message);
            }
        }
    }
}