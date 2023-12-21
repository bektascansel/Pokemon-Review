using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {

        private readonly DataContext _context;

        public ReviewerRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _context.Reviewers.Add(reviewer);

            return Save();


        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _context.Remove(reviewer);
            return Save();

        }

        public Reviewer GetReviewer(int id)
        {
            var reviewer= _context.Reviewers.Where(o => o.Id == id)
                 .Include(e=>e.Reviews).FirstOrDefault();
            /*var reviewer = _context.Reviewers.Where(o => o.Id == id)
                 .FirstOrDefault();*/

            return reviewer;
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _context.Reviewers.OrderBy(o => o.Id).ToList();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        { 
             var reviews= _context.Reviews.Where(r=>r.Reviewer.Id== reviewerId).ToList();

              return reviews;
        }

        public bool ReviewerExists(int reviewerId)
        {
           return _context.Reviewers.Any(x=>x.Id== reviewerId);
        }

        public bool Save()
        {
            var saved=_context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _context.Update(reviewer);
            return Save();
        }
    }
}
