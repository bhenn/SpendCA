using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SpendCA.Models;

public class Context : IdentityDbContext<User, IdentityRole<int>, int>
{
    public Context(DbContextOptions<Context> options) : base(options)
    {

    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Spend> Spends { get; set; }
}