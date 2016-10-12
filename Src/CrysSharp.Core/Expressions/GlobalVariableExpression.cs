namespace CrysSharp.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class GlobalVariableExpression : IExpression
    {
        private string name;

        public GlobalVariableExpression(string name)
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }
    }
}
