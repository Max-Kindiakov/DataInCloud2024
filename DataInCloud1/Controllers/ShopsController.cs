using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DataInCloud.Model.Shop;
using DataInCloud.Orchestrators.Shop.Contract;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;

namespace DataInCloud.Api.Controllers
{
    [ApiController]
    [Route("/api/v1/shops")]
    public class ShopsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ShopsController> _logger;
        private readonly IShopOrchestrator _shopOrchestrator;

        public ShopsController(
            IMapper mapper,
            ILogger<ShopsController> logger,
            IShopOrchestrator shopOrchestrator)
        {
            _mapper = mapper;
            _logger = logger;
            _shopOrchestrator = shopOrchestrator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var shops = await _shopOrchestrator.GetAllAsync();

            return Ok(shops);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var shopById = await _shopOrchestrator.GetByIdAsync(id);

            return Ok(shopById);
        }

        [HttpPost]
        public async Task<IActionResult> PostSync(CreateShop shop)
        {
            var model = _mapper.Map<Model.Shop.Shop>(shop);

            var result = await _shopOrchestrator.CreateAsync(model);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, EditShop model)
        {
            var modelToUpdate = _mapper.Map<Model.Shop.Shop>(model);
            modelToUpdate.Id = id;

            var entity = await _shopOrchestrator.UpdateAsync(modelToUpdate);

            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var deletedEntity = await _shopOrchestrator.DeleteAsync(id);

            return Ok(deletedEntity);
        }
    }
}