using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInCloud.Orchestrators.Shop.Contract
{
    public class Shop
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int PlacesAmount { get; set; }
    }
}
