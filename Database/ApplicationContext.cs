﻿using Microsoft.EntityFrameworkCore;
using NLog;
using YDs_Link_Saver.Models;

namespace YDs_Link_Saver.Database;

public class ApplicationContext : DbContext
{
    public DbSet<StoredGroup> Groups { get; set; }
    public DbSet<StoredLink> Links { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=yds_link_saver.db");
    }
}