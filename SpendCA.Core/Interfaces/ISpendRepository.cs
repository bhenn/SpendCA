﻿
using System.Collections.Generic;
using SpendCA.Core.Entities;

namespace SpendCA.Core.Interfaces
{
    public interface ISpendRepository
    {

        List<Spend> GetAll(int userId);
        Spend GetItem(int id);
        Spend Add(Spend spend);
        Spend Delete(int id, int userId);
        void Update(Spend spendAlter, int userId);

    }
}
