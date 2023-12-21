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
    public class ReviewController : ControllerBase
    {

        private readonly IReviewRepository _repository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository repository, IMapper mapper, IReviewerRepository reviewerRepository, IPokemonRepository pokemonRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _reviewerRepository = reviewerRepository;
            _pokemonRepository = pokemonRepository;

        }



        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReviews() {
               
            var reviews=_mapper.Map<List<ReviewDto>>(_repository.GetReviews());
            return Ok(reviews);
        }


        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReview(int reviewId)
        {
            if (!_repository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var review=_mapper.Map<ReviewDto>(_repository.GetReview(reviewId));
            return Ok(review);
        }



        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReviewsForAPokemon(int pokeId)
        {
            var reviews=_mapper.Map<List<ReviewDto>>(_repository.GetReviewsOfAPokemon(pokeId));

            return Ok(reviews);

        }

        [HttpPost]
        [ProducesResponseType(204)]
        public IActionResult CreateReview([FromQuery] int reviewerId,[FromQuery] int pokemonId,[FromBody] CreateReviewDto review)
        {
            if (review == null)
                return BadRequest(ModelState);

            var reviews=_repository.GetReviews().Where(x=>x.Title.Trim().ToUpper()==review.Title.Trim().ToUpper()).FirstOrDefault();

            if(reviews is not null)
            {
                ModelState.AddModelError("", "Review already exists");
                return StatusCode(422, ModelState);
            }

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewMap = _mapper.Map<Review>(review);
            reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokemonId);
            reviewMap.Reviewer= _reviewerRepository.GetReviewer(reviewerId);


            if (!_repository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500,ModelState);
            }

            return Ok("Successfully created");

        }





        [HttpPut]
        [ProducesResponseType(203)]
        public IActionResult UpdateReview(int reviewId, [FromBody] UpdateReviewDto updateReview)
        {
            if (updateReview == null)
                return BadRequest(ModelState);

            if (!_repository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (_repository.GetReviews().Any(x => x.Title.Trim().ToUpper() == updateReview.Title.Trim().ToUpper()))
            {

                ModelState.AddModelError("", "Title's name  already exists");
                return StatusCode(422, ModelState);
            }

            Review revieww= _repository.GetReview(reviewId);
            var reviewMap = _mapper.Map(updateReview,revieww);

            if (!_repository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return BadRequest(ModelState);
            }

            return NoContent();
        }


        [HttpDelete]
        [ProducesResponseType(204)]
        public IActionResult DeleteReview(int reviewId)
        {
            if (!_repository.ReviewExists(reviewId))
                return NotFound();

            var review = _repository.GetReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_repository.DeleteReview(review))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");

            }

            return Ok("Succesfully deleted");
        }
    }
}
