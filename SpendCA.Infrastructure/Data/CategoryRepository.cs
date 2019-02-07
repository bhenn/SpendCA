using System.Collections.Generic;
using System.Linq;
using SpendCA.Core.Entities;
using SpendCA.Core.Interfaces;
using SpendCA.Core.Exceptions;

namespace SpendCA.Infrastructure.Data
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly Context _context; 

        public CategoryRepository(Context context)
        {
            _context = context; 
        }


        public Category Add(Category category)
        {

            _context.Categories.Add(category);
            _context.SaveChanges();

            return category;
        }

        public List<Category> GetAll(int userId)
        {
            return _context.Categories.Where(x => x.UserId == userId).ToList();
        }


        public Category GetItem(int id)
        {
            var category = _context.Categories.Find(id);

            if (category == null)
                throw new ItemNotFoundException($"Category not found");

            return category;
        }
    }
}
