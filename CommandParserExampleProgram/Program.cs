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

            // Create options.
            Action<Format?> formatAction = format => Console.WriteLine(format);
            var FormatOption = IParametrizedOption.CreateParameterOption<Format?>(formatAction, false, true, new char[] { 'f' }, new string[] { "format" });
            FormatOption.SetHelpString("Specify output format, possibly overriding the format specified in the environment variable TIME.");

            Action markPortable = () => Console.WriteLine("was portable");
            var portabilityOption = IOption.CreateNoParameterOption(markPortable, false, new char[] { 'p' }, new string[] { "portability" });
            portabilityOption.SetHelpString("Use the portable output format.");

            Action<string?> processOutputFile = (string? OutputFileName) => Console.WriteLine();
            var outputFileOption = IParametrizedOption.CreateParameterOption(processOutputFile, false, true, new char[] { 'o' }, new string[] { "output" });
            outputFileOption.SetHelpString("Do not send the results to stderr, but overwrite the specified file.");

            Action markAppend = () => Console.WriteLine("was append");
            var appendOption = IOption.CreateNoParameterOption(markAppend, false, new char[] { 'a' }, new string[] { "append" });
            appendOption.SetHelpString("(Used together with -o.) Do not overwrite but append.");


            Action verboseAction = () => Console.WriteLine("Verbose output");
            var verboseOutputOption = IOption.CreateNoParameterOption(verboseAction, false, new char[] { 'V' }, new string[] { "verbose" });
            verboseOutputOption.SetHelpString("Give very verbose output about all the program knows about.");

            Action markHelp = () => Console.WriteLine("was Help");
            var helpOption = IOption.CreateNoParameterOption(markAppend, false, null, new string[] { "help" });
            helpOption.SetHelpString("(Used together with -o.) Do not overwrite but append.");

            
            // Create Parser.
            Parser parser = new();
            parser.SetPlainArgumentHelpString("Terminate option list.");

            // Fill parser with already created options.
            parser.Add(FormatOption);
            parser.Add(portabilityOption);
            parser.Add(outputFileOption);
            parser.Add(appendOption);
            parser.Add(verboseOutputOption);
            parser.Add(helpOption);

            // Parse command-line input.
            parser.ParseCommandLine(args);
        }
    }
}