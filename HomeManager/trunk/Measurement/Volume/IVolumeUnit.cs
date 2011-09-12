using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Measurement
{
    public interface IVolumeUnit : IUnit<double>
    {

    }

    public class Teaspoons : DoubleUnitBase, IVolumeUnit
    {
        protected override double BaseUnitRatio
        {
            get
            {
                return 128 * 6;
            }
        }
    }

    public class Tablespoons : DoubleUnitBase, IVolumeUnit
    {
        protected override double BaseUnitRatio
        {
            get
            {
                return 128 * 3;
            }
        }
    }

    public class Ounces : DoubleUnitBase, IVolumeUnit
    {
        protected override double BaseUnitRatio
        {
            get
            {
                return 128;
            }
        }
    }

    public class Cups : DoubleUnitBase, IVolumeUnit
    {
        protected override double BaseUnitRatio
        {
            get
            {
                return 16;
            }
        }
    }

    public class Pints : DoubleUnitBase, IVolumeUnit
    {
        protected override double BaseUnitRatio
        {
            get
            {
                return 8;
            }
        }
    }

    public class Quarts : DoubleUnitBase, IVolumeUnit
    {
        protected override double BaseUnitRatio
        {
            get
            {
                return 4;
            }
        }
    }

    public class Gallons : DoubleUnitBase, IVolumeUnit
    {
        protected override double BaseUnitRatio
        {
            get
            {
                return 1;
            }
        }
    }
}
