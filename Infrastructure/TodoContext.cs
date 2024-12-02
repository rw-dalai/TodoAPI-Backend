using Microsoft.EntityFrameworkCore;
using MyFirstWebApp.Models;

namespace MyFirstWebApp.Infrastructure;

// Nuget Packages
//   - Microsoft.EntityFrameworkCore
//   - Microsoft.EntityFrameworkCore.Sqlite

// What is a DbContext?
//  A DbContext is a class that represents a session with the database and allows us to query and save instances of entities.
public class TodoContext(DbContextOptions<TodoContext> options) : DbContext(options)
{
     // What is a DBSet?
     //  A DbSet represents a table in the database.
     public DbSet<TodoItem> TodoItems { get; set; }

     // What is OnModelCreating?
     //  OnModelCreating is a method that is called when the model for a context has been initialized
     
     // Why is it used?
     //  We can configure the Database Model e.g. Primary Keys, Relationships, etc.
     protected override void OnModelCreating(ModelBuilder modelBuilder)
     {
          // modelBuilder.Entity<TodoItem>().HasKey(t => t.Id);
          // modelBuilder.Entity<TodoItem>().Property(t => t.Title).IsRequired();
     }

     // What is Seeding a Database?
     //  Seeding a database is the process of populating a database for testing or development purposes.
     public void Seed()
     {
          var seedTodos = new[]
          {
               new TodoItem(1, "Learn React", false, DateTime.Now.AddDays(7), Priority.High),
               new TodoItem(2, "Learn C#", false, DateTime.Now.AddDays(14), Priority.Medium),
               new TodoItem(3, "Write Unit Tests", false, DateTime.Now.AddDays(21), Priority.Low)
          };

          TodoItems.AddRange(seedTodos);
          SaveChanges();
     }
}

