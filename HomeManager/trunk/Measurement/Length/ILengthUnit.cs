using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Measurement
{
    public interface ILengthUnit : IUnit<double>
    {

    }


    public class Centimeters : DoubleUnitBase, ILengthUnit
    {
        protected override double BaseUnitRatio
        {
            get { return 100; }
        }
    }

    public class Meters : DoubleUnitBase, ILengthUnit
    {
        protected override double BaseUnitRatio
        {
            get { return 1; }
        }
    }

    public class Kilometers : DoubleUnitBase, ILengthUnit
    {
        protected override double BaseUnitRatio
        {
            get { return 1D/1000D; }
        }
    }

}
