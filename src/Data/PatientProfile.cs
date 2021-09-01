using AutoMapper;
using BreastCancerAPI.Data.Entities;
using BreastCancerAPI.Models;

namespace BreastCancerAPI.Data
{
    public class PatientProfile : Profile
    {
        public PatientProfile()
        {
            //mapping for PatientModel
            this.CreateMap<Patient, PatientModel>().ReverseMap();

            //mapping for PrognosticInfoModel
            this.CreateMap<PrognosticInfo, PrognosticInfoModel>().ReverseMap();

            //mapping for CellFeaturesModel
            this.CreateMap<CellFeatures, CellFeaturesModel>().ReverseMap();
        }
    }
}
