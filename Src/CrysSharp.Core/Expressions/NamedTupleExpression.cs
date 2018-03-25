namespace CrysSharp.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class NamedTupleExpression : IExpression
    {
        private IList<NameValueExpression> entries;

        public NamedTupleExpression(IList<NameValueExpression> entries)
        {
            this.entries = entries;
        }

        public IList<NameValueExpression> Entries { get { return this.entries; } }
    }
}
