namespace CrysSharp.Core.Tests.Expressions
{
    using System;
    using CrysSharp.Core.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GlobalVariableAssignmentExpressionTests
    {
        [TestMethod]
        public void CreateWithNameAndExpression()
        {
            ConstantExpression cexpr = new ConstantExpression(1);
            GlobalVariableAssignmentExpression expr = new GlobalVariableAssignmentExpression("foo", cexpr);

            Assert.AreEqual("foo", expr.Name);
            Assert.AreSame(cexpr, expr.Expression);
        }
    }
}
