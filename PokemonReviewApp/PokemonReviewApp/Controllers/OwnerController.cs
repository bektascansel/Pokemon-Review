using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerRepository _repository;
        private readonly  IMapper _mapper;
        private readonly ICountryRepository _countryRepository;


        public OwnerController(IOwnerRepository repository, IMapper mapper, ICountryRepository countryRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _countryRepository = countryRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetOwners()
        {
            var result = _mapper.Map<List<OwnerDto>>(_repository.GetOwners());
            return Ok(result);

        }


        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        public IActionResult GetOwner(int ownerId)
        {
            if (!_repository.OwnerExist(ownerId))
            {
                return NotFound();
            }

            var owner = _mapper.Map<OwnerDto>(_repository.GetOwner(ownerId));

            return Ok(owner);

        }

        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        public IActionResult GetPokemonByOwner(int ownerId)
        {
            if (!_repository.OwnerExist(ownerId))
            {
                return NotFound();
            }

            var pokemons =_mapper.Map<List<PokemonDto>>(_repository.GetPokemonByOwner(ownerId));
            
            return Ok(pokemons);
           
        
        }

        [HttpPost]
        [ProducesResponseType(204)]
        public IActionResult CreateOwner([FromQuery] int countryId,[FromBody]CreateOwnerDto createOwner)
        {

            if (createOwner == null)
                return BadRequest(ModelState);

            
            var owner=_repository.GetOwners().Where(x=>x.FirstName.Trim().ToUpper() == createOwner.FirstName.Trim().ToUpper() && x.LastName.Trim().ToUpper()== createOwner.LastName.Trim().ToUpper()).FirstOrDefault();
            if(owner is not null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            if(!ModelState.IsValid)
                return BadRequest(ModelState);


            var ownerMap = _mapper.Map<Owner>(createOwner);
            ownerMap.Country = _countryRepository.GetCountry(countryId);

            if(_repository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong while creating");
            }

            return Ok("Succesfully Created");
        }




        [HttpPut]
        [ProducesResponseType(203)]
        public IActionResult UpdateOwner(int ownerId, [FromBody] UpdateOwnerDto updateOwner)
        {
            if (updateOwner == null)
                return BadRequest(ModelState);


            if (!_repository.OwnerExist(ownerId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (_repository.GetOwners().Any(x => x.FirstName.Trim().ToUpper() == updateOwner.FirstName.Trim().ToUpper() && x.LastName.Trim().ToUpper() == updateOwner.LastName.Trim().ToUpper()))
            {

                ModelState.AddModelError("", "Owner's name  already exists");
                return StatusCode(422, ModelState);
            }

            Owner owner=_repository.GetOwner(ownerId);
            var ownerMap = _mapper.Map(updateOwner,owner);

            if (!_repository.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return BadRequest(ModelState);
            }

            return NoContent();
        }



        [HttpDelete]
        [ProducesResponseType(204)]
        public IActionResult DeleteOwner(int ownerId)
        {
            if (!_repository.OwnerExist(ownerId))
                return NotFound();

            var owner = _repository.GetOwner(ownerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_repository.DeleteOwner(owner))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");

            }

            return Ok("Succesfully deleted");
        }

    }
}
