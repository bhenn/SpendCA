using System.Collections.Generic;
using SpendCA.Models;

public interface ICategoryService
{
    List<Category> GetAll(int userId);
    Category GetItem(int id);
    Category Add(Category category);

}