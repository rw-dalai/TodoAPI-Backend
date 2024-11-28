using MyFirstWebApp.Models;

namespace MyFirstWebApp.Dto;

// What is a Data Transfer Object (DTO)?
//   A DTO is a class that is used to transfer data between two independent parts of a software system.

// What is a Command?
//   A Command is a DTO that represents an action that should be taken.
public record AddTodoTaskCommand
(
    string Title,
    bool IsDone, 
    DateTime DueAt, 
    Priority Priority
);