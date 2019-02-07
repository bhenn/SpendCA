using System.Collections.Generic;
using SpendCA.Core.Entities;

namespace SpendCA.Core.Interfaces
{
    public interface ICategoryRepository
    {
        List<Category> GetAll(int userId);
        Category Add(Category category);
        Category GetItem(int id);
    }
}
