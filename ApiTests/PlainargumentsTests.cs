using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ApiTests
{
    internal class PlainargumentsTests
    {

        record struct Argument : IPlainArgument
        {
            public bool IsMandatory { get; init; } = false;
            public string HelpString { get; init; } = "";
            public required Action<string> Action { get; init; }

            private string value;
            public Argument()
            {
            }

            public bool ProcessParameter(string parameter)
            {
                value = parameter;
                return true;
            }

            public void TakeAction()
            {
                Action.Invoke(value);
            }

            public void Restore()
            {
                value = string.Empty;
            }
        }


        [TestMethod]
        public void nonMandatoryPlainArgument() {
            // Arrange
            var args = new string[] { "foo" };
            string val = null;
            Parser parser = new( new IPlainArgument[] { new Argument { Action = vl => val = vl } }   );

            
            // Act
            var res = parser.ParseCommandLine(args);

            // Assert
            Assert.IsTrue(res);
            Assert.AreEqual("foo", val);
        }

        [TestMethod]
        public void mandatoryPlainArgument() {
            // Arrange
            var args = new string[] { "foo" };
            string val = null;
            Parser parser = new( new IPlainArgument[] { new Argument { Action = vl => val = vl, IsMandatory = true } }   );

            
            // Act
            var res = parser.ParseCommandLine(args);

            // Assert
            Assert.IsTrue(res);
            Assert.AreEqual("foo", val);
        }

        [TestMethod]

        public void mandatoryPlainArgumentMissing() {
            // Arrange
            var args = new string[] { };
            string val = null;
            Parser parser = new( new IPlainArgument[] { new Argument { Action = vl => val = vl, IsMandatory = true } }   );
            
            // Act
            var res = parser.ParseCommandLine(args);

            // Assert
            Assert.IsFalse(res);
            Assert.IsNull(val);
        }


        [TestMethod]
        public void separator() {
            // Arrange
            var args = new string[] { "--", "foo" };
            string val = null;
            Parser parser = new( new IPlainArgument[] { new Argument { Action = vl => val = vl, IsMandatory = true } }   );

            
            // Act
            var res = parser.ParseCommandLine(args);

            // Assert
            Assert.IsTrue(res);
            Assert.AreEqual("foo", val);
        }

        [TestMethod]
        public void separatorAfter() {
            // Arrange
            var args = new string[] { "foo", "--" };
            string val = null;
            Parser parser = new( new IPlainArgument[] { new Argument { Action = vl => val = vl, IsMandatory = true } }   );

            
            // Act
            var res = parser.ParseCommandLine(args);

            // Assert
            Assert.IsTrue(res);
            Assert.AreEqual("foo", val);
        }

        [TestMethod]
        public void Remaining() {
            // Arrange
            var args = new string[] { "foo", "bar" };
            string val = null;
            Parser parser = new( new IPlainArgument[] { new Argument { Action = vl => val = vl, IsMandatory = true } }   );

            
            // Act
            var res = parser.ParseCommandLine(args);

            // Assert
            
            Assert.IsTrue(res);
            Assert.AreEqual("foo", val);
            Assert.AreEqual(parser.RemainingPlainArguments.Length, 1);
            Assert.AreEqual(parser.RemainingPlainArguments[0], "bar");
        }

        [TestMethod]
        public void RemainingOnly()
        {
            // Arrange
            var args = new string[] { "foo", "bar" };
            Parser parser = new();


            // Act
            var res = parser.ParseCommandLine(args);

            // Assert

            Assert.IsTrue(res);
            Assert.AreEqual(parser.RemainingPlainArguments.Length, 2);
            Assert.AreEqual(parser.RemainingPlainArguments[0], "foo");
            Assert.AreEqual(parser.RemainingPlainArguments[1], "bar");
        }


        [TestMethod]
        public void RemainingOnlyWithSeparator()
        {
            // Arrange
            var args = new string[] { "foo", "--" , "bar" };
            Parser parser = new();


            // Act
            var res = parser.ParseCommandLine(args);

            // Assert

            Assert.IsTrue(res);
            Assert.AreEqual(parser.RemainingPlainArguments.Length, 2);
            Assert.AreEqual(parser.RemainingPlainArguments[0], "foo");
            Assert.AreEqual(parser.RemainingPlainArguments[1], "bar");
        }


        [TestMethod]
        public void MoreSeparators()
        {
            // Arrange
            var args = new string[] { "--", "--", "--", "surprise" };
            Parser parser = new();


            // Act
            var res = parser.ParseCommandLine(args);

            // Assert

            Assert.IsTrue(res);
            Assert.AreEqual(parser.RemainingPlainArguments.Length, 3);
            Assert.AreEqual(parser.RemainingPlainArguments[0], "--");
            Assert.AreEqual(parser.RemainingPlainArguments[1], "--");
            Assert.AreEqual(parser.RemainingPlainArguments[2], "surprise");
        }

               [TestMethod]
        public void MandatorySeparator()
        {
            // Arrange
            var args = new string[] { "--", "--", "Kraftfahrzeug-Haftpflichtversicherung" };
            string val = null;
            Parser parser = new( new IPlainArgument[] { new Argument { Action = vl => val = vl, IsMandatory = true } }   );


            // Act
            var res = parser.ParseCommandLine(args);

            // Assert

            Assert.IsTrue(res);
            Assert.AreEqual(val, "--");
        }

        [TestMethod]
        public void MoreArguments() {
            // Arrange
            var args = new string[] { "foo", "bar", "baz" };
            string val1 = null;
            string val2 = null;
            string val3 = null;
            Parser parser = new( new IPlainArgument[] { 
                new Argument { Action = vl => val1 = vl, IsMandatory = true },
                new Argument { Action = vl => val2 = vl, IsMandatory = true },
                new Argument { Action = vl => val3 = vl, IsMandatory = true },
                 }   );

            
            // Act
            var res = parser.ParseCommandLine(args);

            // Assert
            
            Assert.IsTrue(res);
            Assert.AreEqual("foo", val1);
            Assert.AreEqual("bar", val2);
            Assert.AreEqual("baz", val3);
            Assert.AreEqual(parser.RemainingPlainArguments.Length, 0);
        }
        
        [TestMethod]
        public void MandatoryNonMandatoryMix() {
            // Arrange
            var args = new string[] { "foo", "bar" };
            string val1 = null;
            string val2 = null;
            string val3 = null;
            Parser parser = new( new IPlainArgument[] { 
                new Argument { Action = vl => val1 = vl, IsMandatory = true },
                new Argument { Action = vl => val2 = vl, IsMandatory = true },
                new Argument { Action = vl => val3 = vl, IsMandatory = false },
                 }   );

            
            // Act
            var res = parser.ParseCommandLine(args);

            // Assert
            
            Assert.IsTrue(res);
            Assert.AreEqual("foo", val1);
            Assert.AreEqual("bar", val2);
            Assert.IsNull(val3);
            Assert.AreEqual(parser.RemainingPlainArguments.Length, 0);
        }

        
        [TestMethod]
        public void MandatoryNonMandatoryMixValid() {
            // Arrange
            var args = new string[] { "foo", "bar" };
            string val1 = null;
            string val2 = null;
            string val3 = null;
            Parser parser = new( new IPlainArgument[] { 
                new Argument { Action = vl => val1 = vl, IsMandatory = true },
                new Argument { Action = vl => val2 = vl, IsMandatory = false },
                new Argument { Action = vl => val3 = vl, IsMandatory = false },
                 }   );

            
            // Act
            var res = parser.ParseCommandLine(args);

            // Assert
            
            Assert.IsTrue(res);
            Assert.AreEqual("foo", val1);
            Assert.AreEqual("bar", val2);
            Assert.IsNull(val3);
            Assert.AreEqual(parser.RemainingPlainArguments.Length, 0);
        }

         [TestMethod]
        public void PlainWithNonplain() {
            // Arrange
            var args = new string[] { "foo",  "-p", "bar" };
            string val1 = null;
            string val2 = null;
            string valO = null;
            Parser parser = new( new IPlainArgument[] { 
                new Argument { Action = vl => val1 = vl, IsMandatory = true },
                new Argument { Action = vl => val2 = vl, IsMandatory = false },
                 }   );


            new OptionBuilder()
                .WithShortSynonyms('p')
                .WithLongSynonyms("p")
                .WithParametrizedAction<string>(s => valO = s)
                .RequiresParameter()
                .RegisterOption(parser);


            // Act
            var res = parser.ParseCommandLine(args);

            // Assert
            
            Assert.IsTrue(res);
            Assert.AreEqual("foo", val1);
            Assert.AreEqual("bar", valO);
            Assert.IsNull(val2);
            Assert.AreEqual(parser.RemainingPlainArguments.Length, 0);
        }

        [TestMethod]
        public void PlainWithNonplainPriotizeMandatory()
        {
            // Arrange
            var args = new string[] { "foo", "-p", "bar" };
            string val1 = null;
            string val2 = null;
            string valO = null;
            Parser parser = new(new IPlainArgument[] {
                new Argument { Action = vl => val1 = vl, IsMandatory = true },
                new Argument { Action = vl => val2 = vl, IsMandatory = true },
                 });


            new OptionBuilder()
                .WithShortSynonyms('p')
                .WithLongSynonyms("p")
                .WithParametrizedAction<string>(s => valO = s)
                .RegisterOption(parser);


            // Act
            var res = parser.ParseCommandLine(args);

            // Assert

            Assert.IsTrue(res);
            Assert.AreEqual("foo", val1);
            Assert.AreEqual("bar", valO);
            Assert.IsNull(val2);
            Assert.AreEqual(parser.RemainingPlainArguments.Length, 0);
        }


        [TestMethod]
        public void PlainWithNonplainConflict()
        {
            // Arrange
            var args = new string[] { "foo", "-p", "bar" };
            string val1 = null;
            string val2 = null;
            string valO = null;
            Parser parser = new(new IPlainArgument[] {
                new Argument { Action = vl => val1 = vl, IsMandatory = true },
                new Argument { Action = vl => val2 = vl, IsMandatory = true },
                 });


            new OptionBuilder()
                .WithShortSynonyms('p')
                .WithLongSynonyms("mrpoopybutthole")
                .WithParametrizedAction<string>(s => valO = s)
                .RequiresParameter()
                .RegisterOption(parser);


            // Act
            var res = parser.ParseCommandLine(args);

            // Assert

            Assert.IsFalse(res);
            Assert.IsNull(val1);
            Assert.IsNull(val2);
            Assert.IsNull(valO);
            Assert.AreEqual(parser.RemainingPlainArguments.Length, 0);
            Assert.IsNotNull(parser.Error);
        }


    }
}
