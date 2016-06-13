namespace CrysSharp.Core.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CrysSharp.Core.Expressions;

    public class Parser
    {
        Lexer lexer;

        public Parser(string text)
        {
            this.lexer = new Lexer(text);
        }

        public IExpression ParseExpression()
        {
            var token = this.lexer.NextToken();

            if (token == null)
                return null;

            return new NameExpression(token.Value);
        }
    }
}
