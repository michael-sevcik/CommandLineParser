using System.Linq;
namespace MultipleParameterOptionTests
{
    public class IntMultipleParameterOptionParsingTests
    {

        [Test]
        [TestCase("-10,-100,156", new int[] {-10,-100,156})]
        [TestCase("150", new int[] { 150 })]
        [TestCase("-164654,0", new int[] { -164654,0 })]
        [TestCase("2147483647,-1,-2,-3", new int[] { 2147483647 , -1,-2,-3})]
        public void SimpleTestWithoutBoundsShouldPass(string input, int[] expectedOutput)
        {
            OptionBuilder ob = new OptionBuilder();
            int[]? result = null;
            ob.WithMultipleParametersAction<int>((int[]? parsedInt) => result = parsedInt).
                WithSeparator(',');
            var option = (IParametrizedOption)ob.CreateParticularOptionForRegistration();
            var success = option.ProcessParameter(input);
            option.TakeAction();
            Assert.AreEqual(true, success);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        [TestCase("0,1,2,3", new int[] { 0 ,1,2,3})]
        [TestCase("150", new int[] { 150 })]
        [TestCase("2147483647,100", new int[] { 2147483647 ,100})]
        public void SimpleTestWithLowerBoundShouldPass(string input, int[] expectedOutput)
        {
            OptionBuilder ob = new OptionBuilder();
            int[]? result = null;
            ob.WithMultipleParametersAction<int>((int[]? parsedInt) => result = parsedInt).
                WithLowerBound(0);

            var option = (IParametrizedOption)ob.CreateParticularOptionForRegistration();
            var success = option.ProcessParameter(input);
            option.TakeAction();
            Assert.AreEqual(true, success);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        [TestCase("0,156,-1", null)]
        [TestCase("-150,1,0", null)]
        [TestCase("156,156656,64565,-147483647,156", null)]
        public void SimpleTestWithLowerBoundShouldNotPass(string input, int[]? expectedOutput)
        {
            OptionBuilder ob = new OptionBuilder();
            int[]? result = null;
            ob.WithMultipleParametersAction<int>((int[]? parsedInt) => result = parsedInt).
                WithLowerBound(0);

            var option = (IParametrizedOption)ob.CreateParticularOptionForRegistration();
            var success = option.ProcessParameter(input);
            option.TakeAction();
            Assert.AreEqual(false, success);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        [TestCase("0,1,2,3", new int[] { 0, 1, 2, 3 })]
        [TestCase("150,156654", new int[] { 150, 156654 })]
        [TestCase("2147483647", new int[] { 2147483647 })]
        public void SimpleTestWithUpperBoundShouldPass(string input, int[] expectedOutput)
        {
            OptionBuilder ob = new OptionBuilder();
            int[]? result = null;
            ob.WithMultipleParametersAction<int>((int[]? parsedInt) => result = parsedInt).
                WithUpperBound(2147483647);

            var option = (IParametrizedOption)ob.CreateParticularOptionForRegistration();
            var success = option.ProcessParameter(input);
            option.TakeAction();
            Assert.AreEqual(true, success);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        [TestCase("0,1", null)]
        [TestCase("-150,-156,-16161,0,1", null)]
        [TestCase("-14748364,-156,0,1565,0,0,0", null)]
        public void SimpleTestWithUpperBoundShouldNotPass(string input, int[]? expectedOutput)
        {
            OptionBuilder ob = new OptionBuilder();
            int[]? result = null;
            ob.WithMultipleParametersAction<int>((int[]? parsedInt) => result = parsedInt).
                WithUpperBound(0);

            var option = (IParametrizedOption)ob.CreateParticularOptionForRegistration();
            var success = option.ProcessParameter(input);
            option.TakeAction();
            Assert.AreEqual(false, success);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        [TestCase("0,150,156465,456465", new int[] { 0, 150, 156465, 456465 })]
        [TestCase("150,1,23,32", new int[] { 150, 1, 23, 32 })]
        [TestCase("2147483647", new int[] { 2147483647 })]
        public void SimpleTestWithUpperAndLowerBoundShouldPass(string input, int[] expectedOutput)
        {
            OptionBuilder ob = new OptionBuilder();
            int[]? result = null;
            ob.WithMultipleParametersAction<int>((int[]? parsedInt) => result = parsedInt).
                WithUpperBound(2147483647).
                WithLowerBound(0);

            var option = (IParametrizedOption)ob.CreateParticularOptionForRegistration();
            var success = option.ProcessParameter(input);
            option.TakeAction();
            Assert.AreEqual(true, success);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        [TestCase("156,1566,-1", null)]
        [TestCase("10,20,30,2147483647", null)]
        public void SimpleTestWithUpperAndLowerBoundShouldNotPass(string input, int? expectedOutput)
        {
            OptionBuilder ob = new OptionBuilder();
            int[]? result = null;
            ob.WithMultipleParametersAction<int>((int[]? parsedInt) => result = parsedInt).
                WithUpperBound(2147483646).
                WithLowerBound(0);

            var option = (IParametrizedOption)ob.CreateParticularOptionForRegistration();
            var success = option.ProcessParameter(input);
            option.TakeAction();
            Assert.AreEqual(false, success);
            Assert.AreEqual(expectedOutput, result);
        }

    }
    public class BoolMultipleParameterOptionParsingTests
    {
        [Test]
        [TestCase("True,true,TRUE")]
        [TestCase("true,True")]
        [TestCase("TRUE,True,TRuE")]

        public void SimpleTestShouldReturnTrue(string input)
        {
            OptionBuilder ob = new OptionBuilder();
            bool[]? result = null;
            ob.WithMultipleParametersAction<bool>((bool[]? parsedInt) => result = parsedInt);
            var option = (IParametrizedOption)ob.CreateParticularOptionForRegistration();
            var success = option.ProcessParameter(input);
            option.TakeAction();
            Assert.AreEqual(true, success);
            var splittedInput = input.Split(',');
            var expectedResult = Enumerable.Repeat(true, splittedInput.Length).ToArray();
            Assert.AreEqual(expectedResult,result);
        }

        [Test]
        [TestCase("False,false,FalsE")]
        [TestCase("FALSE,false,false,False,falSE")]
        [TestCase("false")]

        public void SimpleTestShouldReturnFalse(string input)
        {
            OptionBuilder ob = new OptionBuilder();
            bool[]? result = null;
            ob.WithMultipleParametersAction<bool>((bool[]? parsedInt) => result = parsedInt);
            var option = (IParametrizedOption)ob.CreateParticularOptionForRegistration();
            var success = option.ProcessParameter(input);
            option.TakeAction();
            Assert.AreEqual(true, success);
            var splittedInput = input.Split(',');
            var expectedResult = Enumerable.Repeat(false, splittedInput.Length).ToArray();
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase("0,0,0,0")]
        [TestCase("1,1,1")]
        [TestCase("t,t,t,0")]
        [TestCase("f,ko,lo,mn")]
        [TestCase("y")]
        [TestCase("n")]
        [TestCase("on,--true")]
        [TestCase("off,true,True")]
        public void SimpleTestShouldNotPass(string input)
        {
            OptionBuilder ob = new OptionBuilder();
            bool[]? result = null;
            ob.WithMultipleParametersAction<bool>((bool[]? parsedInt) => result = parsedInt);
            var option = (IParametrizedOption)ob.CreateParticularOptionForRegistration();
            var success = option.ProcessParameter(input);
            option.TakeAction();
            Assert.IsFalse(success);
        }
    }
    public class StringOptionMultipleParameterParsingTests
    {
        [Test]
        [TestCase("Trueaasad,165,16516123,5454")]
        [TestCase("true ,hey you ,doing")]
        [TestCase("TRUEorNo,tWhatever")]
        [TestCase("T,asdsa,465")]
        [TestCase("O,k")]
        [TestCase("NotO,kay")]
        [TestCase("NotOkayNotOkayNot,OkayNotOkayNotOkayNotOkayN,otOkayNotOk,ayNotOkayNot,OkayNotOk,ayNotOkay")]
        public void SimpleTestShouldReturnTrue(string input)
        {
            OptionBuilder ob = new OptionBuilder();
            string[]? result = null;
            ob.WithMultipleParametersAction<string>((string[]? parsedInt) => result = parsedInt);
            var option = (IParametrizedOption)ob.CreateParticularOptionForRegistration();
            var success = option.ProcessParameter(input);
            option.TakeAction();
            Assert.AreEqual(true, success);
            Assert.AreEqual(input.Split(','),result);
        }
    }
    public class EnumOptionsMultipleParameterParsingTests
    {
        public enum Format
        {
            format1,
            format2,
            format3
        }

        class FormatHolder
        {
            public Format? format;
        }
        [Test]
        [TestCase("format1,format2,format3", new Format[] { Format.format1,Format.format2,Format.format3 })]
        [TestCase("format2", new Format[] { Format.format2 })]
        [TestCase("format3,format1,format3", new Format[] { Format.format3 , Format.format1,Format.format3})]
        public void SimpleTestShouldPass(string input, Format[] expectedOutput)
        {
            OptionBuilder ob = new OptionBuilder();
            Format[]? result = null;
            ob.WithMultipleParametersAction<Format>((Format[]? parsedInt) => result = parsedInt);
            var option = (IParametrizedOption)ob.CreateParticularOptionForRegistration();
            var success = option.ProcessParameter(input);
            option.TakeAction();
            Assert.IsTrue(success);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        [TestCase("format12,format2,format3")]
        [TestCase("format1,format1,Format2")]
        [TestCase("format3,--format356")]
        [TestCase("xd,format1,format2,format3")]
        public void SimpleTestShouldNotPass(string input)
        {
            OptionBuilder ob = new OptionBuilder();
            Format[]? result = null;
            ob.WithMultipleParametersAction<Format>((Format[]? parsedInt) => result = parsedInt);
            var option = (IParametrizedOption)ob.CreateParticularOptionForRegistration();
            var success = option.ProcessParameter(input);
            option.TakeAction();
            Assert.IsFalse(success);
            Assert.IsNull(result);
        }
    }
}