namespace CrysSharp.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ClassVariableExpression : IExpression
    {
        private string name;

        public ClassVariableExpression(string name)
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }
    }
}
