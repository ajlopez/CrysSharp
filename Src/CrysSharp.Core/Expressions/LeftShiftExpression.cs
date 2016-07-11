namespace CrysSharp.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class LeftShiftExpression : IExpression
    {
        private IExpression leftexpr;
        private IExpression rightexpr;

        public LeftShiftExpression(IExpression leftexpr, IExpression rightexpr)
        {
            this.leftexpr = leftexpr;
            this.rightexpr = rightexpr;
        }

        public IExpression LeftExpression { get { return this.leftexpr; } }

        public IExpression RightExpression { get { return this.rightexpr; } }
    }
}
