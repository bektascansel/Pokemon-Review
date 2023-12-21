using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICountryRepository _repository;

        public CountryController(IMapper mapper, ICountryRepository repository)
        {
            _mapper = mapper;
            _repository = repository;

        }


        [HttpGet]
        [ProducesResponseType(200,Type=typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(_repository.GetCountries());
       
            return Ok(countries);


        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountry(int countryId)
        {
            if (!_repository.CountryExists(countryId))
                return NotFound();

            var country= _mapper.Map<CountryDto>(_repository.GetCountry(countryId));

            return Ok(country);
        }



        [HttpGet("/owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountryOfAnOwner(int ownerId)
        {
            var country = _mapper.Map<CountryDto>(_repository.GetCountryByOwner(ownerId));
            
             return Ok(country);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        public IActionResult CreateCountry([FromBody] CreateCountryDto country)
        {
            if (country == null)
                return BadRequest(ModelState);

            var countryy=_repository.GetCountries().Where(x=>x.Name.Trim().ToUpper()==country.Name.Trim().ToUpper()).FirstOrDefault();

            if(countryy is not null)
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdMap = _mapper.Map<Country>(country);

            if (!_repository.CreateCountry(createdMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500,ModelState);
            }

            return Ok("Succesfully created");
        }



        [HttpPut]
        [ProducesResponseType(203)]
        public IActionResult UpdateCountry(int countryId, [FromBody] UpdateCountryDto updateCountry)
        {
            if (updateCountry == null)
                return BadRequest(ModelState);

            if (!_repository.CountryExists(countryId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (_repository.GetCountries().Any(x => x.Name.Trim().ToUpper() == updateCountry.Name.Trim().ToUpper()))
            {

                ModelState.AddModelError("", "Country name  already exists");
                return StatusCode(422, ModelState);
            }

            Country country=_repository.GetCountry(countryId);
            var countryMap = _mapper.Map(updateCountry, country);

            if (!_repository.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return BadRequest(ModelState);
            }

            return NoContent();
        }





        [HttpDelete]
        [ProducesResponseType(204)]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!_repository.CountryExists(countryId))
                return NotFound();

            var country  = _repository.GetCountry(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_repository.DeleteCountry(country))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");

            }

            return Ok("Succesfully deleted");
        }

    }
}
