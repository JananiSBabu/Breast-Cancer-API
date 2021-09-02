using BreastCancerAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BreastCancerAPI.Models
{
    public class PrognosticInfoModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "The Outcome must be 1 characters.")]
        [RegularExpression("R|N", ErrorMessage = "The Outcome must be either 'R' recurrent or 'N' Non-recurrent only.")]
        public string Outcome { get; set; }

        [Required]
        [Range(1, 200)]
        public int Time { get; set; }

        [Range(0, 50)]
        public double TumorSize { get; set; }
        public int? LymphNodeStatus { get; set; }
        public CellFeaturesModel CellFeatures { get; set; }
    }
}
