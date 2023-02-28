# Command Line Parser Library
The Command Line Parser Library offers an easy to use API for parsing command line arguments and storing option's parameters. 
It also allows you to display a help screen and a simple syntax error feedback.

## Key Concepts

The library uses a hierarchy of building components for setting up command-line parsing.

### Options
Base building component is a class `Option`, which is a abstract ancestor of all options. Options further divides to two categories:
- `NoParameterOption` — Is used to represent options with no parameter (see example TODO: add link to example)
- `ParameterOption` — An ancestor of all parametrized options.

`ParameterOption` have descendant called `MultipleParameterOption` which is common ancestor for options which take 0/1 to unlimited
number of parameters (have Multiple in prefix). This ancestor contains property *Delimeter*, which is used to set delimeter by which are multiple parameters separated.


All option, when constructing, takes `Action` parameter, which encapsulates method that is used to notify that the option occurred on
command-line. For `ParameterOption` `Action<T?>` takes parameter of a corresponding type.

**Option with parameters** for different basic parameter types, there are derived option classes from the `ParameterOption`:
- IntOption
- MultipleIntOption
- StringOption
- MultipleStringOption
- BoolOption
- MultipleBoolOption
- EnumOption\<TEnum>
- MultipleEnumOption\<TEnum>

When using `EnumOption<TEnum>`, or `MultipleEnumOption<TEnum>` first create `enum` struct with corresponding set of names. The enum's names will be used for
matching the parsed argument, matched name's constant will be passed to the specified `Action`.



### OptionSet
Second building component is `OptionSet` which is used as data structure that stores created options by user. After creating all desired options,
user creates an instance of OptionSet a fills it with those desired options.

For adding options to the OptionSet user uses method *Add* which takes as parameter instance of Option and its descendant.
(Returns true if there were no problems adding an option to the OptionSet, returns false if an error occured,
such as synonyms colliding with already added options, no short options and no long options at the same time and other undefined behaviour)

### Parser
Third building component is `Parser` which is used for the actual parsing of the command line arguments.

User creates an instance of Parser and before actual parsing he can configure via *SetPlainArgumentHelpString* help text to be shown next to -- when
-h / --help is present on command line. 

Then he can proceed to the actual Parsing by calling the method *ParseCommandLine* which takes as parameter  string arguments passed to the program.
(Returns true if no error ocurred during the parsing, false otherwise).

To retrieve plain arguments user calls (after the parsing) method *GetPlainParameters*, which will return him list of all plain arguments.

To get "HelpString" (man page info) user calls method *GetHelpString* which will provide HelpString to him (based on HelpString settings at each submitted option).