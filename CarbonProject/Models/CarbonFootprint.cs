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
        public int MilesDrivenData { get; set; }
        public string FuelTypeData { get; set; }
        public int PlasticBagsUsedData { get; set; }
        public int PlasticBottlesUsedData { get; set; }
        public int PowerUsedData { get; set; }
    }
}
