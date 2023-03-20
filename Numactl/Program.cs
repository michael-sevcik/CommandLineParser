using System;
using System.Collections.Generic;
using ArgumentParsing;

namespace Numactl;

public class Program
{
    static void Main(string[] args)
    {
        ParsedArgs parsedArgs = new();
        Parser parser = BuildParser(parsedArgs);
        bool wasParsingSuccessful = parser.ParseCommandLine(args);
        if (!wasParsingSuccessful)
        {
            ParserError error = parser.Error!.Value;
            Console.Error.WriteLine("Error parsing command line: {0}", error.message);
            return;
        }

        if (!parsedArgs.WasChanged())
        {
            // no arguments given: print help
            Console.WriteLine(parser.GetHelpString());
            return;
        }
        if (parsedArgs.Hardware)
        {
            Numa.PrintHardwareConfiguration();
            return;
        }
        if (parsedArgs.Show)
        {
            Numa.PrintConfiguration(Numa.CurrentConfig);
            return;
        }
        if (parsedArgs.Config.HasConflict())
        {
            Console.Error.WriteLine("Error: conflicting NUMA node options");
            return;
        }

        Numa.RunCommand(parsedArgs.Command!, parsedArgs.Arguments.ToArray(), parsedArgs.Config);
    }

    /// <summary>Configure a parser object.</summary>
    /// <param name="args">Object where parsed values should be stored.</param>
    static Parser BuildParser(ParsedArgs args)
    {
        List<IPlainArgument> plainArguments = new();
        plainArguments.Add(IPlainArgument.CreatePlainArgument(
            (string? x) => args.Command = x!, isMandatory: true
        ));
        // WORKAROUND: library doesn't support a variable number of plain arguments
        int maxNumArguments = 10;
        for (int i = 0; i < maxNumArguments; i++)
        {
            plainArguments.Add(IPlainArgument.CreatePlainArgument(
                (string? x) => args.Arguments.Add(x!), isMandatory: false
            ));
        }

        // WORKAROUND: the signature of the constructor is likely wrong
        // using an explicit cast to silence the compiler error
        Parser parser = new Parser((IParametrizedOption[])plainArguments.ToArray());
        parser.SetPlainArgumentHelpString("Terminate option list.");

        var interleave = IParametrizedOption.CreateParameterOption(
            (string? x) => args.Config.Interleave = (x == null) ? null : new IdList(x),
            isMandatory: false, isParameterRequired: true, new[] { 'i' }, new[] { "interleave" }
        );
        interleave.SetHelpString("Interleave memory allocation across given nodes.");
        parser.Add(interleave);

        var preferred = IParametrizedOption.CreateParameterOption(
            (int? x) => args.Config.Preferred = x,
            isMandatory: false, isParameterRequired: true, new[] { 'p' }, new[] { "preferred" }
        );
        preferred.SetHelpString("Prefer memory allocations from given node.");
        parser.Add(preferred);

        var membind = IParametrizedOption.CreateParameterOption(
            (string? x) => args.Config.Membind = (x == null) ? null : new IdList(x),
            isMandatory: false, isParameterRequired: true, new[] { 'm' }, new[] { "membind" }
        );
        membind.SetHelpString("Allocate memory from given nodes only.");
        parser.Add(membind);

        var physcpubind = IParametrizedOption.CreateParameterOption(
            (string? x) => args.Config.Physcpubind = (x == null) ? null : new IdList(x),
            isMandatory: false, isParameterRequired: true, new[] { 'C' }, new[] { "physcpubind" }
        );
        physcpubind.SetHelpString("Run on given CPUs only.");
        parser.Add(physcpubind);

        var hardware = IOption.CreateNoParameterOption(
            () => args.Hardware = true,
            isMandatory: false, new[] { 'H' }, new[] { "hardware" }
        );
        hardware.SetHelpString("Print hardware configuration.");
        parser.Add(hardware);

        var show = IOption.CreateNoParameterOption(
            () => args.Show = true,
            isMandatory: false, new[] { 'S' }, new[] { "show" }
        );
        hardware.SetHelpString("Show current NUMA policy.");
        parser.Add(hardware);

        return parser;
    }

    /// <summary>Data class holding values parsed from the command line.</summary>
    class ParsedArgs
    {
        public NumaConfig Config;
        public bool Hardware;
        public bool Show;
        public string? Command;
        public List<string> Arguments = new();

        /// <summary>Checks whether any field values were changed.</summary>
        public bool WasChanged()
        {
            return (
                Config.Interleave != null || Config.Preferred != null ||
                Config.Membind != null || Config.Physcpubind != null ||
                Hardware || Show ||
                Command != null || Arguments.Count > 0
            );
        }
    }
}
