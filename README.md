# Command Line Parser Library
The Command Line Parser Library offers an easy to use API for parsing command line arguments and storing option's parameters. 
It also allows you to display a help screen and a simple syntax error feedback.

## Easy Example to Get Started

Following program, when started with -g option or its long synonym --greeting, prints greeting from predefined set of greetings,
which is represented with `Greeting` enum.
```c#

enum Greeting {
    Hello,
    hi,
    ciao
}

void Main(string[] args) {

    // Create parser object
    Parser parser = new();

    //Create option builder object
    var optionBuilder = new OptionBuilder();

    //Create and register option
    optionBuilder.WithShortSynonyms('g')
                .WithLongSynonyms("greeting")
                .WithParametrizedAction<Greeting?>(greeting => Console.WriteLine(greeting))
                .RequiresParameter()
                .RegisterOption(parser);  


    // Parse command-line input.
    if (!parser.ParseCommandLine(args)) 
    {
        Console.WriteLine(parser.Error?.message);
    }
}
```

When the application is opened with arguments "-g Hello --greeting=ciao", the output is:
```
Hello
ciao
```

This was just a simple example, now we'll proceed to some key concepts.
## Key Concepts

The library uses a hierarchy of building components for setting up command-line parsing. The main component is [Parser](.Documentation/ArgumentParsing.Parser.md) class, to which [IOption](.Documentation/ArgumentParsing.IOption.md)s and [IPlainArgument](.Documentation/ArgumentParsing.IPlainArgument.md)s can be added. Basic types of options (non parametrized, bool, string, int and enum) can be created  with [OptionBuilder](.Documentation/ArgumentParsing.OptionBuilder.md) using fluent syntax. See the text bellow for details.

## More complex example

Lets say we want to parse the following options on command line.

```bash
   -A, --show-all
        equivalent to -vET

   -b, --number-nonblank
        number nonempty output lines, overrides -n

   -e   adds equivalent file name

   -E, --show-ends
        display $ at end of each line

   -n, --number
        number all output lines

   -s, --squeeze-blank
        suppress repeated empty output lines, int number must follow

   -t   equivalent to -vT

```
Following program will do that for us.
```c#
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
            Parser parser = new(plainArguments, "Here we can describe what the individual plain arguments do.");

            //Create option builder object
            var optionBuilder = new();


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
```
## Build instructions
For building you need to have installed .NET SDK version at least 7.0.

To build the library use `dotnet build ArgumentParsing.csproj` command.
Then reference the library in your project properties.

## Running a .NET application.
For running .NET applications that uses this library you need to have .NET runtime version at least 7.0 installed.

In order to run the application use `dotnet <built dll path>`.

## Generating Documentation

In order to generate documentation use `doxygen <config_file>` command. Config file for Doxygen is 
`Doxyfile` in the `t12-api-design` directory, with this configuration HTML documntation will be generated in the `html` directory.
