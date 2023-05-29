# Command Line Parser Library Review


## First Impressions From The README.md

After opening the `README.md` file, what caught my attention was the code, which was not properly colored. You can imporove this by adding `c#` right next to the triple dash symbolizing the beginning of the code (```c#). The structure of the code, on the other hand, was very easily readable, mainly due to the pretty to look at fluent-syntax-like argument definition. A good idea was to add a method to call an action after parsing the argument. Build and run instructions were very clear and straightforward.

A confusing thing for me right from the start was the `Easy example` program, more particularly the output. I thought that both long and short synonyms would stand for the same command and the `Action` would only get executed once, while the correct `Greeting` would get overwritten. This was not the case. The next confusing part for me were the `-vET` and `-vT` arguments in the `More complex example` part. I hope they weren't too important and their presence only represented an example of usage, because there's no definition included and I wasn't able to find out their meaning.

Also, the `README.md` doesn't act consistent, as output was divided from the input in the `Easy example` while they were joint together in the `Hard example`.


## Documentation Review

Documentation from the file `.Documentation` was not pleasing to read when there was a decription of many methods separated only by dots in the beginning of the lines (for example in the `ArgumentParsing.OptionBuilder`). I would use a triple dash (```) instead of a single dash (`), making the method name stand out more. There was a lot of typos in the text documents as well.

I had to stick to this kind of documentation and the comment summaries written above the methods right in the code because I wasn't able to generate the documentation from `Doxyfile`. The comments in the code were very specific, sometimes unnecessarily. The code itself is self-explanatory at times and when this is the case the comments only repeat what's already clear from the code and variable names (for example in `Parser.ErrorType`). There are also long method doscriptions present with several use cases mentioned which are not that necessary and/or should be written somewhere else, for example in a `How to use` or `Usage` section of the documentation, not in the code. Also, there were some typos present in the code summaries as well. I recommend reading the summaries out loud once again.


## API Remarks

`OptionBuilder` methods `WithLowerBound`, `WithUpperBound` and `WithBounds` don't belong to a general builder of options as they only matter to a type `int`. The correct solution to this problem in my opinion would be preventing the project to compile when the option doesn't receive a parameter of type `int` when any of the `WithLowerBound`,`WithUpperBound` or `WithBounds` methods is present.

The code itself is nice to look at thanks to the fluent-syntax style of option definiton. Names of the methods used to define options are self-explanatory and even without a documentation you could probably guess what is the purpose of the individual methods. I also like the extendability of the library through the `IOption` interface and his descendants and `IPlainArgument`.


## Library Code / Implementation Review

The code is written consistently and divided into several separate files. I like that. The names of methods and classes are very clear. The example program is easily readable. I like that you have written your own unit test even though you got the API tests from someone else. The warnings produced by the tests are horrible though (as you can see in the Gitlab CI). Either fix them - it's not that difficult, or turn them off completely.

The method `OptionBuilder.CreateParticularOptionForRegistration` is about 150 lines long. Surely there's a way to make it more readable even without all the comments acompanying the individual steps by dividing it into multiple private methods.

I really like the method `OptionBuilder.invalidateParseIntMultipleParametersOutput` as a solution on how to return an output parameter. I like it more than your solution in the method `OptionBuilder.parseInt`. Specifically, I don't like the unnecessary comment on line 439 as it repeats what's clear. Also, the method is `public`. I expected it to be `internal` instead because I don't think it is meant to be accessible by the user.

I like that in the `ArgumentProcessor` there are no unnecessary comments of the private classes. The code is readable, no need to overcomplicate it by the comments. Also, I learnt something new from you which I will surely use in my future career. You can divide the code of a class into multiple files using the keyword `partial`! Thank you very much!


## Experience Of Writing The Date Converter Application

Before I could start writing my `Date Converter` application I had to import the library. But not only was it necessary to import the library `.dll`, I had to import the `nunit.framework.dll` as well. I think both the unit and API tests should remain hidden for the user and the library shouldn't require him to import the unnecessary stuff he does not have any intentions of using.

Other than that, the app was very easy to implement. I started by registering the options via `OptionBuilder` and saving the parsed value through the `.WithParametrizedAction` method to my own data storage class. This was easy thanks to the sufficient provided documentation, self explanatory method names and fluent-syntax naming style. It even looks easy. After parsing the values I just converted them to a `DateTime` struct, formatted the output by the provided format and added a help message check. Very straightforward.


## Final Words

Your library is a fine project overall. It must have taken a lot of time. It is easy to work with and I would definitely consider using it in my future project if I needed to parse the command line arguments.
