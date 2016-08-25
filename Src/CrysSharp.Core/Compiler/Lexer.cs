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

            int ich = this.NextFirstChar();

            if (ich == -1)
                return null;

            char ch = (char)ich;

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

            if (char.IsDigit(ch))
                return this.NextInteger(ch);

            if (char.IsLetter(ch) || ch == '_' || ch == '$')
                return this.NextName(ch);

            if (operators.Contains(ch.ToString()))
                return NextOperator(ref ich, ch);

            if (operators.Any(op => op.StartsWith(ch.ToString())))
            {
                string value = ch.ToString();
                ich = this.NextChar();

                if (ich >= 0)
                {
                    value += (char)ich;

                    if (operators.Contains(value))
                        return new Token(TokenType.Operator, value);

                    this.BackChar();
                }
            }

            if (Separators.Contains(ch))
                return new Token(TokenType.Separator, ch.ToString());

            throw new SyntaxError(string.Format("unexpected '{0}'", ch));
        }

        public void PushToken(Token token)
        {
            this.tokens.Push(token);
        }

        private Token NextOperator(ref int ich, char ch)
        {
            string value = ch.ToString();
            ich = this.NextChar();

            if (ich >= 0)
            {
                value += (char)ich;

                if (operators.Any(op => op.StartsWith(value)))
                {
                    int ich2 = this.NextChar();

                    if (ich2 >= 0)
                    {
                        string value2 = value + (char)ich2;

                        if (operators.Contains(value2))
                            return new Token(TokenType.Operator, value2);

                        this.BackChar();
                    }
                }

                if (operators.Contains(value))
                    return new Token(TokenType.Operator, value);

                this.BackChar();
            }

            return new Token(TokenType.Operator, ch.ToString());
        }

        private Token NextName(char ch)
        {
            string value = ch.ToString();
            int ich;

            for (ich = this.NextChar(); ich >= 0 && ((char)ich == '_' || char.IsLetterOrDigit((char)ich)); ich = this.NextChar())
                value += (char)ich;

            if (ich >= 0)
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
            int ich;

            for (ich = this.NextChar(); ich >= 0 && ((char)ich == '_' || char.IsLetterOrDigit((char)ich)); ich = this.NextChar())
                value += (char)ich;

            if (ich >= 0)
            {
                if (string.IsNullOrEmpty(value) && (char)ich == Variable)
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
            int ich;

            for (ich = this.NextChar(); ich >= 0 && char.IsLetterOrDigit((char)ich); ich = this.NextChar())
                value += (char)ich;

            return new Token(TokenType.GlobalVarName, value);
        }

        private Token NextClassVariableName()
        {
            string value = string.Empty;
            int ich;

            for (ich = this.NextChar(); ich >= 0 && ((char)ich == '_' || char.IsLetterOrDigit((char)ich)); ich = this.NextChar())
                value += (char)ich;

            if (ich >= 0)
                this.BackChar();

            if (string.IsNullOrEmpty(value) || char.IsDigit(value[0]))
                throw new SyntaxError("invalid class variable name");

            return new Token(TokenType.ClassVarName, value);
        }

        private Token NextSymbol()
        {
            string value = string.Empty;
            int ich;

            ich = this.NextChar();

            if (ich >= 0 && (char)ich == DoubleQuote)
                return new Token(TokenType.Symbol, this.NextString().Value);

            for (; ich >= 0 && ((char)ich == '_' || char.IsLetterOrDigit((char)ich)); ich = this.NextChar())
            {
                char ch = (char)ich;

                if (char.IsDigit(ch) && string.IsNullOrEmpty(value))
                    throw new SyntaxError("unexpected integer");

                value += ch;
            }

            if (value == string.Empty && ich >= 0 && operators.Contains(((char)ich).ToString()))
                value += (char)ich;
            else if (ich >= 0)
            {
                char ch = (char)ich;

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
            int ich;

            for (ich = this.NextChar(); ich >= 0 && ((char)ich) != DoubleQuote; ich = this.NextChar())
            {
                char ch = (char)ich;

                if (ch == '\\')
                {
                    int ich2 = this.NextChar();

                    if (ich2 > 0)
                    {
                        char ch2 = (char)ich2;

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

                value += (char)ich;
            }

            if (ich < 0)
                throw new SyntaxError("unclosed string");

            return new Token(TokenType.String, value);
        }

        private Token NextCharacter()
        {
            string value = string.Empty;

            char ch = (char)this.NextChar();

            if (ch == '\\')
            {
                char ch2 = (char)this.NextChar();

                if (ch2 == 't')
                    value += '\t';
                else if (ch2 == 'r')
                    value += '\r';
                else if (ch2 == 'n')
                    value += '\n';
                else
                    value += ch2;
            }
            else
                value += ch;

            if ((char)this.NextChar() != Quote)
                throw new SyntaxError("unclosed character");

            return new Token(TokenType.Character, value);
        }

        private Token NextInteger(char ch)
        {
            string value = ch.ToString();
            int ich;

            for (ich = this.NextChar(); ich >= 0 && (char.IsDigit((char)ich) || (char)ich == '_'); ich = this.NextChar())
                if ((char)ich != '_')
                    value += (char)ich;

            if (ich >= 0 && (char)ich == '.')
                return this.NextReal(value);

            if (ich >= 0)
                this.BackChar();

            return new Token(TokenType.Integer, value);
        }

        private Token NextReal(string ivalue)
        {
            string value = ivalue + ".";
            int ich;

            for (ich = this.NextChar(); ich >= 0 && char.IsDigit((char)ich); ich = this.NextChar())
                value += (char)ich;

            if (ich >= 0)
                this.BackChar();

            if (value.EndsWith("."))
            {
                this.BackChar();
                return new Token(TokenType.Integer, ivalue);
            }

            return new Token(TokenType.Real, value);
        }

        private int NextFirstChar()
        {
            int ich = this.NextChar();

            while (true)
            {
                while (ich > 0 && (char)ich != '\n' && char.IsWhiteSpace((char)ich))
                    ich = this.NextChar();

                if (ich > 0 && (char)ich == StartComment)
                {
                    for (ich = this.stream.NextChar(); ich >= 0 && (char)ich != '\n';)
                        ich = this.stream.NextChar();

                    if (ich < 0)
                        return -1;

                    continue;
                }

                break;
            }

            return ich;
        }

        private int NextChar()
        {
            return this.stream.NextChar();
        }

        private void BackChar()
        {
            this.stream.BackChar();
        }
    }
}
