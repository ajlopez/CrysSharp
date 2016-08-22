namespace CrysSharp.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ArrayExpression : IExpression
    {
        private List<IExpression> expressions;

        public ArrayExpression(List<IExpression> expressions)
        {
            this.expressions = expressions;
        }

        public List<IExpression> Expressions { get { return this.expressions; } }
    }
}
