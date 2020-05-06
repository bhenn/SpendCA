using System;
using System.Collections.Generic;
using SpendCA.Core.Entities;
using SpendCA.Core.Interfaces;

namespace SpendCA.Core.Services

{
    public class SpendService : ISpendService
    {

        private readonly ISpendRepository _spendRepository; 

        public SpendService(ISpendRepository spendRepository)
        {
            _spendRepository = spendRepository;
        }

        public Spend Add(Spend spend)
        {

            spend.Date = SetDateToTwoPM(spend.Date);
            return _spendRepository.Add(spend);
            

        }

        public Spend Delete(int id, int userId)
        {
            return _spendRepository.Delete(id, userId);
        }

        public List<Spend> GetAll(int userId, FilterModel filter)
        {
            return _spendRepository.GetAll(userId, filter);
        }

        public Spend GetItem(int id)
        {
            return _spendRepository.GetItem(id);
        }

        public void Update(Spend spendAlter, int userId)
        {
            spendAlter.Date = SetDateToTwoPM(spendAlter.Date);
            _spendRepository.Update(spendAlter, userId);
        }


        private DateTime SetDateToTwoPM(DateTime date)
        {
            if (date == DateTime.MinValue)
                date = DateTime.Now;
            
            return date.Date + new TimeSpan(14, 0, 0);
        }

    }
}
