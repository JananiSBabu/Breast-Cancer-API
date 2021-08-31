using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BreastCancerAPI.Data.Entities
{
    public class Patient
    {
        public int Id { get; set; }

        public ICollection<PrognosticInfo> PrognosticInfos { get; set; }

        //public CellFeatures CellFeatures { get; set; }

    }
}
