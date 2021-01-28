using System;
using System.Collections.Generic;
using System.Text;


namespace VitoTestAPI.Helpers
{
    public class Hexhelper
    {
        public static List<String> HexConv(string hex)
        {
            List<string> callwaarden = new List<string>();
            List<string> binary = new List<string>();
            int Calltype = 0;
            string temp = "";
            binary.AddRange(hextobinary(hex));

            temp = binary[0] + binary[1];
            Calltype = Convert.ToInt32(temp, 2);
            callwaarden.Add(Calltype.ToString());
            binary.RemoveAt(0);
            binary.RemoveAt(0);
            switch (Calltype)
            {

                case 1:
                    callwaarden.AddRange(Bytointmeting(binary));
                    break;
                case 2:
                    callwaarden.AddRange(Bytointmeting(binary));
                    break;
                case 3:

                    callwaarden.AddRange(monitoring(binary));
                    break;
                default:

                    break;
            };
            return callwaarden;
        }
        public static List<string> monitoring(List<string> binary)
        {
            string val = "";
            List<string> monitoringWaarden = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                val = Convert.ToString(bytetodec(binary[0] + binary[1]));
                monitoringWaarden.Add(val);
                binary.RemoveAt(0);
                binary.RemoveAt(0);
            }


            val = Convert.ToString(bytetodec(binary[0] + binary[1]) - 30);
            monitoringWaarden.Add(val);
            binary.RemoveAt(0);
            binary.RemoveAt(0);
            monitoringWaarden.AddRange(coordextract(binary));
            return monitoringWaarden;
        }
        public static List<string> coordextract(List<string> binary)
        {
            List<string> coordinaten = new List<string>();

            int val = bytetodec(binary[0] + binary[1]);
            int latp = 0;
            int longp = 0;
            binary.RemoveAt(0);
            binary.RemoveAt(0);

            switch (val)
            {
                case 0:
                    latp = 1;
                    longp = 1;
                    break;
                case 1:
                    latp = -1;
                    longp = 1;
                    break;
                case 2:
                    latp = 1;
                    longp = -1;
                    break;
                case 3:
                    latp = -1;
                    longp = -1;
                    break;
            }
            string longe = "";
            string late = "";
            int pos = 1;
            for (int n = 0; n < 6; n++)
            {
                longe += binary[0];

                binary.RemoveAt(0);
            }
            Console.WriteLine(longe);

            longe = Convert.ToString((Convert.ToDouble(bytetodec(longe) * longp)) / 100000);
            pos = 1;

            for (int n = 0; n < 6; n++)
            {
                late += binary[0];

                binary.RemoveAt(0);
            }

            late = Convert.ToString((Convert.ToDouble(bytetodec(late) * latp)) / 100000);
            coordinaten.Add(longe);
            coordinaten.Add(late);
            coordinaten.Add(longe + ";" + late);
            //Console.WriteLine("long " + longe);
            //Console.WriteLine("late " + late);
            return coordinaten;

        }
        public static List<string> Bytointmeting(List<string> binary)
        {
            List<string> metingen = new List<string>();
            for (int c = 0; c < binary.Count; c = c + 2)
            {
                string current = binary[c] + binary[c + 1];
                metingen.Add(Convert.ToString(Convert.ToInt32(current, 2)));


            }
            return metingen;
        }
        public static int bytetodec(string decvalue)
        {
            int value = 0;
            value = Convert.ToInt32(decvalue, 2);
            return value;
        }
        public static List<string> hextobinary(string hex)
        {
            List<string> binary = new List<string>();

            string Fbit = "";
            int decimalconv = 0;
            foreach (char c in hex)
            {

                decimalconv = Convert.ToInt32(c.ToString(), 16);
                Fbit = Convert.ToString(decimalconv, 2);
                if (Fbit.Length < 4)
                {
                    Fbit = new string('0', 4 - Fbit.Length) + Fbit;
                }
                binary.Add(Fbit);




            }
            return binary;
        }

        public List<double> longconv(double lon, double lat)
        {
            List<double> vertex = new List<double>();
            vertex.Add(lon);
            vertex.Add(lat);
            double smRadius = 6378136.98;
            double smRange = smRadius * Math.PI * 2.0;
            double smLonToX = smRange / 360.0;
            double smRadiansOverDegrees = Math.PI / 180.0;

            // compute x-map-unit
            vertex[0] *= smLonToX;

            double y = vertex[1];

            // compute y-map-unit
            if (y > 86.0)
            {
                vertex[1] = smRange;
            }
            else if (y < -86.0)
            {
                vertex[1] = -smRange;
            }
            else
            {
                y *= smRadiansOverDegrees;
                y = Math.Log(Math.Tan(y) + (1.0 / Math.Cos(y)), Math.E);
                vertex[1] = y * smRadius;
            }

            return vertex;
        }
        public List<double> bboxcalc(double lon, double lat)
        {
            List<double> calc = new List<double>();
        https://gis.stackexchange.com/questions/2951/algorithm-for-offsetting-a-latitude-longitude-by-some-amount-of-meters

            return calc;
        }


    }
    
}

