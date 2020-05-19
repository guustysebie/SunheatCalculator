using System;
using System.Collections.Generic;
using System.Linq;
using SunHeatCalculator.Data;

namespace SunHeatCalculator.Models
{
    public class TotalSunHeat
    {
        private DataInput _input;
        private Zonzimuth _data;
        private const int StartHour = 4;
        private const int EndHour = 22;

        public TotalSunHeat(DataInput input)
        {
            _input = input;
            _data = new Zonzimuth();
        }

        public double CalculateQDIR0()
        {
            var dayOfTheYear = _input.Date.DayOfYear;
            var temp = Math.Sin(CalculateDegreeToRadian((((double) dayOfTheYear - 93) * (360)) / 365));
            return 1355 * (1 - 0.033 * temp);
        }

        public double CalculateDeclination()
        {
            var k = _input.Date.DayOfYear;
            var temp = Math.Sin(CalculateDegreeToRadian(((double) k - 82) * 360 / 365));
            return 23.45 * temp;
        }

        public double CalculateSunHeight(int hour)
        {
            // ReSharper disable once InconsistentNaming
            var L = CalculateDegreeToRadian(_input.GeoGraphicWidth);
            // ReSharper disable once InconsistentNaming
            var D = CalculateDegreeToRadian(CalculateDeclination());
            // ReSharper disable once InconsistentNaming
            var U = CalculateDegreeToRadian(CalculateHourAngle(hour));
            return Math.Asin(Math.Sin(L) *
                             Math.Sin(D) -
                             Math.Cos(L) *
                             Math.Cos(D) *
                             Math.Cos(U));
        }

        public List<Tuple<int, double, double, double>> CalculateDataForDay()
        {
            var data = new List<Tuple<int, double, double, double>>();

            for (var i = StartHour; i <= EndHour; i++)
            {
                var hour = i;
                var a = new Tuple<int, double, double, double>(hour,
                    CalculateRadianToDegree(CalculateSunHeight(hour)),
                    CalculateQdirN(hour)
                    , CalculateSunsAzimuthForHour(hour)
                );
                data.Add(a);
            }

            return data;
        }


        private static double CalculateHourAngle(int hour)
        {
            return hour * 15;
        }


        /// <summary>
        /// QDIRN
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        public double CalculateQdirN(int hour)
        {
            var paramameters = Data.GetDataByMonth(_input.Date);
            // ReSharper disable once InconsistentNaming
            var X = paramameters.Item2;
            // ReSharper disable once InconsistentNaming
            var Y = paramameters.Item3;
            // ReSharper disable once InconsistentNaming
            var Z = paramameters.Item4;
            return X - Y * Math.Pow(Math.E, (-Z * Math.Sin(CalculateSunHeight(hour))));
        }

        public double CalculateSunsAzimuthForHour(int hour)
        {
            var a = _data.GetFromMonth(_input.Date).FirstOrDefault(p => p.Item1 == hour);
            return a?.Item2 ?? 0.0;
        }


        public List<Tuple<double, double, List<double[]>>> CalculateEntryAngleForAll()
        {
            var dataList = new List<Tuple<double, double, List<double[]>>>();

            foreach (var alpha in _input.AlphaListInRadial)
            {
                foreach (var Aw in _input.WallAzimuthListInDegrees)
                {
                    var data = new Tuple<double, double, List<double[]>>(alpha, Aw,
                        CalculateEntryAngleForDay(alpha, Aw));
                    dataList.Add(data);
                }
            }

            return dataList;
        }


        public List<Tuple<int, double, double>> CalculateSunRayOnHorizontalSpot()
        {
            var data = new List<Tuple<int, double, double>>();

            for (var i = StartHour; i <= EndHour; i++)
            {
                data.Add(new Tuple<int, double, double>(i, CalculateQdirH(i), CalculateQhemH(i)));
            }

            return data;
        }


        public List<Tuple<double, double, List<Tuple<int, double, double, double>>>> CalculateSunRayOnVerticalScreen()
        {
            var datList = new List<Tuple<double, double, List<Tuple<int, double, double, double>>>>();

            foreach (var alpha in _input.AlphaListInRadial)
            {
                foreach (var Aw in _input.WallAzimuthListInDegrees)
                {
                    var hourList = new List<Tuple<int, double, double, double>>();
                    for (int i = StartHour; i <= EndHour; i++)
                    {
                        var data = new Tuple<int, double, double, double>(i,
                            CalculateQdirV(i, Aw)
                            , CalculateHemV(i, alpha, Aw),
                            CalculateQgrV(i)
                        );
                        hourList.Add(data);
                    }

                    var dataMain = new Tuple<double, double, List<Tuple<int, double, double, double>>>(
                        alpha,
                        Aw,
                        hourList
                    );
                    datList.Add(dataMain);
                }
            }

            return datList;
        }

        public double CalculateQdir(int hour, double aplha, double Aw)
        {
            return CalculateQdirH(hour) * Math.Cos(aplha) + CalculateQdirV(hour, Aw) * Math.Sin(aplha);
        }

        public double CalculateQhem(int hour, double aplha, double Aw)
        {
            return CalculateQhemH(hour) * Math.Cos(aplha) + CalculateHemV(hour, aplha, Aw) * Math.Sin(aplha);
        }

