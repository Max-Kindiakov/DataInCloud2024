using AutoMapper;
using DataInCloud.Model.Shop;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataInCloud.Dal.Shop
{
    public class ShopRepository : IShopRepository
    {
        private readonly IMapper _mapper;
        private readonly CosmosDbContext _context;

        public ShopRepository(
            IMapper mapper,
            CosmosDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<Model.Shop.Shop>> GetAllAsync()
        {
            var entities = await _context.Shops.ToListAsync();

            return _mapper.Map<List<Model.Shop.Shop>>(entities);
        }

        public async Task<Model.Shop.Shop> GetByIdAsync(Guid id)
        {
            var entity = await _context.Shops.FirstOrDefaultAsync(c => c.Id == id);

            return _mapper.Map<Model.Shop.Shop>(entity);
        }

        public async Task<Model.Shop.Shop> CreateAsync(Model.Shop.Shop model)
        {
            var entity = _mapper.Map<ShopDao>(model);

            var trackedEntity = await _context.Shops.AddAsync(entity);

            await _context.SaveChangesAsync();

            return _mapper.Map<Model.Shop.Shop>(trackedEntity.Entity);
        }

        public async Task<Model.Shop.Shop> DeleteAsync(Guid id)
        {
            var shopToDelete = _context.Shops.AsNoTracking().FirstOrDefault(c => c.Id == id);
            if (shopToDelete != null)
            {
                _context.Shops.Remove(shopToDelete);
                await _context.SaveChangesAsync();
                return _mapper.Map<Model.Shop.Shop>(shopToDelete);
            }
            return null;
        }

        public async Task<Model.Shop.Shop> UpdateAsync(Model.Shop.Shop existingShop)
        {
            var shopEntity = await _context.Shops.FirstAsync(c => c.Id == existingShop.Id);

            shopEntity.PlacesAmount = existingShop.PlacesAmount;

            await _context.SaveChangesAsync();

            return _mapper.Map<Model.Shop.Shop>(shopEntity);
        }
    }
}
