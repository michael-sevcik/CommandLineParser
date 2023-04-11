using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTests
{
    public class MoreActionsTests
    {

        [TestMethod]
        public void OneOptionTwoActions()
        {
            // Arrange
            var b1 = false;
            var b2 = false;
            var args = new string[] { "-q" };
            var parser = new Parser();

            new OptionBuilder()
                .WithShortSynonyms('q')
                .WithAction(() => b1 = true)
                .WithAction(() => b2 = true)
                .RegisterOption(parser);


            // Act
            var res = parser.ParseCommandLine(args);

            // Assert
            Assert.IsTrue(res);
            Assert.IsTrue(b1);
            Assert.IsFalse(b2);
        }

        [TestMethod]
        public void OneOptionTwoActionsDifferent()
        {
            // Arrange
            var b1 = false;
            var b2 = false;
            var args = new string[] { "-q", "val" };
            var parser = new Parser();

            new OptionBuilder()
                .WithShortSynonyms('q')
                .WithAction(() => b1 = true)
                .WithParametrizedAction<string>(s => b2 = true)
                .RegisterOption(parser);


            // Act
            var res = parser.ParseCommandLine(args);

            // Assert
            Assert.IsTrue(res);
            Assert.IsTrue(b1);
            Assert.IsFalse(b2);
        }

        [TestMethod]
        public void Ordered()
        {
            // Arrange
            string s = "";
            var args = new string[] { "-q" };
            var parser = new Parser();

            new OptionBuilder()
                .WithShortSynonyms('q')
                .WithAction(() => s += "a")
                .WithAction(() => s += "b")
                .RegisterOption(parser);


            // Act
            var res = parser.ParseCommandLine(args);

            // Assert
            Assert.IsTrue(res);
            Assert.AreEqual("ab", s);
        }
    }
}
