using System.Collections.Generic;
using System.Linq;
using SpendCA.Interfaces;
using SpendCA.Models;

namespace SpendCA.Services
{

    public class CategoryService : ICategoryService
    {

        private readonly Context _context;

        public CategoryService(Context context)
        {
            _context = context;
        }

        public List<Category> GetAll(int userId)
        {
            return _context.Categories.Where(x => x.UserId == userId).ToList();
        }

        public Category GetItem(int id){
            return _context.Categories.Find(id);
        }

        public Category Add(Category category){

            _context.Categories.Add(category);
            _context.SaveChanges();

            return category;
        }

    }
}