using System;
using System.Text;
using SunHeatCalculator.Models;

namespace SunHeatCalculator.Formatter
{
    public class Formatting
    {
        public static string FormatHeightOfSun(TotalSunHeat data)
        {
            var sbHour = new StringBuilder();
            sbHour.AppendFormat("|{0,13}|", "uur");
            var sbValue = new StringBuilder();
            sbValue.AppendFormat("|{0,13}|", "zonnehoogte");
            var sbQdirn = new StringBuilder();
            sbQdirn.AppendFormat("|{0,13}|", "qdirn");
            var sbaz = new StringBuilder();
            sbaz.AppendFormat("|{0,13}|", "Zons azimuth");
            Console.WriteLine(new StringBuilder().AppendFormat("{0,-20}", ">>Hoogte zon:"));
            var list = data.CalculateDataForDay();

            foreach (var (item1, item2, item3, item4) in list)
            {
                sbHour.AppendFormat("|{0,8}|",  item1);
                sbValue.AppendFormat("|{0,8:N2}|", item2);
                sbQdirn.AppendFormat("|{0,8:N0}|", item3);
                sbaz.AppendFormat("|{0,8:N0}|", item4);
            }

            return sbHour + "\n" + sbValue + "\n" + sbQdirn + "\n" + sbaz + "\n";
        }

        public static string FormatEntryAngles(TotalSunHeat data)
        {
            var sb = new StringBuilder();
            var sbHour = new StringBuilder();
            var sbVal = new StringBuilder();
            var listData = data.CalculateEntryAngleForAll();
            
           

            sb.AppendLine(">>Overzicht hellingshoeken: ");
            foreach (var dat in listData)
            {
                sb.AppendFormat(">>Alpha: {0}\n", TotalSunHeat.CalculateRadianToDegree(dat.Item1));
                sb.AppendFormat(">>WandAzimuth: {0}\n", dat.Item2);
                var list = dat.Item3;
                sbHour = new StringBuilder();
                sbVal = new StringBuilder();
                sbHour.AppendFormat("|{0,13}|", "uur");
                sbVal.AppendFormat("|{0,13}|", "invals hoek");
                foreach (var yeet in list)
                {
                    sbHour.AppendFormat("|{0,8}|",  yeet[0]);
                    sbVal.AppendFormat("|{0,8:N2}|", TotalSunHeat.CalculateRadianToDegree(yeet[1]));
                }

                sb.AppendLine(sbHour.ToString());
                sb.AppendLine(sbVal.ToString());
                sb.Append("\n");
                sb.Append("\n");
            }

            return sb.ToString();
        }


        public static string FormatSunRayHorizontal(TotalSunHeat data)
        {
            var sb = new StringBuilder();
            var sbHour = new StringBuilder();
            var sbVal = new StringBuilder();
            var sbVal2 = new StringBuilder();
            var listData = data.CalculateSunRayOnHorizontalSpot();

            sbHour.AppendFormat("|{0,13}|", "uur");
            sbVal.AppendFormat("|{0,13}|", "QdirH");
            sbVal2.AppendFormat("|{0,13}|", "QhemH");
            sb.AppendLine(">>Overzicht zonnestraling op horizontaal vlak: ");
            foreach (var dat in listData)
            {
               
                sbHour.AppendFormat("|{0,8}|", + dat.Item1);
                sbVal.AppendFormat("|{0,8:N2}|", dat.Item2);
                sbVal2.AppendFormat("|{0,8:N2}|", dat.Item3);


             
            }
            sb.AppendLine(sbHour.ToString()); 
            sb.AppendLine(sbVal.ToString());
            sb.AppendLine(sbVal2.ToString());
            sb.Append("\n");
            return sb.ToString();
        }



        public static string FormatTotal(TotalSunHeat data)
        {
            
            var sb = new StringBuilder();
            
            var listData = data.CalculateTotal();

          
            sb.AppendLine(">>Totaal Qz1: ");
            foreach (var dat in listData)
            {

                var sbAwaplhaComb = new StringBuilder();
                var aplh = dat.Item1;
                var Aw = dat.Item2;
                sbAwaplhaComb.AppendLine(">>aplha: " + TotalSunHeat.CalculateRadianToDegree(aplh));
                sbAwaplhaComb.AppendLine(">>Aw: " + Aw   );

                var list = dat.Item3;
                
                var sbHour = new StringBuilder();
                var sbVal = new StringBuilder();
                
                sbHour.AppendFormat("|{0,13}|", "uur");
                sbVal.AppendFormat("|{0,13}|", "Qz");
                foreach (var tuple in list)
                {
                    sbHour.AppendFormat("|{0,8}|", + tuple.Item1);
                    sbVal.AppendFormat("|{0,8:N2}|", tuple.Item2);
                  
                }
             
                sbAwaplhaComb.AppendLine(sbHour.ToString()); 
                sbAwaplhaComb.AppendLine(sbVal.ToString());
                sbAwaplhaComb.AppendLine();

                
                sb.Append(sbAwaplhaComb.ToString());
            }
           
            sb.Append("\n");
            return sb.ToString();
            
        }

