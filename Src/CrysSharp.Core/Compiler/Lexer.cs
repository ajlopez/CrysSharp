namespace CrysSharp.Core.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Lexer
    {
        private string text;
        private int position;
        private int length;

        public Lexer(string text)
        {
            this.text = text;
            this.position = 0;
            this.length = text.Length;
        }

        public Token NextToken()
        {
            this.SkipWhiteSpaces();

            if (this.position >= this.length)
                return null;

            string value = string.Empty;

            while (this.position < this.length && char.IsLetter(this.text[this.position]))
                value += this.text[this.position++];

            return new Token(TokenType.Name, value);
        }

        private void SkipWhiteSpaces()
        {
            while (this.position < this.length && char.IsWhiteSpace(this.text[this.position]))
                this.position++;
        }
    }
}
