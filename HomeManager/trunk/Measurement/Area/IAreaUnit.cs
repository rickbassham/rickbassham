using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Measurement
{
    public interface IAreaUnit : IUnit<double>
    {

    }


    public class SquareCentimeters : DoubleUnitBase, IAreaUnit
    {
        protected override double BaseUnitRatio
        {
            get { return 10000; }
        }
    }

    public class SquareMeters : DoubleUnitBase, IAreaUnit
    {
        protected override double BaseUnitRatio
        {
            get { return 1; }
        }
    }

    public class SquareKilometers : DoubleUnitBase, IAreaUnit
    {
        protected override double BaseUnitRatio
        {
            get { return 1D/1000000D; }
        }
    }
}
