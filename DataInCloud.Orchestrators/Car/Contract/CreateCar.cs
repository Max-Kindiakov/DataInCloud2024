using System.ComponentModel.DataAnnotations;

namespace DataInCloud.Orchestrators.Car.Contract
{
    public class CreateCar
    {
        [MaxLength(256, ErrorMessage = "The Thing That Should Not Be")]
        public string Name { get; set; }
        [Range(1,100, ErrorMessage = "Дверей має бути від 1 до 100")]
        public int DoorsCount { get; set; }
        public bool IsBuyEnable { get; set; }
    }
}