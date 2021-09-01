using BreastCancerAPI.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BreastCancerAPI.Data
{
    public class PatientSeeder
    {
        private readonly PatientContext _ctx;
        private readonly IWebHostEnvironment _env;

        public PatientSeeder(PatientContext ctx, IWebHostEnvironment env)
        {
            _ctx = ctx;
            _env = env;
        }

        public void Seed()
        {
            // Check if the DB for the context is present already, if not create it
            _ctx.Database.EnsureCreated();

            if (!_ctx.CellFeatures.Any() && !_ctx.Patients.Any() && !_ctx.PrognosticInfos.Any())
            {
                //// Add CellFeatures Separately
                //var filePath1 = Path.Combine(_env.ContentRootPath, "Data/SeedSource/CellFeatures_Seed.csv");
                //var cellfeaturesSeed = File.ReadAllLines(filePath1)
                //                            .Select(v => CsvToCellFeatures(v))
                //                            .ToList();
                //_ctx.CellFeatures.AddRange(cellfeaturesSeed);

                // Add Patients
                var filePath2 = Path.Combine(_env.ContentRootPath, "Data/SeedSource/BreastCancer_Seed.csv");
                var prognosticInfoSeed = File.ReadAllLines(filePath2)
                                            .Select(v => CsvToPatients(v))
                                            .ToList();
                _ctx.PrognosticInfos.AddRange(prognosticInfoSeed);
            }


            // changes in ctx will be saved to DB only now
            _ctx.SaveChanges();
        }

        public CellFeatures CsvToCellFeatures(string csvLine)
        {
            string[] values = csvLine.Split(',');
            CellFeatures cellFeatures = new CellFeatures();
            cellFeatures.Radius = Convert.ToDouble(values[0]);
            cellFeatures.Texture = Convert.ToDouble(values[1]);
            cellFeatures.Perimeter = Convert.ToDouble(values[2]);
            cellFeatures.Area = Convert.ToDouble(values[3]);
            cellFeatures.Smoothness = Convert.ToDouble(values[4]);
            cellFeatures.Compactness = Convert.ToDouble(values[5]);
            cellFeatures.Concavity = Convert.ToDouble(values[6]);
            cellFeatures.ConcavePoints = Convert.ToDouble(values[7]);
            cellFeatures.Symmetry = Convert.ToDouble(values[8]);
            cellFeatures.FractalDimension = Convert.ToDouble(values[9]);
            return cellFeatures;

        }

        public PrognosticInfo CsvToPatients(string csvLine)
        {
            string[] values = csvLine.Split(',');

            PrognosticInfo prognosticInfo = new PrognosticInfo();
            prognosticInfo.Outcome = Convert.ToString(values[1]);
            prognosticInfo.Time = Convert.ToInt32(values[2]);
            prognosticInfo.TumorSize = Convert.ToDouble(values[3]);
            prognosticInfo.LymphNodeStatus = values[4] == "" ? null : Convert.ToInt32(values[4]);
            prognosticInfo.CellFeatures = new CellFeatures()
            {
                Radius = Convert.ToDouble(values[5]),
                Texture = Convert.ToDouble(values[6]),
                Perimeter = Convert.ToDouble(values[7]),
                Area = Convert.ToDouble(values[8]),
                Smoothness = Convert.ToDouble(values[9]),
                Compactness = Convert.ToDouble(values[10]),
                Concavity = Convert.ToDouble(values[11]),
                ConcavePoints = Convert.ToDouble(values[12]),
                Symmetry = Convert.ToDouble(values[13]),
                FractalDimension = Convert.ToDouble(values[14])
            };
            prognosticInfo.Patient = new Patient()
            {
                MRN = Convert.ToInt32(values[0]),
                PrognosticInfos = new List<PrognosticInfo>() { prognosticInfo }
            };
            return prognosticInfo;
        }
    }
}
