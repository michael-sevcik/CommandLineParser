# Command Line Parser Library
The Command Line Parser Library offers an easy to use API for parsing command line arguments and storing option's parameters. 
It also allows you to display a help screen and a simple syntax error feedback.

## Key Concepts

The library uses a hierarchy of building components for setting up command-line parsing.

### Option Interfaces

Application uses instances of objects, that implement one of the three following interfaces:

#### IOption 
Represents an option, which takes no parameters and class implementing this interface must implement following methods and properties:
- `public bool IsMandatory { get; }` defines, whether an option must be present on command line.
- `public char[]? shortSynonyms { get; }` contains short synonyms for option.
- `public string[]? longSynonyms { get; }` contains long synonyms for option.
- `public bool SetHelpString(string helpString);` contains message to be shown, when help is invoked on command line.
- `public void TakeAction();` method to be called, when option is present on command line. I. e. what should be done, when the option is present.

```C#
public static IOption CreateNoParameterOption(
    Action action,
    bool isMandatory,
    char[]? shortSynonyms = null,
    string[]? longSynonyms = null
    )
```

This is factory method. It enables the user to create an instance of an object implementing this interface, suitable for his purposes.
- `Action action` is method, to be called, when the option is present on the command line. User defines this method to suit his needs and parser
calls it when the option is present. Can be called multiple times if the option is present multiple times on the command line.
- `bool isMandatory` user sets this property to true, if the option must be present on the command line, false otherwise
- `char[]? shortSynonyms = null` defines short synonyms(one letter) for the option, that means what kind of short names should this option respond to.
For example `['p','k']` - option consumes -p and -k on command line.
- `string[]? longSynonyms = null` defines long synonyms for the option, that means what kind of long names should this option respond to.
For example `['portable','king']` - option consumes --portable and --king on command line.
 
Watch out that if you don't provide any synonyms, your action will never be called. Also Synonyms for different options must not collide,
otherwise you will not be able to add the latter colliding option to the Parser (Add method will return false).


#### IParametrizedOption : IOption
represents an option which can take a parameter. Class implementing this interface must implement except inherited methods and properties following:
- `public bool IsParameterRequired { get; }` Indicates whether an option requires a parameter, if it doesn't and no parameter was passed, method `ProcessParameter()` 
won't be called.
- `public bool ProcessParameter(string parameter)` method to be called, when a parameter corresponding to the option occurs on the command line.

```C#
public static IParametrizedOption CreateParameterOption<T>(
    Action<T?> action,
    bool isMandatory,
    bool isParameterRequired = false,
    char[]? shortSynonyms = null,
    string[]? longSynonyms = null
    )
```
This is also a factory method, it enables creation of IParametrizedOptions, which can take a parameter of a type T. Following types are supported:
- `int`
- `bool`
- `string`
- derived `enum` type — this is used to represent set of words that are matched with passed parameter, e.g.:
```C#
enum format {
    txt,
    rtf,
    pdf
}
```

Compared to the `CreateNoParameterOption` method you need to specify whether the parameter is mandatory via the `isParameterRequired` parameter, i.e. option 
can be used without its parameter.

```C#
public static IParametrizedOption CreatePlainArgument<T>(
    Action<T?> action,
    bool isMandatory       
    )
```

Creates an object that represents plain argument, that should stand alone on the command line. It is similar to 
IParametrizedOption and it's derived classes objects, but long and short synonyms are omitted, as in the plain arguments
we only consider the parameters. (There are none options in the plain arguments). Also isParameterRequired is not necessary as isMandatory
property replaces it.

- `Action<T?> action` specifies what action should be taking with the parsed plain argument.
- `bool isMandatory` specifies whether this plain argument must be present on the command line (user must provide it).

#### IMultipleParameterOption : IParametrizedOption
```C#
public static IMultipleParameterOption CreateMulitipleParameterOption<T>(
           Action<T[]?> action,
           bool isMandatory,
           bool isParameterRequired = false,
           char[]? shortSynonyms = null,
           string[]? longSynonyms = null,
           char delimiter = ','
           )
```
This method creates an instance of an object implementing IMultipleParameterOption interface, with desired properties. This option can take
0 to unlimited number of parameters based on user's preferences.

