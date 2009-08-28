using System;
using System.Collections.Generic;

namespace Bassham.Diff
{
    internal class DiffStateList
    {
        List<DiffState> _list;

        public DiffStateList(int destCount)
        {
            _list = new List<DiffState>(destCount);
        }

        public DiffState GetByIndex(int index)
        {
            DiffState val = _list[index];
            if (val == null)
            {
                val = new DiffState();
                _list.Insert(index, val);
            }
            return val;
        }
    }
}
