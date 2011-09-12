using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Measurement
{
    public class Volume : DoubleTypeMeasurementBase<IVolumeUnit>
    {
        public Volume(IVolumeUnit unit, double unitValue) :
            base(unit, unitValue)
        {
        }

        public Volume(double baseValue)
            : base(baseValue)
        {
        }

        public override Type BaseUnit
        {
            get
            {
                return typeof(Gallons);
            }
        }

        protected override IUnitFactory<double, IVolumeUnit> BuildUnitFactory()
        {
            return new VolumeUnitFactory();
        }

        public static Volume operator +(Volume v1, Volume v2)
        {
            return new Volume(v1.BaseValue + v2.BaseValue);
        }

        public static Volume operator -(Volume v1, Volume v2)
        {
            return new Volume(v1.BaseValue - v2.BaseValue);
        }
    }
}
