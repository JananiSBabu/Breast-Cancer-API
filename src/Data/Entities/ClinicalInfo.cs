using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BreastCancerAPI.Data.Entities
{
    public class ClinicalInfo : BaseEntity
    {
        public int Id { get; set; }

        // Laterality = left, right, Bilateral, Unilateral
        public string SideOfTumor { get; set; }
        public bool HasFamilyHistory { get; set; }
        //Alive, Dead
        public string SurvivalStatus { get; set; }
        public double BMI { get; set; }
        // mammography, ultrasound, biopsy, MRI
        public string MethodOfDiagnosis { get; set; }         
        public string BreastCancerType { get; set; }
        public int DateOfFirstDiagnosis { get; set; }
        public bool ReceivedSurgery { get; set; }
        public bool ReceivedRadiationTherapy { get; set; }
        public bool ReceivedChemoTherapy { get; set; }
        public bool ReceivedHormonalTherapy { get; set; }
    }

    public enum BreastCancerType
    {
        Unknown = 0,
        DuctalCarcinomaInSitu = 1,
        InvasiveDuctalCarcinoma = 2,
        Metastatic = 3,
        Inflammatory = 4,
        TripleNegative = 5,
        LobularCarcinomaInSitu = 6,
        InvasiveLobularCarcinoma = 7,
        Other = 8
    }
}
