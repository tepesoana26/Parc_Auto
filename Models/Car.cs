using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parc_Auto.Models
{
    public class Car
    {
        public int CarID { get; set; }
        public string Mark { get; set; }
        public string Combustible { get; set; }
        public decimal Price { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
