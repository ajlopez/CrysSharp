namespace CrysSharp.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class BinaryNotExpression : IExpression
    {
        private IExpression expr;

        public BinaryNotExpression(IExpression expr)
        {
            this.expr = expr;
        }

        public IExpression Expression { get { return this.expr; } }
    }
}
