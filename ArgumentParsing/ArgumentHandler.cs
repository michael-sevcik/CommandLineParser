using ArgumentParsing.Option;
using NUnit.Framework;
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
        enum ArgumentType : byte
        {
            ShortOptionIdentifier,
            LongOptionIdentifier,
            PlainArgumentsDelimiter,
            Other
        }

        enum ParsingState : byte
        {
            PlainArguments,
            Mixed
        }

        public readonly struct ArgumentProcessingResult
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

        readonly IPlainArgument[]? plainArguments;
        readonly OptionSet.OptionSet options;
        readonly IReadOnlyList<IPlainArgument>? mandatoryPlainArguments;

        readonly HashSet<IOption> optionsToTakeAction = new();
        readonly HashSet<IPlainArgument> plainArgumentsToTakeAction = new();
        readonly List<string> excessivePlainArgumentsEntries = new();

        IParametrizedOption? optionAwaitingParameter = null;

        ParsingState state = ParsingState.Mixed;

        int nextPlainArgumentPosition = 0;

        /// <summary>
        /// If an error occurs during parsing it gets a instance of <see cref="ParserError"/> that is describing the problem, otherwise null.
        /// </summary>
        public ParserError? Error { get; private set; } = null;

        public ArgumentProcessor(OptionSet.OptionSet options, IPlainArgument[]? plainArguments, IPlainArgument[]? mandatoryPlainArguments)
        {
            this.plainArguments = plainArguments;
            this.options = options;
            this.mandatoryPlainArguments = mandatoryPlainArguments;
        }

        public bool ProcessArgument(string argument) 
        {
            if (state == ParsingState.PlainArguments)
            {
                return HandlePlainArgument(argument);
            }

            return DetermineArgumentType(argument) switch
            {
                ArgumentType.ShortOptionIdentifier => ProcessPossibleWaitingOption() && HandleShortOptionIdentifier(argument),
                ArgumentType.LongOptionIdentifier => ProcessPossibleWaitingOption() && HandleLongOptionIdentifier(argument),
                ArgumentType.PlainArgumentsDelimiter => ProcessPossibleWaitingOption() && ProcessPlainArgumentDelimiter(),
                ArgumentType.Other when optionAwaitingParameter is not null => PassArgumentToWaitingOption(argument),
                ArgumentType.Other => HandlePlainArgument(argument),
                _ => throw new AssertionException("Not defined Argument type value."), // Cannot occur.
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
                        ErrorType.MissingMandatoryOption,
                        CreateErrorMessage(ErrorType.MissingMandatoryOption),
                        option);

                    return false;
                }
            }

            foreach (var plainArgument in mandatoryPlainArguments ?? Array.Empty<IPlainArgument>())
            {
                if (!plainArgumentsToTakeAction.Contains(plainArgument))
                {
                    Error = new(
                        ErrorType.MissingMandatoryPlainArgument,
                        CreateErrorMessage(ErrorType.MissingMandatoryPlainArgument),
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
                var errorMessage = CreateErrorMessage(ErrorType.CouldNotParseTheParameter, argument);
                Error = new(ErrorType.CouldNotParseTheParameter, errorMessage, optionAwaitingParameter);
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
                var errorMessage = CreateErrorMessage(ErrorType.MissingOptionParameter);
                Error = new(ErrorType.MissingOptionParameter, errorMessage, optionAwaitingParameter);
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
                var errorMessage = CreateErrorMessage(ErrorType.CouldNotParseTheParameter, argument);
                Error = new(ErrorType.CouldNotParseTheParameter, errorMessage, optionAwaitingParameter);
                return false;
            }

            plainArgumentsToTakeAction.Add(plainArgumentToProcess);
            return true;
        }

        private bool HandleShortOptionIdentifier(string identifier)
        {
            var findResult = (identifier.Length == 2) ? options.Find(identifier[1]) : null; // Check the identifier and get the identifier letter.
            if ((Error = CheckFindResult(findResult, identifier)) is not null)
            {
                return false;
            }

            if (findResult is IParametrizedOption parametrizedOption)
            {
                optionAwaitingParameter = parametrizedOption;
            }
            else
            {
                optionsToTakeAction.Add(findResult!); // CheckFindResult also checks null value.
            }

            return true;
        }

        private bool HandleLongOptionIdentifier(string argument)
        {

            var splittedArgument = SplitOptionIdentifierParameterPair(argument);

            var identifier = splittedArgument[0][2..]; // starting "--" is removed
            var findResult = options.Find(identifier);
            ParserError? checkResult;
            if ((checkResult = CheckFindResult(findResult, identifier)) is not null)
            {
                this.Error = checkResult;
                return false;
            }

            if (findResult is not IParametrizedOption parametrizedOption)
            {
                optionsToTakeAction.Add(findResult!); // CheckFindResult also checks null value.
                return true;
            }

            // Processing parametrized option 
            // if there is no parameter
            if (splittedArgument.Length < 2)
            {
                if (parametrizedOption.IsParameterRequired)
                {
                    var errorMessage = CreateErrorMessage(ErrorType.MissingOptionParameter);
                    Error = new(ErrorType.MissingOptionParameter, errorMessage, findResult);
                    return false;
                }
            }

            // There is a parameter, try to parse it and handle an eventual parsing error.
            else if (!parametrizedOption.ProcessParameter(splittedArgument[1]))
            {
                var errorMessage = CreateErrorMessage(ErrorType.CouldNotParseTheParameter, splittedArgument[1]);
                Error = new(ErrorType.CouldNotParseTheParameter, errorMessage, optionAwaitingParameter);
                return false;
            }

            // add successfully parsed option that is ready to take action that is either
            // not requiring parameter or it has one already parsed.
            optionsToTakeAction.Add(parametrizedOption);
            return true;
        }

        static string[] SplitOptionIdentifierParameterPair(string optionIdentifier)
        {
            var equalsSignPosiotion = optionIdentifier.IndexOf("=");
            string[] splittedInput;
            if (equalsSignPosiotion < 0)
            {
                splittedInput = new string[] { optionIdentifier };
            }
            else
            {
                splittedInput = new string[]
                {
                    optionIdentifier[..equalsSignPosiotion], // Option identifier
                    optionIdentifier[(equalsSignPosiotion + 1)..] // Parameter trimmed of equals sign
                };
            }

            return splittedInput;
        }

        ParserError? CheckFindResult(IOption? optionFindResult, string identifier)
        {
            if (optionFindResult is null)
            {
                var errorMessage = CreateErrorMessage(ErrorType.InvalidOptionIdentifier, identifier);
                return new(ErrorType.InvalidOptionIdentifier, errorMessage);
            }

            if (optionsToTakeAction.Contains(optionFindResult))
            {
                return new(
                    ErrorType.RepeatedOccurenceOfOption,
                    CreateErrorMessage(ErrorType.RepeatedOccurenceOfOption),
                    optionFindResult);
            }

            return null;
        }

        bool ProcessPlainArgumentDelimiter()
        {
            state = ParsingState.PlainArguments;
            return true;
        }

        static string CreateErrorMessage(ErrorType type, string? additionalInfo = null)
        {
            string errorMessage;
            if (additionalInfo is not null)
            {
                errorMessage = type switch
                {
                    ErrorType.InvalidOptionIdentifier
                    => $"Option with an identifier \"{additionalInfo}\" could not be found.",
                    ErrorType.CouldNotParseTheParameter
                    => $"Option could not parse the argument: \"{additionalInfo}\".",
                    ErrorType.MissingMandatoryOption
                    => "A mandatory option did not occur on the command-line. " + additionalInfo,
                    ErrorType.MissingMandatoryPlainArgument
                    => "A mandatory plain argument did not occur on the command-line. " + additionalInfo,
                    ErrorType.MissingOptionParameter
                    => "There was no parameter for option requiring one. " + additionalInfo,
                    ErrorType.RepeatedOccurenceOfOption
                    => "The given option already occurred on the commaind-line. " + additionalInfo,
                    ErrorType.Other
                    => "Not closer defined error. " + additionalInfo,
                    _ => throw new AssertionException("Enum type has not defined value")
                };
            }
            else
            {
                errorMessage = type switch
                {
                    ErrorType.InvalidOptionIdentifier
                    => $"Option could not be found.",
                    ErrorType.CouldNotParseTheParameter
                    => $"Option could not parse its argument.",
                    ErrorType.MissingMandatoryOption
                    => "A mandatory option did not occur on the command-line.",
                    ErrorType.MissingMandatoryPlainArgument
                    => "A mandatory plain argument did not occur on the command-line.",
                    ErrorType.MissingOptionParameter
                    => "There was no parameter for option requiring one.",
                    ErrorType.RepeatedOccurenceOfOption
                    => "The given option already occurred on the commaind-line.",
                    ErrorType.Other
                    => "Not closer defined error.",
                    _ => throw new AssertionException("Enum type has not defined value")
                };
            }

            return errorMessage;
        }

        private static ArgumentType DetermineArgumentType(string argument)
        => argument switch
        {
            "--" => ArgumentType.PlainArgumentsDelimiter,
            _ when argument.Length > 2 && argument.StartsWith("--") => ArgumentType.LongOptionIdentifier,
            _ when argument.Length >= 2 && argument.StartsWith('-') => ArgumentType.ShortOptionIdentifier,
            _ => ArgumentType.Other
        };
    }
}


