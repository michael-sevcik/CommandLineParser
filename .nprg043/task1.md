# Task #1: API Design

The general topic of the assignments is the design and implementation
of a library for parsing command-line arguments. It is a well-defined
problem which leaves a lot of room for different design decisions,
and allows different solutions that can all be considered good.

## Definitions

Short option
: A command-line argument that starts with a single dash,
  followed by a single character (e.g. "`-v`")

Long option
: A command-line argument that starts with two dashes,
  followed by one or more characters (e.g. "`--version`")

Option
: Short option or Long option

Option parameter
: A command-line argument that follows an option (if the option is defined
  to accept parameters by the user of your library)

Plain argument
: A command-line argument that is neither an option nor an option parameter.

Delimiter
: A command-line argument consisting of two dashes only, i.e. `--`.
  Any subsequent argument is considered to be a plain argument

For example, in the following command

```shell
cmd -v --version -s OLD --length=20 -- -my-file your-file
```

there are short options `v` and `s`, long options `version` and
`length`, the `OLD` and `20` values represent arguments to `-s` and `--length`
options, respectively. The `-my-file` and `your-file` arguments after the `--`
delimiter are plain arguments.


## API requirements

This is the first task in a sequence of tasks, and you are required to 
**design an API** that allows the user to perform the following operations
in a convenient way:

- Specify options and arguments that a program accepts, specify which of them
  are optional or mandatory, and verify that the actual arguments passed to the
  program conform to the command line specification.
- Define synonyms for options (at the very least 1:1 between short and long
  options, but ideally in a more general way).
- Specify whether an option may/may not/must accept parameters and verify that
  the actual arguments passed to the client program conform to the specification.
- Specify types of option parameters and plain arguments and verify that the
  actual arguments passed to the client program conforms to the specification.
  At the very least the library has to distinguish between string parameters,
  integral parameters (with either or both: lower and upper bound), boolean
  parameters, and string parameters (with fixed domain, i.e., enumeration).
- Document plain arguments and options and present the documentation to the user
  in form of a help text.
- Access the values of options and all plain arguments so that the client program
  can make decisions based on the command line inputs.
- Allow mixing of plain arguments and options on the commandline (unless a
  plain argument starts with `-`, in which case it must come after the `--`
  delimiter).

You are not supposed to implement the API at this point &mdash; you will be
doing that later in another task. For now, write the interface parts of the API
(types, classes, methods with signatures), but leave the implementations
**EMPTY** &mdash; except for statements such as `return null;` (and similar)
needed for the code to compile.

It is not necessary to support all of the above explicitly. For example, the
validation of a type of a parameter may be performed implicitly upon retrieval
of the value by the user. An exception may be thrown if the corresponding
argument has an incorrect type.

The requirements are intentionally not exhaustive. Use your imagination
and be creative in the design...

### Design considerations

- In what way will the user declare individual options, their
  parameters and synonyms? What data structures could capture these?
- In what way will the user react to options? How will the options be
  accessed? Callbacks? List of all options? On-demand access to
  particular options?
- In what way will the library validate the arguments?
  Explicitly/implicitly? Will the library produce warnings displayed
  to the user directly?
- What kind of errors can occur, how will the user find out
  (exceptions/error codes) and what can the user do about it?
- What classes will the library contain? What purpose will they have?


### General suggestions

- Take a look at the existing libraries out there.
- Try to remedy their drawbacks and whatever annoyances you fancy.
- Modify some of your previous projects to use your API to parse its
  command-line options. This may help you discover design issues early
  in the process.


## Example program

Apart from the bare API, also must also submit an example program that uses
the API to parse the following arguments (obviously, it will fail, because
the library has no implementation yet) that are used by `time` command.

```text
time [options] command [arguments...]

GNU Options
    -f FORMAT, --format=FORMAT
           Specify output format, possibly overriding the format specified
           in the environment variable TIME.
    -p, --portability
           Use the portable output format.
    -o FILE, --output=FILE
           Do not send the results to stderr, but overwrite the specified file.
    -a, --append
           (Used together with -o.) Do not overwrite but append.
    -v, --verbose
           Give very verbose output about all the program knows about.

GNU Standard Options
    --help Print a usage message on standard output and exit successfully.
    -V, --version
           Print version information on standard output, then exit successfully.
    --     Terminate option list.
```

Do **NOT** implement the actual command, only show how your API would be
used to set-up option parsing for this command.


## Submission

You will be given access to a Git repository hosted on the faculty GitLab
instance. You are expected to commit your solution there. To make solutions
easier to handle (and to navigate by other students), each solution should
comprise the following components.

### Source code

The main part of the solution is the source code of the library and the
example program. These are really two separate sub-projects sharing the same
(multi-project) repository, which represents a work space in some IDEs. The
goal is to keep the sub-projects separate so that the example program has to
access/use the library in the same way any other program would (there will be
other example programs and tests later on).

Do **NOT** put source code in the root of a repository or sub-project.

The source code **MUST** compile.

### Documentation

You should document the library so that potential users can make sense of it.

A `README.md` with a basic description of the library, key concepts and use
cases, a simple and complex example, and instructions for building (including
dependencies) is an absolute minimum.

You should also document the API elements in the source code using
documentation comments. In addition to providing users with easy-to-find
documentation, it makes it easy to generate a reference documentation later.
Moreover, this kind of documentation is easier to keep up-to-date. 

Avoid comments that just expand the names of methods &mdash; think of
contracts, legal parameter values, nominal and error behavior, etc. At this
stage (no implementation), the documentation comments may easily make the
majority of source code that you submit.

In general, keep in mind that writing means thinking. Writing about design
decisions might help you drive your design. A description of the design
usually works well as a title page of the (generated) reference documentation.

### Build instructions

Both the library and the example program projects must build and run on a
Linux system and outside an IDE. This is usually not much of problem with
languages such as Java, C, or C++, but in the case of C#, it may require using
.NETCore runtime which is supported on Linux.

The `README.md` should provide instructions for building the projects,
generating documentation, and (eventually) running tests. Ideally, each 
action should be a single command. This will also come handy (later) when
setting up CI for the project.

### `.gitignore`

While committing build system or project definition files to the repository is
desired, do **NOT** commit files produced by the build (such as `.class`, `.o`,
`.obj` and similar) or other "garbage" produced by IDEs &mdash; set up your
`.gitignore` file properly.

Also note that each repository will contain a `.nprg043` folder with files
associated with task management, such as task assignments, presentations, and
reviews from other students. Do **NOT** make this folder or its content
ignored, because it interferes with task handling.

### Submission tag

When you are satisfied with your solution to this task, make sure it is
in the **master** branch of your repository and **tag the commit**
using the `task-1-submission` tag. This will indicate the state of the
repository that should be evaluated.

