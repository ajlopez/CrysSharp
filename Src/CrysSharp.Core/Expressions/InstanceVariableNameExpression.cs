namespace CrysSharp.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class InstanceVariableNameExpression : IExpression
    {
        private string name;

        public InstanceVariableNameExpression(string name)
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }
    }
}
