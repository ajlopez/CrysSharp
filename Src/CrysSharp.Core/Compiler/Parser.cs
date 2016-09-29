namespace CrysSharp.Core.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CrysSharp.Core.Expressions;
    using System.Globalization;

    public class Parser
    {
        private static string[][] precedence = new string[][] {
            new string[] { "==", "!=", ">", ">=", "<", "<=", "<=>", "===" },
            new string[] { "||", "&&" },
            new string[] { ".." },
            new string[] { ">>", "<<", "|", "&", "^" },
            new string[] { "+", "-" },
            new string[] { "*", "/", "%" },
            new string[] { "**" }
        };
        
        Lexer lexer;

        public Parser(string text)
        {
            this.lexer = new Lexer(text);
        }

        public IExpression ParseExpression()
        {
            var expr = this.ParseBinaryExpression(0);

            if (expr == null)
                return expr;

            if (expr is VariableExpression)
                if (this.TryParseToken(TokenType.Operator, "="))
                    return new VariableAssignmentExpression(((VariableExpression)expr).Name, this.ParseExpression());

            if (expr is InstanceVariableExpression)
                if (this.TryParseToken(TokenType.Operator, "="))
                    return new InstanceVariableAssignmentExpression(((InstanceVariableExpression)expr).Name, this.ParseExpression());

            if (expr is ClassVariableExpression)
                if (this.TryParseToken(TokenType.Operator, "="))
                    return new ClassVariableAssignmentExpression(((ClassVariableExpression)expr).Name, this.ParseExpression());

            return expr;
        }

        private IExpression ParseBinaryExpression(int level)
        {
            if (level >= precedence.Length)
                return this.ParseTerm();

            IExpression expr = this.ParseBinaryExpression(level + 1);

            Token token = this.lexer.NextToken();

            while (token != null && token.Type == TokenType.Operator && precedence[level].Contains(token.Value))
            {
                if (token.Value == "+")
                    expr = new AddExpression(expr, this.ParseBinaryExpression(level + 1));
                else if (token.Value == "-")
                    expr = new SubtractExpression(expr, this.ParseBinaryExpression(level + 1));
                else if (token.Value == "*")
                    expr = new MultiplyExpression(expr, this.ParseBinaryExpression(level + 1));
                else if (token.Value == "**")
                    expr = new PowerExpression(expr, this.ParseBinaryExpression(level + 1));
                else if (token.Value == "/")
                    expr = new DivideExpression(expr, this.ParseBinaryExpression(level + 1));
                else if (token.Value == "%")
                    expr = new ModuleExpression(expr, this.ParseBinaryExpression(level + 1));
                else if (token.Value == "==")
                    expr = new EqualsExpression(expr, this.ParseBinaryExpression(level + 1));
                else if (token.Value == "!=")
                    expr = new NotEqualsExpression(expr, this.ParseBinaryExpression(level + 1));
                else if (token.Value == "<")
                    expr = new LessExpression(expr, this.ParseBinaryExpression(level + 1));
                else if (token.Value == ">")
                    expr = new GreaterExpression(expr, this.ParseBinaryExpression(level + 1));
                else if (token.Value == ">=")
                    expr = new GreaterEqualsExpression(expr, this.ParseBinaryExpression(level + 1));
                else if (token.Value == "<=")
                    expr = new LessEqualsExpression(expr, this.ParseBinaryExpression(level + 1));
                else if (token.Value == "..")
                    expr = new RangeExpression(expr, this.ParseBinaryExpression(level + 1));
                else if (token.Value == "&&")
                    expr = new LogicalAndExpression(expr, this.ParseBinaryExpression(level + 1));
                else if (token.Value == "||")
                    expr = new LogicalOrExpression(expr, this.ParseBinaryExpression(level + 1));
                else if (token.Value == "&")
                    expr = new BinaryAndExpression(expr, this.ParseBinaryExpression(level + 1));
                else if (token.Value == "|")
                    expr = new BinaryOrExpression(expr, this.ParseBinaryExpression(level + 1));
                else if (token.Value == "^")
                    expr = new BinaryXorExpression(expr, this.ParseBinaryExpression(level + 1));
                else if (token.Value == "<<")
                    expr = new LeftShiftExpression(expr, this.ParseBinaryExpression(level + 1));
                else if (token.Value == ">>")
                    expr = new RightShiftExpression(expr, this.ParseBinaryExpression(level + 1));
                else if (token.Value == "<=>")
                    expr = new ComparisonExpression(expr, this.ParseBinaryExpression(level + 1));
                else if (token.Value == "===")
                    expr = new CaseEqualityExpression(expr, this.ParseBinaryExpression(level + 1));

                token = this.lexer.NextToken();
            }

            this.lexer.PushToken(token);

            return expr;
        }

        private IExpression ParseTerm()
        {
            var expr = this.ParseSimpleTerm();

            if (this.TryParseToken(TokenType.Separator, "."))
                return new DotExpression(expr, this.ParseName());

            return expr;
        }

        private IExpression ParseSimpleTerm()
        {
            var token = this.lexer.NextToken();

            if (token == null)
                return null;

            if (token.Type == TokenType.Operator && token.Value == "~")
                return new UnaryNotExpression(this.ParseTerm());
            if (token.Type == TokenType.Operator && token.Value == "+")
                return new UnaryPlusExpression(this.ParseTerm());
            if (token.Type == TokenType.Operator && token.Value == "!")
                return new LogicalNotExpression(this.ParseTerm());

            if (token.Type == TokenType.Separator && token.Value == "(")
            {
                IExpression expr = this.ParseExpression();
                this.ParseToken(TokenType.Separator, ")");
                return expr;
            }

            if (token.Type == TokenType.Separator && token.Value == "{")
                return ParseTupleExpression();

            if (token.Type == TokenType.Separator && token.Value == "[")
                return ParseArrayExpression();

            if (token.Type == TokenType.Nil)
                return new ConstantExpression(null);

            if (token.Type == TokenType.String)
                return new ConstantExpression(token.Value);

            if (token.Type == TokenType.Integer)
                return new ConstantExpression(int.Parse(token.Value));

            if (token.Type == TokenType.Real)
                return new ConstantExpression(double.Parse(token.Value, CultureInfo.InvariantCulture));

            if (token.Type == TokenType.Boolean)
            {
                if (token.Value == "true")
                    return new ConstantExpression(true);
                if (token.Value == "false")
                    return new ConstantExpression(false);
            }

            if (token.Type == TokenType.GlobalVarName)
                return new GlobalVariableNameExpression(token.Value);
            if (token.Type == TokenType.ClassVarName)
                return new ClassVariableExpression(token.Value);
            if (token.Type == TokenType.InstanceVarName)
                return new InstanceVariableExpression(token.Value);

            return new VariableExpression(token.Value);
        }

        private IExpression ParseTupleExpression()
        {
            List<IExpression> exprs = new List<IExpression>();

            Token token2 = this.lexer.NextToken();

            if (token2 != null && token2.Type == TokenType.Key)
                return ParseNamedTupleExpression(token2);

            this.lexer.PushToken(token2);

            while (!this.TryParseToken(TokenType.Separator, "}"))
            {
                if (exprs.Count > 0)
                    this.ParseToken(TokenType.Separator, ",");

                exprs.Add(this.ParseExpression());
            }

            return new TupleExpression(exprs);
        }

        private IExpression ParseNamedTupleExpression(Token token)
        {
            List<string> keys = new List<string>();
            List<IExpression> expressions = new List<IExpression>();

            keys.Add(token.Value);
            expressions.Add(this.ParseExpression());

            while (!this.TryParseToken(TokenType.Separator, "}"))
            {
                this.ParseToken(TokenType.Separator, ",");
                keys.Add(this.ParseKey());
                expressions.Add(this.ParseExpression());
            }

            return new NamedTupleExpression(keys, expressions);
        }

        private IExpression ParseArrayExpression()
        {
            List<IExpression> exprs = new List<IExpression>();

            while (!this.TryParseToken(TokenType.Separator, "]"))
            {
                if (exprs.Count > 0)
                    this.ParseToken(TokenType.Separator, ",");

                exprs.Add(this.ParseExpression());
            }

            return new ArrayExpression(exprs);
        }

        private void ParseToken(TokenType type, string value)
        {
            Token token = this.lexer.NextToken();

            if (token == null || token.Type != type || token.Value != value)
                throw new ParserException(string.Format("Expected '{0}'", value));
        }

        private string ParseName()
        {
            Token token = this.lexer.NextToken();

            if (token == null || token.Type != TokenType.Name)
                throw new ParserException("Expected name");

            return token.Value;
        }

        private string ParseKey()
        {
            Token token = this.lexer.NextToken();

            if (token == null || token.Type != TokenType.Key)
                throw new ParserException("Expected key");

            return token.Value;
        }

        private bool TryParseToken(TokenType type, string value)
        {
            Token token = this.lexer.NextToken();

            if (token != null && token.Type == type && token.Value == value)
                return true;

            this.lexer.PushToken(token);

            return false;
        }
    }
}
