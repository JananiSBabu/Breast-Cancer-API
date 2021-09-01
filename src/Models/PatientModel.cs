using BreastCancerAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BreastCancerAPI.Models
{
    public class PatientModel
    {
        public int Id { get; set; }
        public int MRN { get; set; }
        public List<PrognosticInfo> PrognosticInfos { get; set; }
    }
}
