using Microsoft.AspNetCore.Mvc;
using MyFirstWebApp.Dto;
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


// [ApiController]
// [Route("api/todos")]
public class TodoController_TestingOnly : ControllerBase
{
    // In-memory database for demonstration purposes
    private static List<TodoItem> _todoItems =
    [
        new TodoItem(1, "Learn React", false, DateTime.Now, Priority.High),
        new TodoItem(2, "Learn C#", false, DateTime.Now, Priority.Medium),
        new TodoItem(3, "Write Unit Tests", false, DateTime.Now, Priority.Low)
    ];
    
    
    
    // GET /api/todos -- Get all TodoItems
    [HttpGet]
    public IActionResult GetAllTodos()
    {
        // 1. Return 200 OK status code with the list of TodoItems
        return Ok(_todoItems);
    }
    
    
    
    // HTTP GET /api/todos/{id}
    [HttpGet("{id:int}")]
    public IActionResult GetTodoById(int id)
    {
        // 1. Find the TodoItem with the specified Id
        var todo = _todoItems.FirstOrDefault(t => t.Id == id);
        
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
        // 1. Generate a new Id for the TodoItem by incrementing the maximum Id
        var newId = _todoItems.Any() ? _todoItems.Max(t => t.Id) + 1 : 1;

        // 2. Create a new TodoItem and add it to the list
        var todoItem = new TodoItem(newId, newTodo.Title, newTodo.IsDone, newTodo.DueAt, newTodo.Priority);
        _todoItems.Add(todoItem);
        
        // 3. Return a 201 Created status code with the new TodoItem
        return CreatedAtAction(nameof(GetTodoById), new { id = todoItem.Id }, newTodo);
    }

    
    
    // HTTP PUT /api/todos/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateTodo(int id, AddTodoTaskCommand updatedTodo)
    {
        // 1. Find an existing TodoItem with the specified Id
        var todo = _todoItems.FirstOrDefault(t => t.Id == id);
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

        // 4. Return 200 OK status code with the updated/existing TodoItem
        return Ok(todo);
    }

    
    
    // DELETE /api/todos/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteTodoById(int id)
    {
        // 1. Find the TodoItem with the specified Id
        var todo = _todoItems.FirstOrDefault(t => t.Id == id);
        if (todo == null)
        {
            // 2. Return 404 Not Found status code if the TodoItem is not found
            return NotFound(new { Message = $"TodoItem with Id {id} not found." });
        }

        // 3. Remove the TodoItem from the list
        _todoItems = _todoItems.Where(t => t.Id != id).ToList();
        
        // 4. Return 204 No Content status code
        return NoContent();
    }
}