using Microsoft.EntityFrameworkCore;
using YDs_Link_Saver.Models;

namespace YDs_Link_Saver.Database;

public class ApplicationContext : DbContext
{
    public DbSet<StoredLink> Links { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=yds_link_saver.db");
    }
}