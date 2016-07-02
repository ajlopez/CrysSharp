namespace CrysSharp.Core.Tests.Compiler
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using CrysSharp.Core.Compiler;
    using CrysSharp.Core.Expressions;

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
            Assert.IsInstanceOfType(result, typeof(NameExpression));
            Assert.AreEqual("foo", ((NameExpression)result).Name);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseString()
        {
            Parser parser = new Parser("\"foo\"");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ConstantExpression));
            Assert.AreEqual("foo", ((ConstantExpression)result).Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseInteger()
        {
            Parser parser = new Parser("42");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ConstantExpression));
            Assert.AreEqual(42, ((ConstantExpression)result).Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseReal()
        {
            Parser parser = new Parser("3.1416");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ConstantExpression));
            Assert.AreEqual(3.1416, ((ConstantExpression)result).Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseTrue()
        {
            Parser parser = new Parser("true");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ConstantExpression));
            Assert.AreEqual(true, ((ConstantExpression)result).Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseFalse()
        {
            Parser parser = new Parser("false");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ConstantExpression));
            Assert.AreEqual(false, ((ConstantExpression)result).Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseNil()
        {
            Parser parser = new Parser("nil");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ConstantExpression));
            Assert.IsNull(((ConstantExpression)result).Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseAddIntegers()
        {
            Parser parser = new Parser("1+2");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(AddExpression));

            var addexpr = (AddExpression)result;

            var lexpr = addexpr.LeftExpression;
            var rexpr = addexpr.RightExpression;

            Assert.IsNotNull(lexpr);
            Assert.IsInstanceOfType(lexpr, typeof(ConstantExpression));
            Assert.AreEqual(1, ((ConstantExpression)lexpr).Value);

            Assert.IsNotNull(rexpr);
            Assert.IsInstanceOfType(rexpr, typeof(ConstantExpression));
            Assert.AreEqual(2, ((ConstantExpression)rexpr).Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseSubtractIntegers()
        {
            Parser parser = new Parser("1-2");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(SubtractExpression));

            var subexpr = (SubtractExpression)result;

            var lexpr = subexpr.LeftExpression;
            var rexpr = subexpr.RightExpression;

            Assert.IsNotNull(lexpr);
            Assert.IsInstanceOfType(lexpr, typeof(ConstantExpression));
            Assert.AreEqual(1, ((ConstantExpression)lexpr).Value);

            Assert.IsNotNull(rexpr);
            Assert.IsInstanceOfType(rexpr, typeof(ConstantExpression));
            Assert.AreEqual(2, ((ConstantExpression)rexpr).Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseMultiplyIntegers()
        {
            Parser parser = new Parser("1*2");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MultiplyExpression));

            var subexpr = (MultiplyExpression)result;

            var lexpr = subexpr.LeftExpression;
            var rexpr = subexpr.RightExpression;

            Assert.IsNotNull(lexpr);
            Assert.IsInstanceOfType(lexpr, typeof(ConstantExpression));
            Assert.AreEqual(1, ((ConstantExpression)lexpr).Value);

            Assert.IsNotNull(rexpr);
            Assert.IsInstanceOfType(rexpr, typeof(ConstantExpression));
            Assert.AreEqual(2, ((ConstantExpression)rexpr).Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseDivideIntegers()
        {
            Parser parser = new Parser("1/2");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DivideExpression));

            var subexpr = (DivideExpression)result;

            var lexpr = subexpr.LeftExpression;
            var rexpr = subexpr.RightExpression;

            Assert.IsNotNull(lexpr);
            Assert.IsInstanceOfType(lexpr, typeof(ConstantExpression));
            Assert.AreEqual(1, ((ConstantExpression)lexpr).Value);

            Assert.IsNotNull(rexpr);
            Assert.IsInstanceOfType(rexpr, typeof(ConstantExpression));
            Assert.AreEqual(2, ((ConstantExpression)rexpr).Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseModuleIntegers()
        {
            Parser parser = new Parser("1%2");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ModuleExpression));

            var subexpr = (ModuleExpression)result;

            var lexpr = subexpr.LeftExpression;
            var rexpr = subexpr.RightExpression;

            Assert.IsNotNull(lexpr);
            Assert.IsInstanceOfType(lexpr, typeof(ConstantExpression));
            Assert.AreEqual(1, ((ConstantExpression)lexpr).Value);

            Assert.IsNotNull(rexpr);
            Assert.IsInstanceOfType(rexpr, typeof(ConstantExpression));
            Assert.AreEqual(2, ((ConstantExpression)rexpr).Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseEqualsIntegers()
        {
            Parser parser = new Parser("1==2");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(EqualsExpression));

            var subexpr = (EqualsExpression)result;

            var lexpr = subexpr.LeftExpression;
            var rexpr = subexpr.RightExpression;

            Assert.IsNotNull(lexpr);
            Assert.IsInstanceOfType(lexpr, typeof(ConstantExpression));
            Assert.AreEqual(1, ((ConstantExpression)lexpr).Value);

            Assert.IsNotNull(rexpr);
            Assert.IsInstanceOfType(rexpr, typeof(ConstantExpression));
            Assert.AreEqual(2, ((ConstantExpression)rexpr).Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseBinaryAndIntegers()
        {
            Parser parser = new Parser("1&2");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BinaryAndExpression));

            var subexpr = (BinaryAndExpression)result;

            var lexpr = subexpr.LeftExpression;
            var rexpr = subexpr.RightExpression;

            Assert.IsNotNull(lexpr);
            Assert.IsInstanceOfType(lexpr, typeof(ConstantExpression));
            Assert.AreEqual(1, ((ConstantExpression)lexpr).Value);

            Assert.IsNotNull(rexpr);
            Assert.IsInstanceOfType(rexpr, typeof(ConstantExpression));
            Assert.AreEqual(2, ((ConstantExpression)rexpr).Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseBinaryOrIntegers()
        {
            Parser parser = new Parser("1|2");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BinaryOrExpression));

            var subexpr = (BinaryOrExpression)result;

            var lexpr = subexpr.LeftExpression;
            var rexpr = subexpr.RightExpression;

            Assert.IsNotNull(lexpr);
            Assert.IsInstanceOfType(lexpr, typeof(ConstantExpression));
            Assert.AreEqual(1, ((ConstantExpression)lexpr).Value);

            Assert.IsNotNull(rexpr);
            Assert.IsInstanceOfType(rexpr, typeof(ConstantExpression));
            Assert.AreEqual(2, ((ConstantExpression)rexpr).Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseLogicalAndBooleans()
        {
            Parser parser = new Parser("true&&false");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(LogicalAndExpression));

            var subexpr = (LogicalAndExpression)result;

            var lexpr = subexpr.LeftExpression;
            var rexpr = subexpr.RightExpression;

            Assert.IsNotNull(lexpr);
            Assert.IsInstanceOfType(lexpr, typeof(ConstantExpression));
            Assert.AreEqual(true, ((ConstantExpression)lexpr).Value);

            Assert.IsNotNull(rexpr);
            Assert.IsInstanceOfType(rexpr, typeof(ConstantExpression));
            Assert.AreEqual(false, ((ConstantExpression)rexpr).Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseLogicalOrBooleans()
        {
            Parser parser = new Parser("true||false");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(LogicalOrExpression));

            var subexpr = (LogicalOrExpression)result;

            var lexpr = subexpr.LeftExpression;
            var rexpr = subexpr.RightExpression;

            Assert.IsNotNull(lexpr);
            Assert.IsInstanceOfType(lexpr, typeof(ConstantExpression));
            Assert.AreEqual(true, ((ConstantExpression)lexpr).Value);

            Assert.IsNotNull(rexpr);
            Assert.IsInstanceOfType(rexpr, typeof(ConstantExpression));
            Assert.AreEqual(false, ((ConstantExpression)rexpr).Value);

            Assert.IsNull(parser.ParseExpression());
        }
    }
}
