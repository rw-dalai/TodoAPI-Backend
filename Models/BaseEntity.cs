namespace MyFirstWebApp.Models;

// What is a Domain?
// A domain is the subject area that the software is built to address.

// What is a Model?
// A model is an abstraction of a real thing, focusing on the most important aspects while omitting irrelevant details.  

// What is a Domain Model?
// A domain model is a representation of the entities in the domain.

// What is an Entity?
// An entity is a thing or concept that is relevant to the domain of the application.
// Entities have identity, state, and behavior.

// What is Identity?
// Identity is a unique identifier that distinguishes an entity from other entities.

public abstract class BaseEntity<T>() where T : struct
{
    // EF Core uses the Id property as the primary key by convention.
    public T Id { get; set; }
}