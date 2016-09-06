namespace CrysSharp.Core.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using CrysSharp.Core.Exceptions;

    public class Lexer
    {
        private const char Quote = '\'';
        private const char DoubleQuote = '"';
        private const char Colon = ':';
        private const char StartComment = '#';
        private const char EndOfLine = '\n';
        private const char Variable = '@';
        private const char Global = '$';
        private const char QuestionMark = '?';
        private const char ExclamationMark = '!';

        private const string Separators = ";()[],.{}:";

        private static string[] operators = new string[] { "+", "-", "*", "/", "%", "=", "<", ">", "!", "==", "<=", ">=", "!=", "=>", "..", "&", "|", "^", "~", "&&", "||", "**", "<<", ">>", "<=>", "===" };

        private ICharStream stream;
        private Stack<char> chars = new Stack<char>();
        private Stack<Token> tokens = new Stack<Token>();

        public Lexer(string text)
        {
            this.stream = new TextCharStream(text == null ? string.Empty : text);
        }

        public Lexer(TextReader reader)
        {
            this.stream = new TextReaderCharStream(reader);
        }

        public Token NextToken()
        {
            if (this.tokens.Count > 0)
                return this.tokens.Pop();

            char? ch = this.NextFirstChar();

            if (ch == null)
                return null;

            if (ch == EndOfLine)
                return new Token(TokenType.EndOfLine, "\n");

            if (ch == Quote)
                return this.NextCharacter();

            if (ch == DoubleQuote)
                return this.NextString();

            if (ch == Colon)
                return this.NextSymbol();

            if (ch == Variable)
                return this.NextInstanceVariableName();

            if (ch == Global)
                return this.NextGlobalVariableName();

            if (char.IsDigit((char)ch))
                return this.NextInteger(ch);

            if (char.IsLetter((char)ch) || ch == '_' || ch == '$')
                return this.NextName(ch);

            if (operators.Contains(ch.ToString()))
                return NextOperator(ch);

            if (operators.Any(op => op.StartsWith(ch.ToString())))
            {
                string value = ch.ToString();
                char? ch2 = this.NextChar();

                if (ch2 != null)
                {
                    value += ch2;

                    if (operators.Contains(value))
                        return new Token(TokenType.Operator, value);

                    this.BackChar();
                }
            }

            if (Separators.Contains((char)ch))
                return new Token(TokenType.Separator, ch.ToString());

            throw new SyntaxError(string.Format("unexpected '{0}'", ch));
        }

        public void PushToken(Token token)
        {
            this.tokens.Push(token);
        }

        private Token NextOperator(char? ch)
        {
            string value = ch.ToString();
            ch = this.NextChar();

            if (ch != null)
            {
                string value1 = value + ch;

                if (operators.Any(op => op.StartsWith(value)))
                {
                    char? ch2 = this.NextChar();

                    if (ch2 != null)
                    {
                        string value2 = value1 + ch2;

                        if (operators.Contains(value2))
                            return new Token(TokenType.Operator, value2);

                        this.BackChar();
                    }
                }

                if (operators.Contains(value1))
                    return new Token(TokenType.Operator, value1);

                this.BackChar();
            }

            return new Token(TokenType.Operator, value);
        }

        private Token NextName(char? ch)
        {
            string value = ch.ToString();

            for (ch = this.NextChar(); ch != null && (ch == '_' || char.IsLetterOrDigit((char)ch)); ch = this.NextChar())
                value += ch;

            if (ch != null)
                this.BackChar();

            if (value == "nil")
                return new Token(TokenType.Nil, value);

            if (value == "false" || value == "true")
                return new Token(TokenType.Boolean, value);

            return new Token(TokenType.Name, value);
        }

        private Token NextInstanceVariableName()
        {
            string value = string.Empty;
            char? ch;

            for (ch = this.NextChar(); ch != null && (ch == '_' || char.IsLetterOrDigit((char)ch)); ch = this.NextChar())
                value += ch;

            if (ch != null)
            {
                if (string.IsNullOrEmpty(value) && ch == Variable)
                    return this.NextClassVariableName();

                this.BackChar();
            }

            if (string.IsNullOrEmpty(value) || char.IsDigit(value[0]))
                throw new SyntaxError("invalid instance variable name");

            return new Token(TokenType.InstanceVarName, value);
        }

        private Token NextGlobalVariableName()
        {
            string value = string.Empty;
            char? ch;

            for (ch = this.NextChar(); ch != null && char.IsLetterOrDigit((char)ch); ch = this.NextChar())
                value += ch;

            return new Token(TokenType.GlobalVarName, value);
        }

        private Token NextClassVariableName()
        {
            string value = string.Empty;
            char? ch;

            for (ch = this.NextChar(); ch != null && (ch == '_' || char.IsLetterOrDigit((char)ch)); ch = this.NextChar())
                value += ch;

            if (ch != null)
                this.BackChar();

            if (string.IsNullOrEmpty(value) || char.IsDigit(value[0]))
                throw new SyntaxError("invalid class variable name");

            return new Token(TokenType.ClassVarName, value);
        }

        private Token NextSymbol()
        {
            string value = string.Empty;
            char? ch;

            ch = this.NextChar();

            if (ch != null && ch == DoubleQuote)
                return new Token(TokenType.Symbol, this.NextString().Value);

            for (; ch != null && (ch == '_' || char.IsLetterOrDigit((char)ch)); ch = this.NextChar())
            {
                if (char.IsDigit((char)ch) && string.IsNullOrEmpty(value))
                    throw new SyntaxError("unexpected integer");

                value += ch;
            }

            if (value == string.Empty && ch != null && operators.Contains(((char)ch).ToString()))
                value += ch;
            else if (ch != null)
            {
                if (ch == ':' && string.IsNullOrEmpty(value))
                    return new Token(TokenType.Separator, "::");

                if (ch == QuestionMark || ch == ExclamationMark)
                    value += ch;
                else
                    this.BackChar();
            }

            if (String.IsNullOrEmpty(value))
                return new Token(TokenType.Separator, ":");

            return new Token(TokenType.Symbol, value);
        }

        private Token NextString()
        {
            string value = string.Empty;
            char? ch;

            for (ch = this.NextChar(); ch != null && ch != DoubleQuote; ch = this.NextChar())
            {
                if (ch == '\\')
                {
                    char? ch2 = this.NextChar();

                    if (ch2 != null)
                    {
                        if (ch2 == 't')
                        {
                            value += '\t';
                            continue;
                        }

                        if (ch2 == 'r')
                        {
                            value += '\r';
                            continue;
                        }

                        if (ch2 == 'n')
                        {
                            value += '\n';
                            continue;
                        }

                        value += ch2;
                        continue;
                    }
                }

                value += ch;
            }

            if (ch == null)
                throw new SyntaxError("unclosed string");

            return new Token(TokenType.String, value);
        }

        private Token NextCharacter()
        {
            string value = string.Empty;

            char? ch = this.NextChar();

            if (ch == null)
                throw new SyntaxError("unclosed character");

            if (ch == '\\')
            {
                char? ch2 = this.NextChar();

                if (ch2 == null)
                    throw new SyntaxError("unclosed character");

                if (ch2 == 't')
                    value += '\t';
                else if (ch2 == 'r')
                    value += '\r';
                else if (ch2 == 'n')
                    value += '\n';
                else if (ch2 == 'v')
                    value += '\v';
                else if (ch2 == 'f')
                    value += '\f';
                else if (ch2 == '\\')
                    value += '\\';
                else if (ch2 >= '0' && ch2 <= '7')
                {
                    value += ch2;
                    ch2 = this.NextChar();

                    while (ch2 != null && ch2 >= '0' && ch2 <= '7')
                    {
                        value += ch2;
                        ch2 = (char)this.NextChar();
                    }

                    if (ch2 != null)
                        this.BackChar();

                    value = ((char)Convert.ToInt16(value, 8)).ToString();
                }
                else
                    value += ch2;
            }
            else
                value += ch;

            ch = this.NextChar();

            if (ch == null || ch != Quote)
                throw new SyntaxError("unclosed character");

            return new Token(TokenType.Character, value);
        }

        private Token NextInteger(char? ch)
        {
            string value = ch.ToString();

            for (ch = this.NextChar(); ch != null && (char.IsDigit((char)ch) || (char)ch == '_'); ch = this.NextChar())
                if (ch != '_')
                    value += ch;

            if (ch != null && ch == '.')
                return this.NextReal(value);

            if (value.Length == 1 && value[0] == '0' && ch != null && ch == 'o')
            {
                value += 'o';
                for (ch = this.NextChar(); ch != null && ((ch >= '0' && ch <= '7') || ch == '_'); ch = this.NextChar())
                    if (ch != '_')
                        value += ch;
            }
            else if (value.Length == 1 && value[0] == '0' && ch != null && ch == 'b')
            {
                value += 'b';
                for (ch = this.NextChar(); ch != null && ((ch >= '0' && ch <= '1') || ch == '_'); ch = this.NextChar())
                    if (ch != '_')
                        value += ch;
            }
            else if (value.Length == 1 && value[0] == '0' && ch != null && ch == 'x')
            {
                value += 'x';
                for (ch = this.NextChar(); ch != null && ((ch >= '0' && ch <= '9') || (ch >= 'a' && ch <= 'f') || (ch >= 'A' && ch <= 'F') || ch == '_'); ch = this.NextChar())
                    if (ch != '_')
                        value += ch;
            }


            if (ch != null)
                this.BackChar();

            return new Token(TokenType.Integer, value);
        }

        private Token NextReal(string ivalue)
        {
            string value = ivalue + ".";
            char? ch;

            for (ch = this.NextChar(); ch != null && char.IsDigit((char)ch); ch = this.NextChar())
                value += ch;

            if (ch != null)
                this.BackChar();

            if (value.EndsWith("."))
            {
                this.BackChar();
                return new Token(TokenType.Integer, ivalue);
            }

            return new Token(TokenType.Real, value);
        }

        private char? NextFirstChar()
        {
            char? ch = this.NextChar();

            while (true)
            {
                while (ch != null && ch != '\n' && char.IsWhiteSpace((char)ch))
                    ch = this.NextChar();

                if (ch != null && ch == StartComment)
                {
                    for (ch = this.stream.NextChar(); ch != null && ch != '\n';)
                        ch = this.stream.NextChar();

                    if (ch == null)
                        return null;

                    continue;
                }

                break;
            }

            return ch;
        }

        private char? NextChar()
        {
            if (this.chars.Count > 0)
                return this.chars.Pop();

            return this.stream.NextChar();
        }

        private void BackChar()
        {
            this.stream.BackChar();
        }
    }
}
