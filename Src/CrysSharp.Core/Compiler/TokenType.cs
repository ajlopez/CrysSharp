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
        Boolean = 5,
        Operator = 6,
        Separator = 7,
        Symbol = 8,
        InstanceVarName = 9,
        ClassVarName = 10,
        GlobalVarName = 11,
        EndOfLine = 12
    }
}
