using DataInCloud.Model.CarsInShop;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace DataInCloud.Api.CarInShop
{
    [ApiController]
    [Route("/api/v1/shops")]
    public class CarInShopController : ControllerBase
    {
        private readonly ICarsInShopOrchestrator _orchestrator;

        public CarInShopController(ICarsInShopOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        [HttpGet("{shopId}/cars")]
        public async Task<IActionResult> GetCarsByShopId(Guid shopId)
        {
            var carIds = await _orchestrator.GetCarsAsync(shopId);

            return Ok(carIds);
        }

        [HttpPost("{shopId}/cars/{carId}")]
        public async Task<IActionResult> PostAsync(Guid shopId, int carId)
        {
            var model = await _orchestrator.CreateAsync(shopId, carId);

            return Ok(model);
        }

        [HttpDelete("{shopId}/cars/{carId}")]
        [SwaggerOperation(Summary = "Delete a car from a shop")]
        public async Task<IActionResult> DeleteAsync(Guid shopId, int carId)
        {
            await _orchestrator.DeleteAsync(shopId, carId);

            return NoContent();
        }
    }
}