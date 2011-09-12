using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Measurement
{
    public interface IDurationUnit : IUnit<double>
    {

    }

    public class Seconds : DoubleUnitBase, IDurationUnit
    {
        protected override double BaseUnitRatio
        {
            get { return 3600; }
        }
    }

    public class Minutes : DoubleUnitBase, IDurationUnit
    {
        protected override double BaseUnitRatio
        {
            get { return 60; }
        }
    }

    public class Hours : DoubleUnitBase, IDurationUnit
    {
        protected override double BaseUnitRatio
        {
            get { return 1; }
        }
    }

    public class Days : DoubleUnitBase, IDurationUnit
    {
        protected override double BaseUnitRatio
        {
            get { return 1D/24D; }
        }
    }
}
