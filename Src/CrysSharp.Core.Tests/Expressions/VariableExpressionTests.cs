namespace CrysSharp.Core.Tests.Expressions
{
    using System;
    using CrysSharp.Core.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class VariableExpressionTests
    {
        [TestMethod]
        public void CreateWithName()
        {
            VariableExpression expr = new VariableExpression("foo");

            Assert.AreEqual("foo", expr.Name);
        }
    }
}
