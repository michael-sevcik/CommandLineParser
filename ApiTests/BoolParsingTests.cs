using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTests
{
    [TestClass]
    public class BoolParsingTests
    {
        [DataRow("true")]
        [DataTestMethod]
        public void TestParseBool_True(string value)
        {
            // Arrange
            string[] args = { "-t", value };
            var parser = new Parser();
            var optionBuilder = new OptionBuilder();
            optionBuilder.WithShortSynonyms('t')
                          .WithParametrizedAction<bool?>(value => Assert.IsTrue(value.Value))
                          .RequiresParameter()
                          .RegisterOption(parser);

            // Act
            parser.ParseCommandLine(args);

            // Assert
            Assert.IsNull(parser.Error);
        }

        [DataRow("false")]
        [DataTestMethod]
        public void TestParseBool_False(string value)
        {
            // Arrange
            string[] args = { "-f", value };
            var parser = new Parser();
            var optionBuilder = new OptionBuilder();
            optionBuilder.WithShortSynonyms('f')
                          .WithParametrizedAction<bool?>(value => Assert.IsFalse(value.Value))
                          .RequiresParameter()
                          .RegisterOption(parser);

            // Act
            var res = parser.ParseCommandLine(args);

            // Assert
            Assert.IsTrue(res);
            Assert.IsNull(parser.Error);
        }

        [DataRow("-")]
        [DataRow("2")]
        [DataRow("truex")]
        [DataRow("tr")]
        [DataRow("+1")]
        [DataRow("-0")]
        [DataRow("Ne")]
        [DataRow("Ahojd")]
        [DataRow("")]
        [DataRow("z")]
        [DataTestMethod]
        public void TestParseBool_Invalid(string value)
        {
            // Arrange
            string[] args = { "-f", value };
            var parser = new Parser();
            var optionBuilder = new OptionBuilder();
            optionBuilder.WithShortSynonyms('f')
                          .WithParametrizedAction<bool?>(value => { })
                          .RequiresParameter()
                          .RegisterOption(parser);

            // Act
            var res = parser.ParseCommandLine(args);

            // Assert
            Assert.IsFalse(res);
            Assert.IsNotNull(parser.Error);
        }

    }

}
