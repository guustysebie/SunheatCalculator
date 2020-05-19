using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using static System.Double;

namespace SunHeatCalculator.Models
{
    public class DataInput
    {
        public DataInput()
        {
            AlphaListInRadial = new List<double>();
            WallAzimuthListInDegrees = new List<double>();
        }

        private string _dateString;
        public DateTime Date { get; set; }
        public double GeoGraphicWidth { get; set; }
        public double Rs { get; set; }
        public List<double> AlphaListInRadial { get; set; }
        public List<double> WallAzimuthListInDegrees { get; set; }

        public string FilleName { get; set; }

        public void ReadData()
        {
            Console.WriteLine(">>Ingevoerde data: ");
            Console.Write(">>Geef file naam: ");
            FilleName = Console.ReadLine();
            Console.Write(">>Geef Datum(dd-MM-yyyy): ");
            _dateString = Console.ReadLine();

            Date = DateTime.ParseExact(string.IsNullOrEmpty(_dateString) ? "15-04-2019" : _dateString, "dd-MM-yyyy",
                CultureInfo.InvariantCulture);

            Console.Write(">>Geografishe breedte: ");

            var temp_width = Console.ReadLine();
            GeoGraphicWidth = string.IsNullOrEmpty(temp_width) ? 52.0 : Parse(temp_width);
            Console.WriteLine();

            Console.Write(">>hellings hoeken: ");
            var tiltAngleRaw = Console.ReadLine();
            if (string.IsNullOrEmpty(tiltAngleRaw))
            {
            /*    var sb = new StringBuilder();
                for (int i = 0; i < 50; i++)
                {
                    sb.Append(i + ",");
                }

                tiltAngleRaw = sb.ToString().Remove(sb.Length - 1);
              */  tiltAngleRaw = "90";
            }

            if (tiltAngleRaw != null)
            {
                var splittedTiltAngleRaw = tiltAngleRaw.Split(",");
                foreach (var tiltAngle in splittedTiltAngleRaw)
                {
                    AlphaListInRadial.Add(TotalSunHeat.CalculateDegreeToRadian(double.Parse(tiltAngle)));
                }
            }

            Console.Write(">>wand azimuth: ");
            var wantAzimuthRaw = Console.ReadLine();
            if (string.IsNullOrEmpty(wantAzimuthRaw))
            {
              /*  var sb = new StringBuilder();
                for (int i = 0; i < 20; i++)
                {
                    sb.Append(i + ",");
                }*/

                //wantAzimuthRaw = sb.ToString().Remove(sb.Length - 1);
                wantAzimuthRaw = "180";
            }

            var splittedWallAZimuthRaw = wantAzimuthRaw.Split(",");
            foreach (var azimuth in splittedWallAZimuthRaw)
            {
                WallAzimuthListInDegrees.Add(double.Parse(azimuth));
            }

            Console.Write(">>Rs: ");
            var rsTemp = Console.ReadLine();
            if (string.IsNullOrEmpty(rsTemp))
            {
                Rs = double.Parse("0.2");
            }
            else
            {
                Rs = double.Parse(rsTemp);
            }

            Console.WriteLine(">>Berekende data");
        }
    }
}