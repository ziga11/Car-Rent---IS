using Microsoft.AspNetCore.Identity;
using web.Models;

namespace web.Data;
using System;
using System.Linq;

public class DbInitializer {
    public static void Initialize(RentContext context) {
        context.Database.EnsureCreated();
        if (context.Cars.Any())
            return;
        
        var roles = new IdentityRole[] {
            new IdentityRole {Id = "1", Name = "Staff",  NormalizedName = "STAFF"},
            new IdentityRole {Id = "2", Name = "Member", NormalizedName = "MEMBER"},
        };
        context.Roles.AddRange(roles);
        context.SaveChanges();
        
        var users = new User[] {
            new User {Email = "admin@gmail.com", NormalizedEmail = "aaa@gmail.com", UserName = "admin@gmail.com", NormalizedUserName = "admin@gmail.com",
            PhoneNumber = "+100000150", EmailConfirmed = true, PhoneNumberConfirmed = true, SecurityStamp = Guid.NewGuid().ToString("D")},
            new User {Email = "ad2@gmail.com", NormalizedEmail = "aaa@gmail.com", UserName = "ad2@gmail.com", NormalizedUserName = "ad2@gmail.com",
            PhoneNumber = "+100042000", EmailConfirmed = true, PhoneNumberConfirmed = true, SecurityStamp = Guid.NewGuid().ToString("D")},
            new User {Email = "ad7@gmail.com", NormalizedEmail = "aaa@gmail.com", UserName = "ad7@gmail.com", NormalizedUserName = "ad7@gmail.com",
            PhoneNumber = "+100012000", EmailConfirmed = true, PhoneNumberConfirmed = true, SecurityStamp = Guid.NewGuid().ToString("D")},
            new User {Email = "ad3@gmail.com", NormalizedEmail = "aaa@gmail.com", UserName = "ad3@gmail.com", NormalizedUserName = "ad3@gmail.com",
            PhoneNumber = "+100001000", EmailConfirmed = true, PhoneNumberConfirmed = true, SecurityStamp = Guid.NewGuid().ToString("D")},
            new User {Email = "ad41@gmail.com", NormalizedEmail = "aaa@gmail.com", UserName = "ad41@gmail.com", NormalizedUserName = "ad41@gmail.com",
            PhoneNumber = "+100002000", EmailConfirmed = true, PhoneNumberConfirmed = true, SecurityStamp = Guid.NewGuid().ToString("D")},
            new User {Email = "ad12@gmail.com", NormalizedEmail = "aaa@gmail.com", UserName = "ad12@gmail.com", NormalizedUserName = "ad12@gmail.com",
            PhoneNumber = "+100004000", EmailConfirmed = true, PhoneNumberConfirmed = true, SecurityStamp = Guid.NewGuid().ToString("D")},
        };
        
        var pw = new PasswordHasher<User>();
        if (!context.Users.Any(u => u.UserName == users[0].UserName)) {
            var hashed = pw.HashPassword(users[0], "Admin123!");
            users[0].PasswordHash = hashed;
        }
        
        for (int i = 1; i < users.Length; i++) {
            var hashed = pw.HashPassword(users[i], "Abc123!");
            users[i].PasswordHash = hashed;
        }
        context.Users.AddRange(users);
        context.SaveChanges();
        
        var userRoles = new IdentityUserRole<string>[] {
            new IdentityUserRole<string>{RoleId = roles[0].Id, UserId = users[0].Id},
            new IdentityUserRole<string>{RoleId = roles[1].Id, UserId = users[0].Id},
            new IdentityUserRole<string>{RoleId = roles[1].Id, UserId = users[1].Id},
            new IdentityUserRole<string>{RoleId = roles[1].Id, UserId = users[2].Id},
            new IdentityUserRole<string>{RoleId = roles[1].Id, UserId = users[3].Id},
            new IdentityUserRole<string>{RoleId = roles[1].Id, UserId = users[4].Id},
            new IdentityUserRole<string>{RoleId = roles[1].Id, UserId = users[5].Id},
        };
        context.UserRoles.AddRange(userRoles);
        context.SaveChanges();
        
        var locations = new Location[] {
            new Location {ZipCode = 1000, City = "Ljubljana", Address = "Tržaška cesta 15"},
            new Location {ZipCode = 1000, City = "Ljubljana", Address = "Večna pot 151"},
            new Location {ZipCode = 1000, City = "Ljubljana", Address = "Dunajska cesta 21"}
        };
        
        context.Locations.AddRange(locations);
        context.SaveChanges();

        
        var cars = new Car[] {
            new Car {Model = "Corolla",   LocationID = locations[0].LocationID, Make = "Toyota",        Color = "White",      RegistrationNumber = "LJ 497AR",  DailyRate = 20, imageURL = "/images/Toyota-Corolla.png"},
            new Car {Model = "Corolla",   LocationID = locations[1].LocationID, Make = "Toyota",        Color = "Red",        RegistrationNumber = "LJ 497AR",  DailyRate = 20, imageURL = "/images/Toyota-Corolla.png"},
            new Car {Model = "Sportsvan", LocationID = locations[0].LocationID, Make = "Volkswagen",    Color = "White",      RegistrationNumber = "MB VL-11",  DailyRate = 30, imageURL = "/images/Volksvagen-Sportsvan.png"},
            new Car {Model = "Equinox",   LocationID = locations[1].LocationID, Make = "Chevrolet",     Color = "Light Gray", RegistrationNumber = "LJ 898-KP", DailyRate = 35, imageURL = "/images/Chevrolet-Equinox.png"},
            new Car {Model = "Q7",        LocationID = locations[1].LocationID, Make = "Audi",          Color = "White",      RegistrationNumber = "LJ 487-AM", DailyRate = 50, imageURL = "/images/Audi-Q7.png"},
            new Car {Model = "i4",        LocationID = locations[2].LocationID, Make = "BMW",           Color = "White",      RegistrationNumber = "KP P7-700", DailyRate = 55, imageURL = "/images/BMW-i4.png"},
            new Car {Model = "EQC",       LocationID = locations[2].LocationID, Make = "Mercedes Benz", Color = "Black",      RegistrationNumber = "LJ N6-43",  DailyRate = 40, imageURL = "/images/Mercedes-EQC.png"}
        };
        
        context.Cars.AddRange(cars);
        context.SaveChanges();
        
        var customers = new Customer[] {
            new Customer {FirstName = "Alex",  UserID = users[0].Id, LastName = "Reese",    PhoneNumber = "072415246", Address = "AAAA", City = "BBBB"},
            new Customer {FirstName = "John",  UserID = users[1].Id, LastName = "Gates",    PhoneNumber = "072521524", Address = "AAAA", City = "BBBB"},
            new Customer {FirstName = "Linda", UserID = users[2].Id, LastName = "Shaw",     PhoneNumber = "032877521", Address = "AAAA", City = "BBBB"},
            new Customer {FirstName = "Ana",   UserID = users[3].Id, LastName = "Statham",  PhoneNumber = "042801342", Address = "AAAA", City = "BBBB"},
            new Customer {FirstName = "Berta", UserID = users[4].Id, LastName = "Kolaric",  PhoneNumber = "032106431", Address = "AAAA", City = "BBBB"},
            new Customer {FirstName = "Cilka", UserID = users[5].Id, LastName = "Soštaric", PhoneNumber = "042420524", Address = "AAAA", City = "BBBB"}
        };
        context.Customers.AddRange(customers);
        context.SaveChanges();
        
        var bookings = new Booking[] {
            new Booking {CustomerID = customers[0].CustomerID, CarID = 1, startDate = new DateTime(2020, 1, 19), endDate = new DateTime(2020, 1, 20)},
            new Booking {CustomerID = customers[4].CustomerID, CarID = 2, startDate = new DateTime(2020, 6, 15), endDate = new DateTime(2020, 6, 25)},
            new Booking {CustomerID = customers[4].CustomerID, CarID = 6, startDate = new DateTime(2020, 6, 15), endDate = new DateTime(2020, 6, 29)},
            new Booking {CustomerID = customers[2].CustomerID, CarID = 4, startDate = new DateTime(2020, 9, 13), endDate = new DateTime(2020, 9, 15)},
            new Booking {CustomerID = customers[2].CustomerID, CarID = 4, startDate = new DateTime(2020, 9, 11), endDate = new DateTime(2020, 9, 20)},
            new Booking {CustomerID = customers[2].CustomerID, CarID = 4, startDate = new DateTime(2020, 4, 11), endDate = new DateTime(2020, 4, 12)}
        };

        context.Bookings.AddRange(bookings);
        context.SaveChanges();

        var payments = new Payment[] {
            new Payment {BookingID = 1, PaymentDate = new DateTime(2020, 1, 19), PayAmount = 90, Paid = true,  PaymentMethod = Models.Type.Card},
            new Payment {BookingID = 2, PaymentDate = null,                      PayAmount = 0,  Paid = false, PaymentMethod = null},
            new Payment {BookingID = 3, PaymentDate = null,                      PayAmount = 0,  Paid = false, PaymentMethod = null},
            new Payment {BookingID = 4, PaymentDate = new DateTime(2020, 6, 15), PayAmount = 55, Paid = true,  PaymentMethod = Models.Type.Cash},
            new Payment {BookingID = 5, PaymentDate = null,                      PayAmount = 0,  Paid = false, PaymentMethod = null},
            new Payment {BookingID = 6, PaymentDate = null,                      PayAmount = 0,  Paid = false, PaymentMethod = null}
        };
        
        context.Payments.AddRange(payments);
        context.SaveChanges();
    }
}