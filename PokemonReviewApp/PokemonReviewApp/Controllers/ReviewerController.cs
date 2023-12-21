using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerRepository _repository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        public IActionResult GetReviewers()
        {
            var reviewers = _mapper.Map < List < ReviewerDto >> (_repository.GetReviewers());
            return Ok(reviewers);
        }



        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        public IActionResult GetReviewer(int reviewerId)
        {
            if (!_repository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }

            var reviewer = _mapper.Map<ReviewerDto>(_repository.GetReviewer(reviewerId));

            return Ok(reviewer);
        }


        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        public IActionResult GetReviewsByAReviewer(int reviewerId)
        {
            if (!_repository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }

            var reviews=_mapper.Map<List<ReviewDto>>(_repository.GetReviewsByReviewer(reviewerId)) ;
            return Ok(reviews);

        }


        [HttpPost]
        [ProducesResponseType(204)]
        public IActionResult CreateReviewer([FromBody] CreateReviewerDto reviewerCreate)
        {

            if (reviewerCreate == null)
                return BadRequest(ModelState);


            var reviewer = _repository.GetReviewers().Where(x => x.FirstName.Trim().ToUpper() == reviewerCreate.FirstName.Trim().ToUpper() && x.LastName.Trim().ToUpper() == reviewerCreate.LastName.Trim().ToUpper()).FirstOrDefault();
            if (reviewer is not null)
            {
                ModelState.AddModelError("", "Reviewer already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var reviewerMap = _mapper.Map<Reviewer>(reviewerCreate);
           

            if (_repository.CreateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something went wrong while creating");
            }

            return Ok("Succesfully Created");
        }


        [HttpPut]
        [ProducesResponseType(203)]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] UpdateReviewerDto updateReviewer)
        {
            if (updateReviewer == null)
                return BadRequest(ModelState);

       

            if (!_repository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (_repository.GetReviewers().Any(x => x.FirstName.Trim().ToUpper() == updateReviewer.FirstName.Trim().ToUpper()&&x.LastName.Trim().ToUpper() == updateReviewer.LastName.Trim().ToUpper()))
            {

                ModelState.AddModelError("", "Reviewer's name  already exists");
                return StatusCode(422, ModelState);
            }

            Reviewer reviewerr=_repository.GetReviewer(reviewerId);

            var reviewerMap = _mapper.Map(updateReviewer,reviewerr);

            if (!_repository.UpdateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return BadRequest(ModelState);
            }

            return NoContent();
        }


        [HttpDelete]
        [ProducesResponseType(204)]
        public IActionResult DeleteReviewer(int reviewerId)
        {
            if (!_repository.ReviewerExists(reviewerId))
                return NotFound();

            var reviewer = _repository.GetReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_repository.DeleteReviewer(reviewer))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");

            }

            return Ok("Succesfully deleted");
        }



    }
}
