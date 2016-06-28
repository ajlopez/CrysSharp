namespace CrysSharp.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class LogicalOrExpression : IExpression
    {
        private IExpression leftexpr;
        private IExpression rightexpr;

        public LogicalOrExpression(IExpression leftexpr, IExpression rightexpr)
        {
            this.leftexpr = leftexpr;
            this.rightexpr = rightexpr;
        }

        public IExpression LeftExpression { get { return this.leftexpr; } }

        public IExpression RightExpression { get { return this.rightexpr; } }
    }
}
