namespace CrysSharp.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class GlobalNameExpression : IExpression
    {
        private string name;

        public GlobalNameExpression(string name)
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }
    }
}
