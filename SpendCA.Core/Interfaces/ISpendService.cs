using System.Collections.Generic;
using SpendCA.Core.Entities;

namespace SpendCA.Core.Interfaces
{
    public interface ISpendService
    {

        List<Spend> GetAll(int userId, FilterModel filter);
        Spend GetItem(int id);
        Spend Add(Spend spend);
        Spend Delete(int id, int userId);
        void Update(Spend spendAlter, int userId);

    }
}
