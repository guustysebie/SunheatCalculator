using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using CsvHelper;
using CsvHelper.Configuration;

namespace SunHeatCalculator.Data
{
    public class Zonzimuth
    {
        private List<ZonzimuthDataClass> data;

        public Zonzimuth()
        {
            data = LoadData();
        }

        private List<ZonzimuthDataClass> LoadData()
        {
            using var reader =
                new StreamReader("/home/guust/Code/ward/troutenspel/SunHeatCalculator/SunHeatCalculator/zonzimuth.csv");
            using var csv = new CsvReader(reader);
            csv.Configuration.Delimiter = ",";
            csv.Configuration.HeaderValidated = null;
            csv.Configuration.MissingFieldFound = null;
            var dataFromCsv = csv.GetRecords<ZonzimuthDataClass>().ToList();
            return dataFromCsv;
        }

        public List<Tuple<int, double>> GetFromMonth(DateTime dateTime)
        {
            var month = dateTime.Month;
            return data.Select(p => new Tuple<int, double>(p.zonnetijd, GiveZonzimuth(p, month))).ToList();
        }


        private double GiveZonzimuth(ZonzimuthDataClass data, int month)
        {
            var val = month switch
            {
                1 => data.jan,
                2 => data.feb,
                3 => data.maa,
                4 => data.apr,
                5 => data.mei,
                6 => data.jun,
                7 => data.jul,
                8 => data.aug,
                9 => data.sept,
                10 => data.okt,
                11 => data.nov,
                12 => data.dec,
                _ => 0
            };
            return val;
        }
    }

    public class ZonzimuthDataClass
    {
        public int zonnetijd { get; set; }
        public double jan { get; set; }
        public double feb { get; set; }
        public double maa { get; set; }
        public double apr { get; set; }
        public double mei { get; set; }
        public double jun { get; set; }
        public double jul { get; set; }
        public double aug { get; set; }
        public double sept { get; set; }
        public double okt { get; set; }
        public double nov { get; set; }
        public double dec { get; set; }
    }
}