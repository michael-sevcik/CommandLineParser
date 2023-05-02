namespace OptionBuilderTests
{
    class NoParameterOptions
    {
        [Test]
        public void CreationOfNonMandatoryNoParameterOption()
        {
            var builder = new OptionBuilder();
            builder.WithLongSynonyms(new string[] { "format", "fifik" }).
                WithShortSynonyms(new char[] { 'f' }).
                WithAction(() => Console.Write("here"));

            var option = (IOption)builder.CreateParticularOptionForRegistration();

            Assert.AreEqual(new string[] { "format", "fifik" }, option.LongSynonyms);
            Assert.AreEqual(new char[] { 'f'}, option.ShortSynonyms);
            Assert.IsFalse(option.IsMandatory);

        }
        [Test]
        public void CreationOfMandatoryNoParameterOption()
        {
            var builder = new OptionBuilder();
            builder.WithLongSynonyms(new string[] { "format", "fifik" }).
                WithShortSynonyms(new char[] { 'f' }).
                WithAction(() => Console.Write("here")).SetAsMandatory();

            var option = (IOption)builder.CreateParticularOptionForRegistration();

            Assert.AreEqual(new string[] { "format", "fifik" }, option.LongSynonyms);
            Assert.AreEqual(new char[] { 'f' }, option.ShortSynonyms);
            Assert.IsTrue(option.IsMandatory);

        }
    }

    class ParameterOptions
    {
        [Test]
        public void CreationOfNonMandatoryIntOption()
        {
            var builder = new OptionBuilder();
            builder.
                WithLongSynonyms(new string[] { "format", "fifik" }).
                WithShortSynonyms(new char[] { 'f' }).
                WithParametrizedAction<int?>((int? x) => Console.Write(x)).
                RequiresParameter();
            var option = (IParametrizedOption)builder.CreateParticularOptionForRegistration();

            Assert.AreEqual(new string[] { "format", "fifik" }, option.LongSynonyms);
            Assert.AreEqual(new char[] { 'f' }, option.ShortSynonyms);
            Assert.IsFalse(option.IsMandatory);
            Assert.IsTrue(option.IsParameterRequired);
        }

        public enum Format
        {
            format1,
            format2,
            format3
        }

        [Test]
        public void CreationOfMandatoryEnumOption()
        {
            var builder = new OptionBuilder();
            builder.
                WithLongSynonyms(new string[] { "format", "fifik" }).
                WithShortSynonyms(new char[] { 'f' }).
                WithParametrizedAction<Format?>((Format? x) => Console.Write(x)).
                SetAsMandatory();
            var option = (IParametrizedOption)builder.CreateParticularOptionForRegistration();

            Assert.AreEqual(new string[] { "format", "fifik" }, option.LongSynonyms);
            Assert.AreEqual(new char[] { 'f' }, option.ShortSynonyms);
            Assert.IsTrue(option.IsMandatory);
            Assert.IsFalse(option.IsParameterRequired);
        }
    }
    
    class MultipleParameterOptions
    {
        public enum Format
        {
            format1,
            format2,
            format3
        }
        [Test]
        public void CreationOfNonMandatoryMultipleParamIntOption()
        {
            var builder = new OptionBuilder();
            builder.
                WithLongSynonyms(new string[] { "format", "fifik" }).
                WithShortSynonyms(new char[] { 'f' }).
                WithMultipleParametersAction<int>((int[]? x) => Console.Write(x)).
                RequiresParameter().
                WithSeparator(';');
            var option = (IMultipleParameterOption)builder.CreateParticularOptionForRegistration();

            Assert.AreEqual(new string[] { "format", "fifik" }, option.LongSynonyms);
            Assert.AreEqual(new char[] { 'f' }, option.ShortSynonyms);
            Assert.AreEqual(';', option.Separator);
            Assert.IsFalse(option.IsMandatory);
            Assert.IsTrue(option.IsParameterRequired);
        }
        [Test]
        public void CreationOfMandatoryMultipleParamEnumOption()
        {
            var builder = new OptionBuilder();
            builder.
                WithLongSynonyms(new string[] { "format", "fifik" }).
                WithShortSynonyms(new char[] { 'f' }).
                WithMultipleParametersAction<Format>((Format[]? x) => Console.Write(x)).
                SetAsMandatory();
            var option = (IMultipleParameterOption)builder.CreateParticularOptionForRegistration();

            Assert.AreEqual(new string[] { "format", "fifik" }, option.LongSynonyms);
            Assert.AreEqual(new char[] { 'f' }, option.ShortSynonyms);
            Assert.AreEqual(',',option.Separator);
            Assert.IsTrue(option.IsMandatory);
            Assert.IsFalse(option.IsParameterRequired);
        }
    }

    

}