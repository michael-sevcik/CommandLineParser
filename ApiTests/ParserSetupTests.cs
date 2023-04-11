using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTests
{
    internal class ParserSetupTests
    {

        [TestMethod]
        public void Empty()
        {
            // Arrange
            var parser = new Parser();

            // Act
            var res = parser.ParseCommandLine(new string[] { });

            // Assert
            Assert.IsTrue(res);
        }


        [TestMethod]
        public void OverlapShort()
        {
            // Arrange
            var parser = new Parser();

            new OptionBuilder()
                .WithShortSynonyms(new char[] { 'l', 'w' })
                .RegisterOption(parser);

            // Act

            var res = new OptionBuilder()
                .WithShortSynonyms(new char[] {  'w' })
                .RegisterOption(parser);

            // Assert

            Assert.IsFalse(res);
        }

        [TestMethod]
        public void OverlapLong()
        {
            // Arrange
            var parser = new Parser();

            new OptionBuilder()
                .WithShortSynonyms( 'L', 'w' )
                .WithLongSynonyms("long", "words")
                .RegisterOption(parser);

            // Act

            var res = new OptionBuilder()
                .WithShortSynonyms('q' )
                .WithLongSynonyms( "long", "word" )
                .RegisterOption(parser);

            // Assert

            Assert.IsFalse(res);
        }

        [TestMethod]
        public void NoName()
        {
            // Arrange
            var parser = new Parser();

            new OptionBuilder()
                .WithShortSynonyms('L', 'w')
                .WithLongSynonyms("long", "words")
                .RegisterOption(parser);

            // Act

            var res = new OptionBuilder()
                .WithAction(default)
                .RegisterOption(parser);

            // Assert

            Assert.IsFalse(res);
        }

        [TestMethod]
        public void CaseSensitive() {
            // Arrange
            var parser = new Parser();

            new OptionBuilder()
                .WithShortSynonyms('L', 'w')
                .WithLongSynonyms("long", "words")
                .RegisterOption(parser);

            // Act

            var res = new OptionBuilder()
                .WithShortSynonyms('l')
                .WithLongSynonyms("LONG")
                .RegisterOption(parser);

            // Assert

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void PlainArgumentsFirstMandatory() {

            try
            {
                var parser = new Parser(new IPlainArgument[] {
                IPlainArgument.CreatePlainArgument<string>(s => { }, false),
                IPlainArgument.CreatePlainArgument<string>(s => { }, true),
                }
                );

            } catch // what else?
            {
                return;
            }
            
            Assert.Inconclusive();
        }

    }
}
