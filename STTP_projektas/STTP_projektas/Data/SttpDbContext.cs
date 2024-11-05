using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using STTP_projektas.Auth.Model;
using STTP_projektas.Data.Entities;

namespace STTP_projektas.Data;

public class SttpDbContext(IConfiguration configuration) : IdentityDbContext<ForumUser>
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<Forum> Forums { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(("PostgreSQL")));
        Console.WriteLine($"Connection String: {configuration.GetConnectionString(("PostgreSQL"))}"); // Log the connection string
    }
}