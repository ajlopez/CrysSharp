namespace CrysSharp.Core.Exceptions
{
    using System;

    public class SyntaxError : Exception
    {
        public SyntaxError(string message)
            : base(message)
        {
        }
    }
}
