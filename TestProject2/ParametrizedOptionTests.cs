namespace ParametrizedOptionTests
{
    public class IntOptionParsingTests
    {

        [Test]
        [TestCase("0",0)]
        [TestCase("150", 150)]
        [TestCase("-164654", -164654)]
        [TestCase("2147483647", 2147483647)]
        public void SimpleTestWithoutBoundsShouldPass(string input,int expectedOutput)
        {
            OptionBuilder ob = new OptionBuilder();
            int? result = null;
            ob.WithParametrizedAction<int?>((int? parsedInt) => result = parsedInt) ;
            var option = (IParametrizedOption)ob.CreateParticularOptionForRegistration();
            var success = option.ProcessParameter(input);
            option.TakeAction();
            Assert.AreEqual(true, success);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        [TestCase("0", 0)]
        [TestCase("150", 150)]
        [TestCase("2147483647", 2147483647)]
        public void SimpleTestWithLowerBoundShouldPass(string input, int expectedOutput)
        {
            OptionBuilder ob = new OptionBuilder();
            int? result = null;
            ob.WithParametrizedAction<int?>((int? parsedInt) => result = parsedInt).
                WithLowerBound(0);

            var option = (IParametrizedOption)ob.CreateParticularOptionForRegistration();
            var success = option.ProcessParameter(input);
            option.TakeAction();
            Assert.AreEqual(true, success);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        [TestCase("-1", null)]
        [TestCase("-150", null)]
        [TestCase("-147483647", null)]
        public void SimpleTestWithLowerBoundShouldNotPass(string input, int? expectedOutput)
        {
            OptionBuilder ob = new OptionBuilder();
            int? result = null;
            ob.WithParametrizedAction<int?>((int? parsedInt) => result = parsedInt).
                WithLowerBound(0);

            var option = (IParametrizedOption)ob.CreateParticularOptionForRegistration();
            var success = option.ProcessParameter(input);
            option.TakeAction();
            Assert.AreEqual(false, success);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        [TestCase("0", 0)]
        [TestCase("150", 150)]
        [TestCase("2147483647", 2147483647)]
        public void SimpleTestWithUpperBoundShouldPass(string input, int expectedOutput)
        {
            OptionBuilder ob = new OptionBuilder();
            int? result = null;
            ob.WithParametrizedAction<int?>((int? parsedInt) => result = parsedInt).
                WithUpperBound(2147483647);

            var option = (IParametrizedOption)ob.CreateParticularOptionForRegistration();
            var success = option.ProcessParameter(input);
            option.TakeAction();
            Assert.AreEqual(true, success);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        [TestCase("1", null)]
        [TestCase("150", null)]
        [TestCase("147483647", null)]
        public void SimpleTestWithUpperBoundShouldNotPass(string input, int? expectedOutput)
        {
            OptionBuilder ob = new OptionBuilder();
            int? result = null;
            ob.WithParametrizedAction<int?>((int? parsedInt) => result = parsedInt).
                WithUpperBound(0);

            var option = (IParametrizedOption)ob.CreateParticularOptionForRegistration();
            var success = option.ProcessParameter(input);
            option.TakeAction();
            Assert.AreEqual(false, success);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        [TestCase("0", 0)]
        [TestCase("150", 150)]
        [TestCase("2147483647", 2147483647)]
        public void SimpleTestWithUpperAndLowerBoundShouldPass(string input, int expectedOutput)
        {
            OptionBuilder ob = new OptionBuilder();
            int? result = null;
            ob.WithParametrizedAction<int?>((int? parsedInt) => result = parsedInt).
                WithUpperBound(2147483647).
                WithLowerBound(0);

            var option = (IParametrizedOption)ob.CreateParticularOptionForRegistration();
            var success = option.ProcessParameter(input);
            option.TakeAction();
            Assert.AreEqual(true, success);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        [TestCase("-1", null)]
        [TestCase("2147483647", null)]
        public void SimpleTestWithUpperAndLowerBoundShouldNotPass(string input, int? expectedOutput)
        {
            OptionBuilder ob = new OptionBuilder();
            int? result = null;
            ob.WithParametrizedAction<int?>((int? parsedInt) => result = parsedInt).
                WithUpperBound(2147483646).
                WithLowerBound(0);

            var option = (IParametrizedOption)ob.CreateParticularOptionForRegistration();
            var success = option.ProcessParameter(input);
            option.TakeAction();
            Assert.AreEqual(false, success);
            Assert.AreEqual(expectedOutput, result);
        }

    }
    public class BoolOptionParsingTests
    {
        [Test]
        [TestCase("True")]
        [TestCase("true")]
        [TestCase("TRUE")]
        [TestCase("TruE")]
        [TestCase("trUE")]
        [TestCase("tRue")]
        [TestCase("tRUE")]
        public void SimpleTestShouldReturnTrue(string input)
        {
            OptionBuilder ob = new OptionBuilder();
            bool? result = null;
            ob.WithParametrizedAction<bool?>((bool? parsedInt) => result = parsedInt);
            var option = (IParametrizedOption)ob.CreateParticularOptionForRegistration();
            var success = option.ProcessParameter(input);
            option.TakeAction();
            Assert.AreEqual(true, success);
            Assert.IsTrue(result);
        }

        [Test]
        [TestCase("False")]
        [TestCase("FALSE")]
        [TestCase("false")]
        [TestCase("FalSe")]
        [TestCase("FalSE")]
        [TestCase("falSe")]
        [TestCase("fAlSe")]
        public void SimpleTestShouldReturnFalse(string input)
        {
            OptionBuilder ob = new OptionBuilder();
            bool? result = null;
            ob.WithParametrizedAction<bool?>((bool? parsedInt) => result = parsedInt);
            var option = (IParametrizedOption)ob.CreateParticularOptionForRegistration();
            var success = option.ProcessParameter(input);
            option.TakeAction();
            Assert.AreEqual(true, success);
            Assert.IsFalse(result);
        }

        [Test]
        [TestCase("0")]
        [TestCase("1")]
        [TestCase("t")]
        [TestCase("f")]
        [TestCase("y")]
        [TestCase("n")]
        [TestCase("on")]
        [TestCase("off")]
        public void SimpleTestShouldNotPass(string input)
        {
            OptionBuilder ob = new OptionBuilder();
            bool? result = null;
            ob.WithParametrizedAction<bool?>((bool? parsedInt) => result = parsedInt);
            var option = (IParametrizedOption)ob.CreateParticularOptionForRegistration();
            var success = option.ProcessParameter(input);
            option.TakeAction();
            Assert.IsFalse(success);
        }
    }
    public class StringOptionParsingTests
    {
        [Test]
        [TestCase("Trueaasad")]
        [TestCase("true hey you doing")]
        [TestCase("TRUEorNotWhatever")]
        [TestCase("T")]
        [TestCase("Ok")]
        [TestCase("NotOkay")]
        [TestCase("NotOkayNotOkayNotOkayNotOkayNotOkayNotOkayNotOkayNotOkayNotOkayNotOkayNotOkayNotOkay")]
        public void SimpleTestShouldReturnTrue(string input)
        {
            OptionBuilder ob = new OptionBuilder();
            string? result = null;
            ob.WithParametrizedAction<string?>((string? parsedInt) => result = parsedInt);
            var option = (IParametrizedOption)ob.CreateParticularOptionForRegistration();
            var success = option.ProcessParameter(input);
            option.TakeAction();
            Assert.AreEqual(true, success);
            Assert.AreEqual(input, result);
        }
    }

    public class EnumOptionsParsingTests
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
        [TestCase("format1", Format.format1)]
        [TestCase("format2", Format.format2)]
        [TestCase("format3", Format.format3)]
        public void SimpleTestShouldPass(string input, Format expectedOutput)
        {
            OptionBuilder ob = new OptionBuilder();
            var fh = new FormatHolder();
            ob.WithParametrizedAction<Format?>((Format? parsedInt) => fh.format = parsedInt);
            var option = (IParametrizedOption)ob.CreateParticularOptionForRegistration();
            var success = option.ProcessParameter(input);
            option.TakeAction();
            Assert.IsTrue(success);
            Assert.AreEqual(expectedOutput, fh.format);
        }

        [Test]
        [TestCase("format12")]
        [TestCase("Format2")]
        [TestCase("format356")]
        [TestCase("xd")]
        public void SimpleTestShouldNotPass(string input)
        {
            OptionBuilder ob = new OptionBuilder();
            var fh = new FormatHolder();
            ob.WithParametrizedAction<Format?>((Format? parsedInt) => fh.format = parsedInt);
            var option = (IParametrizedOption)ob.CreateParticularOptionForRegistration();
            var success = option.ProcessParameter(input);
            option.TakeAction();
            Assert.IsFalse(success);
            Assert.IsNull(fh.format);
        }
    }
}