# Api Review 

### First Impression

##### Readme

Overall first impression was good. README started good, it had the first basic example which was simple and good enough to let me start with the library and I knew where to find the methods which I wanted to use.

The more advanced examples were a little more confusing, but still menegable to understand.

Overall the readme was well done.

##### Design
First thing I have noticed was the builder design, which made the start easier as the user can see most of what he is able to do with the library. Some things need more explanation and are provided when user looks at the implementation and its comments. But pretty much evertyhing is clear once you try to understand. 
There were the examples in the readme and some examples in the solution itself.

##### Building the library
As the project contains .net solution the library was easy to use and build.
Even if a user was not using the VisualStudio the build process is explained in the readme.

### High level review

The builder design strikes the user at the very beginning and it is easy to start writing the api provided there are the examples. Registering the option on the parser with RegisterOption and Resetting the  builder I did not like that much. But it makes sense and it is what is needed for the builder pattern to work. And it was done in a very good way. 
I do not like that the main library project is called ArgumentParsing but that is just a detail.
Overall it provided funcionality for most if not all of what was required in the task 1.
One thing I did not understand that well is how is the user supposed to use the parsed line. The parse method does not return any value and it was not clear to me how can the value be retrieved.
For the sake of the task I used action to retrieve the value, but I am not sure if that is the way it should be done.
Maybe it would be nice If the method returned some values.
It checks all the neccesary conditions which are set by builder. But how can we use this parsed value ?

### Extending the library

The library provides us with a way to set Actions which are to be executed on the string parameters, therefore I did not have much problem implementing the extensions.
As I have previously mentioned it was possible to retrieve the parsed values, but I had to catch them in the action methods and I am not sure if this is the correct approach.
I did not want to extend the library with parsed some container with parsed values as that would be too complicated and I would have to modify a lot of code in order to extend it in that way. 

### Documentation

I think the documentation was good. Everything was at least in some way documented and explained. Maybe some things were only explained on the surface and did not really explain the meaning of using those things.

### Implementation

The implementation was good. They have managed to implement the chosen pattern very well. The methods are well written and named in explenatory way.
The implementation makes sense to me.

### Specifi implementation details

I do not like that methods start with lowercase. I think the Csharp convention is uppercase methods. But the formatting is convenient.

All public methods and classes are documented

I personally do not like the IOption.cs file because it consists of multiple classes defined in the same file. I think it is better to have each class in a separate file because it is easier to know what is what if the classes are separated. 

The naming of methods and classes fits the problem well.

The user can define multiple independed parser and builders, which is good.

I am not sure who is responsible for storing the parsed data in the end.

The library can generate help text.