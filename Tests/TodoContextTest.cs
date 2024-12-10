using Microsoft.EntityFrameworkCore;
using MyFirstWebApp.Infrastructure;
using MyFirstWebApp.Models;
using Xunit;
using Microsoft.Data.Sqlite;

namespace MyFirstWebApp.Tests;

// NugGet Packages
//   - Microsoft.Net.Test.Sdk
//   - Xunit

// Test Naming Convention
//  Should_[ExpectedBehavior]_When_[TestCondition]

// Examples
//   Should_ThrowException_When_AgeLessThan18
//   Should_FailToWithdrawMoney_ForInvalidAccount
//   Should_FailToAdmit_IfMandatoryFieldsAreMissing

public class TodoContextTest
{
    // Test Fixtures ------------------------------------------------------------------
    // A test fixture (also called "test context") is used to set up the system state and input data needed for test execution
    private static readonly List<TodoItem> TodoFixtures =
    [
        new("Learn React", false, DateTime.Now.AddDays(7), Priority.High),
        new("Learn C#", false, DateTime.Now.AddDays(14), Priority.Medium),
        new("Write Unit Tests", false, DateTime.Now.AddDays(21), Priority.Low)
    ];

    
    // Create a new Database Context --------------------------------------------------
    private static TodoContext CreateContext()
    {
        
        // Setup SQLite In-Memory Database
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        
        // Setup Database Options like Logging for easier debugging
        var options = new DbContextOptionsBuilder<TodoContext>()
            .UseSqlite(connection)
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine)
            .Options;
        
        // Create the DbContext
        var db = new TodoContext(options);

        // Start with empty test database
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        
        // Add Test Data to Database
        db.TodoItems.AddRange(TodoFixtures);
        db.SaveChanges();
        
        // Return the Database Context
        return db;
    }
    
    
    // Database Schema Tests ----------------------------------------------------------
    [Fact]
    public void Should_CreateDatabaseSchema()
    {
        // This test verifies that our database schema can be created.
        // If there are any issues with our entity configurations, this will fail.
        CreateContext();
    }

    
    // TodoItem Tests -----------------------------------------------------------------
    [Fact]
    public void Should_AddTodoItem_When_SavingToDatabase()
    {
        using var db = CreateContext();
        
        // Given
        var newTodo = new TodoItem("Test Todo", false, DateTime.Now, Priority.High);

        // When
        // INSERT INTO TodoItems (..) VALUES (..)
        db.TodoItems.Add(newTodo);
        db.SaveChanges();

        // Then
        // SELECT * FROM TodoItems WHERE Id = newTodo.Id
        var retrievedTodo = db.TodoItems.Find(newTodo.Id);
        Assert.NotNull(retrievedTodo);
    }

    
    [Fact]
    public void Should_UpdateTodoItem_When_SavingToDatabase()
    {
        using var db = CreateContext();
        
        // Given
        // SELECT * FROM TodoItems LIMIT 1
        var todo = db.TodoItems.First();

        // When
        todo.Title = "Updated Todo Title";
        db.SaveChanges();

        // Then
        // SELECT * FROM TodoItems WHERE Id = todo.Id
        var updatedTodo = db.TodoItems.Find(todo.Id);
        Assert.NotNull(updatedTodo);
        Assert.Equal(todo.Title, updatedTodo.Title);
    }

    
    [Fact]
    public void Should_DeleteTodoItem_When_SavingToDatabase()
    {
        using var db = CreateContext();
        
        // Given
        // SELECT * FROM TodoItems LIMIT 1
        var todo = db.TodoItems.First();

        // When
        // DELETE FROM TodoItems WHERE Id = todo.Id
        db.TodoItems.Remove(todo);
        db.SaveChanges();

        // Then
        // SELECT * FROM TodoItems WHERE Id = todo.Id
        var deletedTodo = db.TodoItems.Find(todo.Id);
        Assert.Null(deletedTodo);
    }
}