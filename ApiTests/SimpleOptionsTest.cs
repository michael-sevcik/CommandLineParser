using System;
using ArgumentParsing;

namespace ApiTests;

[TestClass]
public class SimpleOptionsTest
{
    private Parser parser;

    private string result;
    private bool portable = false;

    [TestInitialize]
    public void SetupParser()
    {
        // Create Parser.
        parser = new();

        //Create option builder object
        var optionBuilder = new OptionBuilder();

        // Create options via cool builder.

        optionBuilder
            .WithShortSynonyms('f')
            .WithLongSynonyms("format")
            .WithParametrizedAction<string>(format => result = format!)
            .RequiresParameter()
            .WithHelpString("Specify output format, possibly overriding the format specified in the environment variable TIME.")
            .RegisterOption(parser);

        optionBuilder
            .WithShortSynonyms('p')
            .WithLongSynonyms("portable")
            .WithAction(() => portable = true)
            .WithHelpString("Specify output format, possibly overriding the format specified in the environment variable TIME.")
            .RegisterOption(parser);
    }

    [TestCleanup]
    public void CleanupParser()
    {
        result = null;
        portable = false;
    }

   

    [TestMethod]
    public void ShortOption()
    {
        // Arrange
        var args = new string[] { "-p" };

        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsTrue(res);
        Assert.IsTrue(portable);
    }

    [TestMethod]
    public void LongOption()
    {
        // Arrange
        var args = new string[] { "--portable" };

        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsTrue(res);
        Assert.IsTrue(portable);
    }

    [TestMethod]
    public void ShortSynonymArgument()
    {
        // Arrange
        var args = new string[] { "-f", "json" };
        var expectedOutput = "json";

        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsTrue(res);
        Assert.AreEqual(expectedOutput, result);
    }

    [TestMethod]
    public void LongSynonymArgument()
    {
        // Arrange
        var args = new string[] { "--format=csv" };
        var expectedOutput = "csv";

        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsTrue(res);
        Assert.AreEqual(expectedOutput, result);
    }

  

    [TestMethod]
    public void InvalidOptionFails()
    {
        // Arrange
        var args = new string[] { "-x" };
        // "Invalid option: -x";

        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsFalse(res);
        Assert.AreEqual(ParserErrorType.InvalidOptionIdentifier, parser.Error?.type);
    }

    [TestMethod]
    public void MissingParameterFails()
    {
        // Arrange
        var args = new string[] { "-f" };
        // "Option -f requires a parameter.";

        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsFalse(res);
        Assert.AreEqual(ParserErrorType.MissingOptionParameter, parser.Error?.type);
    }

    [TestMethod]

    public void NotRequiredOptionsAreNotRequired()
    {
        // Arrange
        var args = new string[] { };
        // "Option -f requires a parameter.";

        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsTrue(res);
    }
    [TestMethod]
    public void TwoOptions()
    {
        // Arrange
        var args = new string[] { "-p", "-f", "format" };
        // "Option -f requires a parameter.";

        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsTrue(res);
        Assert.IsTrue(portable);
        Assert.AreEqual(result, "format");
    }
    [TestMethod]
    public void TwoOptionsCondensed()
    {
        // Arrange
        var args = new string[] { "-pf=format" };
        // "Option -f requires a parameter.";

        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsFalse(res);
        Assert.IsFalse(portable);
        Assert.AreEqual(result, null);
    }
    [TestMethod]
    public void RepeatedShortOption()
    {
        // Arrange
        var args = new string[] { "-p", "-p" };
        // "Option -f requires a parameter.";

        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsFalse(res);
        Assert.IsFalse(portable);
        Assert.IsNotNull(parser.Error);
    }

    [TestMethod]
    public void RepeatedShortOptionCondensed()
    {
        // Arrange
        var args = new string[] { "-pp" };
        // "Option -f requires a parameter.";

        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsFalse(res);
        Assert.IsFalse(portable);
        Assert.IsNotNull(parser.Error);
    }


