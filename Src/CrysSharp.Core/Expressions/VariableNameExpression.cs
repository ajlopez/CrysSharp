namespace CrysSharp.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class VariableNameExpression : IExpression
    {
        private string name;

        public VariableNameExpression(string name)
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }
    }
}
