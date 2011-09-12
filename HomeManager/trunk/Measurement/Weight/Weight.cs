using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Measurement
{
    public class Weight : DoubleTypeMeasurementBase<IWeightUnit>
    {

        public Weight(IWeightUnit unit, double unitValue) :
            base(unit, unitValue) { }

        public Weight(double baseValue) : base(baseValue) { }

        public override Type BaseUnit
        {
            get { return typeof(Pounds); }
        }

        protected override IUnitFactory<double, IWeightUnit> BuildUnitFactory()
        {
            return new WeightUnitFactory();
        }

        public static Weight operator +(Weight w1, Weight w2)
        {
            return new Weight(w1.BaseValue + w2.BaseValue);
        }

        public static Weight operator -(Weight w1, Weight w2)
        {
            return new Weight(w1.BaseValue - w2.BaseValue);
        }


    }
}
