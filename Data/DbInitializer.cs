using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Parc_Auto.Models;

namespace Parc_Auto.Data
{
    public class DbInitializer
    {

        public static void Initialize(ParcContext context)
        {
            context.Database.EnsureCreated();
            if (context.Cars.Any())
            {
                return; // BD a fost creata anterior
            }
            var cars = new Car[]
            {
                 new Car{Mark="Audi S3",Combustible="Diesel",Price=Decimal.Parse("33000")},
                 new Car{Mark="BMW Seria3",Combustible="Benzine",Price=Decimal.Parse("40000")},
                 new Car{Mark="Mercedes C Class",Combustible="Diesel",Price=Decimal.Parse("55000")}
            };
            foreach (Car s in cars)
            {
                context.Cars.Add(s);
            }
            context.SaveChanges();
            var customers = new Customer[]
            {

                    new Customer{CustomerID=1050,Name="Ionescu Adi",BirthDate=DateTime.Parse("1988-12-01")},
                    new Customer{CustomerID=1045,Name="Pralea Bogdan ",BirthDate=DateTime.Parse("1998-10-03")},
                    new Customer{CustomerID=1035,Name="Popescu Ioana",BirthDate=DateTime.Parse("200-06-03")},

 };
            foreach (Customer c in customers)
            {
                context.Customers.Add(c);
            }
            context.SaveChanges();
            var orders = new Order[]
            {
                new Order{CarID=1,CustomerID=1050},
                new Order{CarID=3,CustomerID=1045},
                new Order{CarID=2,CustomerID=1035},
            };
            foreach (Order e in orders)
            {
                context.Orders.Add(e);
            }
            context.SaveChanges();
        }
    }
}
