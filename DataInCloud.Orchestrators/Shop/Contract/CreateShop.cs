using System.ComponentModel.DataAnnotations;

namespace DataInCloud.Orchestrators.Shop.Contract
{
    public class CreateShop
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required, Range(typeof(int), "0", "100")]
        public int PlacesAmount { get; set; }
    }
}
