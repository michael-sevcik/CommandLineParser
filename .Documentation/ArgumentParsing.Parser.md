[Back to mainpage](../README.md)

---

# Parser

One of the main building components of this library is `Parser` which is used for the actual parsing of the command line arguments.

For adding IOption(instance of an object implementing this interface) to the Parser user uses method *Add* which takes as parameter instance
of IOption and its descendants. (Returns true if there were no problems adding an option to the Parser, returns false if an error occurred,
such as synonyms colliding with already added options, no short options and no long options at the same time and other undefined behavior)

User creates an instance of Parser and before actual parsing he can configure via *SetPlainArgumentHelpString* help text to be shown next to -- when
-h / --help is present on command line. 

Then he can proceed to the actual Parsing by calling the method *ParseCommandLine* which takes as parameter  string arguments passed to the program.
It returns true if no error occurred during the parsing, false otherwise — in that case Parser's property `Error` contains information about the Error.

To get "HelpString" (man page info) user calls method *GetHelpString* which will provide HelpString to him 
(based on HelpString settings at each submitted option).

Unused (were not passed to any `IPlainArgument instance`) plain arguments can be accessed via the `RemainingPlainArguments` property.

Parser has two types of constructor:
- `Parser ()` -> when this constructor is invoked, we do not expect any plain arguments on the command line. If there
are any plain arguments present, they are saved and accessible via the `Parser.RemainingPlainArguments` property.
- `Parser(IParametrizedOption[]? plainArguments = null, string? plainArgumentsHelpMessage = null)` -> parameter 'plainArguments' 
represents expected plain arguments present
on the command line. Parser then passes first plain argument to the first object in the `plainArguments` and continues
until there are any plain arguments left. If there is more plain arguments present on command line than objects in 
`plainArguments` the redundant ones are added to the `RemainingPlainArguments` property. If there is not enough plain arguments 
to satisfy number of mandatory plain arguments, it results in `ParseCommandLine` method returning false. The `plainArgumentsHelpMessage`
parameter enables user to specify help message for plain arguments, which is displayed after the help message for options.

## Parsing Errors

Parsing Errors are returned via `ParserError` object, which encapsulates the information about the error which has occurred.
```C#
public readonly struct ParserError
{
    public readonly ErrorType type;       
    public readonly string message;        
}
```

Its type field specifies what type of error has occurred. There are following possible error types:

- InvalidOptionIdentifier -> Occurs when there is on the command line -{InvalidIdentifier} or --{InvalidIdentifier} before the plain arguments separator --.
- CouldNotParseTheParameter -> Occurs when the option could not parse the parameter belonging to her.
- MissingMandatoryOption -> Occurs when there is Mandatory option missing on the command line.
- MissingMandatoryPlainArgument -> Occurs when there is not enough plain arguments to satisfy number of the mandatory plain arguments.
- Other -> When other errors occur.

---

[Back to mainpage](../README.md)
