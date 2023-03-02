# Command Line Parser Library
The Command Line Parser Library offers an easy to use API for parsing command line arguments and storing option's parameters. 
It also allows you to display a help screen and a simple syntax error feedback.

## Key Concepts

The library uses a hierarchy of building components for setting up command-line parsing.

### Option Interfaces

Application uses instances of objects, that implement one of the three following interfaces:

**`IOption`** — 
Represents an option, which takes no parameters and class implementing this interface must implement following methods and properties:
- `public bool IsMandatory { get; }` defines, whether an option must be present on command line.
- `public char[]? shortSynonyms { get; }` contains short synonyms for option.
- `public string[]? longSynonyms { get; }` contains long synonyms for option.
- `public bool SetHelpString(string helpString);` contains message to be shown, when help is invoked on command line.
- `public void TakeAction();` method to be called, when option is present on command line. I. e. what should be done, when the option is present.
Our implementation calls the Action provided by the user in the CreateNoParameterOption method. 
```C#
public static IOption CreateNoParameterOption(
    Action action,
    bool isMandatory,
    char[]? shortSynonyms = null,
    string[]? longSynonyms = null
    )
```

This is factory method. Presents the user ability to create an instance of an object implementing this interface, suitable his purposes.
- `Action action` is method, to be called, when the option is present on the command line. User defines this method to suit his needs and parser
calls it when the option is present. Can be called multiple times if the option is present multiple times on the command line.
- `bool isMandatory` user sets this property to true, if the option must be present on the command line, false otherwise
- `char[]? shortSynonyms = null` defines short synonyms(one letter) for the option, that means what kind of short names should this option respond to.
For example `['p','k']` - option consumes -p and -k on command line.
- `string[]? longSynonyms = null` defines long synonyms(2+ letters) for the option, that means what kind of long names should this option respond to.
For example `['portable','king']` - option consumes --portable and --king on command line.
 
Watch out that if you don't provide any synonyms, your action will never be called. Also Synonyms for different options must not collide,
otherwise you will not be able to add the latter colliding option to the Parser (Add method will return false).


**`IParametrizedOption : IOption`** —
represents an option which can take a parameter. Class implementing this interface must implement except inherited methods and properties following:
- `public bool IsParameterRequired { get; }` Indicates whether an option requires a parameter, if it doesn't and no parameter was passed, method `ProcessParameter()` 
won't be called.
- `public bool ProcessParameter(string parameter)` method to be called, when a parameter corresponding to the option occurs on the command line.

```C#
public static IParametrizedOption CreateParameterOption<T>(
    Action<T?> action,
    bool isMandatory,
    bool isParameterRequired,
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



**IMultipleParameterOption : IParametrizedOption**
- `public char Delimiter { get; }` sets the delimiter, which user is expected to use on the command line to separate multiple parameters.
```C#
public static IMultipleParameterOption CreateMulitipleParameterOption<T>(
           Action<T[]?> action,
           bool isParameterRequired,
           bool isMandatory,
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

   -e   equivalent to -vE

   -E, --show-ends
        display $ at end of each line

   -n, --number
        number all output lines

   -s, --squeeze-blank
        suppress repeated empty output lines

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

            Action markAll = () => Console.WriteLine("Show all");
            var portabilityOption = IOption.CreateNoParameterOption(markAll, false, new char[] { 'A' }, new string[] { "show-all" });
            portabilityOption.SetHelpString("equivalent to -vET");

            Action markNonBlank = () => Console.WriteLine("was portable");
            var portabilityOption = IOption.CreateNoParameterOption(markPortable, false, new char[] { 'p' }, new string[] { "portability" });
            portabilityOption.SetHelpString("Use the portable output format.");

        }
    }
}
```
## Build instructions