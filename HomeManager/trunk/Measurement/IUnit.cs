using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Measurement
{
    public abstract class DoubleUnitBase : NumericUnitBase<double>
    {

        public override double ConvertToBase(double unitValue)
        {
            return unitValue / BaseUnitRatio;
        }

        public override double ConvertToUnit(double baseValue)
        {
            return baseValue * BaseUnitRatio;
        }
    }

    public abstract class Int64UnitBase : NumericUnitBase<long>
    {

        public override long ConvertToBase(long unitValue)
        {
            return (long)Math.Round((unitValue / BaseUnitRatio));
        }

        public override long ConvertToUnit(long baseValue)
        {
            return (long)Math.Round((baseValue * BaseUnitRatio));
        }
    }

    public abstract class NumericUnitBase<T> : UnitBase<T>
        where T: struct
    {
        protected abstract double BaseUnitRatio { get; }
    }

    public abstract class UnitBase<T> : IUnit<T>
    {
        
        public T ToBaseValue(T unitValue)
        {
            return ConvertToBase(unitValue);
        }

        public T ToUnitValue(T baseValue)
        {
            return ConvertToUnit(baseValue);
        }

        public abstract T ConvertToBase(T unitValue);
        public abstract T ConvertToUnit(T baseValue);
    }

    public interface IUnit<T>
    {
        T ToBaseValue(T unitValue);
        T ToUnitValue(T baseValue);
    }
}
