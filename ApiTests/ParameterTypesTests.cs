using ArgumentParsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTests;

public class ParameterTypesTests
{


    [TestMethod]
    public void ObjectArgType()
    {

        Assert.ThrowsException<InvalidOperationException>(
            () =>
            {
                var parser = new Parser(new IPlainArgument[] {
                IPlainArgument.CreatePlainArgument<object>(s => { }, false),
            }
            );

            });
    }

    [TestMethod]
    public void ClassArgType()
    {

        Assert.ThrowsException<InvalidOperationException>(
            () =>
            {
                var parser = new Parser(new IPlainArgument[] {
                IPlainArgument.CreatePlainArgument<Enum>(s => { }, false),
            }
            );

            });
    }

    [TestMethod]
    public void ParserArgType()
    {

        Assert.ThrowsException<InvalidOperationException>(
            () =>
            {
                var parser = new Parser(new IPlainArgument[] {
                IPlainArgument.CreatePlainArgument<Parser>(s => { }, false),
            }
            );

            });
    }

    [TestMethod]
    public void InterfaceArgType()
    {

        Assert.ThrowsException<InvalidOperationException>(
            () =>
            {
                var parser = new Parser(new IPlainArgument[] {
                IPlainArgument.CreatePlainArgument<IEnumerable<int>>(s => { }, false),
            }
            );

            });
    }


    public enum MyEnum
    {
        harry,
        hermione,
        lucius,
        bumblebee,
        luna,
        @this
    }
    
    [DataRow(new string[] {"luna"}, true, MyEnum.luna)]
    [DataRow(new string[] {"Luna"}, null, null)]
    [DataRow(new string[] {}, true, null)]
    [DataRow(new string[] {"luna", "bumblebee"}, true, "luna")]
    [DataRow(new string[] {"this", "bumblebee"}, true, "this")]
    [DataTestMethod]
    public void EnumArgType(string[] args, bool result, MyEnum? result_e)
    {
        // Arrange 
        MyEnum? re = null;
        
        var parser = new Parser(new IPlainArgument[] {
                IPlainArgument.CreatePlainArgument<MyEnum>(s => re = s, false) });


        // Act
        bool res = parser.ParseCommandLine(args);

        // Assert
        Assert.AreEqual(res, result);

        if(result)
        {
            Assert.IsNotNull(result_e);
            Assert.AreEqual(result_e, re);
        }
    }

    


}
