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

            if (token.Type == TokenType.Operator && token.Value == "~")
                return new BinaryNotExpression(this.ParseExpression());

            this.lexer.PushToken(token);

            var expr = this.ParseTerm();

            if (expr == null)
                return null;

            token = this.lexer.NextToken();

            if (token != null && token.Type == TokenType.Operator) { 
                if (token.Value == "+")
                    expr = new AddExpression(expr, this.ParseExpression());
                else if (token.Value == "-")
                    expr = new SubtractExpression(expr, this.ParseExpression());
                else if (token.Value == "*")
                    expr = new MultiplyExpression(expr, this.ParseExpression());
                else if (token.Value == "**")
                    expr = new PowerExpression(expr, this.ParseExpression());
                else if (token.Value == "/")
                    expr = new DivideExpression(expr, this.ParseExpression());
                else if (token.Value == "%")
                    expr = new ModuleExpression(expr, this.ParseExpression());
                else if (token.Value == "==")
                    expr = new EqualsExpression(expr, this.ParseExpression());
                else if (token.Value == "!=")
                    expr = new NotEqualsExpression(expr, this.ParseExpression());
                else if (token.Value == "<")
                    expr = new LessExpression(expr, this.ParseExpression());
                else if (token.Value == ">")
                    expr = new GreaterExpression(expr, this.ParseExpression());
                else if (token.Value == ">=")
                    expr = new GreaterEqualsExpression(expr, this.ParseExpression());
                else if (token.Value == "<=")
                    expr = new LessEqualsExpression(expr, this.ParseExpression());
                else if (token.Value == "..")
                    expr = new RangeExpression(expr, this.ParseExpression());
                else if (token.Value == "&&")
                    expr = new LogicalAndExpression(expr, this.ParseExpression());
                else if (token.Value == "||")
                    expr = new LogicalOrExpression(expr, this.ParseExpression());
                else if (token.Value == "&")
                    expr = new BinaryAndExpression(expr, this.ParseExpression());
                else if (token.Value == "|")
                    expr = new BinaryOrExpression(expr, this.ParseExpression());
                else if (token.Value == "^")
                    expr = new BinaryXorExpression(expr, this.ParseExpression());
                else if (token.Value == "<<")
                    expr = new LeftShiftExpression(expr, this.ParseExpression());
                else if (token.Value == ">>")
                    expr = new RightShiftExpression(expr, this.ParseExpression());
                else if (token.Value == "<=>")
                    expr = new ComparisonExpression(expr, this.ParseExpression());
                else if (token.Value == "===")
                    expr = new CaseEqualityExpression(expr, this.ParseExpression());
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

            if (token.Type == TokenType.Separator && token.Value == "(")
            {
                IExpression expr = this.ParseExpression();
                this.ParseToken(TokenType.Separator, ")");
                return expr;
            }

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

        private void ParseToken(TokenType type, string value)
        {
            Token token = this.lexer.NextToken();

            if (token == null || token.Type != type || token.Value != value)
                throw new ParserException(string.Format("Expected '{0}'", value));
        }
    }
}
