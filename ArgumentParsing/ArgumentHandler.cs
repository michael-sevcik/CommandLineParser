using ArgumentParsing.Option;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgumentParsing;

public partial class Parser
{
    partial class ArgumentProcessor // TODO: Should there be a clear/restart method?
    {
        public struct ArgumentProcessingResult
        {
            public readonly IOption[] optionsToTakeAction;
            public readonly IPlainArgument[] plainArgumentsToTakeAction;
            public readonly string[] excessivePlainArgumentsEntries;

            public ArgumentProcessingResult(
                IOption[] optionsToTakeAction,
                IPlainArgument[] plainArgumentsToTakeAction,
                string[] excessivePlainArgumentsEntries)
            {
                this.optionsToTakeAction = optionsToTakeAction;
                this.plainArgumentsToTakeAction = plainArgumentsToTakeAction;
                this.excessivePlainArgumentsEntries = excessivePlainArgumentsEntries;
            }
        }

        enum ParsingState : byte
        {
            PlainArguments,
            Mixed
        }

        readonly IPlainArgument[]? plainArguments;
        readonly OptionSet.OptionSet options;
        readonly IReadOnlyList<IPlainArgument>? mandatoryPlainArguments;

        HashSet<IOption> optionsToTakeAction = new();
        HashSet<IPlainArgument> plainArgumentsToTakeAction = new();
        List<string> excessivePlainArgumentsEntries = new();

        IParametrizedOption? optionAwaitingParameter = null;

        ParsingState state = ParsingState.Mixed;

        int nextPlainArgumentPosition = 0;

        /// <summary>
        /// If an error occurs during parsing it gets a instance of <see cref="ParserError"/> that is describing the problem, otherwise null.
        /// </summary>
        public ParserError? Error 
        { 
            get; 
            private set; 
        } = null;

        public ArgumentProcessor(Parser parser)
        {
            plainArguments = parser.plainArguments;
            options = parser.options;
            mandatoryPlainArguments = parser.mandatoryPlainArguments;
        }

        public bool ProcessArgument(string argument) // TODO: test what happens when an argument enclosed in '"' is passed. If it is already trimmed or not.
        {            
            if (state == ParsingState.PlainArguments)
            {
                return HandlePlainArgument(argument);
            }

            return DetermineArgumentType(argument) switch
            {
                // TODO: If the option identifier is invalid, should it be passed to optionAwaitingParameter or it should be perceived as a error?
                ArgumentType.ShortOptionIdentifier => ProcessPossibleWaitingOption() && HandleShortOptionIdentifier(argument),
                ArgumentType.LongOptionIdentifier => ProcessPossibleWaitingOption() && HandleLongOptionIdentifier(argument),
                ArgumentType.PlainArgumentsDelimiter => ProcessPossibleWaitingOption() && ProcessPlainArgumentDelimiter(),
                ArgumentType.Other when optionAwaitingParameter is not null => PassArgumentToWaitingOption(argument),
                ArgumentType.Other => HandlePlainArgument(argument),
                _ => false, // Cannot occur.
            };
        }

        public bool FinalizeProcessing(out ArgumentProcessingResult result)
        {
            result = new();

            if (!ProcessPossibleWaitingOption())
            {
                return false;
            }

            foreach (var option in options.MandatoryOptions)
            {
                if (!optionsToTakeAction.Contains(option))
                {
                    Error = new(
                        ParserErrorType.MissingMandatoryOption,
                        "A mandatory option did not occur on the command-line.",
                        option);

                    return false;
                }
            }

            foreach (var plainArgument in mandatoryPlainArguments ?? Array.Empty<IPlainArgument>())
            {
                if (!plainArgumentsToTakeAction.Contains(plainArgument))
                {
                    Error = new(
                        ParserErrorType.MissingMandatoryPlainArgument,
                        "A mandatory plain argument did not occur on the command-line.",
                        plainArgumentInError: plainArgument);

                    return false;
                }
            }

            result = new(
                optionsToTakeAction: optionsToTakeAction.ToArray(),
                plainArgumentsToTakeAction: plainArgumentsToTakeAction.ToArray(),
                excessivePlainArgumentsEntries: excessivePlainArgumentsEntries.ToArray());

            optionsToTakeAction.Clear();
            plainArgumentsToTakeAction.Clear();
            excessivePlainArgumentsEntries.Clear();

            return true;
        }
        private bool PassArgumentToWaitingOption(string argument)
        {
            if (!optionAwaitingParameter!.ProcessParameter(argument))
            {
                var errorMessage = $"Option could not parse the argument: \"{argument}\".";
                Error = new(ParserErrorType.CouldNotParseTheParameter, errorMessage, optionAwaitingParameter);
                return false;
            }

            optionsToTakeAction.Add(optionAwaitingParameter);
            optionAwaitingParameter = null;
            return true;
        }

