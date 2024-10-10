using Microsoft.EntityFrameworkCore;
using STTP_projektas.Data.Entities;

namespace STTP_projektas.Data;

public class DbContext: Microsoft.EntityFrameworkCore.DbContext
{
    private readonly IConfiguration _configuration;
    public DbSet<Topic> Topics { get; set; }

    public DbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString(("PostgreSQL")));
    }
}