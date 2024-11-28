using System.Text.Json.Serialization;

namespace MyFirstWebApp.Models;

// What is JsonConverter?
//  Its converts a JSON string to a .NET object and vice versa.

// Look at the MyFirstWebApp.http file
/*
{
     "title": "Write API Documentation", 
     "isDone": false,
     "dueAt": "2021-12-31T23:59:59",
     "priority": 1
   }
 */

// Example:
// "priority": 0 -> Priority.High
// "priority": 1 -> Priority.Medium
// "priority": 2 -> Priority.Low

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Priority
{
    High, Medium, Low
}