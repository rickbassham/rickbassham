using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Measurement
{
    public interface IWeightUnit : IUnit<double>
    {

    }

    public class OuncesWeight : DoubleUnitBase, IWeightUnit
    {
        protected override double BaseUnitRatio
        {
            get
            {
                return 16;
            }
        }
    }

    public class Pounds : DoubleUnitBase, IWeightUnit
    {
        protected override double BaseUnitRatio
        {
            get { return 1; }
        }
    }
}
