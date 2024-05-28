using System;
using System.ComponentModel.DataAnnotations;

namespace DataInCloud.Dal.Shop
{
    public class ShopDao
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int PlacesAmount { get; set; }
    }
}
