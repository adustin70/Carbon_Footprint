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
        public double MilesDrivenData { get; set; }
        public string FuelTypeData { get; set; }
        public double PlasticBagsUsedData { get; set; }
        public double PlasticBottlesUsedData { get; set; }
        public double PowerUsedData { get; set; }
    }
}
