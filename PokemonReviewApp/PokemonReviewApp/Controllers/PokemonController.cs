using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonRepository _repository;
        private readonly IMapper _mapper;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IReviewRepository _reviewRepository;

        public PokemonController(IPokemonRepository repository, IMapper mapper, IOwnerRepository ownerRepository, IReviewRepository reviewRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _ownerRepository = ownerRepository;
            _reviewRepository = reviewRepository;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons=_repository.GetPokemons();
            ICollection<PokemonDto> pokemonDto=_mapper.Map<ICollection<PokemonDto>>(pokemons);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemonDto);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200,Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId)
        {
           if (!_repository.PokemonExist(pokeId))
                return NotFound();
        

            var pokemon=_mapper.Map<PokemonDto>(_repository.GetPokemon(pokeId));
            /*
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            */

            return Ok(pokemon);
        }


        [HttpPost]
        [ProducesResponseType(204)]
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] CreatePokemonDto pokemon)
        {
            if(pokemon==null)
                return BadRequest(ModelState);

            if (_repository.GetPokemonsByOwner(ownerId).Any(x => x.Name.Trim().ToUpper() == pokemon.Name.Trim().ToUpper()))
            {

                ModelState.AddModelError("", "Pokemon's name  already exists");
                return StatusCode(422, ModelState);
            }



            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            

            var pokemonMap = _mapper.Map<Pokemon>(pokemon);

            if (!_repository.CreatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500,ModelState);
            }

            return Ok("Succesfully created");

        }

        [HttpPut]
        [ProducesResponseType(203)]
        public IActionResult UpdatePokemon([FromQuery] int categoryId, [FromQuery]int ownerId, [FromQuery] int pokemonId, [FromBody] UpdatePokemonDto updatePokemon)
        {
            if (updatePokemon == null)
                return BadRequest(ModelState);

            if (!_repository.PokemonExist(pokemonId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (_repository.GetPokemonsByOwner(ownerId).Any(x => x.Name.Trim().ToUpper() == updatePokemon.Name.Trim().ToUpper())) 
            {

                ModelState.AddModelError("", "Pokemon's name  already exists");
                return StatusCode(422, ModelState);
            }

            Pokemon pokemon=_repository.GetPokemon(pokemonId);
            var pokemonMap = _mapper.Map(updatePokemon,pokemon); 

            if (!_repository.UpdatePokemon(ownerId,categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return BadRequest(ModelState);
            }

            return NoContent();
        }


        [HttpDelete]
        [ProducesResponseType(204)]
        public IActionResult DeletePokemon(int pokemonId)
        {
            if (!_repository.PokemonExist(pokemonId))
                return NotFound();

            var reviews=_reviewRepository.GetReviewsOfAPokemon(pokemonId);

            var pokemon = _repository.GetPokemon(pokemonId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReviews(reviews.ToList()));
            {
                ModelState.AddModelError("", "Something went wrong while deleting reviews");
            }

            if (!_repository.DeletePokemon(pokemon))
            {
                ModelState.AddModelError("", "Something went wrong while deleting pokemon");

            }

            return Ok("Succesfully deleted");
        }
    }
}
