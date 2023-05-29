using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateConverter
{
    public class Program
    {
        public static void Main(string[] args)
        { 
            DateTimeParser parser = new DateTimeParser();

            parser.CreateDateTimeParser();

            args = new string[] { "--date=2018-12-31T14:45", "--format=%d.%m." };

            parser.Parse(args);
        }
    }
}