- `Action<T[]?> action` works same as for the IParametrizedOption, but takes array as an argument, because number of the parameters might exceed 1.
- `bool isParameterRequired` works the same as in the previous Interfaces.
- `bool isMandatory` works the same as in the previous Interfaces.
- `char[]? shortSynonyms = null` works the same as in the previous Interfaces.
- `string[]? longSynonyms = null` string[]? longSynonyms = null.
- `char delimiter = ','` sets the delimiter, which user is expected to use on the command line to separate multiple parameters.

```C#
public static IMultipleParameterOption CreateMultipleParametersPlainArgument<T>(
    Action<T[]?> action,
    bool isMandatory,
    char separator = ','
    )
```

Creates an object that represents multiple plain arguments separated by non-white-space separator. This object is similar to <see cref="IOption"/> and its derived
classes objects, but some non-necessary details (mention in IParametrizedOption) are omitted. I. e. if you want to take multiple plain arguments of same type you choose this object.
Note that you do not define synonyms or names for this object, you just define what kind of parameters should this "option" take.

- `Action<T[]?> action` specifies what action should be taking with the parsed plain arguments.
- `bool isMandatory` specifies whether these plain arguments must be present on the command line (user must provide them).
- `char separator` specifies by what char should be arguments separated.


### Plain Arguments

#### How to use plain arguments

First of all we have 2 types of plain arguments:
- mandatory, which must be present on the command line
- non-mandatory, which can be omitted on command line

Non-mandatory plain arguments can come only after all the mandatory plain arguments.

Then we have another 2 types of plain arguments you can have on the command line:
- plain argument -> Argument that stands by itself on the command line. For example `2` can be argument that can be interpreted
as int plain argument.
- multiple parameter plain argument -> Argument that represents multiple plain arguments separated by `separator`.
For example Joe,Josh,John can be argument representing 3 string plain arguments separated by `,`.

For representing plain arguments objects we use 2 interfaces which we have already used:
- `IParametrizedOption` -> User creates an instance of this object when he wants to use the plain argument. We can ommit
`longSynonyms`,`shortSynonyms` and `isParameterRequired` as they are not needed in plain arguments.
- `IMultipleParameterOption` -> User creates an instance of this object when he wants to use the plain multiple parameter plain argument. We can ommit
`longSynonyms`,`shortSynonyms` and `isParameterRequired` as they are not needed in plain arguments.

Next user proceeds to creating an array of these options and he inserts it into the constructor of `Parser` object.

### Parser
Second building component is `Parser` which is used for the actual parsing of the command line arguments.

For adding IOption(instance of an object implementing this interface) to the Parser user uses method *Add* which takes as parameter instance
of IOption and its descendants. (Returns true if there were no problems adding an option to the Parser, returns false if an error occurred,
such as synonyms colliding with already added options, no short options and no long options at the same time and other undefined behavior)

User creates an instance of Parser and before actual parsing he can configure via *SetPlainArgumentHelpString* help text to be shown next to -- when
-h / --help is present on command line. 

Then he can proceed to the actual Parsing by calling the method *ParseCommandLine* which takes as parameter  string arguments passed to the program.
(Returns true if no error occurred during the parsing, false otherwise).

To retrieve plain arguments user calls (after the parsing) method *GetPlainParameters*, which will return him list of all plain arguments.

To get "HelpString" (man page info) user calls method *GetHelpString* which will provide HelpString to him (based on HelpString settings at each submitted option).

Parser has two types of constructor:
- `Parser ()` -> when this constructor is invoked, we do not expect any plain arguments on the command line. If there
are any plain arguments present, they are ignored.
- `Parser(IParametrizedOption[] plainArguments)` -> parameter 'plainArguments' represents expected plain arguments present
on the command line. Parser then passes first plain argument to the first object in the `plainArguments` and continues
until there are any plain arguments left. If there is more plain arguments present on command line than objects in 
`plainArguments` the redundant ones are ignored. If there is not enough plain arguments to satisfy number of mandatory
plain arguments, it results in `ParseCommandLine` method returning false. 

## Examples

### Easy one

```C#

enum Greeting {
    Hello,
    hi,
    ciao
}

void Main(string[] args) {
    // Create option
    Action<Greeting?> greetingAction = greeting => Console.WriteLine(greeting); // This local function is called with the parsed argument.
    var greetingOption = IParametrizedOption.CreateParameterOption<Format?>(formatAction, false, true, new char[] { 'g' }, new string[] { "greeting" });

    // Create parser
    Parser parser = new();

    // Fill parser with the created option.
    parser.Add(FormatOption);


    // Parse command-line input.
    parser.ParseCommandLine(args);
}
```

