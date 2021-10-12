using System;
using System.Collections.Generic;

namespace HashFields.Data
{
    public interface IColumnOperator
    {
        List<string> Header { get; }

        void Apply(Func<string, string> func, params string[] columns);

        void Remove(params string[] columns);
    }
}
