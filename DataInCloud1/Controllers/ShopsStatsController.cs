using DataInCloud.Model.ShopStats;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace DataInCloud.Api.Controllers
{
    [ApiController]
    [Route("api/v1/stats")]
    public class ShopsStatsController : ControllerBase
    {
        private readonly IShopStatsOrchestrator _statsProvider;
        private readonly ILogger<ShopsStatsController> _logger;

        public ShopsStatsController(
            IShopStatsOrchestrator statsProvider,
            ILogger<ShopsStatsController> logger)
        {
            _statsProvider = statsProvider;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get newly added Shops",
            Description = "Return newly added Shops",
            OperationId = "GetNewShopIds"
        )]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(400, "Bad request")]
        public async Task<IActionResult> GetNewShopIdsAsync()
        {
            try
            {
                var dataResult = await _statsProvider.GetStatsAsync();
                return Ok(dataResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving new Shop IDs.");
                return BadRequest("An error occurred while retrieving new Shop IDs.");
            }
        }
    }
}