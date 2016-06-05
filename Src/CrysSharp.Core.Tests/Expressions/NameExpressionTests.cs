namespace CrysSharp.Core.Tests.Expressions
{
    using System;
    using CrysSharp.Core.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class NameExpressionTests
    {
        [TestMethod]
        public void CreateWithName()
        {
            NameExpression expr = new NameExpression("foo");

            Assert.AreEqual("foo", expr.Name);
        }
    }
}
