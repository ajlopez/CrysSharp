namespace CrysSharp.Core.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class HashExpression : IExpression
    {
        public IList<KeyValueExpression> entries;

        public HashExpression(IList<KeyValueExpression> entries)
        {
            this.entries = entries;
        }

        public IList<KeyValueExpression> Entries { get { return this.entries; } }
    }
}
