using ArgumentParsing;
namespace ExampleProgramHard
{

    public class Program
    {
        public static void Main(string[] args)
        {
            // we define what kind of plain arguments we want and what action to be called upon them

            //first plain argument is mandatory, i. e. must be present and is of type string
            Action<string?> firstPlainArgumentAction = (string? name) => Console.WriteLine($"Hi{name}");
            var firstPlainArgument = IPlainArgument.CreatePlainArgument(firstPlainArgumentAction, true);

            //second plain argument is not mandatory and can be omitted, is of type int
            //remember that all plain arguments that are not mandatory must come after all mandatory plain arguments
            Action<int?> secondPlainArgumentAction = (int? age) => Console.WriteLine($"Your age: {age}");
            var secondPlainArgument = IPlainArgument.CreatePlainArgument(secondPlainArgumentAction, false);

            var plainArguments = new IPlainArgument[] { firstPlainArgument, secondPlainArgument };

            //create a new Parser
            Parser parser = new Parser(plainArguments);

            //we can add helpString that will be shown next to the -- when -h is invoked
            parser.SetPlainArgumentHelpString("This will be shown next to --");

            //Create option builder object
            var optionBuilder = new OptionBuilder();


            //then we need to create desired options

            //these type of options are most common, no parameter needed and are not mandatory either

            optionBuilder.Reset()
                .WithShortSynonyms('A')
                .WithLongSynonyms("show-all")
                .WithAction(() => Console.WriteLine("Show all"))
                .WithHelpString("equivalent to -vET")
                .RegisterOption(parser);

            optionBuilder.Reset()
                .WithShortSynonyms('b')
                .WithLongSynonyms("number-nonblank")
                .WithAction(() => Console.WriteLine("Use non blank"))
                .WithHelpString("number nonempty output lines, overrides -n")
                .RegisterOption(parser);

            //look at the type of option we choose, after -e must follow one string parameter, we choose ParametrizedOption with string parameter type

            optionBuilder.Reset()
                .WithShortSynonyms('e')
                .WithParametrizedAction<string?>((string? fileName) => Console.WriteLine($"Is equal to{fileName}"))
                .RequiresParameter()
                .WithHelpString("adds equivalent file name")
                .RegisterOption(parser);

            optionBuilder.Reset()
                .WithShortSynonyms('E')
                .WithLongSynonyms("show-ends")
                .WithAction(() => Console.WriteLine("Show ends of lines"))
                .WithHelpString("display $ at end of each line")
                .RegisterOption(parser);

            optionBuilder.Reset()
                .WithShortSynonyms('n')
                .WithLongSynonyms("number")
                .WithAction(() => Console.WriteLine("Number the lines"))
                .WithHelpString("number all output lines")
                .RegisterOption(parser);

            //again as with the equalFileOption, but we need int parameter to follow
            optionBuilder.Reset()
                .WithShortSynonyms('s')
                .WithLongSynonyms ("squeeze-blank")
                .WithParametrizedAction<int?>((int? intensity) => Console.WriteLine($"Squeezing spaces to the intensity of {intensity}"))
                .RequiresParameter()
                .WithHelpString("suppress repeated empty output lines, int number must follow")
                .RegisterOption(parser);

            optionBuilder.Reset()
                .WithShortSynonyms('t')
                .WithAction(() => Console.WriteLine("TOption was present"))
                .WithHelpString("equivalent to -vT")
                .RegisterOption(parser);

            //now we finally parse the command line arguments
            parser.ParseCommandLine(args);

            /*
             * Some possible outputs:
             * args[] = { "-e" , "MyFile.txt","Joe" }
             * output: Is Equal to MyFile.txt
             *         Hi Joe.
             * 
             * args[] = { "--squeeze-blank=2", "-n", "Joe" , "60" }
             * output: Squeezing spaces to the intensity of 2
             *         Number the lines
             *         Hi Joe
             *         Your age: 60
            */
        }
    }
}