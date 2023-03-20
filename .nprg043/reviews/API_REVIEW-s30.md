# Review of the parser library design

## First impression

The process of building the library and example program was straightforward.
The parser configuration approach reminded me of other existing libraries,
simplifying the familiarization process. I also noticed that the program was
rather verbose, with each option requiring many lines of code.

The README file was difficult to read because it immediately dived into the
details of the API. I would prefer a quick-start section with a simple usage
overview. At the very least, I recommend moving the examples to the top of the
file.

## API review

The library allows the user to construct a parser object and configure it by
adding option objects one by one. In contrast to options, plain arguments are
passed to the constructor; I see no reason for this inconsistency. The user
defines the result of parsing using delegates, which provides more control over
the parser's behavior. The library does not mention how the parsed values can
be accessed, but storing them in local variables or class fields is a
reasonable option. Additionally, a help message string can be generated and
used in any way the user sees fit.

Discovering the API from a new user's perspective seems fine. Finding the right
options types may be a little difficult, but the factory functions provide
enough help in the form of parameter documentation.

Errors are reported using a boolean return value and the error message can be
retrieved if needed. I would be surprised if anyone bothers to check the return
value, moreover, nothing else prevents the code from executing further. I
strongly recommend using exceptions instead.

## Writing `numactl`

In the end, I managed to implement every aspect of the task. The interface
provided for options offered all the functionality I needed. Custom types are
not supported, but parsing them as strings works well, too. Unfortunately, the
library does not offer a way to parse an initially unknown number of plain
arguments. Defining many optional arguments is possible, although questionable
from a performance perspective.

The definition of callback actions proved cumbersome since a `null` check is
necessary. In many cases, this would lead to the lambda function spanning more
lines than necessary. Perhaps a more suitable solution should be chosen for
handling optional parameters, which are hardly ever used in command-line tools.

Lastly, some mistakes in the documentation complicated my work. The most
notable one was the incorrect type used in the `Parser` constructor. However,
the documentation contains more such errors, most of which seem to have arisen
due to copy-pasting.

## Detailed comments

I appreciate the detailed documentation comments with examples. The terminology
adheres to the definitions from the task. However, there are minor
inconsistencies in the code, such as in the capitalization of the properties of
the `IOption` interface. Parsing values to an `enum` type requires the in-code
values to use lowercase, which does not match the language convention. There
seems to be no rule for the type where the factory methods are defined.

The library uses interfaces and static functions to provide the API, which
provides a lot of extensibility but also leaks some implementation details,
such as the `TakeAction()` method. Peeking into the internal classes, I noticed
that functions with support for integer constraints exist but are not exposed
in the API.

The objects representing options and plain arguments are mutable, and
preconditions are presumably only checked when the option is added to the
parser. However, the reference remains available to the user. This could be
problematic if the array of option names is modified. Although it likely poses
no major issue right now, it could become a problem if the API is extended.
