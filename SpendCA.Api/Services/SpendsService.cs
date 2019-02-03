using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SpendCA.Interfaces;
using SpendCA.Models;

public class SpendsService : ISpendService
{

    private readonly Context _context;

    public SpendsService(Context context)
    {
        _context = context;
    }

    public Spend Add(Spend spend)
    {
        _context.Spends.Add(spend);
        _context.SaveChanges();

        return spend;
    }

    public void Update(Spend spendAlter, int userId){

        var spend = _context.Spends.Find(spendAlter.Id);

        if (spend.UserId != userId)
            throw new Exception("Not found");

        spend.CategoryId = spendAlter.CategoryId;
        spend.Description = spendAlter.Description;
        spend.Location = spendAlter.Location;
        spend.Date = spendAlter.Date;
        spend.Value = spendAlter.Value;
        
        _context.Entry(spend).State = EntityState.Modified;
        _context.SaveChanges();

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
        return _context.Spends.Find(id);
    }

    public Spend Delete(int id, int userId)
    {
        var spend = _context.Spends.Find(id);
        if (spend == null)
            throw new Exception("Spend not found");

        if(spend.UserId != userId)
            throw new Exception("This spend is not yours");

        _context.Spends.Remove(spend);
        _context.SaveChanges();

        return spend;

    }
}