using ArgumentParsing;
using System.Text;

namespace DateConverter
{
    public class DateTimeParser
    {
        private string format;

        private DateTime dateTime = new DateTime();

        private Parser parser;

        public bool Parse(string[] args)
        {
            parser.ParseCommandLine(args);

            StringBuilder sb = new StringBuilder();

            foreach (char ch in format)
            {
                if (ch == 'm')
                {
                    sb.Append('M');
                }
                else
                {
                    sb.Append(ch);
                }
            }

            if (!parser.ParseCommandLine(args))
            {
                return false;
            }


            format = sb.ToString();

            Console.WriteLine(dateTime.ToString(format));

            return true;
        }

        public void CreateDateTimeParser()
        {
            this.parser = new Parser();

            var optionBuilder = new OptionBuilder();

            optionBuilder.WithShortSynonyms('d')
                .WithLongSynonyms("date")
                .SetAsMandatory()
                .WithParametrizedAction<string>(x =>
                {
                    if (!DateTime.TryParse(x, out dateTime))
                    {
                        Console.WriteLine("Invalid DateTime string");
                    }
                })
                .RegisterOption(parser);

            optionBuilder.WithShortSynonyms('f')
                .WithLongSynonyms("format")
                .WithParametrizedAction<string>(x =>
                {
                    this.format = x;
                }).RegisterOption(parser);
        }
    }
}