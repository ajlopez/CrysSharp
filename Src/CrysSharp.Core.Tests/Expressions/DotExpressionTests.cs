namespace CrysSharp.Core.Tests.Expressions
{
    using System;
    using CrysSharp.Core.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DotExpressionTests
    {
        [TestMethod]
        public void CreateWithVariableAndName()
        {
            DotExpression expr = new DotExpression(new VariableExpression("bar"), "foo");

            Assert.IsNotNull(expr.Expression);
            Assert.IsInstanceOfType(expr.Expression, typeof(VariableExpression));
            Assert.AreEqual("bar", ((VariableExpression)expr.Expression).Name);
            Assert.AreEqual("foo", expr.Name);
        }
    }
}
