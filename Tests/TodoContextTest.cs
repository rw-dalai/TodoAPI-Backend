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
    private TodoContext CreateContext()
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
        
        // Seed for testing
        db.Seed();
        
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

    
    // TodoItem Tests ----------------------------------------------------------
    [Fact]
    public void Should_AddTodoItem_When_SavingToDatabase()
    {
        // Given
        using var db = CreateContext();
        var newTodo = new TodoItem("Test Todo", false, DateTime.Now, Priority.High);

        // When
        db.TodoItems.Add(newTodo);
        db.SaveChanges();

        // Then
        var retrievedTodo = db.TodoItems.Find(newTodo.Id);
        Assert.NotNull(retrievedTodo);
    }

    
    [Fact]
    public void Should_UpdateTodoItem_When_SavingToDatabase()
    {
        // Given
        using var context = CreateContext();
        var todo = context.TodoItems.First();
        Assert.NotNull(todo);

        // When
        todo.Title = "Updated Todo Title";
        context.SaveChanges();

        // Then
        var updatedTodo = context.TodoItems.Find(todo.Id);
        Assert.NotNull(updatedTodo);
        Assert.Equal(todo.Title, updatedTodo.Title);
    }

    
    [Fact]
    public void Should_DeleteTodoItem_When_SavingToDatabase()
    {
        // Given
        using var context = CreateContext();
        var todo = context.TodoItems.First();

        // When
        context.TodoItems.Remove(todo);
        context.SaveChanges();

        // Then
        var deletedTodo = context.TodoItems.Find(todo.Id);
        Assert.Null(deletedTodo);
    }
}