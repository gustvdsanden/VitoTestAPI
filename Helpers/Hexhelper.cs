using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using BAMCIS.GeoJSON;
using System.Threading.Tasks;
using VitoTestAPI.Models;
using Newtonsoft.Json;
using System.Collections;
using Newtonsoft.Json.Linq;

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
            for(int i = 0; i < 3; i++)
            {
                 val = Convert.ToString( bytetodec(binary[0] + binary[1]));
                monitoringWaarden.Add(val);
                binary.RemoveAt(0);
                binary.RemoveAt(0);
            }
            

            val = Convert.ToString(bytetodec(binary[0] + binary[1])-30);
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
                    longp= 1;
                    break;
                case 2:
                    latp = 1;
                    longp = -1;
                    break;
                case 3:
                    latp = -1;
                    longp= -1;
                    break;
            }
            string longe = "";
            string late = "";
            for (int n = 0; n < 6; n++)
            {
                longe += binary[0];

                binary.RemoveAt(0);
            }
          
            
            longe = Convert.ToString((Convert.ToDouble(bytetodec(longe) * longp)) / 100000);
            int pos = 1;

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

        public static List<double> longconv(double lat, double lon)
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
        public static async Task<List<string>> bboxcalc(double lon, double lat, double afmeeting)
        {
       
            HttpClient client = new HttpClient();
            List<double> calc = new List<double>();
            List<string> Rwaarden = new List<string>();
            string highDate = "";
            double highData = 0.00;
            string uri = "";
            int R = 6371007;
            string eindatum = DateTime.Now.Date.ToString("yyyy-MM-dd");
            string begindatum = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
            string url = "";
            double dn = afmeeting*4;
            double de =  afmeeting*4;
            double Llat = (de * -1) / R;
            double Llong = (dn * -1)/(R * Math.Cos(Math.PI * lat / 180));
            Llat  = (lat + Llat * 180 / Math.PI);
            Llong = (lon + Llong * 180 / Math.PI);
            double dlat = de / R;
            double dlong = dn / (R * Math.Cos(Math.PI * lat / 180));
            dlat = (lat + dlat * 180 / Math.PI);
            dlong = (lon + dlong * 180 / Math.PI);
          
            Position Pos1 = new Position(Llat, Llong);
            Position Pos12 = new Position(dlat,  Llong);
            Position Pos2 = new Position(Llat, dlong);
            Position Pos21 = new Position(Llat,Llong );
            Position[] posa = { Pos2, Pos1, Pos21, Pos12 , Pos2};
            LineString test = new LineString(posa);
            LinearRing lrna = new LinearRing(posa);
            LinearRing[] lr = { lrna};
            Polygon pl = new Polygon(lr);
          
            string Json = JsonConvert.SerializeObject(pl);
            
                calc.Add(Llat);
            calc.Add(Llong);
            calc.Add(dlat);
            calc.Add(dlong);
            calc.AddRange(longconv(Llong, Llat));
            calc.AddRange(longconv(dlong, dlat));
            uri = "https://cropsar.vito.be/api/v1.0/cropsar-analysis/?product=S2_FAPAR&start="+ begindatum +"&end="+eindatum+"&crs=epsg:4326&source=probav-mep";
            var httpcontent = new StringContent(Json, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(uri, httpcontent);
            var responseString = await postResponse.Content.ReadAsStringAsync();
            dynamic responseBody = JsonConvert.DeserializeObject(responseString);
            dynamic s2Data = responseBody["clean"]["s2-data"];
            foreach (dynamic valuePerJsonDate in responseBody["clean"]["s2-data"])
            {
                string date = valuePerJsonDate.Name;
                double data = valuePerJsonDate.First["data"];
                if (data > 0.40 && highData <= data)
                {
                    highData = data;
                    highDate = date;
                }
            }
            string url2 = "https://services.terrascope.be/wms/v2?service=WMS&version=1.3.0&request=GetMap&layers=CGS_S2_FAPAR&format=image/png&time=" + highDate + "&width=" + dn/4 + "&height=" + de/4 + "&bbox=" + calc[4].ToString().Replace(",", ".") + "," + calc[5].ToString().Replace(",", ".") + "," + calc[6].ToString().Replace(",", ".") + "," + calc[7].ToString().Replace(",", ".") + "&styles=&srs=EPSG:3857";
            List<string> returnList = new List<string>();
            returnList.Add(url2);
            returnList.Add(highDate);
            returnList.Add(highData.ToString());
            return returnList;
        }
    }
    
    
    }

