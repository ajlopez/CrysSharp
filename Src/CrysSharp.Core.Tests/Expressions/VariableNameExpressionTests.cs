namespace CrysSharp.Core.Tests.Expressions
{
    using System;
    using CrysSharp.Core.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class VariableNameExpressionTests
    {
        [TestMethod]
        public void CreateWithName()
        {
            VariableNameExpression expr = new VariableNameExpression("foo");

            Assert.AreEqual("foo", expr.Name);
        }
    }
}
