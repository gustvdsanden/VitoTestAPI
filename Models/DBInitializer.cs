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
        }
    }
}
