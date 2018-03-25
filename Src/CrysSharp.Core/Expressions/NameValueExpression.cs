namespace CrysSharp.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class NameValueExpression : IExpression
    {
        private string name;
        private IExpression valueexpr;

        public NameValueExpression(string name, IExpression valueexpr)
        {
            this.name = name;
            this.valueexpr = valueexpr;
        }

        public string Name { get { return this.name; } }

        public IExpression ValueExpression { get { return this.valueexpr; } }
    }
}
