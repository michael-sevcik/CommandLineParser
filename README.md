# Command Line Parser Library
The Command Line Parser Library offers an easy to use API for parsing command line arguments and storing option's parameters. 
It also allows you to display a help screen and a simple syntax error feedback.

## Easy Example to Get Started

Following program, when started with -g option or its long synonym --greeting, prints greeting from predefined set of greetings,
which is represented with `Greeting` enum.
```C#

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

The library uses a hierarchy of building components for setting up command-line parsing. The main component is `Parser` class, to which `IOption`s
can be added. Basic types of options (non parametrized, bool, string, int and enum) can be created  with `OptionBuilder` using fluent syntax. See the text bellow for details.

### OptionBuilder
Object that enables creating of options using fluent syntax. To specify a option's configuration use the following listed methods:

- `public OptionBuilder WithShortSynonyms(params char[]? shortSynonyms)` -> Sets the short synonyms of the option, e.g. "-f".
Default value is null.
- `public OptionBuilder WithLongSynonyms(params string[]? longSynonyms)` -> Lets you define short synonyms for the option being built.
Default value is null.
- `public OptionBuilder SetAsMandatory()` -> Sets the option as mandatory, i.e. it must be present on the command-line.
By default the option is non mandatory.
- `public OptionBuilder RequiresParameter()` -> If the option has parametrized action, it must be invoked only with an argument,
i.e. not null (must have argument e.g. "--format=argument" or "-f argument"). By default the option does not require parameter.
- `public OptionBuilder WithSeparator(char separator = ',')` -> Specifies by what char should be possible arguments separated in multiple parameter options.
By default the separator is ','.
- `public OptionBuilder WithHelpString(string helpString)` -> Sets explanation string - string which is shown when someone uses -h/--help on command line.
Empty explanation will be showed if user does not provide any helpString (does not call this method).
- `public bool RegisterOption(Parser parser)` -> Adds the configured option to the particular parser.
- `public OptionBuilder Reset()` -> resets the object to the same state as when it was created.
Is used after registering one option, then the reset is called and you can start creating another one. If not called, then the defined properties stays in the option builder object.
You can maybe just change the ones needing the change.
- `public OptionBuilder WithLowerBound(int lowerBound)` -> sets the lower bound for int option (its parameters). If it's called
on another type of Option, it is ignored.
- `public OptionBuilder WithUpperBound(int upperBound)` -> sets the upper bound for int option (its parameters). If it's called
on another type of Option, it is ignored.
- `public OptionBuilder WithBounds(int lowerBound,int upperBound)` -> sets both the upper and lower bound
for int option. If called on another type of Option it is ignored.
- `public OptionBuilder WithAction(Action action)` -> Lets you define encapsulated method to call, when the option occurs in the parsed command.
I.e. this is how you will be let know that the option occurred on command line. This determines that the option will be parameterless.
- `public OptionBuilder WithParametrizedAction<TArgument> (Action<TArgument> action)` -> Calling this method will determine that the option will be Parametrized and take 0 to 1 parameters, as specified
in the IParametrizedOption interface. Also allows you to specify action to be called with the parsed parameter. Allowed types are int?, string?, bool?,
Enum? and its descendants.
- `public OptionBuilder WithMultipleParametersAction<TArgument>(Action<TArgument[]?> action)` -> Calling this method will determine that the option will be MultipleParameter and take 0 to unlimited parameters, as specified
in the IMultipleParameterOption interface. Also allows you to specify action to be called with the parsed parameter(s). Accepted types are: int,
string, bool, Enum and its descendatns. NOTE: we do not use nullable variants here compared to `WithParametrizedAction<TArgument>`.

NOTE: we want to emphasize that you have to call at least one of the Action methods as they are setting the type of desired option.
If you do not call any of them, then calling RegisterOption will return false, as it is invalid option.



### Option Interfaces

Application uses instances of objects, that implement one of the three following interfaces, which you can also use for implementing you own option types.
If you wish to implement your own option class, it has to implement one of these interface, based on what type of option you want.

#### IOption 
Represents an option, which takes no parameters and class implementing this interface must implement following methods and properties:
- `public bool IsMandatory { get; }` defines, whether an option must be present on command line.
- `public char[]? shortSynonyms { get; }` contains short synonyms for option. (one char names without the '-')
- `public string[]? longSynonyms { get; }` contains long synonyms for option. (multiple char names without the '--')
- `public bool SetHelpString(string helpString);` contains message to be shown, when help is invoked on command line.
- `public void TakeAction();` method to be called, when option is present on command line. I. e. what should be done, when the option is present.
 
Watch out that if you don't provide any synonyms, your action will never be called. Also Synonyms for different options must not collide,
otherwise you will not be able to add the latter colliding option to the Parser (Add method will return false).


#### IParametrizedOption : IOption
NOTE: that Long synonym on command line has form of: --longSynonym=parameter -> after the long option synonym
follows and equal sign and then the parameter(if present).
Represents an option which can take a parameter. Class implementing this interface must implement except inherited methods and properties following:
- `public bool IsParameterRequired { get; }` Indicates whether an option requires a parameter, if it doesn't and no parameter was passed, method `ProcessParameter()` 
won't be called.
- `public bool ProcessParameter(string parameter)` method to be called, when a parameter corresponding to the option occurs on the command line.

Compared to the `CreateNoParameterOption` method you need to specify whether the parameter is mandatory via the `isParameterRequired` parameter, i.e. option 
can be used without its parameter.
#### IMultipleParameterOption : IParametrizedOption

- `public char Separator { get; }` -> Separator, by which the multiple parameters are separated with on command line following the option.
Must be non-whitespace.

### Plain Arguments

On the command line can occur 2 types of plain arguments:
- Simple plain argument -> one word representing one plain argument, for example: "1" or "hello"
- Multiple parameters plain argument -> one string representing multiple plain arguments (similarly to multiple parameter option). 
For example: "1,2,3" . This can be interpreted as 3 int numbers separated by ',' separator. NOTE: the separator must be non-white-space.

#### How to use plain arguments

First we have interface IPlainArgument, which object representing a plain argument must implement.
It must implement the following:
- `public bool IsMandatory { get; }` -> if true, the plain argument must occur on the command line, otherwise can be omitted (specified below)
- `public void TakeAction();` -> this is the action, which should be called after the plain argument is parsed and processed. In objects
returned by our factory methods, this method simply calls an action provided by the user in Factory methods's parameters, with particular
plain arguments(arguments).
- `public bool ProcessParameter(string parameter);` -> method used to parse the string form of the plain argument. In the multiple parameter
plain option, we first need to split string according to particular separator, then parse the output. Returns false when an error occurs,
like being unable to parse the input.

Then we present two factory methods to create instances, that allow the user create objects representing the plain arguments with desired
properties.

```C#
public static IPlainArgument CreatePlainArgument<TArgument>(
           Action<TArgument?> action,
           bool isMandatory
           );