        public double CalculateQgr(int hour, double aplha, double Aw)
        {
            return 0.5 * (1 - Math.Cos(aplha)) * _input.Rs * (CalculateQdirH(hour) + CalculateQhemH(hour));
        }

        
        public List<Tuple<double, double, List<Tuple<int, double, double, double>>>> CalculateSunRayOnTiltedScreen()
        {
            var datList = new List<Tuple<double, double, List<Tuple<int, double, double, double>>>>();

            foreach (var alpha in _input.AlphaListInRadial)
            {
                foreach (var Aw in _input.WallAzimuthListInDegrees)
                {
                    var hourList = new List<Tuple<int, double, double, double>>();
                    for (int i = StartHour; i <= EndHour; i++)
                    {
                        var data = new Tuple<int, double, double, double>(i,
                            CalculateQdir(i,alpha,Aw)
                            , CalculateQhem(i,alpha,Aw),
                            CalculateQgr(i,alpha,Aw)
                        );
                        hourList.Add(data);
                    }

                    var dataMain = new Tuple<double, double, List<Tuple<int, double, double, double>>>(
                        alpha,
                        Aw,
                        hourList
                    );
                    datList.Add(dataMain);
                }
            }

            return datList;
        }


        public List<Tuple<double, double, List<Tuple<int, double>>>> CalculateTotal()
        {
            var datList = new List<Tuple<double, double, List<Tuple<int, double>>>>();
            foreach (var alpha in _input.AlphaListInRadial)
            {
                foreach (var Aw in _input.WallAzimuthListInDegrees)
                {
                    var hourList = new List<Tuple<int, double>>();
                    for (int i = StartHour; i <= EndHour; i++)
                    {
                        var data = new Tuple<int, double>(i,
                            CalculateQz(i,alpha,Aw)
                        );
                        hourList.Add(data);
                    }
                    var dataMain = new Tuple<double, double, List<Tuple<int, double>>>(
                        alpha,
                        Aw,
                        hourList
                    );
                    datList.Add(dataMain);
                }
            }
            return datList;
        }


        public double CalculateQz(int hour, double aplha, double Aw)
        {
            return CalculateQdir(hour, aplha, Aw) + CalculateQhem(hour, aplha, Aw) + CalculateQgr(hour, aplha, Aw);
        }


        public double CalculateQdirV(int hour, double Aw)
        {
            var Azero = CalculateSunsAzimuthForHour(hour);
            var AzeroMinusAwInRadial = CalculateDegreeToRadian(Azero - Aw);
            return CalculateQdirN(hour) * Math.Cos(CalculateSunHeight(hour) * Math.Cos(AzeroMinusAwInRadial));
        }

        public double CalculateHemV(int hour, double alpha, double Aw)
        {
            var phi = CalculateEntryAngleSingle(hour, alpha, Aw)[1];
            var cosOfPhi = Math.Cos(phi);
            if (cosOfPhi < -0.3)
            {
                return CalculateQhemH(hour) * (0.473 + 0.043 * cosOfPhi);
            }
            else
            {
                return CalculateQhemH(hour) * (0.56 + 0.436 * cosOfPhi + 0.35 * (cosOfPhi * cosOfPhi));
            }
        }

        public double CalculateQgrV(int hour)
        {
            return 0.5 * _input.Rs * (CalculateQdirH(hour) + CalculateQhemH(hour));
        }


        public double CalculateQdirH(int hour)
        {
            var QDiRN = CalculateQdirN(hour);
            return QDiRN * Math.Sin(CalculateSunHeight(hour));
        }

        public double CalculateQhemH(int hour)
        {
            return ((CalculateQDIR0() - CalculateQdirN(hour)) * Math.Sin(CalculateSunHeight(hour))) / 3;
        }

        public List<double[]> CalculateEntryAngleForDay(double aplha, double Aw)
        {
            var data = new List<double[]>();
            for (int i = StartHour; i <= EndHour; i++)
            {
                data.Add(CalculateEntryAngleSingle(i, aplha, Aw));
            }

            return data;
        }


        public double[] CalculateEntryAngleSingle(int hour, double alpha, double Aw)
        {
            var h = CalculateSunHeight(hour);
            var Azero = CalculateSunsAzimuthForHour(hour);
            var AzeroMinusAwInRadial = CalculateDegreeToRadian(Azero - Aw);
            return new double[]
            {
                hour,
                Math.Acos(Math.Sin(h) * Math.Cos(alpha) +
                          Math.Cos(h) * Math.Sin(alpha) * Math.Cos(AzeroMinusAwInRadial))
            };
        }

        public static double CalculateDegreeToRadian(double degrees)
        {
            return Math.PI * degrees / 180.0;
        }

        public static double CalculateRadianToDegree(double radian)
        {
            return (radian * 180) / Math.PI;
        }
    }


    public static class Data
    {
        private static List<Tuple<int, int, int, double>> MonthlyData()
        {
            var data = new List<Tuple<int, int, int, double>>()
            {
                new Tuple<int, int, int, double>(1, 900, 1129, 5.25),
                new Tuple<int, int, int, double>(2, 958, 1104, 4.35),
                new Tuple<int, int, int, double>(3, 968, 1104, 4.35),
                new Tuple<int, int, int, double>(4, 976, 1067, 3.76),
                new Tuple<int, int, int, double>(5, 966, 1010, 3.09),
                new Tuple<int, int, int, double>(6, 949, 988, 2.97),
                new Tuple<int, int, int, double>(7, 942, 983, 2.99),
                new Tuple<int, int, int, double>(8, 937, 991, 3.15),
                new Tuple<int, int, int, double>(9, 943, 1063, 3.49),
                new Tuple<int, int, int, double>(10, 939, 1063, 4.07),
                new Tuple<int, int, int, double>(11, 957, 1142, 5.84),
                new Tuple<int, int, int, double>(12, 857, 1142, 5.84),
            };
            return data;
        }


        public static Tuple<int, int, int, double> GetDataByMonth(DateTime date)
        {
            return MonthlyData()[date.Month - 1];
        }
    }
}