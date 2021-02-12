using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitoTestAPI.Models;

namespace VitoTestAPI.Helpers
{
    public class ErrorHelper
    {
        
        public static String CheckForErrors(ApiContext context,string sensorid,string value )
        {
            //if something went wrong and sensorid=0
            if( sensorid == "0")
            {
                return "No sensor (id: 0)";
            }
            //if the sensor did not get added to db
            if (int.Parse(sensorid) >= 255)
            {
                return "Sensor with id: " + (int.Parse(sensorid) - 255) + " was not present in DB. Add it and make a new Config call";
            }
            Sensor sensor = context.Sensors.Find(int.Parse(sensorid));
            int intValue = int.Parse(value);
            string sensorName = sensor.Name;
            switch (sensorName)
            {
                case "B4 - DHT22 Luchttemperatuur":
                    switch (intValue)
                    {
                        case 252:
                            return "Broken Wire";
                        case 253:
                            return "Error Checksum";
                        case 254:
                            return "Time-out Error";
                        case 255:
                            return "Unknown Error";
                    }
                    return ((intValue - 30) / 2).ToString();
                case "B4 - DHT22 Luchtvochtigheid":
                    switch (intValue)
                    {
                        case 252:
                            return "Broken Wire";
                        case 253:
                            return "Error Checksum";
                        case 254:
                            return "Time-out Error";
                        case 255:
                            return "Unknown Error";
                    }
                    return value;
                case "B4 - temperatuursensor DHT11-module":
                    if (intValue ==255) {
                        return "Checksum Error";
                    } else if (intValue > 50 && intValue < 0)
                    {
                        return "Measurement not within limit";
                    }
                    return value;
                case "B4 - luchtvochtigheidssensor DHT11-module":
                    if (intValue == 255)
                    {
                        return "Checksum Error";
                    }
                    else if (intValue > 100 && intValue < 0)
                    {
                        return "Measurement not within limit";
                    }
                    return value;
                case "B4 - Capacitive Moisture Sensor Adafruit - Temperatuur":
                    if (intValue == 255)
                    {
                        return "Not found";
                    }
                    return (intValue - 30).ToString();
                case "B4 - Capacitive Moisture Sensor Adafruit - Grondvochtigheid":
                    if (intValue == 255)
                    {
                        return "Not found";
                    }
                    return value;
                case "B4 - Arduino lichtsensor lm393":
                    if (intValue > 100 || intValue < 0)
                    {
                        return "Loose wire";
                    }
                    return value;
                case "B4 - GAS Sensor":
                    if (intValue > 200 || intValue < 0)
                    {
                        return "Loose wire";
                    }
                    return value;
                case "B4 - PMS5003 particle concentration sensor PM10":
                    switch (intValue)
                    {
                        case 253:
                            return "Measurement not within reach";
                        case 254:
                            return "Could not read data";
                        case 255:
                            return "Could not find Sensor";
                    }
                    if(intValue<=100 && intValue >= 0)
                    {
                      return value;
                    }
                    return (((intValue-100)*6)+100).ToString();
                case "B4 - PMS5003 particle concentration sensor PM2.5":
                    switch (intValue)
                    {
                        case 253:
                            return "Measurement not within reach";
                        case 254:
                            return "Could not read data";
                        case 255:
                            return "Could not find Sensor";
                    }
                    if (intValue <= 100 && intValue >= 0)
                    {
                        return value;
                    }
                    return (((intValue - 100) * 4) + 100).ToString();
            }


            return value;
        }
    }
}
