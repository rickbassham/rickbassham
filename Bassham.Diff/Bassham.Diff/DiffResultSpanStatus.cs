using System;

namespace Bassham.Diff
{
    public enum DiffResultSpanStatus
    {
        NoChange,
        Replace,
        DeleteSource,
        AddDestination
    }
}
