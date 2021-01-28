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

            if (context.Users.Any())
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
                new User { FirstName = "admin", LastName = "admin",Password= BCrypt.Net.BCrypt.HashPassword("admin"), Email="admin@test.be",Address="Testlaan", PostalCode = "3920", City="Lommel",UserTypeID=1},
                new User { FirstName = "Jurgen", LastName = "Everaerts", Password = BCrypt.Net.BCrypt.HashPassword("jurgen"), Email = "jurgen.Everaerts@vito.be", Address = "Hakkeltjesweg 65", PostalCode = "2400", City = "Mol", UserTypeID = 1 },
                new User { FirstName = "Hans", LastName = "van Echelpoel", Password = BCrypt.Net.BCrypt.HashPassword("hans"), Email = "hans.echelpoel@vito.be", Address = "Schaaikesweg 81", PostalCode = "2400", City = "Mol", UserTypeID = 2 },
                new User { FirstName = "Frans", LastName = "Bosmans", Password = BCrypt.Net.BCrypt.HashPassword("frans"), Email = "frans.b@vito.be", Address = "VanderWurmDreef 89", PostalCode = "2400", City = "Mol", UserTypeID = 2 },
                new User { FirstName = "Eline", LastName = "vande Meyer", Password = BCrypt.Net.BCrypt.HashPassword("eline"), Email = "eline.m@vito.be", Address = "Linkerarmstraat 11", PostalCode = "2400", City = "Mol", UserTypeID = 2 },
                new User { FirstName = "Gerrit", LastName = "Schampers", Password = BCrypt.Net.BCrypt.HashPassword("gerrit"), Email = "Gerrit.S@vito.be", Address = "Linkerarmstraat 11", PostalCode = "2400", City = "Mol", UserTypeID = 2 },
                new User { FirstName = "Aniek", LastName = "Meyendonck", Password = BCrypt.Net.BCrypt.HashPassword("aniek"), Email = "Aniek.M@vito.be", Address = "Linkerarmstraat 11", PostalCode = "2400", City = "Mol", UserTypeID = 2 },
                new User { FirstName = "Joost", LastName = "Plugge", Password = BCrypt.Net.BCrypt.HashPassword("joost"), Email = "Joost.P@gmail.com", Address = "Linkerarmstraat 11", PostalCode = "2400", City = "Mol", UserTypeID = 3 },
                new User { FirstName = "Edgidio", LastName = "Valentino", Password = BCrypt.Net.BCrypt.HashPassword("edgidio"), Email = "Edgidio.V@gmail.com", Address = "Linkerarmstraat 11", PostalCode = "2400", City = "Mol", UserTypeID = 3 },
                new User { FirstName = "Tijs", LastName = "Bakermans", Password = BCrypt.Net.BCrypt.HashPassword("tijs"), Email = "Tijs.B@gnail.be", Address = "Koning Albertlaan 11", PostalCode = "3920", City = "Lommel", UserTypeID = 3 },
                new User { FirstName = "Elodie", LastName = "Oedrogo", Password = BCrypt.Net.BCrypt.HashPassword("elodie"), Email = "eline.m@gmail.com", Address = "Linkerarmstraat 11", PostalCode = "2400", City = "Mol", UserTypeID = 3 },
                new User { FirstName = "Jonas", LastName = "Faasen", Password = BCrypt.Net.BCrypt.HashPassword("jonas"), Email = "Jonas.F@gmail.com", Address = "Linkerarmstraat 11", PostalCode = "2400", City = "Mol", UserTypeID = 3 },
                new User { FirstName = "Fabienne", LastName = "Poos", Password = BCrypt.Net.BCrypt.HashPassword("fabienne"), Email = "Fabienne.P@gmail.com", Address = "Linkerarmstraat 11", PostalCode = "2400", City = "Mol", UserTypeID = 3 },
                new User { FirstName = "Nikita", LastName = "Vondels", Password = BCrypt.Net.BCrypt.HashPassword("nikita"), Email = "Nikita.V@gmail.com", Address = "Linkerarmstraat 11", PostalCode = "2400", City = "Mol", UserTypeID = 3 },
                new User { FirstName = "Gust", LastName = "van der Sanden", Password = BCrypt.Net.BCrypt.HashPassword("gust"), Email = "gustvdsanden@gmail.com", Address = "wijerken 41", PostalCode = "3920", City = "Lommel", UserTypeID = 3 }
                );
            context.SaveChanges();
            context.Boxes.AddRange(
                new Box { MacAddress = "1D882D", Name = "B4 Prototype Box",Comment="De box van team B4", Active = true},
                new Box { MacAddress = "00:0a:95:9d:68:16", Name = "4G Test box", Comment = "De box van team 2", Active = true },
                new Box { MacAddress = "box3", Name = "SensorBox 2b", Comment = "De box van team 2", Active = true },
                new Box { MacAddress = "123ABCD", Name = "SensorBox 2b", Comment = "De box van team 2", Active = true },
                new Box { MacAddress = "123ABCD", Name = "SensorBox 2b", Comment = "De box van team 2", Active = false },
                new Box { MacAddress = "123ABCD", Name = "SensorBox 2b", Comment = "De box van team 2", Active = false },
                new Box { MacAddress = "123ABCD", Name = "SensorBox 2b", Comment = "De box van team 2", Active = true },
                new Box { MacAddress = "123ABCD", Name = "SensorBox 2b", Comment = "De box van team 2", Active = true },
                new Box { MacAddress = "123ABCD", Name = "SensorBox 2b", Comment = "De box van team 2", Active = true },
                new Box { MacAddress = "123ABCDE", Name = "SensorBox 3b", Comment = "De box van team 3", Active = true }
                );
            context.SaveChanges();
            context.BoxUsers.AddRange(
                new BoxUser { StartDate = DateTime.Now, BoxID = 1, UserID = 12 },
                new BoxUser { StartDate = DateTime.Now, BoxID = 1, UserID = 12 },
                new BoxUser { StartDate = DateTime.Now, BoxID = 1, UserID = 12 },
                new BoxUser { StartDate = DateTime.Now, BoxID = 1, UserID = 12 },
                new BoxUser { StartDate = DateTime.Now, BoxID = 1, UserID = 12 },
                new BoxUser { StartDate = DateTime.Now, BoxID = 1, UserID = 12 },
                new BoxUser { StartDate = DateTime.Now, BoxID = 1, UserID = 12 },
                new BoxUser { StartDate = DateTime.Now, BoxID = 1, UserID = 12 },
                new BoxUser { StartDate = DateTime.Now, BoxID = 1, UserID = 12 },
                new BoxUser { StartDate = DateTime.Now, BoxID = 1, UserID = 12 },
                new BoxUser { StartDate = DateTime.Now, BoxID = 1, UserID = 12 }
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
