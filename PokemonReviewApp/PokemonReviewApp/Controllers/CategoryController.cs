using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        readonly private ICategoryRepository _repository;
        readonly private IMapper _mapper;


        public CategoryController(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetPokemons()
        {
            var result=_mapper.Map<List<CategoryDto>>( _repository.GetCategories());
            
            return Ok(result);
        }


        [HttpGet("{categoryId}")]
        [ProducesResponseType(200,Type=typeof(Category))]
        public IActionResult GetCategory(int categoryId)
        {
            if (!_repository.CategoriesExists(categoryId))
            {
                return NotFound();
            }

            var category=_mapper.Map<CategoryDto>(_repository.GetCategory(categoryId));

            return Ok(category);

        }

        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemonByCategoryId(int categoryId)
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_repository.GetPokemonByCategory(categoryId));
           
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(pokemons);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        public IActionResult CreateCategory([FromBody] CreateCategoryDto categoryCreate)
        {
            if (categoryCreate == null) 
                    return BadRequest(ModelState);



            var category=_repository.GetCategories().Where(o=>o.Name.Trim().ToUpper()== categoryCreate.Name.Trim().ToUpper()).FirstOrDefault();

            if(category is not null)
            {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryMap = _mapper.Map<Category>(categoryCreate);

            if (!_repository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500,ModelState);
            }

            return Ok("Succesfully Created");
        }


        [HttpPut]
        [ProducesResponseType(203)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] UpdateCategoryDto updateCategory)
        {
            if(updateCategory == null)
                return BadRequest(ModelState);


            if(!_repository.CategoriesExists(categoryId))
            {
                return NotFound("Category does not exist");
            }
            
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            if(_repository.GetCategories().Any(x=>x.Name.Trim().ToUpper() == updateCategory.Name.Trim().ToUpper()))
            {

                ModelState.AddModelError("", "Category name  already exists");
                return StatusCode(422, ModelState);
            }

            Category category=_repository.GetCategory(categoryId);

            var categoryMap = _mapper.Map(updateCategory,category);
           
            if (!_repository.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(204)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if(!_repository.CategoriesExists(categoryId))
                return NotFound();

            var category=_repository.GetCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_repository.DeleteCategory(category))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                
            }

            return Ok("Succesfully deleted") ;
        }




    }
}
