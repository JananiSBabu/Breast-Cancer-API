using BreastCancerAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BreastCancerAPI.Models
{
    public class PatientModel
    {
        public int Id { get; set; }
        [Required]
        public int MRN { get; set; }
        public List<PrognosticInfoModel> PrognosticInfos { get; set; }
    }
}
