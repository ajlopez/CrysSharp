namespace CrysSharp.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class UnaryNotExpression : IExpression
    {
        private IExpression expr;

        public UnaryNotExpression(IExpression expr)
        {
            this.expr = expr;
        }

        public IExpression Expression { get { return this.expr; } }
    }
}
