namespace MyFirstWebApp.Models;

// What is a Domain?
//   A domain is the subject area that the software is built to address.

// TodoItem is an Entity in the Domain Model
//   It represents a task that needs to be done from the real world.


public class TodoItem : BaseEntity<int>
{
    public string Title { get; set; }
    public bool IsDone { get; set; }
    public DateTime DueAt { get; set; }
    public Priority Priority { get; set; }
    
    // Constructor for `TodoController` where the Database sets the id
    public TodoItem(string title, bool isDone, DateTime dueAt, Priority priority)
    {
        Title = title;
        IsDone = isDone;
        DueAt = dueAt;
        Priority = priority;
    }
    
    // Constructor for `TodoController_Testing` where we set the id manually
    public TodoItem(int id, string title, bool isDone, DateTime dueAt, Priority priority)
    {
        Id = id;
        Title = title;
        IsDone = isDone;
        DueAt = dueAt;
        Priority = priority;
    }
    
    // Constructor for Entity Framework .NET Core
    //   when the TodoItem is materialized from the database into memory
    protected TodoItem() { }
}