    [TestMethod]
    public void RepeatedLongOption()
    {
        // Arrange
        var args = new string[] { "--portable", "--portable" };
        // "Option -f requires a parameter.";

        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsFalse(res);
        Assert.IsFalse(portable);
        Assert.IsNotNull(parser.Error);
    }

    [TestMethod]
    public void ShortOptionAndEqualsSign()
    {
        // Arrange
        var args = new string[] { "--portable", "--portable" };
        // "Option -f requires a parameter.";

        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsFalse(res);
        Assert.IsFalse(portable);
        Assert.IsNotNull(parser.Error);
    }


    [TestMethod]
    public void OnFailNothingIsInvoked() {
        // Arrange
        var args = new string[] { "-x" };
        // "Invalid option: -x";

        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsFalse(res);
        Assert.IsFalse(portable);
        Assert.IsNull(result);
    }

    [TestMethod]
    public void OnFailNothingIsInvokedEvenWhenValidOptionsAreSpecified() {
        // Arrange
        var args = new string[] { "-x", "-p" };
        // "Invalid option: -x";

        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsFalse(res);
        Assert.IsFalse(portable);
        Assert.IsNull(result);
    }

    [TestMethod]
    public void OnFailNothingIsInvokedEvenWhenValidOptionsAreSpecifiedBeforeInvalid() {
        // Arrange
        var args = new string[] { "-p", "-x" };
        // "Invalid option: -x";

        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsFalse(res);
        Assert.IsFalse(portable);
        Assert.IsNull(result);
    }

    [TestMethod]
    public void OnFailNothingIsInvokedEvenWhenValidOptionsAreSpecifiedInMiddle() {
        // Arrange
        var args = new string[] { "-p", "-x", "-f", "json" };
        // "Invalid option: -x";

        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsFalse(res);
        Assert.IsFalse(portable);
        Assert.IsNull(result);
    }

    [TestMethod]
    public void OnFailNothingIsInvokedEvenWhenValidOptionsAreSpecifiedAfterAllOptions() {
        // Arrange
        var args = new string[] { "-p", "-f", "json", "-x" };
        // "Invalid option: -x";

        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsFalse(res);
        Assert.IsFalse(portable);
        Assert.IsNull(result);
    }

    [TestMethod]
    public void OnFailNothingIsInvokedEvenWhenValidOptionsAreSpecifiedAdvanced() {
        // Arrange
        var args = new string[] { "-p", "-f", "json", "-p" };
        // "Invalid option: -x";

        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsFalse(res);
        Assert.IsFalse(portable);
        Assert.IsNull(result);
    }

    [TestMethod]
    public void ValidParametersSucceed() {
        // Arrange
        var args = new string[] { "-p", "invalid" };

        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsTrue(res);
        Assert.IsTrue(portable);
        Assert.IsNull(result);
    }

    [TestMethod]
    public void Separator() {
        // Arrange
        var args = new string[] { "-p",  "--", "-f", "json" };

        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsTrue(res);
        Assert.IsTrue(portable);
        Assert.IsNull(result);
    }

    [TestMethod]
    public void DashInside() {
        // Arrange
        var args = new string[] {"-f", "j-son" };

        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsTrue(res);
        Assert.AreEqual("j-son", result);
    }

    [TestMethod]
    public void EqualsSignInside() {
        // Arrange
        var args = new string[] {"--format=equals=notation" };

        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsTrue(res);
        Assert.AreEqual("equals=notation", result);
    }

    [TestMethod]
    public void Empty() {
        // Arrange
        var args = new string[] {"--format=" };

        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsTrue(res);
        Assert.AreEqual("", result);
    }

    [TestMethod]
    public void ShortCaseSensitive() {
        // Arrange
        var args = new string[] {"-P" };

        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsFalse(res);
        Assert.IsFalse(portable);
        Assert.IsNull(result);
    }

    [TestMethod]
    public void LongCaseSensitive() {
        // Arrange
        var args = new string[] {"--Portable" };

        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsFalse(res);
        Assert.IsFalse(portable);
        Assert.IsNull(result);
    }

}