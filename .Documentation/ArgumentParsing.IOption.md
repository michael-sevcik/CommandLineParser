[Back to mainpage](../README.md)

---

# Option Interfaces

Application uses instances of objects, that implement one of the three following interfaces, which you can also use for implementing your own option types.
If you wish to implement your own option or plain argument class, it has to implement one of these interface, based on what type of option you want, and 
**be immutable like the native library options** (this is important for a preprocessing that the parser does.).

## IOption 
Represents an option, which takes no parameters and class implementing this interface must implement following methods and properties:
- `public bool IsMandatory { get; }` defines, whether an option must be present on command line.
- `public char[]? shortSynonyms { get; }` contains short synonyms for option. (one char names without the '-')
- `public string[]? longSynonyms { get; }` contains long synonyms for option. (multiple char names without the '--')
- `public bool SetHelpString(string helpString);` contains message to be shown, when help is invoked on command line.
- `public void TakeAction()` method to be called, when option is present on command line. I. e. what should be done, when the option is present.
- `public void Restore()` is called when a parsing error occurs, so that the option can restore its state to the state before parsing.
 
Watch out that if you don't provide any synonyms, your action will never be called. Also Synonyms for different options must not collide,
otherwise you will not be able to add the latter colliding option to the Parser (Add method will return false).


## IParametrizedOption : IOption

NOTE: that Long synonym on command line has form of: --longSynonym=parameter -> after the long option synonym
follows and equal sign and then the parameter(if present).
Represents an option which can take a parameter. Class implementing this interface must implement except inherited methods and properties following:
- `public bool IsParameterRequired { get; }` Indicates whether an option requires a parameter, if it doesn't and no parameter was passed, method `ProcessParameter()` 
won't be called.
- `public bool ProcessParameter(string parameter)` method to be called, when a parameter corresponding to the option occurs on the command line.

Compared to the `CreateNoParameterOption` method you need to specify whether the parameter is mandatory via the `isParameterRequired` parameter, i.e. option 
can be used without its parameter.

## IMultipleParameterOption : IParametrizedOption

- `public char Separator { get; }` -> Separator, by which the multiple parameters are separated with on command line following the option.
Must be non-whitespace.

---

[Back to mainpage](../README.md)