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
            var expr = this.ParseTerm();

            if (expr == null)
                return null;

            var token = this.lexer.NextToken();

            if (token != null && token.Type == TokenType.Operator) { 
                if (token.Value == "+")
                    expr = new AddExpression(expr, this.ParseExpression());
                else if (token.Value == "-")
                    expr = new SubtractExpression(expr, this.ParseExpression());
                else if (token.Value == "*")
                    expr = new MultiplyExpression(expr, this.ParseExpression());
                else if (token.Value == "/")
                    expr = new DivideExpression(expr, this.ParseExpression());
            }
            else
                this.lexer.PushToken(token);

            return expr;
        }

        private IExpression ParseTerm()
        {
            var token = this.lexer.NextToken();

            if (token == null)
                return null;

            if (token.Type == TokenType.Nil)
                return new ConstantExpression(null);

            if (token.Type == TokenType.String)
                return new ConstantExpression(token.Value);

            if (token.Type == TokenType.Integer)
                return new ConstantExpression(int.Parse(token.Value));

            if (token.Type == TokenType.Real)
                return new ConstantExpression(double.Parse(token.Value));

            if (token.Type == TokenType.Boolean)
            {
                if (token.Value == "true")
                    return new ConstantExpression(true);
                if (token.Value == "false")
                    return new ConstantExpression(false);
            }

            return new NameExpression(token.Value);
        }
    }
}
