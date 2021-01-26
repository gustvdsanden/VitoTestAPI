using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitoTestAPI.Models
{
    public class DBInitializer
    {
        public static void Initialize(ApiContext context)
        {
            context.Database.EnsureCreated();

            if (context.Measurements.Any())
            {
                return;
            }
         
            context.UserTypes.AddRange(
                new UserType { UserTypeName = "admin" },
                new UserType { UserTypeName = "medewerker" },
                new UserType { UserTypeName = "user" }
                );
            context.SaveChanges();
            context.Users.AddRange(
                new User { FirstName = "admin", LastName = "adminson",Password= BCrypt.Net.BCrypt.HashPassword("test"), Email="admin@test.be",Address="wijerken 41", PostalCode = "3920", City="Lommel",UserTypeID=1},
                new User { FirstName = "medewerker", LastName = "vito", Password = BCrypt.Net.BCrypt.HashPassword("test"), Email = "user1@vito.be", Address = "wijerken 41", PostalCode = "3920", City = "Lommel", UserTypeID = 2 },
                new User { FirstName = "Gust", LastName = "van der Sanden", Password = BCrypt.Net.BCrypt.HashPassword("gust"), Email = "gustvdsanden@gmail.com", Address = "wijerken 41", PostalCode = "3920", City = "Lommel", UserTypeID = 3 }
                );
            context.SaveChanges();
            context.Boxes.AddRange(
                new Box { MacAddress = "123ABC", Name = "SensorBox 1b",Comment="De box van team 1", Active = true},
                new Box { MacAddress = "123ABC", Name = "SensorBox 2b", Comment = "De box van team 2", Active = true },
                new Box { MacAddress = "123ABC", Name = "SensorBox 3b", Comment = "De box van team 3", Active = true }
                );
            context.SaveChanges();
            context.SensorTypes.AddRange(
                new SensorType { Name = "Grondtemperatuur", Unit="C" },
                new SensorType { Name = "Luchttemperatuur", Unit = "C" },
                new SensorType { Name = "windsnelheid", Unit = "km/u" }
                );
            context.SaveChanges();
            context.Sensors.AddRange(
                new Sensor { Name="Grondtemperatuur", SensorTypeID=1},
                new Sensor { Name = "Luchttemperatuur", SensorTypeID = 2 },
                new Sensor { Name = "Coole sensor", SensorTypeID = 1 },
                new Sensor { Name = "Windsnelheid", SensorTypeID = 3 }
                );
            context.SaveChanges();
            context.SensorBoxes.AddRange(
                new SensorBox {  BoxID = 1, SensorID =1  },
                new SensorBox { BoxID = 1, SensorID = 2 },
                new SensorBox { BoxID = 1, SensorID = 3 },
                new SensorBox { BoxID = 1, SensorID = 4 },
                new SensorBox { BoxID = 2, SensorID = 1 },
                new SensorBox { BoxID = 2, SensorID = 2 }

                );
            context.SaveChanges();
            context.Measurements.AddRange(
                new Measurement { TimeStamp = DateTime.Now, SensorID = 1, BoxID = 1, Value = "34" },
                new Measurement { TimeStamp = DateTime.Now, SensorID = 2, BoxID = 1, Value = "11" },
                new Measurement { TimeStamp = DateTime.Now, SensorID = 1, BoxID = 1, Value = "12" },
                new Measurement { TimeStamp = DateTime.Now, SensorID = 3, BoxID = 1, Value = "12" }
                );
            context.SaveChanges();
        }
    }
}
