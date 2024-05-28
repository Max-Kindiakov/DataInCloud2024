using AutoMapper;
using DataInCloud.Model.Car;
using DataInCloud.Orchestrators.Car.Contract;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataInCloud.Api.Controllers
{
    [ApiController]
    [Route("/api/v1/cars")]
    public class CarsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICarOrchestrator _carOrchestrator;

        public CarsController(IMapper mapper,ICarOrchestrator carOrchestrator)
        {
            _mapper = mapper;
            _carOrchestrator = carOrchestrator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var entities = await _carOrchestrator.GetAllAsync();

            var cars = _mapper.Map<List<Orchestrators.Car.Contract.Car>>(entities);

            return Ok(cars);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(CreateCar model)
        {
            var entityToCreate = _mapper.Map<DataInCloud.Model.Car.Car>(model);

            var createdEntity = await _carOrchestrator.CreateAsync(entityToCreate);
            return Ok(createdEntity);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var entity = await _carOrchestrator.GetCarAsync(id);

            return Ok(_mapper.Map<Orchestrators.Car.Contract.Car>(entity));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var deletedCar = await _carOrchestrator.DeleteAsync(id);
            return Ok(deletedCar);
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PatchAsync(int id, EditCar model)
        {
            var entityToUpdate = _mapper.Map<DataInCloud.Model.Car.Car>(model);
            entityToUpdate.Id = id;

            var updatedEntity = await _carOrchestrator.EditAsync(entityToUpdate);
            return Ok(updatedEntity);
        }

    }
}