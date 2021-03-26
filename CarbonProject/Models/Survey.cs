using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarbonProject.Models
{
    public class Survey
    {
        [Key]
        public int? Id { get; set; }
        public double MilesDriven { get; set; }
        public string FuelType { get; set; }
        public double PlasticBagsUsed { get; set; }
        public double PlasticBottlesUsed { get; set; }
        public double PowerUsed { get; set; }

        [ForeignKey("Environmentalist")]
        public int? EnvironmentalistId { get; set; }
        public Environmentalist Environmentalist { get; set; }
    }
}
