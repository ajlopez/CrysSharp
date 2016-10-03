namespace CrysSharp.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DotExpression : IExpression
    {
        private IExpression expression;
        private string name;
        private IList<IExpression> arguments;

        public DotExpression(IExpression expression, string name, IList<IExpression> arguments)
        {
            this.expression = expression;
            this.name = name;
            this.arguments = arguments;
        }

        public IExpression Expression { get { return this.expression; } }

        public string Name { get { return this.name; } }

        public IList<IExpression> Arguments { get { return this.arguments; } }
    }
}
