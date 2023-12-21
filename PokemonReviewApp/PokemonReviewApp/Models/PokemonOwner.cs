namespace PokemonReviewApp.Models
{
    //pokemon ile owner arasında many to many ilikşki ayrı tabloda tuttuk (join)
    public class PokemonOwner
    {
        public int PokemonId { get; set; }
        public int OwnerId { get; set; }
        public Pokemon Pokemon { get; set; }
        public Owner Owner { get; set; }




    }
}
