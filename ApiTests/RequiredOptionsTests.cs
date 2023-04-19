using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTests;

[TestClass]
class RequiredOptionsTests
{
    private Parser parser;

    private bool portable = false;
    private bool vortexable = false;
    private bool qourmatable = false;
    private string qourmat;
    private string format;

    [TestInitialize]
    public void SetupParser()
    {
        // Create Parser.
        parser = new();
        parser.SetPlainArgumentHelpString("Terminate option list.");

        //Create option builder object
        var optionBuilder = new OptionBuilder();

        // Create options via cool builder.

        optionBuilder
            .WithShortSynonyms('f')
            .WithLongSynonyms("format")
            .WithParametrizedAction<string>(f => format = f!)
            .RequiresParameter()
            .RegisterOption(parser);

        optionBuilder
            .WithShortSynonyms('q')
            .WithLongSynonyms("qourmat")
            .WithParametrizedAction<string>(q => qourmat = q!)
            .RegisterOption(parser);

        optionBuilder
            .WithShortSynonyms('p')
            .WithLongSynonyms("portable")
            .WithAction(() => portable = true)
            .RegisterOption(parser);

            optionBuilder
            .WithShortSynonyms('v')
            .WithLongSynonyms("vortexable")
            .WithAction(() => vortexable = true)
            .SetAsMandatory()
            .RegisterOption(parser);
    }

    [TestCleanup]
    public void CleanupParser()
    {
        format = null;
        qourmat = null;
        qourmatable = false;
        portable = false;
        vortexable = false;
    }

    [TestMethod]
    public void RequiredOption()
    {
        // Arrange
        var args = new string[] { "--vortexable"};

        // Act
        var res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsTrue(res);
        Assert.IsTrue(vortexable);
        Assert.IsFalse(portable);
    }

    [TestMethod]
    public void RequiredOptionMissing()
    {
        // Arrange
        var args = new string[] { "-p"};

        // Act
        var res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsFalse(res);
        Assert.IsFalse(vortexable);
        Assert.IsFalse(portable);
    }

    [TestMethod]
    public void RequiredOptionValid()
    {
        // Arrange
        var args = new string[] { "-v", "-q"};

        // Act
        var res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsTrue(res);
        Assert.IsTrue(vortexable);
        Assert.IsFalse(portable);
    }

    [TestMethod]
    public void RequiredOptionValidWithVagueValue()
    {
        // Arrange
        var args = new string[] {  "-q", "-v"};

        // Act
        var res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsTrue(res);
        Assert.IsTrue(vortexable);
        Assert.IsTrue(qourmatable);
        Assert.IsNull(qourmat);
        Assert.IsFalse(portable);
    }

    [TestMethod]
    public void RequiredOptionValidWithVagueValueLong()
    {
        // Arrange
        var args = new string[] {  "-q", "--vortexable"};

        // Act
        var res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsTrue(res);
        Assert.IsTrue(vortexable);
        Assert.IsNull(qourmat);
        Assert.IsTrue(qourmatable);
        Assert.IsFalse(portable);
    }

    [TestMethod]
    public void RequiredOptionValidWithVagueValueLong2()
    {
        // Arrange
        var args = new string[] {  "--qourmat", "--vortexable"};

        // Act
        var res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsTrue(res);
        Assert.IsTrue(vortexable);
        Assert.IsTrue(qourmatable);
        Assert.IsNull(qourmat);
        Assert.IsFalse(portable);
    }

    [TestMethod]
    public void RequiredOptionMissingAfterSeparator()
    {
        // Arrange
        var args = new string[] { "--", "-v"};

        // Act
        var res = parser.ParseCommandLine(args);

        // Assert
        Assert.IsFalse(res);
        Assert.IsFalse(vortexable);
        Assert.IsFalse(portable);
    }

}