        /// <summary>
        /// Processes the option waiting for parameter that is stored in <see cref="optionAwaitingParameter"/> when there is no argument to pass.
        /// </summary>
        /// <returns>True if the processing was successful or there was no option waiting for an argument.</returns>
        bool ProcessPossibleWaitingOption()
        {
            if (optionAwaitingParameter is null)
            {
                return true;
            }

            if (optionAwaitingParameter.IsParameterRequired)
            {
                var errorMessage = "There was no parameter for option requiring one.";
                Error = new(ParserErrorType.MissingOptionParameter, errorMessage, optionAwaitingParameter);
                return false;
            }
            else
            {
                optionsToTakeAction.Add(optionAwaitingParameter);
                optionAwaitingParameter = null;
            }
            
            return true;
        }

        private bool HandlePlainArgument(string argument)
        {
            if ((plainArguments?.Length ?? 0) <= nextPlainArgumentPosition)
            {
                excessivePlainArgumentsEntries.Add(argument);
                return true;
            }

            // plainArguments won't ever be null here thanks to the starting check
            var plainArgumentToProcess = plainArguments![nextPlainArgumentPosition++];

            if (!plainArgumentToProcess.ProcessParameter(argument))
            {
                var errorMessage = $"Option could not parse the argument: \"{argument}\".";
                Error = new(ParserErrorType.CouldNotParseTheParameter, errorMessage, optionAwaitingParameter);
                return false;
            }

            plainArgumentsToTakeAction.Add(plainArgumentToProcess);
            return true;
        }

        private bool HandleShortOptionIdentifier(string identifier)
        {
            var findResult = options.Find(identifier[1]); // get the identifier letter.
            if (findResult is null)
            {
                var errorMessage = $"Option with an identifier \"{identifier}\" could not be found."; // TODO: Try to encapsulate error in a function together.
                Error = new(ParserErrorType.InvalidOptionIdentifier, errorMessage);
                return false;
            }

            if (optionsToTakeAction.Contains(findResult)) // double occurrence of the same option // TODO: Consider MOVING this to a method.
            {
                Error = new(ParserErrorType.RepeatedOccurenceOfOption, "The given option already occurred on the commaind-line.", findResult);
                return false;
            }

            if (findResult is IParametrizedOption parametrizedOption)
            {
                optionAwaitingParameter = parametrizedOption;
            }
            else 
            { 
                optionsToTakeAction.Add(findResult);
            }

            return true;
        }

        private bool HandleLongOptionIdentifier(string identifier)
        {
            var splitted = identifier.Split('='); // TODO: Consider using find and substring.
            var optionFindResult = options.Find(splitted[0].Substring(2)); // starting "--" is removed
            if (optionFindResult is null)
            {
                var errorMessage = $"Option with an identifier \"{identifier}\" could not be found."; // TODO: duplicit
                Error = new(ParserErrorType.InvalidOptionIdentifier, errorMessage);
                return false;
            }

            if (optionsToTakeAction.Contains(optionFindResult)) // double occurrence of the same option  // TODO: duplicit
            {
                Error = new(ParserErrorType.RepeatedOccurenceOfOption, "The given option already occurred on the commaind-line.", optionFindResult);
                return false;
            }

            if (optionFindResult is not IParametrizedOption parametrizedOption)
            {
                optionsToTakeAction.Add(optionFindResult);
                return true;
            }

            // Processing parametrized option // TODO: Consider encapsulating this in a function.

            // if there is no parameter
            if (splitted.Length < 2)
            {
                if (parametrizedOption.IsParameterRequired)
                {
                    var errorMessage = "There was no parameter for option requiring one.";
                    Error = new(ParserErrorType.MissingOptionParameter, errorMessage, optionFindResult);
                    return false;
                }
            }

            // There is a parameter, try to parse it and handle an eventual parsing error.
            else if (!parametrizedOption.ProcessParameter(splitted[1]))
            {
                var errorMessage = $"Option could not parse the argument: \"{splitted[1]}\".";
                Error = new(ParserErrorType.CouldNotParseTheParameter, errorMessage, optionAwaitingParameter);
                return false;
            }

            // add successfully parsed option that is ready to take action that is either
            // not requiring parameter or it has one already parsed.
            optionsToTakeAction.Add(parametrizedOption);
            return true;
        }

        bool ProcessPlainArgumentDelimiter()
        {
            state = ParsingState.PlainArguments;
            return true;
        }

        void HandleError(string message) {  } // TODO: remove?

        private ArgumentType DetermineArgumentType(string argument)
        => argument switch
        {
            "--" => ArgumentType.PlainArgumentsDelimiter,
            _ when argument.Length > 2 && argument.StartsWith("--") => ArgumentType.ShortOptionIdentifier,
            _ when argument.Length >= 2 && argument.StartsWith('-') => ArgumentType.ShortOptionIdentifier,
            _ => ArgumentType.Other
        };
    }
}

