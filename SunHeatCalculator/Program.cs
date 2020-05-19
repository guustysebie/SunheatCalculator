using System;
using System.Diagnostics;
using System.Text;
using SunHeatCalculator.Data;
using SunHeatCalculator.Formatter;
using SunHeatCalculator.Models;

namespace SunHeatCalculator
{
    class Program
    {
        static void Main(string[] args)
        
        {

            Console.WriteLine("Welcome to sun heat calculator alpha-1.01!");
            Console.WriteLine("Made by WHIZX-IT");
            var input = new DataInput();
            input.ReadData();
            Stopwatch stopwatch = Stopwatch.StartNew();
            var calc = new TotalSunHeat(input);
            ExcelFormatter formatter = new ExcelFormatter(calc,input);
            
            formatter.GenerateAll();
            
           /*
            Console.WriteLine(Formatting.FormatOutput("QDIR_0", calc.CalculateQDIR0().ToString()));
            Console.WriteLine(Formatting.FormatOutput("D", calc.CalculateDeclination()+ " graden"));
            Console.WriteLine(Formatting.FormatHeightOfSun(calc));
            Console.WriteLine(Formatting.FormatEntryAngles(calc));
            Console.WriteLine(Formatting.FormatSunRayHorizontal(calc));
            Console.WriteLine(Formatting.FormatSunRayOnVertical(calc));
            Console.WriteLine(Formatting.FormatSunRayOnTilted(calc));
            Console.WriteLine(Formatting.FormatTotal(calc));*/
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            Console.WriteLine("Done");
        }
    }
}