        public static string FormatSunRayOnVertical(TotalSunHeat data)
        {
            
            var sb = new StringBuilder();
            
            var listData = data.CalculateSunRayOnVerticalScreen();

          
            sb.AppendLine(">>Overzicht zonnestraling op verticaal vlak: ");
            foreach (var dat in listData)
            {

                var sbAwaplhaComb = new StringBuilder();
                var aplh = dat.Item1;
                var Aw = dat.Item2;
                sbAwaplhaComb.AppendLine(">>aplha: " + TotalSunHeat.CalculateRadianToDegree(aplh));
                sbAwaplhaComb.AppendLine(">>Aw: " + Aw   );

                var list = dat.Item3;
                
                var sbHour = new StringBuilder();
                var sbVal = new StringBuilder();
                var sbVal2 = new StringBuilder();
                var sbVal3 = new StringBuilder();
                
                sbHour.AppendFormat("|{0,13}|", "uur");
                sbVal.AppendFormat("|{0,13}|", "QdirV");
                sbVal2.AppendFormat("|{0,13}|", "QhemV");
                sbVal3.AppendFormat("|{0,13}|", "Qgrv");
                foreach (var tuple in list)
                {
                    sbHour.AppendFormat("|{0,8}|", + tuple.Item1);
                    sbVal.AppendFormat("|{0,8:N2}|", tuple.Item2);
                    sbVal2.AppendFormat("|{0,8:N2}|", tuple.Item3);
                    sbVal3.AppendFormat("|{0,8:N2}|", tuple.Item4);
                }
             
                sbAwaplhaComb.AppendLine(sbHour.ToString()); 
                sbAwaplhaComb.AppendLine(sbVal.ToString());
                sbAwaplhaComb.AppendLine(sbVal2.ToString());
                sbAwaplhaComb.AppendLine(sbVal3.ToString());
                sbAwaplhaComb.AppendLine();

                
                sb.Append(sbAwaplhaComb.ToString());
            }
           
            sb.Append("\n");
            return sb.ToString();
            
        }

        public static string FormatSunRayOnTilted(TotalSunHeat data)
        {
            
            var sb = new StringBuilder();
            
            var listData = data.CalculateSunRayOnTiltedScreen();

          
            sb.AppendLine(">>Overzicht zonnestraling op scheef vlak: ");
            foreach (var dat in listData)
            {

                var sbAwaplhaComb = new StringBuilder();
                var aplh = dat.Item1;
                var Aw = dat.Item2;
                sbAwaplhaComb.AppendLine(">>aplha: " + TotalSunHeat.CalculateRadianToDegree( aplh));
                sbAwaplhaComb.AppendLine(">>Aw: " + Aw   );

                var list = dat.Item3;
                
                var sbHour = new StringBuilder();
                var sbVal = new StringBuilder();
                var sbVal2 = new StringBuilder();
                var sbVal3 = new StringBuilder();
                
                sbHour.AppendFormat("|{0,13}|", "uur");
                sbVal.AppendFormat("|{0,13}|", "Qdir");
                sbVal2.AppendFormat("|{0,13}|", "Qhem");
                sbVal3.AppendFormat("|{0,13}|", "Qgr");
                foreach (var tuple in list)
                {
                    sbHour.AppendFormat("|{0,8}|", + tuple.Item1);
                    sbVal.AppendFormat("|{0,8:N2}|", tuple.Item2);
                    sbVal2.AppendFormat("|{0,8:N2}|", tuple.Item3);
                    sbVal3.AppendFormat("|{0,8:N2}|", tuple.Item4);
                }
             
                sbAwaplhaComb.AppendLine(sbHour.ToString()); 
                sbAwaplhaComb.AppendLine(sbVal.ToString());
                sbAwaplhaComb.AppendLine(sbVal2.ToString());
                sbAwaplhaComb.AppendLine(sbVal3.ToString());
                sbAwaplhaComb.AppendLine();

                
                sb.Append(sbAwaplhaComb.ToString());
            }
           
            sb.Append("\n");
            return sb.ToString();
            
        }

        

        public static string FormatOutput(string command, string solution)
        {
            return new StringBuilder().AppendFormat("{0,-20}{1}", ">>" + command + ":", solution).ToString();
        }
    }
}