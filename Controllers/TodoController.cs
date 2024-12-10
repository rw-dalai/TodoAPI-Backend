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

// HTTP Request consists of 3 parts:
// 1. Request Line          // e.g. HTTP POST /api/todos
// 2. Request Headers       // e.g. Content-Type: application/json
// 3. Request Body          // e.g. JSON Data

// TodoAPI Endpoints:
//  GET /api/todos            - Get all TodoItems
//  GET /api/todos/{id}       - Get a TodoItem by Id
//  POST /api/todos           - Create a new TodoItem
//  PUT /api/todos/{id}       - Update a TodoItem
//  DELETE /api/todos/{id}    - Delete a TodoItem

// Status Codes are divided into 5 categories:
//  100-199 - Informational
//  200-299 - Success
//  300-399 - Redirection
//  400-499 - Client Error
//  500-599 - Server Error

// Important Status Codes:
//  200 OK                    - The request was successful, e.g. get all TodoItems
//  201 Created               - The request was successful, a new resource was created, e.g. create a new TodoItem
//  204 No Content            - The request was successful, there is no content to return, e.g. delete a TodoItem
//  400 Bad Request           - The request was invalid, e.g. missing required fields
//  401 Unauthorized          - The request requires authentication, e.g. wrong username or password
//  404 Not Found             - The requested resource was not found, e.g. invalid id of the resource
//  500 Internal Server Error - An unexpected error occurred, e.g. database connection failed, Server Unavailable, etc.

[ApiController]
[Route("api/todos")]
public class TodoController(TodoContext db) : ControllerBase
{
    
    // GET /api/todos
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    public IActionResult GetAllTodos()
    {
        // 1. Get all TodoItems from the database
        var todos = db.TodoItems.ToList();
        
        // 2. Return (200 OK) status code with the list of TodoItems
        return Ok(todos);
    }
    
    
    
    // HTTP GET /api/todos/{id}
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id:int}")]
    public IActionResult GetTodoById([FromRoute] int id)
    {
        // 1. Find the TodoItem by its primary key form the DBSet Database context
        var todo = db.TodoItems.Find(id);
        if (todo == null)
        {
            // 2. Return (404 Not Found) status code if the TodoItem is not found
            return NotFound(new { Message = $"TodoItem with Id {id} not found." });
        }
        
        // 3. Return (200 OK) status code if the TodoItem is found with the TodoItem
        return Ok(todo);
    }
    
    
    // HTTP POST /api/todos
    [ProducesResponseType(StatusCodes.Status201Created)]
    [HttpPost]
    public IActionResult CreateTodo([FromBody] AddTodoTaskCommand newTodo)
    {
        // 1. Create a new TodoItem
        var todoItem = new TodoItem(newTodo.Title, newTodo.IsDone, newTodo.DueAt, newTodo.Priority);
        
        // 2. Add the TodoItem and save DB changes
        db.TodoItems.Add(todoItem);
        db.SaveChanges();
        
        // 3. Return (201 Created) status code and the new TodoItem
        return CreatedAtAction(nameof(GetTodoById), new { id = todoItem.Id }, todoItem);
    }

    
    
    // HTTP PUT /api/todos/{id}
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id:int}")]
    public IActionResult UpdateTodo([FromRoute] int id, [FromBody] AddTodoTaskCommand updatedTodo)
    {
        // 1. Find the TodoItem by its primary key from the DB
        var todo = db.TodoItems.Find(id);
        if (todo == null)
        {
            // 2. Return (404 Not Found) status code if the TodoItem is not found
            return NotFound(new { Message = $"TodoItem with Id {id} not found." });
        }
        
        // 3. Update the TodoItem and save DB changes
        todo.Title = updatedTodo.Title;
        todo.IsDone = updatedTodo.IsDone;
        todo.DueAt = updatedTodo.DueAt;
        todo.Priority = updatedTodo.Priority;
        db.SaveChanges();

        // 4. Return (200 OK) status code with the updated TodoItem
        return Ok(todo);
    }

    
    
    // DELETE /api/todos/{id}
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id:int}")]
    public IActionResult DeleteTodoById([FromRoute] int id)
    {
        // 1. Find the TodoItem by its primary key form the DB
        var todo = db.TodoItems.Find(id);
        if (todo == null)
        {
            // 2. Return (404 Not Found) status code if the TodoItem is not found
            return NotFound(new { Message = $"TodoItem with Id {id} not found." });
        }

        // 3. Remove the TodoItem and save DB changes
        db.TodoItems.Remove(todo);
        db.SaveChanges();
        
        // 4. Return (204 No Content) status code
        return NoContent();
    }
}