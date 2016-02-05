namespace CrysSharp.Core.Tests.Compiler
{
    using System;
    using CrysSharp.Core.Compiler;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LexerTests
    {
        [TestMethod]
        public void NextTokenIsNull()
        {
            Lexer lexer = new Lexer("");

            Assert.IsNull(lexer.NextToken());
        }
    }
}
