using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BreastCancerAPI.Models
{
    public class ClinicalInfoModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        // Laterality = left, right, Bilateral, Unilateral
        public string SideOfTumor { get; set; }
        public bool HasFamilyHistory { get; set; }
        //Alive, Dead
        public string SurvivalStatus { get; set; }
        public double BMI { get; set; }
        // mammography, ultrasound, biopsy, MRI
        public string MethodOfDiagnosis { get; set; }

        [JsonProperty("BreastCancerType")]
        public string BreastCancerType { get; set; }
        public int DateOfFirstDiagnosis { get; set; }
        public bool ReceivedSurgery { get; set; }
        public bool ReceivedRadiationTherapy { get; set; }
        public bool ReceivedChemoTherapy { get; set; }
        public bool ReceivedHormonalTherapy { get; set; }
    }
}
