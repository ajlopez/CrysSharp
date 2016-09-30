namespace CrysSharp.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class UnaryMinusExpression : IExpression
    {
        private IExpression expr;

        public UnaryMinusExpression(IExpression expr)
        {
            this.expr = expr;
        }

        public IExpression Expression { get { return this.expr; } }
    }
}
