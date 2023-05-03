using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTests
{
    [TestClass]
    public class MultiArgsTests
    {
        [DataRow(new string[] {"-a" , "a=1,2,3" }, true, 3)]
        [DataRow(new string[] {"-a" , "1,2,3" }, true, 1, ';')]
        [DataRow(new string[] { "--a=1a2a3" }, true, 3, 'a')]
        [DataRow(new string[] { "--a=" }, true, 0, '=')]
        [DataRow(new string[] { "--a=" }, true, 0, 'a')]
        [DataRow(new string[] { "--a=" }, true, 0, '-')]
        [DataRow(new string[] { "--a=aaa" }, true, 4, 'a')]
        [DataRow(new string[] { "--a=,,," }, true, 4)]
        [DataRow(new string[] { "--ab=,,," }, false, 0, 'b')]
        [DataTestMethod]
        public void MultiArgs(string[] arg, bool valid, int expectedCount, char separator = ',')
        {
            // Arrange
            var parser = new Parser();

            string[] resultArray = null;

            var builder = new OptionBuilder();
            builder.WithMultipleParametersAction<string>(x => { resultArray = x; }).
                SetAsMandatory().
                RequiresParameter().
                WithShortSynonyms(new char[] { 'a' }).
                WithLongSynonyms("a").
                WithSeparator(separator).
                RegisterOption(parser);

            // Act
            var res = parser.ParseCommandLine(arg);

            // Assert
            if(valid)
            {
                Assert.IsTrue(res);
                Assert.AreEqual(expectedCount, resultArray.Count());
            }
            else
            {
                Assert.IsFalse(res);
                Assert.IsNull(resultArray);
            }
        }
        
    }
}
