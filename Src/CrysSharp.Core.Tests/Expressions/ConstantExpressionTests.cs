namespace CrysSharp.Core.Tests.Expressions
{
    using System;
    using System.IO;
    using CrysSharp.Core.Compiler;
    using CrysSharp.Core.Exceptions;
    using CrysSharp.Core.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ConstantExpressionTests
    {
        [TestMethod]
        public void CreateInstanceWithValue()
        {
            var expr = new ConstantExpression(42);

            Assert.IsNotNull(expr.Value);
            Assert.AreEqual(42, expr.Value);
        }
    }
}
