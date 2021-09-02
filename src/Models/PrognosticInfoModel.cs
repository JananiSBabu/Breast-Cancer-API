using BreastCancerAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BreastCancerAPI.Models
{
    public class PrognosticInfoModel
    {
        public int Id { get; set; }
        // values = R or N
        public string Outcome { get; set; }
        public int Time { get; set; }
        public double TumorSize { get; set; }
        public int? LymphNodeStatus { get; set; }
        public CellFeaturesModel CellFeatures { get; set; }
    }
}
