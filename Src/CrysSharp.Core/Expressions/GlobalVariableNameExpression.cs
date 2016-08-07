namespace CrysSharp.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class GlobalVariableNameExpression : IExpression
    {
        private string name;

        public GlobalVariableNameExpression(string name)
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }
    }
}
