using System;
using System.Globalization;
using SunHeatCalculator.Models;
using Xunit;

namespace SunHeatCalculator_Test
{
    public class UnitTest1
    {

        [Fact]
        public void TestRadianToAngleConversion()
        {
            var input = new DataInput();
            input.Date  =DateTime.ParseExact("15-08-2019", "dd-MM-yyyy", CultureInfo.InvariantCulture);
            input.GeoGraphicWidth = 52.0;
            var calc = new TotalSunHeat(input);
            Assert.Equal(57.2958, TotalSunHeat.CalculateRadianToDegree(1),4);
            
        }

        
        [Fact]
        public void TestSunheatStraightOOnSurface()
        {
            var input = new DataInput();
            input.Date  =DateTime.ParseExact("15-08-2019", "dd-MM-yyyy", CultureInfo.InvariantCulture);
            input.GeoGraphicWidth = 52.0;
            var calc = new TotalSunHeat(input);
            Assert.Equal(52.1, TotalSunHeat.CalculateRadianToDegree( calc.CalculateSunHeight(12)),1);
            Assert.Equal(854.5, calc.CalculateQdirN(12),1);
            
        }

        
    }
}