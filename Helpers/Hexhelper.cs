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

            List<string> monitoringWaarden = new List<string>();
            for(int i = 0; i < 3; i++)
            {
                string val = Convert.ToString( bytetodec(binary[0] + binary[1]));
                monitoringWaarden.Add(val);
                binary.RemoveAt(0);
                binary.RemoveAt(0);
            }
            monitoringWaarden.AddRange( coordextract(binary));


            return monitoringWaarden;
        }
        public static List<string> coordextract(List<string> binary)
        {
            List<string> coordinaten = new List<string>();
            
            string longe = "";
            string late = "";
            int pos = 1;
            for (int n = 0; n < 6; n++)
            {
                longe += binary[0];

                binary.RemoveAt(0);
            }
            Console.WriteLine(longe);
            if (longe.Substring(0) == "1")
            {
                pos = -1;
                longe = longe.Remove(0);
            }
            longe = Convert.ToString((Convert.ToDouble(bytetodec(longe) * pos)) / 10000);
            pos = 1;

            for (int n = 0; n < 6; n++)
            {
                late += binary[0];

                binary.RemoveAt(0);
            }
            if (late.Substring(0) == "1")
            {
                pos = -1;
                longe = longe.Remove(0);
            }
            late = Convert.ToString((Convert.ToDouble(bytetodec(late) * pos)) / 10000);
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

        
    }
    
}

