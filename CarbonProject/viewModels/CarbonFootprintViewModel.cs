using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarbonProject.viewModels
{
    public class CarbonFootprintViewModel
    {
        public double FuelEmissions { get; set; }
        public double PlasticBagsEmissions { get; set; }
        public double PlasticBottlesEmissions { get; set; }
        public double PowerUsedEmissions { get; set; }
        public double SignedInFuelEmissions { get; set; }
        public double SignedInPlasticBagsEmissions { get; set; }
        public double SignedInPlasticBottlesEmissions { get; set; }
        public double SignedInPowerUsedEmissions { get; set; }
    }
}
