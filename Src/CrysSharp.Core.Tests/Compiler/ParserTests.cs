﻿namespace CrysSharp.Core.Tests.Compiler
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
            Assert.IsInstanceOfType(result, typeof(VariableExpression));
            Assert.AreEqual("foo", ((VariableExpression)result).Name);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseVariableAssignment()
        {
            Parser parser = new Parser("foo = 42");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(VariableAssignmentExpression));
            Assert.AreEqual("foo", ((VariableAssignmentExpression)result).Name);

            var expr = ((VariableAssignmentExpression)result).Expression;

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseGlobalName()
        {
            Parser parser = new Parser("$foo");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GlobalVariableNameExpression));
            Assert.AreEqual("foo", ((GlobalVariableNameExpression)result).Name);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseInstanceVariableName()
        {
            Parser parser = new Parser("@foo");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(InstanceVariableExpression));
            Assert.AreEqual("foo", ((InstanceVariableExpression)result).Name);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseClassVariableName()
        {
            Parser parser = new Parser("@@foo");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ClassVariableExpression));
            Assert.AreEqual("foo", ((ClassVariableExpression)result).Name);

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
        public void ParseAddIntegersUsingParenthesis()
        {
            Parser parser = new Parser("(1+2)");

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
        public void ParseAddThreeIntegers()
        {
            Parser parser = new Parser("1+2+3");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(AddExpression));

            var addexpr = (AddExpression)result;

            var lexpr = addexpr.LeftExpression;
            var rexpr = addexpr.RightExpression;

            Assert.IsNotNull(rexpr);
            Assert.IsInstanceOfType(rexpr, typeof(ConstantExpression));
            Assert.AreEqual(3, ((ConstantExpression)rexpr).Value);

            Assert.IsNotNull(lexpr);
            Assert.IsInstanceOfType(lexpr, typeof(AddExpression));

            addexpr = (AddExpression)lexpr;

            lexpr = addexpr.LeftExpression;
            rexpr = addexpr.RightExpression;

            Assert.AreEqual(1, ((ConstantExpression)lexpr).Value);

            Assert.IsNotNull(rexpr);
            Assert.IsInstanceOfType(rexpr, typeof(ConstantExpression));
            Assert.AreEqual(2, ((ConstantExpression)rexpr).Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseAddSubtractThreeIntegers()
        {
            Parser parser = new Parser("1+2-3");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(SubtractExpression));

            var subexpr = (SubtractExpression)result;

            var lexpr = subexpr.LeftExpression;
            var rexpr = subexpr.RightExpression;

            Assert.IsNotNull(rexpr);
            Assert.IsInstanceOfType(rexpr, typeof(ConstantExpression));
            Assert.AreEqual(3, ((ConstantExpression)rexpr).Value);

            Assert.IsNotNull(lexpr);
            Assert.IsInstanceOfType(lexpr, typeof(AddExpression));

            var addexpr = (AddExpression)lexpr;

            lexpr = addexpr.LeftExpression;
            rexpr = addexpr.RightExpression;

            Assert.AreEqual(1, ((ConstantExpression)lexpr).Value);

            Assert.IsNotNull(rexpr);
            Assert.IsInstanceOfType(rexpr, typeof(ConstantExpression));
            Assert.AreEqual(2, ((ConstantExpression)rexpr).Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseSubtractAddThreeIntegers()
        {
            Parser parser = new Parser("1-2+3");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(AddExpression));

            var addexpr = (AddExpression)result;

            var lexpr = addexpr.LeftExpression;
            var rexpr = addexpr.RightExpression;

            Assert.IsNotNull(rexpr);
            Assert.IsInstanceOfType(rexpr, typeof(ConstantExpression));
            Assert.AreEqual(3, ((ConstantExpression)rexpr).Value);

            Assert.IsNotNull(lexpr);
            Assert.IsInstanceOfType(lexpr, typeof(SubtractExpression));

            var subexpr = (SubtractExpression)lexpr;

            lexpr = subexpr.LeftExpression;
            rexpr = subexpr.RightExpression;

            Assert.AreEqual(1, ((ConstantExpression)lexpr).Value);

            Assert.IsNotNull(rexpr);
            Assert.IsInstanceOfType(rexpr, typeof(ConstantExpression));
            Assert.AreEqual(2, ((ConstantExpression)rexpr).Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseMultiplyAddThreeIntegers()
        {
            Parser parser = new Parser("1*2+3");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(AddExpression));

            var addexpr = (AddExpression)result;

            var lexpr = addexpr.LeftExpression;
            var rexpr = addexpr.RightExpression;

            Assert.IsNotNull(rexpr);
            Assert.IsInstanceOfType(rexpr, typeof(ConstantExpression));
            Assert.AreEqual(3, ((ConstantExpression)rexpr).Value);

            Assert.IsNotNull(lexpr);
            Assert.IsInstanceOfType(lexpr, typeof(MultiplyExpression));

            var multexpr = (MultiplyExpression)lexpr;

            lexpr = multexpr.LeftExpression;
            rexpr = multexpr.RightExpression;

            Assert.AreEqual(1, ((ConstantExpression)lexpr).Value);

            Assert.IsNotNull(rexpr);
            Assert.IsInstanceOfType(rexpr, typeof(ConstantExpression));
            Assert.AreEqual(2, ((ConstantExpression)rexpr).Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseAddMultiplyThreeIntegers()
        {
            Parser parser = new Parser("1+2*3");

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
            Assert.IsInstanceOfType(rexpr, typeof(MultiplyExpression));

            var multexpr = (MultiplyExpression)rexpr;

            lexpr = multexpr.LeftExpression;
            rexpr = multexpr.RightExpression;

            Assert.AreEqual(2, ((ConstantExpression)lexpr).Value);

            Assert.IsNotNull(rexpr);
            Assert.IsInstanceOfType(rexpr, typeof(ConstantExpression));
            Assert.AreEqual(3, ((ConstantExpression)rexpr).Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseAddDivideThreeIntegers()
        {
            Parser parser = new Parser("1+2/3");

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
            Assert.IsInstanceOfType(rexpr, typeof(DivideExpression));

            var multexpr = (DivideExpression)rexpr;

            lexpr = multexpr.LeftExpression;
            rexpr = multexpr.RightExpression;

            Assert.AreEqual(2, ((ConstantExpression)lexpr).Value);

            Assert.IsNotNull(rexpr);
            Assert.IsInstanceOfType(rexpr, typeof(ConstantExpression));
            Assert.AreEqual(3, ((ConstantExpression)rexpr).Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseMultiplyAddThreeIntegersUsingParenthesis()
        {
            Parser parser = new Parser("1*(2+3)");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MultiplyExpression));

            var multexpr = (MultiplyExpression)result;

            var lexpr = multexpr.LeftExpression;
            var rexpr = multexpr.RightExpression;

            Assert.IsNotNull(lexpr);
            Assert.IsInstanceOfType(lexpr, typeof(ConstantExpression));
            Assert.AreEqual(1, ((ConstantExpression)lexpr).Value);

            Assert.IsNotNull(rexpr);
            Assert.IsInstanceOfType(rexpr, typeof(AddExpression));

            var addexpr = (AddExpression)rexpr;

            lexpr = addexpr.LeftExpression;
            rexpr = addexpr.RightExpression;

            Assert.AreEqual(2, ((ConstantExpression)lexpr).Value);

            Assert.IsNotNull(rexpr);
            Assert.IsInstanceOfType(rexpr, typeof(ConstantExpression));
            Assert.AreEqual(3, ((ConstantExpression)rexpr).Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseDivideAddThreeIntegers()
        {
            Parser parser = new Parser("1/2+3");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(AddExpression));

            var addexpr = (AddExpression)result;

            var lexpr = addexpr.LeftExpression;
            var rexpr = addexpr.RightExpression;

            Assert.IsNotNull(rexpr);
            Assert.IsInstanceOfType(rexpr, typeof(ConstantExpression));
            Assert.AreEqual(3, ((ConstantExpression)rexpr).Value);

            Assert.IsNotNull(lexpr);
            Assert.IsInstanceOfType(lexpr, typeof(DivideExpression));

            var divexpr = (DivideExpression)lexpr;

            lexpr = divexpr.LeftExpression;
            rexpr = divexpr.RightExpression;

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
        public void ParsePowerIntegers()
        {
            Parser parser = new Parser("2**3");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(PowerExpression));

            var subexpr = (PowerExpression)result;

            var lexpr = subexpr.LeftExpression;
            var rexpr = subexpr.RightExpression;

            Assert.IsNotNull(lexpr);
            Assert.IsInstanceOfType(lexpr, typeof(ConstantExpression));
            Assert.AreEqual(2, ((ConstantExpression)lexpr).Value);

            Assert.IsNotNull(rexpr);
            Assert.IsInstanceOfType(rexpr, typeof(ConstantExpression));
            Assert.AreEqual(3, ((ConstantExpression)rexpr).Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseLeftShiftIntegers()
        {
            Parser parser = new Parser("2<<3");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(LeftShiftExpression));

            var subexpr = (LeftShiftExpression)result;

            var lexpr = subexpr.LeftExpression;
            var rexpr = subexpr.RightExpression;

            Assert.IsNotNull(lexpr);
            Assert.IsInstanceOfType(lexpr, typeof(ConstantExpression));
            Assert.AreEqual(2, ((ConstantExpression)lexpr).Value);

            Assert.IsNotNull(rexpr);
            Assert.IsInstanceOfType(rexpr, typeof(ConstantExpression));
            Assert.AreEqual(3, ((ConstantExpression)rexpr).Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseRightShiftIntegers()
        {
            Parser parser = new Parser("2>>3");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RightShiftExpression));

            var subexpr = (RightShiftExpression)result;

            var lexpr = subexpr.LeftExpression;
            var rexpr = subexpr.RightExpression;

            Assert.IsNotNull(lexpr);
            Assert.IsInstanceOfType(lexpr, typeof(ConstantExpression));
            Assert.AreEqual(2, ((ConstantExpression)lexpr).Value);

            Assert.IsNotNull(rexpr);
            Assert.IsInstanceOfType(rexpr, typeof(ConstantExpression));
            Assert.AreEqual(3, ((ConstantExpression)rexpr).Value);

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
        public void ParseRangeIntegers()
        {
            Parser parser = new Parser("1..2");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RangeExpression));

            var rangexpr = (RangeExpression)result;

            var lexpr = rangexpr.LeftExpression;
            var rexpr = rangexpr.RightExpression;

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
        public void ParseComparisonIntegers()
        {
            Parser parser = new Parser("1<=>2");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ComparisonExpression));

            var subexpr = (ComparisonExpression)result;

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
        public void ParseCaseEqualityIntegers()
        {
            Parser parser = new Parser("1===2");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CaseEqualityExpression));

            var subexpr = (CaseEqualityExpression)result;

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
        public void ParseNotEqualsIntegers()
        {
            Parser parser = new Parser("1!=2");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotEqualsExpression));

            var subexpr = (NotEqualsExpression)result;

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
        public void ParseLessIntegers()
        {
            Parser parser = new Parser("1<2");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(LessExpression));

            var subexpr = (LessExpression)result;

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
        public void ParseGreaterIntegers()
        {
            Parser parser = new Parser("1>2");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GreaterExpression));

            var subexpr = (GreaterExpression)result;

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
        public void ParseGreaterEqualsIntegers()
        {
            Parser parser = new Parser("1>=2");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GreaterEqualsExpression));

            var subexpr = (GreaterEqualsExpression)result;

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
        public void ParseLessEqualsIntegers()
        {
            Parser parser = new Parser("1<=2");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(LessEqualsExpression));

            var subexpr = (LessEqualsExpression)result;

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
        public void ParseBinaryXorIntegers()
        {
            Parser parser = new Parser("1^2");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BinaryXorExpression));

            var subexpr = (BinaryXorExpression)result;

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
        public void ParseUnaryNotInteger()
        {
            Parser parser = new Parser("~3");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UnaryNotExpression));

            var notexpr = (UnaryNotExpression)result;

            var expr = notexpr.Expression;

            Assert.IsNotNull(expr);
            Assert.IsInstanceOfType(expr, typeof(ConstantExpression));
            Assert.AreEqual(3, ((ConstantExpression)expr).Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseLogicalNotInteger()
        {
            Parser parser = new Parser("!false");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(LogicalNotExpression));

            var notexpr = (LogicalNotExpression)result;

            var expr = notexpr.Expression;

            Assert.IsNotNull(expr);
            Assert.IsInstanceOfType(expr, typeof(ConstantExpression));
            Assert.AreEqual(false, ((ConstantExpression)expr).Value);

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

        [TestMethod]
        public void ParseTuple()
        {
            Parser parser = new Parser("{ 1, 2, 3 }");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(TupleExpression));

            var tuplexpr = (TupleExpression)result;

            Assert.IsNotNull(tuplexpr.Expressions);
            Assert.AreEqual(3, tuplexpr.Expressions.Count);

            Assert.IsInstanceOfType(tuplexpr.Expressions[0], typeof(ConstantExpression));
            Assert.AreEqual(1, ((ConstantExpression)tuplexpr.Expressions[0]).Value);

            Assert.IsInstanceOfType(tuplexpr.Expressions[1], typeof(ConstantExpression));
            Assert.AreEqual(2, ((ConstantExpression)tuplexpr.Expressions[1]).Value);

            Assert.IsInstanceOfType(tuplexpr.Expressions[2], typeof(ConstantExpression));
            Assert.AreEqual(3, ((ConstantExpression)tuplexpr.Expressions[2]).Value);
        }

        [TestMethod]
        public void ParseArray()
        {
            Parser parser = new Parser("[ 1, 2, 3 ]");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ArrayExpression));

            var arrexpr = (ArrayExpression)result;

            Assert.IsNotNull(arrexpr.Expressions);
            Assert.AreEqual(3, arrexpr.Expressions.Count);

            Assert.IsInstanceOfType(arrexpr.Expressions[0], typeof(ConstantExpression));
            Assert.AreEqual(1, ((ConstantExpression)arrexpr.Expressions[0]).Value);

            Assert.IsInstanceOfType(arrexpr.Expressions[1], typeof(ConstantExpression));
            Assert.AreEqual(2, ((ConstantExpression)arrexpr.Expressions[1]).Value);

            Assert.IsInstanceOfType(arrexpr.Expressions[2], typeof(ConstantExpression));
            Assert.AreEqual(3, ((ConstantExpression)arrexpr.Expressions[2]).Value);
        }

        [TestMethod]
        public void ParseNamedTuple()
        {
            Parser parser = new Parser("{ name: \"Crystal\", year: 2011 }");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NamedTupleExpression));

            var ntupleexpr = (NamedTupleExpression)result;

            Assert.IsNotNull(ntupleexpr.Names);
            Assert.AreEqual(2, ntupleexpr.Names.Count);

            Assert.IsNotNull(ntupleexpr.Expressions);
            Assert.AreEqual(2, ntupleexpr.Expressions.Count);

            Assert.IsInstanceOfType(ntupleexpr.Expressions[0], typeof(ConstantExpression));
            Assert.AreEqual("Crystal", ((ConstantExpression)ntupleexpr.Expressions[0]).Value);

            Assert.IsInstanceOfType(ntupleexpr.Expressions[1], typeof(ConstantExpression));
            Assert.AreEqual(2011, ((ConstantExpression)ntupleexpr.Expressions[1]).Value);
        }
    }
}