```

This method allows user to create object representing simple plain argument.
- type `TArgument` represents of what type the plain argument should be
- `Action<T?> action` is action which is called in the TakeAction method, with the parsed plain argument, i. e. what should be done
with the parsed plain argument.
- `bool isMandatory` meaning is the same as in the interface.

```C#
public static IPlainArgument CreateMultipleParametersPlainArgument<TArgument>(
           Action<TArgument[]?> action,
           bool isMandatory,
           char separator = ','
           );
```

This method allows user to create object representing multiple parameters plain argument.
Meaning of the parameters is identical to the previous factory method, but user can provide separator, by which the values should be
separated, and action takes array of TArgument objects, because there can be multiple values.

When user created all of his desired plain argument objects, user should create array, in which the mandatory arguments come
before the non mandatory ones.
During the parsing of command line, parses passes the first plain argument to the first object in the array, second plain argument
to the second object and so on. You must be explicitly careful with the non-mandatory ones, because if you want for example
int, string, int non mandatory plain arguments and on command line are int int plain arguments (user intended to omit the middle one),
then the int plain argument will be parsed by the string object, which is not correct.

When there are more plain arguments than the user defined, the remaining plain arguments can user access via Parser property `RemainingPlainArguments`,
where unused plain arguments are stored.

### Parser
Second building component is `Parser` which is used for the actual parsing of the command line arguments.

For adding IOption(instance of an object implementing this interface) to the Parser user uses method *Add* which takes as parameter instance
of IOption and its descendants. (Returns true if there were no problems adding an option to the Parser, returns false if an error occurred,
such as synonyms colliding with already added options, no short options and no long options at the same time and other undefined behavior)

User creates an instance of Parser and before actual parsing he can configure via *SetPlainArgumentHelpString* help text to be shown next to -- when
-h / --help is present on command line. 

Then he can proceed to the actual Parsing by calling the method *ParseCommandLine* which takes as parameter  string arguments passed to the program.
It returns true if no error occurred during the parsing, false otherwise — in that case Parser's property `Error` contains information about the Error.

To get "HelpString" (man page info) user calls method *GetHelpString* which will provide HelpString to him 
(based on HelpString settings at each submitted option).

Redundant (unused) plain arguments the user can access via the `RemainingPlainArguments` property.

Parser has two types of constructor:
- `Parser ()` -> when this constructor is invoked, we do not expect any plain arguments on the command line. If there
are any plain arguments present, they are ignored.
- `Parser(IParametrizedOption[] plainArguments)` -> parameter 'plainArguments' represents expected plain arguments present
on the command line. Parser then passes first plain argument to the first object in the `plainArguments` and continues
until there are any plain arguments left. If there is more plain arguments present on command line than objects in 
`plainArguments` the redundant ones are added to the `RemainingPlainArguments` property. If there is not enough plain arguments to satisfy number of mandatory
plain arguments, it results in `ParseCommandLine` method returning false. 

### Parsing Errors
Parsing Errors are returned via `ParserError` object, which encapsulates the information about the error which has occurred.
```C#
public readonly struct ParserError
{
    public readonly ParserErrorType type;       
    public readonly string message;        
}
```

Its type field specifies what type of error has occurred. There are following possible error types:

- InvalidOptionIdentifier -> Occurs when there is on the command line -{InvalidIdentifier} or --{InvalidIdentifier} before the plain arguments separator --.
- CouldNotParseTheParameter -> Occurs when the option could not parse the parameter belonging to her.
- MissingMandatoryOption -> Occurs when there is Mandatory option missing on the command line.
- MissingMandatoryPlainArgument -> Occurs when there is not enough plain arguments to satisfy number of the mandatory plain arguments.
- Other -> When other errors occur.

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
```C#
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
```
## Build instructions
For building you need to have installed .NET SDK version at least 7.0.

To build the library use `dotnet build ArgumentParsing.csproj` command.
Then reference the library in your project properties.

## Running a .NET application.
For running .NET applications that uses this library you need to have .NET runtime version at least 7.0 installed.

In order to run the application use `dotnet <built dll path>`.

## Generating Documentation

In order to generate documentation use `doxygen -g <config_file>` command. Config file for Doxygen is 
`Doxyfile` in the `t12-api-design` directory, with this configureation HTML documntation witll .


