namespace CrysSharp.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class KeyValueExpression : IExpression
    {
        private IExpression keyexpr;
        private IExpression valueexpr;

        public KeyValueExpression(IExpression keyexpr, IExpression valueexpr)
        {
            this.keyexpr = keyexpr;
            this.valueexpr = valueexpr;
        }

        public IExpression KeyExpression { get { return this.keyexpr; } }

        public IExpression ValueExpression { get { return this.valueexpr; } }
    }
}
