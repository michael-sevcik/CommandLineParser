[Back to mainpage](../README.md)

---

# OptionBuilder

This is `OptionBuilder` class which represents object that enables user to create one of the default options. User just needs to use
some of the OptionBuilder's methods to customize their desired option and consequently register the option to the particular `Parser`.

## Methods

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
string, bool, Enum and its descendants. NOTE: we do not use nullable variants here compared to `WithParametrizedAction<TArgument>`.

NOTE: we want to emphasize that you have to call at least one of the Action methods as they are setting the type of desired option.
If you do not call any of them, then calling RegisterOption will return false, as it is invalid option.

---

[Back to mainpage](../README.md)