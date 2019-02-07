using System.Collections.Generic;
using SpendCA.Core.Entities;
using SpendCA.Core.Interfaces;
using SpendCA.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public List<Spend> GetAll(int userId)
        {

            return _context
                .Spends
                .Include(x => x.Category)
                .Where(x => x.UserId == userId)
                .OrderByDescending(o => o.Date)
                .ToList();

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
