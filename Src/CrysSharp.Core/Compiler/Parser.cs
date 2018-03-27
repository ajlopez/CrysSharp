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
            new string[] { "=>" },
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

            if (this.TryParseToken(TokenType.Operator, "="))
            {
                if (expr is VariableExpression)
                    return new VariableAssignmentExpression(((VariableExpression)expr).Name, this.ParseExpression());

                if (expr is InstanceVariableExpression)
                   return new InstanceVariableAssignmentExpression(((InstanceVariableExpression)expr).Name, this.ParseExpression());

                if (expr is ClassVariableExpression)
                   return new ClassVariableAssignmentExpression(((ClassVariableExpression)expr).Name, this.ParseExpression());

                if (expr is GlobalVariableExpression)
                    return new GlobalVariableAssignmentExpression(((GlobalVariableExpression)expr).Name, this.ParseExpression());
            }

            return expr;
        }

        private IExpression ParseBinaryExpression(int level)
        {
            if (level >= precedence.Length)
                return this.ParseTerm();

            IExpression expr = this.ParseBinaryExpression(level + 1);

            Token token = this.NextToken();

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
                else if (token.Value == "=>")
                    expr = new KeyValueExpression(expr, this.ParseBinaryExpression(level + 1));

                token = this.NextToken();
            }

            this.PushToken(token);

            return expr;
        }

        private IExpression ParseTerm()
        {
            var expr = this.ParseSimpleTerm();

            if (expr == null)
                return null;

            while (this.TryParseToken(TokenType.Separator, "."))
                expr = this.ParseDotExpression(expr);

            return expr;
        }

        private DotExpression ParseDotExpression(IExpression expr)
        {
            String name = this.ParseName();

            IList<IExpression> arguments;

            if (this.TryParseToken(TokenType.Separator, "("))
                arguments = this.ParseEnclosedArguments();
            else
                arguments = this.ParseOpenArguments();

            return new DotExpression(expr, name, arguments);
        }

        private IList<IExpression> ParseEnclosedArguments()
        {
            IList<IExpression> arguments = new List<IExpression>();

            while (!this.TryParseToken(TokenType.Separator, ")"))
            {
                if (arguments.Count > 0)
                    this.ParseToken(TokenType.Separator, ",");

                arguments.Add(this.ParseExpression());
            }

            return arguments;
        }

        private IList<IExpression> ParseOpenArguments()
        {
            IList<IExpression> arguments = new List<IExpression>();

            for (IExpression expr = this.ParseExpression(); expr != null; expr = this.ParseExpression())
            {
                arguments.Add(expr);

                if (!this.TryParseToken(TokenType.Separator, ","))
                    break;
            }

            return arguments;
        }

        private IExpression ParseSimpleTerm()
        {
            var token = this.NextToken();

            if (token == null)
                return null;

            if (token.Type == TokenType.Operator)
            {
                if (token.Value == "~")
                    return new UnaryNotExpression(this.ParseTerm());
                if (token.Value == "+")
                    return new UnaryPlusExpression(this.ParseTerm());
                if (token.Value == "-")
                    return new UnaryMinusExpression(this.ParseTerm());
                if (token.Value == "!")
                    return new LogicalNotExpression(this.ParseTerm());
            }

            if (token.Type == TokenType.Separator && token.Value == "(")
                return ParseEnclosedExpression();

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
                return new GlobalVariableExpression(token.Value);
            if (token.Type == TokenType.ClassVarName)
                return new ClassVariableExpression(token.Value);
            if (token.Type == TokenType.InstanceVarName)
                return new InstanceVariableExpression(token.Value);
            if (token.Type == TokenType.Name)
                return new VariableExpression(token.Value);

            this.PushToken(token);

            return null;
        }

        private IExpression ParseEnclosedExpression()
        {
            IExpression expr = this.ParseExpression();
            this.ParseToken(TokenType.Separator, ")");
            return expr;
        }

        private IExpression ParseTupleExpression()
        {
            List<IExpression> exprs = new List<IExpression>();

            Token token2 = this.NextToken();

            if (token2 != null && token2.Type == TokenType.Key)
                return ParseNamedTupleExpression(token2);

            this.PushToken(token2);

            while (!this.TryParseToken(TokenType.Separator, "}"))
            {
                if (exprs.Count > 0)
                    this.ParseToken(TokenType.Separator, ",");

                exprs.Add(this.ParseExpression());
            }

            if (exprs.Count > 0 && exprs.All(expr => expr is KeyValueExpression))
            {
                IList<KeyValueExpression> keyvalues = exprs.Select(expr => (KeyValueExpression)expr).ToList<KeyValueExpression>();

                return new HashExpression(keyvalues);
            }

            return new TupleExpression(exprs);
        }

        private IExpression ParseNamedTupleExpression(Token token)
        {
            IList<NameValueExpression> entries = new List<NameValueExpression>();

            entries.Add(new NameValueExpression(token.Value, this.ParseExpression()));

            while (!this.TryParseToken(TokenType.Separator, "}"))
            {
                this.ParseToken(TokenType.Separator, ",");
                string name = this.ParseKey();
                IExpression expr = this.ParseExpression();
                entries.Add(new NameValueExpression(name, expr));
            }

            return new NamedTupleExpression(entries);
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

        private Token NextToken()
        {
            return this.lexer.NextToken();
        }

        private void PushToken(Token token)
        {
            this.lexer.PushToken(token);
        }

        private void ParseToken(TokenType type, string value)
        {
            Token token = this.NextToken();

            if (token == null || token.Type != type || token.Value != value)
                throw new ParserException(string.Format("Expected '{0}'", value));
        }

        private string ParseName()
        {
            Token token = this.NextToken();

            if (token == null || token.Type != TokenType.Name)
                throw new ParserException("Expected name");

            return token.Value;
        }

        private string ParseKey()
        {
            Token token = this.NextToken();

            if (token == null || token.Type != TokenType.Key)
                throw new ParserException("Expected key");

            return token.Value;
        }

        private bool TryParseToken(TokenType type, string value)
        {
            Token token = this.NextToken();

            if (token != null && token.Type == type && token.Value == value)
                return true;

            this.PushToken(token);

            return false;
        }
    }
}
