using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharper.Models
{
    public class AppDbContext : DbContext
    {
        private const string ConnectionString = "Data Source=CSharper.db";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Batteries.Init();
            optionsBuilder.UseSqlite(ConnectionString);
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Subject> Subjects => Set<Subject>();
        public DbSet<Lesson> Lessons => Set<Lesson>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Video> Videos => Set<Video>();
        public DbSet<Article> Articles => Set<Article>();
        public DbSet<Assignment> Assignments => Set<Assignment>();


    }
}
