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
                        ParserErrorType.MissingMandatoryOption,
                        CreateErrorMessage(ParserErrorType.MissingMandatoryOption),
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
                        CreateErrorMessage(ParserErrorType.MissingMandatoryPlainArgument),
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
                var errorMessage = CreateErrorMessage(ParserErrorType.CouldNotParseTheParameter, argument);
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
                var errorMessage = CreateErrorMessage(ParserErrorType.MissingOptionParameter);
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
                var errorMessage = CreateErrorMessage(ParserErrorType.CouldNotParseTheParameter, argument);
                Error = new(ParserErrorType.CouldNotParseTheParameter, errorMessage, optionAwaitingParameter);
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

            var identifier = splittedArgument[0].Substring(2); // starting "--" is removed
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

            // Processing parametrized option // TODO: Consider encapsulating this in a function.

            // if there is no parameter
            if (splittedArgument.Length < 2)
            {
                if (parametrizedOption.IsParameterRequired)
                {
                    var errorMessage = CreateErrorMessage(ParserErrorType.MissingOptionParameter);
                    Error = new(ParserErrorType.MissingOptionParameter, errorMessage, findResult);
                    return false;
                }
            }

            // There is a parameter, try to parse it and handle an eventual parsing error.
            else if (!parametrizedOption.ProcessParameter(splittedArgument[1]))
            {
                var errorMessage = CreateErrorMessage(ParserErrorType.CouldNotParseTheParameter, splittedArgument[1]);
                Error = new(ParserErrorType.CouldNotParseTheParameter, errorMessage, optionAwaitingParameter);
                return false;
            }

            // add successfully parsed option that is ready to take action that is either
            // not requiring parameter or it has one already parsed.
            optionsToTakeAction.Add(parametrizedOption);
            return true;
        }

        string[] SplitOptionIdentifierParameterPair(string optionIdentifier)
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
                    optionIdentifier.Substring(0, equalsSignPosiotion), // Option identifier
                    optionIdentifier.Substring(equalsSignPosiotion + 1) // Parameter trimmed of equals sign
                };
            }

            return splittedInput;
        }

        ParserError? CheckFindResult(IOption? optionFindResult, string identifier)
        {
            if (optionFindResult is null)
            {
                var errorMessage = CreateErrorMessage(ParserErrorType.InvalidOptionIdentifier, identifier);
                return new(ParserErrorType.InvalidOptionIdentifier, errorMessage);
            }

            if (optionsToTakeAction.Contains(optionFindResult))
            {
                return new(
                    ParserErrorType.RepeatedOccurenceOfOption,
                    CreateErrorMessage(ParserErrorType.RepeatedOccurenceOfOption),
                    optionFindResult);
            }

            return null;
        }

        ParserError? ProcessArgument()
        {
            throw new NotImplementedException();
        }

        bool ProcessPlainArgumentDelimiter()
        {
            state = ParsingState.PlainArguments;
            return true;
        }

        string CreateErrorMessage(ParserErrorType type, string? additionalInfo = null)
        {
            string errorMessage = string.Empty;
            if (additionalInfo is not null)
            {
                errorMessage = type switch
                {
                    ParserErrorType.InvalidOptionIdentifier
                    => $"Option with an identifier \"{additionalInfo}\" could not be found.",
                    ParserErrorType.CouldNotParseTheParameter
                    => $"Option could not parse the argument: \"{additionalInfo}\".",
                    ParserErrorType.MissingMandatoryOption
                    => "A mandatory option did not occur on the command-line. " + additionalInfo,
                    ParserErrorType.MissingMandatoryPlainArgument
                    => "A mandatory plain argument did not occur on the command-line. " + additionalInfo,
                    ParserErrorType.MissingOptionParameter
                    => "There was no parameter for option requiring one. " + additionalInfo,
                    ParserErrorType.RepeatedOccurenceOfOption
                    => "The given option already occurred on the commaind-line. " + additionalInfo,
                    ParserErrorType.Other
                    => "Not closer defined error. " + additionalInfo,
                    _ => throw new AssertionException("Enum type has not defined value")
                };
            }
            else
            {
                errorMessage = type switch
                {
                    ParserErrorType.InvalidOptionIdentifier
                    => $"Option could not be found.",
                    ParserErrorType.CouldNotParseTheParameter
                    => $"Option could not parse its argument.",
                    ParserErrorType.MissingMandatoryOption
                    => "A mandatory option did not occur on the command-line.",
                    ParserErrorType.MissingMandatoryPlainArgument
                    => "A mandatory plain argument did not occur on the command-line.",
                    ParserErrorType.MissingOptionParameter
                    => "There was no parameter for option requiring one.",
                    ParserErrorType.RepeatedOccurenceOfOption
                    => "The given option already occurred on the commaind-line.",
                    ParserErrorType.Other
                    => "Not closer defined error.",
                    _ => throw new AssertionException("Enum type has not defined value")
                };
            }

            return errorMessage;
        }

        private ArgumentType DetermineArgumentType(string argument)
        => argument switch
        {
            "--" => ArgumentType.PlainArgumentsDelimiter,
            _ when argument.Length > 2 && argument.StartsWith("--") => ArgumentType.LongOptionIdentifier,
            _ when argument.Length >= 2 && argument.StartsWith('-') => ArgumentType.ShortOptionIdentifier,
            _ => ArgumentType.Other
        };
    }
}

