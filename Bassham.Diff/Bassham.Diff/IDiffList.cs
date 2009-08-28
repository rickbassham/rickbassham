using System;

namespace Bassham.Diff
{
    public interface IDiffList
    {
        int Count();
        IComparable GetByIndex(int index);
    }
}
