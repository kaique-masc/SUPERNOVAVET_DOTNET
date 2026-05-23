using ChallengePetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ChallengePetApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Pet> Pets { get; set; }
}