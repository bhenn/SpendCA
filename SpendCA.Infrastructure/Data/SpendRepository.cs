using System.Collections.Generic;
using SpendCA.Core.Entities;
using SpendCA.Core.Interfaces;
using SpendCA.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace SpendCA.Infrastructure.Data
{
    public class SpendRepository : ISpendRepository
    {

        private readonly Context _context;

        public SpendRepository(Context context)
        {
            _context = context;
        }

        public Spend Add(Spend spend)
        {
            _context.Spends.Add(spend);
            _context.SaveChanges();

            return spend;
        }

        public Spend Delete(int id, int userId)
        {

            var spend = _context.Spends.Find(id);
            if (spend == null)
                throw new ItemNotFoundException("Spend not found");

            if (spend.UserId != userId)
                throw new ItemNotFoundException("Spend not found");

            _context.Spends.Remove(spend);
            _context.SaveChanges();

            return spend;

        }

        public List<Spend> GetAll(int userId, FilterModel filter)
        {


            if (filter.MinDate != DateTime.MinValue)
                filter.MinDate = filter.MinDate.Subtract(filter.MinDate.TimeOfDay);

            if(filter.MaxDate != DateTime.MinValue)
                filter.MaxDate = filter.MaxDate.AddDays(1).Subtract(filter.MaxDate.TimeOfDay).AddMilliseconds(-1);


            List<Spend> list;

            IQueryable<Spend> query = _context.Spends.Include(x => x.Category).OrderByDescending(o => o.Date);
            query = query.Where(x => x.UserId == userId);

            if(filter.MinDate != DateTime.MinValue || filter.MaxDate != DateTime.MinValue)
                query = query.Where(x => x.Date >= filter.MinDate && x.Date <= filter.MaxDate);

            if (filter.SelectedCategories?.Count > 0)
                query = query.Where(x => filter.SelectedCategories.Contains(x.CategoryId));

            list = query.ToList();

            return list;
        }

        public Spend GetItem(int id)
        {
            var spend = _context.Spends.Find(id);
            if (spend == null)
                throw new ItemNotFoundException("Spend not found");

            return spend;
        }

        public void Update(Spend spendAlter, int userId)
        {
            var spend = _context.Spends.Find(spendAlter.Id);

            if (spend.UserId != userId)
                throw new ItemNotFoundException("Not found");

            spend.CategoryId = spendAlter.CategoryId;
            spend.Description = spendAlter.Description;
            spend.Location = spendAlter.Location;
            spend.Date = spendAlter.Date;
            spend.Value = spendAlter.Value;

            _context.Entry(spend).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
