using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;
        
        public CategoryRepository(DataContext context) {
             _context = context;
        }


        public bool CategoriesExists(int categoryId)
        {
            return _context.Categories.Any(x=>x.Id == categoryId);
        }

        public bool CreateCategory(Category category)
        {  
          

            _context.Categories.Add(category);

            return Save();

        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            var categories = _context.Categories.OrderBy(x => x.Id).ToList();
            return categories;
        }



        public Category GetCategory(int id)
        {
            Category category=_context.Categories.Where(x=>x.Id==id).FirstOrDefault();
            return category;
        }



        public ICollection<Pokemon> GetPokemonByCategory(int categoryId)
        {
            return _context.PokemonCategories.Where(e=>e.CategoryId==categoryId).Select(x=>x.Pokemon).ToList();
        }

        public bool Save()
        {
           var saved=_context.SaveChanges();
            return saved > 0 ? true : false;

        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }
    }
}
