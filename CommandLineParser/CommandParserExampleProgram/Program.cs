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
            Action<Format?> formatAction = f => Console.WriteLine(f);
            EnumOption<Format> FormatOption = new(formatAction, true, false, new char[] { 'f' }, new string[] { "format" });
            FormatOption.SetHelpString("Specify output format, possibly overriding the format specified in the environment variable TIME.");

            Action markPortable = () => Console.WriteLine("was portable");
            NoParameterOption PortabilityOption = new(markPortable,false,new char[] {'p'},new string[] {"portability"});
            PortabilityOption.SetHelpString("Use the portable output format.");

            Action<String?> processOutputFile = (string? OutputFileName) => Console.WriteLine();
            StringOption outputFileOption = new(processOutputFile, true, false, new char[] { 'o' }, new string[] { "output" });
            outputFileOption.SetHelpString("Do not send the results to stderr, but overwrite the specified file.");

            Action markAppend = () => Console.WriteLine("was append");
            NoParameterOption appendOption = new(markAppend, false, new char[] { 'a' }, new string[] { "append" });
            appendOption.SetHelpString("(Used together with -o.) Do not overwrite but append.");


            Action verboseAction = () => Console.WriteLine("Verbose output");
            NoParameterOption verboseOutputOption = new(verboseAction, false, new char[] { 'V' }, new string[] { "verbose" });
            verboseOutputOption.SetHelpString("Give very verbose output about all the program knows about.");

            Action markHelp = () => Console.WriteLine("was Help");
            NoParameterOption helpOption = new(markAppend, false, null, new string[] { "help" });
            helpOption.SetHelpString("(Used together with -o.) Do not overwrite but append.");

            OptionSet optionSet = new();
            optionSet.Add(FormatOption);
            optionSet.Add(PortabilityOption);
            optionSet.Add(outputFileOption);
            optionSet.Add(appendOption);
            optionSet.Add(verboseOutputOption);
            optionSet.Add(helpOption);

            Parser parser = new(optionSet);

        }
    }
}