using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Measurement
{
    public interface IUnitFactory<TValue,TUnit>
        where TUnit : IUnit<TValue>
    {
        T Create<T>() where T : TUnit, new();
    }
}
