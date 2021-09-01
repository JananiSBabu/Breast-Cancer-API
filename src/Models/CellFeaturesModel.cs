using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BreastCancerAPI.Models
{
    public class CellFeaturesModel
    {
        public int Id { get; set; }
        public double Radius { get; set; }
        public double Texture { get; set; }
        public double Perimeter { get; set; }
        public double Area { get; set; }
        public double Smoothness { get; set; }
        public double Compactness { get; set; }
        public double Concavity { get; set; }
        public double ConcavePoints { get; set; }
        public double Symmetry { get; set; }
        public double FractalDimension { get; set; }
    }
}
