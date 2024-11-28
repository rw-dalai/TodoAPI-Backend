using Microsoft.AspNetCore.Mvc;
using MyFirstWebApp.Dto;
using MyFirstWebApp.Infrastructure;
using MyFirstWebApp.Models;

namespace MyFirstWebApp.Controllers;

// What is a Controller?
//   A controller is a class that handles HTTP requests and responses.

// What are API Endpoints?
//   An API Endpoint is a URL that is exposed by an API.

// What is a Route?
//   A Route is a URL pattern that is used to match an incoming request to a controller action.

// TodoAPI Endpoints:
//  GET /api/todos            - Get all TodoItems
//  GET /api/todos/{id}       - Get a TodoItem by Id
//  POST /api/todos           - Create a new TodoItem
//  PUT /api/todos/{id}       - Update a TodoItem
//  DELETE /api/todos/{id}    - Delete a TodoItem

// Important Status Codes
//  200 OK                    - The request was successful
//  201 Created               - The request was successful and a new resource was created
//  204 No Content            - The request was successful and there is no content to return
//  400 Bad Request           - The request was invalid by the client
//  401 Unauthorized          - The request requires authentication
//  404 Not Found             - The requested resource was not found
//  500 Internal Server Error - An unexpected error occurred


[ApiController]
[Route("api/todos")]
public class TodoController(TodoContext db) : ControllerBase
{
    
    // GET /api/todos
    [HttpGet]
    public IActionResult GetAllTodos()
    {
        // 1. Get all TodoItems from the database
        var todos = db.TodoItems.ToList();
        
        // 2. Return 200 OK status code with the list of TodoItems
        return Ok(todos);
    }
    
    
    
    // HTTP GET /api/todos/{id}
    [HttpGet("{id:int}")]
    public IActionResult GetTodoById(int id)
    {
        // 1. Find the TodoItem by its primary key form the DBSet Database context
        var todo = db.TodoItems.Find(id);
        if (todo == null)
        {
            // 2. Return 404 Not Found status code if the TodoItem is not found
            return NotFound(new { Message = $"TodoItem with Id {id} not found." });
        }
        
        // 3. Return 200 OK status code if the TodoItem is found with the TodoItem
        return Ok(todo);
    }
    
    
    
    // HTTP POST /api/todos
    [HttpPost]
    public IActionResult CreateTodo(AddTodoTaskCommand newTodo)
    {
        // 1. Create a new TodoItem and add to the database
        var todoItem = new TodoItem(newTodo.Title, newTodo.IsDone, newTodo.DueAt, newTodo.Priority);
        
        // 2. Add the TodoItem to the DBSet Database context and save the changes to the Database
        db.TodoItems.Add(todoItem);
        db.SaveChanges();
        
        // 3. Return a 201 Created status code with the new TodoItem
        return CreatedAtAction(nameof(GetTodoById), new { id = todoItem.Id }, newTodo);
    }

    
    
    // HTTP PUT /api/todos/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateTodo(int id, AddTodoTaskCommand updatedTodo)
    {
        // 1. Find the TodoItem by its primary key form the DBSet Database context
        var todo = db.TodoItems.Find(id);
        if (todo == null)
        {
            // 2. Return 404 Not Found status code if the TodoItem is not found
            return NotFound(new { Message = $"TodoItem with Id {id} not found." });
        }
        
        // 3. Update the existing TodoItem
        todo.Title = updatedTodo.Title;
        todo.IsDone = updatedTodo.IsDone;
        todo.DueAt = updatedTodo.DueAt;
        todo.Priority = updatedTodo.Priority;
        
        // 4. Save the changes to the Database
        db.SaveChanges();

        // 5. Return 200 OK status code with the updated/existing TodoItem
        return Ok(todo);
    }

    
    
    // DELETE /api/todos/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteTodoById(int id)
    {
        // 1. Find the TodoItem by its primary key form the DBSet Database context
        var todo = db.TodoItems.Find(id);
        if (todo == null)
        {
            // 2. Return 404 Not Found status code if the TodoItem is not found
            return NotFound(new { Message = $"TodoItem with Id {id} not found." });
        }

        // 3. Remove the TodoItem from the database
        db.TodoItems.Remove(todo);
        
        // 4. Return 204 No Content status code
        return NoContent();
    }
}