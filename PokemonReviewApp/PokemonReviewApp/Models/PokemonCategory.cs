namespace PokemonReviewApp.Models
{

    //many to many ilişki pokemon ve category arasında başka tabloda tutmak istedik (join)
    public class PokemonCategory
    {
        public int PokemonId { get; set; }
        public int CategoryId { get; set; }
        public Pokemon Pokemon { get; set; }
        public Category Category { get; set; }

    }
}
