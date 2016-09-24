namespace CrysSharp.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class InstanceVariableAssignmentExpression : IExpression
    {
        private string name;
        private IExpression expression;

        public InstanceVariableAssignmentExpression(string name, IExpression expression)
        {
            this.name = name;
            this.expression = expression;
        }

        public string Name { get { return this.name; } }

        public IExpression Expression { get { return this.expression; } }
    }
}
