using System;
using System.Collections.Generic;
using SpendCA.Models;

namespace SpendCA.Interfaces
{
    public interface ISpendService
    {
        List<Spend> GetAll(int userId);
        Spend GetItem(int id);
        Spend Add(Spend spend);
        void Update(Spend spend, int userId);
        Spend Delete(int id, int userId);
    }
}