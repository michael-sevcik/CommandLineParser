using ArgumentParsing;

namespace datetime_converter
{
    public record Data
    {
        public DateTime dateTime { get; set; }
        public string? format { get; set; }
    }
    internal class Program
    {
        private static void registerOptionDate(OptionBuilder optionBuilder, Parser parser, Data data)
        {
            optionBuilder.Reset()
                        .WithLongSynonyms("date")
                        .WithShortSynonyms('d')
                        .WithHelpString("Define the date and/or time.")
                        .SetAsMandatory()
                        .RequiresParameter()
                        .WithParametrizedAction<string>(date => data.dateTime = DateTime.Parse(date))
                        .RegisterOption(parser);
        }

        private static void registerOptionFormat(OptionBuilder optionBuilder, Parser parser, Data data)
        {
            optionBuilder.Reset()
                        .WithLongSynonyms("format")
                        .WithShortSynonyms('f')
                        .WithHelpString("Set the format of output date and/or time.")
                        .SetAsMandatory()
                        .RequiresParameter()
                        .WithParametrizedAction<string>(format => data.format = format)
                        .RegisterOption(parser);
        }

        private static void processOutput(Data data)
        {
            var result = data.dateTime.ToString(data.format);
            Console.WriteLine(result);
        }

        static void Main(string[] args)
        {
            var parser = new Parser();
            var optionBuilder = new OptionBuilder();
            var data = new Data();

            registerOptionDate(optionBuilder, parser, data);
            registerOptionFormat(optionBuilder, parser, data);

            parser.ParseCommandLine(args);

            if (args.Length == 0 || (args.Length == 1 && (args[0] == "-h" || args[0] == "--help")))
            {
                Console.WriteLine(parser.GetHelpString());
            }
            else processOutput(data);
        }
    }
}