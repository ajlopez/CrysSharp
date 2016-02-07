namespace CrysSharp.Core.Compiler
{
    using System;

    public interface ICharStream
    {
        void BackChar();

        int NextChar();
    }
}
