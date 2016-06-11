namespace CrysSharp.Core.Tests.Compiler
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using CrysSharp.Core.Compiler;

    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void ParseEmptyString()
        {
            Parser parser = new Parser(string.Empty);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseNullString()
        {
            Parser parser = new Parser(null);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseName()
        {
            Parser parser = new Parser("foo");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.AreEqual("foo", result.Name);

            Assert.IsNull(parser.ParseExpression());
        }
    }
}
