﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BreastCancerAPI.Data.Entities
{
    public class Patient
    {
        public int Id { get; set; }
        public int MRN { get; set; }
        public List<PrognosticInfo> PrognosticInfos { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string PrimaryPhysician { get; set; }

    }
}