When the application is opened with arguments "-g Hello --greeting=ciao", the output is:
```
Hello
ciao
```

### More complex one

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
            //first we need to create desired options

            //these type of options are most common, no parameter needed and are not mandatory either
            Action markAll = () => Console.WriteLine("Show all");
            var portabilityOption = IOption.CreateNoParameterOption(markAll, false, new char[] { 'A' }, new string[] { "show-all" });
            portabilityOption.SetHelpString("equivalent to -vET");

            Action markNonBlank = () => Console.WriteLine("Use non blank");
            var nonBlankOption = IOption.CreateNoParameterOption(markNonBlank, false, new char[] { 'b' }, new string[] { "number-nonblank" });
            nonBlankOption.SetHelpString("number nonempty output lines, overrides -n");

            //look at the type of option we choose, after -e must follow one string parameter, we choose ParametrizedOption with string parameter type
            Action<string?> actionForEquals = (string? fileName) => Console.WriteLine($"Is equal to{fileName}");
            var equalFileOption = IParametrizedOption.CreateParameterOption(actionForEquals, false, true, new char[] { 'e' });
            equalFileOption.SetHelpString("adds equivalent file name");

            Action markEndsOfLine = () => Console.WriteLine("Show ends of lines");
            var endsOfLineOption = IOption.CreateNoParameterOption(markEndsOfLine, false, new char[] { 'E' }, new string[] { "show-ends" });
            endsOfLineOption.SetHelpString("display $ at end of each line");

            Action markNumberLines = () => Console.WriteLine("Number the lines");
            var numberLinesOption = IOption.CreateNoParameterOption(markNumberLines, false, new char[] { 'n' }, new string[] { "number" });
            numberLinesOption.SetHelpString("number all output lines");

            //again as with the EqualFileOption, but we need int parameter to folllow
            Action<int?> ActionForSqueezingSpaces = (int? intensity) => Console.WriteLine($"Squeezing spaces to the intensity of {intensity}");
            var squeezingSpacesOption = IParametrizedOption.CreateParameterOption(ActionForSqueezingSpaces, false, true, new char[] { 's' }, new string[] { "squeeze-blank" });
            squeezingSpacesOption.SetHelpString("suppress repeated empty output lines, int number must follow");

            Action markTOption = () => Console.WriteLine("TOption was present");
            var tOption = IOption.CreateNoParameterOption(markTOption, false, new char[] { 't' });
            tOption.SetHelpString("equivalent to -vT");

            // we define what kind of plain arguments we want and what action to be called upon them

            //first plain arguement is mandatory, i. e. must be present and is of type string
            Action<string?> firstPlainArgumentAction = (string? name) => Console.WriteLine($"Hi{name}");
            var firstPlainArgument = IParametrizedOption.CreateParameterOption<string>(firstPlainArgumentAction, true);

            //second plain argument is not mandatory and can be ommited, is of type int
            //remember that all plain arguments that are not mandatory must come after all mandatory plain arguments
            Action<int?> secondPlainArgumentAction = (int? intensity) => Console.WriteLine($"Your age: {intensity}");
            var secondPlainArgument = IParametrizedOption.CreateParameterOption(secondPlainArgumentAction, false);

            var plainArguments = new IParametrizedOption[] { firstPlainArgument, secondPlainArgument };

            //create a new Parser
            Parser parser = new Parser(plainArguments);

            //fill the parser with the correctly created options
            parser.Add(portabilityOption);
            parser.Add(nonBlankOption);
            parser.Add(equalFileOption);
            parser.Add(endsOfLineOption);
            parser.Add(numberLinesOption);
            parser.Add(squeezingSpacesOption);
            parser.Add(tOption);

            //we can add helpString that will be shown next to the -- when -h is invoked
            parser.SetPlainArgumentHelpString("This will be shown next to --");

            //now we finally parse the command line arguments
            parser.ParseCommandLine(args);           

            /*
             * Some possible outputs:
             * args[] = { "-e" , "MyFile.txt","Joe" }
             * output: Is Equal to MyFile.txt
             *         Hi Joe.
             * 
             * args[] = { "--squeeze-blank=2", "-number", "Joe" , "60" }
             * output: Squeezing spaces to the intensity of 2
             *         Number the lines
             *         Hi Joe
             *         You arge: 60
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


