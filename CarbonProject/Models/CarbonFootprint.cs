using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarbonProject.Models
{
    public class CarbonFootprint
    {
        [Key]
        public int? Id { get; set; }
        public double FuelEmissions { get; set; }
        public string FuelType { get; set; }
        public double PlasticBagsEmissions { get; set; }
        public double PlasticBottlesEmissions { get; set; }
        public double PowerUsedEmissions { get; set; }

        [ForeignKey("Environmentalist")]
        public int? EnvironmentalistId { get; set; }
        public Environmentalist Environmentalist { get; set; }
    }
}
