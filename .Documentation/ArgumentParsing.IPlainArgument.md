[Back to mainpage](../README.md)

---

# Plain Arguments

On the command line can occur 2 types of plain arguments:
- Simple plain argument -> one word representing one plain argument, for example: "1" or "hello"
- Multiple parameters plain argument -> one string representing multiple plain arguments (similarly to multiple parameter option). 
For example: "1,2,3" . This can be interpreted as 3 int numbers separated by ',' separator. NOTE: the separator must be non-white-space.

## How to use plain arguments

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

---

[Back to mainpage](../README.md)