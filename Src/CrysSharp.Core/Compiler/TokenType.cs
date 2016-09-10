namespace CrysSharp.Core.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public enum TokenType
    {
        Nil,
        Name,
        Key,
        Integer,
        Real,
        String,
        Character,
        Boolean,
        Operator,
        Separator,
        Symbol,
        InstanceVarName,
        ClassVarName,
        GlobalVarName,
        EndOfLine
    }
}
