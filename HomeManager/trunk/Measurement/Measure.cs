using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Measurement
{
    public class Measure
    {
        public static double Tolerance = 0.00001;

        private static DurationUnitFactory _durationUnitFactory = new DurationUnitFactory();
        private static LengthUnitFactory _lengthUnitFactory = new LengthUnitFactory();
        private static WeightUnitFactory _weightUnitFactory = new WeightUnitFactory();
        private static AreaUnitFactory _areaUnitFactory = new AreaUnitFactory();
        private static VolumeUnitFactory _volumeUnitFactory = new VolumeUnitFactory();

        public static Length Length<TUnit>(double unitValue)
            where TUnit : ILengthUnit, new()
        {
            TUnit unit = _lengthUnitFactory.Create<TUnit>();
            return new Length(unit, unitValue);
        }

        public static Duration Duration<TUnit>(double unitValue)
            where TUnit : IDurationUnit, new()
        {
            TUnit unit = _durationUnitFactory.Create<TUnit>();
            return new Duration(unit, unitValue);
        }

        public static Weight Weight<TUnit>(double unitValue)
             where TUnit : IWeightUnit, new()
        {
            TUnit unit = _weightUnitFactory.Create<TUnit>();
            return new Weight(unit, unitValue);
        }

        public static Area Area<TUnit>(double unitValue)
            where TUnit : IAreaUnit, new()
        {
            TUnit unit = _areaUnitFactory.Create<TUnit>();
            return new Area(unit, unitValue);
        }

        public static Volume Volume<TUnit>(double unitValue)
            where TUnit : IVolumeUnit, new()
        {
            TUnit unit = _volumeUnitFactory.Create<TUnit>();
            return new Volume(unit, unitValue);
        }
    }
}
