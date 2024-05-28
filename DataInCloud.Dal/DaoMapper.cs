using AutoMapper;
using DataInCloud.Dal.Car;
using DataInCloud.Dal.Shop;

namespace DataInCloud.Dal
{
    public class DaoMapper : Profile
    {
        public DaoMapper()
        {
            CreateMap<CarDao, Model.Car.Car>()
                .ReverseMap();

            CreateMap<ShopDao, Model.Shop.Shop>()
                .ReverseMap();
        }
    }
}
