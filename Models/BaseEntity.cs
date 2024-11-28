namespace MyFirstWebApp.Models;

// What is a Domain?
//   A domain is the subject area that the software is built to address.

// What is a Model?
//   A model is an abstraction of a real thing, focusing on the most important aspects while omitting irrelevant details.  

// What is a Domain Model?
//   A domain model is a conceptual representation of the entities, their relationships,
//   and the rules or behaviors within the problem domain of the application.

// What is an Entity?
//   An entity is a thing or concept that is relevant to the domain of the application.
//   Entities have identity, state, and behavior.

// What is Identity?
//   Identity is a unique identifier that distinguishes an entity from other entities.

// All our entities will inherit from this class.
//   This class will have an Id property that will be used as the primary key in the database.
public abstract class BaseEntity<T>() where T : struct
{
    // EF Core uses the Id property as the primary key by convention.
    public T Id { get; set; }
}