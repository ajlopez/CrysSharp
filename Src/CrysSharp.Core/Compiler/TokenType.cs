namespace CrysSharp.Core.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public enum TokenType
    {
        Nil = 0,
        Name = 1,
        Integer = 2,
        Real = 3,
        String = 4,
        Operator = 5,
        Separator = 6,
        Symbol = 7,
        InstanceVarName = 8,
        ClassVarName = 9,
        EndOfLine = 10
    }
}
