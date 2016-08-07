namespace CrysSharp.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ClassVariableNameExpression : IExpression
    {
        private string name;

        public ClassVariableNameExpression(string name)
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }
    }
}
