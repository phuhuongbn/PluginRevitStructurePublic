using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dl = DSCore.List;
using ds = DSCore.String;

namespace Structure.OOP
{
    class ObjectList
    {
        public Tuple<List<object>, List<double>> SortByKeys(List<object> _values, List<double> _keys)
        {
            var result = new Tuple<List<object>, List<double>>(new List<object>(), new List<double>());

            if (dl.Count(_values) != dl.Count(_keys) || dl.Count(_values) == 0 || dl.Count(_keys) == 0)  return result; 

            List<double> keysSort = new List<double>();
            List<object> valuesSort = new List<object>();

            while (dl.Count(_keys) != 0)
            {
                var minKeys = _keys.Min();
                var keysSave = new List<double>();
                var valuesSave = new List<object>();
                var index = 0;
                foreach(var key in _keys)
                {
                    if (key == minKeys)
                    {
                        keysSort.Add(key);
                        valuesSort.Add(_values[index]);
                    }
                    else
                    {
                        keysSave.Add(key);
                        valuesSave.Add(_values[index]);
                    }
                    index = index + 1;
                }
                _keys = keysSave;
                _values = valuesSave;
                if (keysSave.Count == 0) { break; }
            }

            return new Tuple<List<object>, List<double>>(valuesSort, keysSort);
        }
    }
}
