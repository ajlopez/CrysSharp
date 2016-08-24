namespace CrysSharp.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class NamedTupleExpression : IExpression
    {
        private List<String> names;
        private List<IExpression> expressions;

        public NamedTupleExpression(List<String> names, List<IExpression> expressions)
        {
            this.names = names;
            this.expressions = expressions;
        }

        public List<String> Names { get { return this.names; } }

        public List<IExpression> Expressions { get { return this.expressions; } }
    }
}
