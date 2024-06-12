using System;
using System.ComponentModel.DataAnnotations;
using Dierentuin_App.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;

public class Animal
{
    [Key] // Primary Key
    public int Id { get; set; }
    public string AnimalRace { get; set; }
    public string AnimalName { get; set; }
    public bool IsAwake { get; set; }
    public bool IsHungry { get; set; }
    public string Environment { get; set; }
}


public static class AnimalEndpoints
{
	public static void MapAnimalEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Animal").WithTags(nameof(Animal));

        group.MapGet("/", async (Dierentuin_AppContext db) =>
        {
            return await db.Animal.ToListAsync();
        })
        .WithName("GetAllAnimals")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Animal>, NotFound>> (int id, Dierentuin_AppContext db) =>
        {
            return await db.Animal.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Animal model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetAnimalById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Animal animal, Dierentuin_AppContext db) =>
        {
            var affected = await db.Animal
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, animal.Id)
                  .SetProperty(m => m.AnimalRace, animal.AnimalRace)
                  .SetProperty(m => m.AnimalName, animal.AnimalName)
                  .SetProperty(m => m.IsAwake, animal.IsAwake)
                  .SetProperty(m => m.IsHungry, animal.IsHungry)
                  .SetProperty(m => m.Environment, animal.Environment)
                  );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateAnimal")
        .WithOpenApi();

        group.MapPost("/", async (Animal animal, Dierentuin_AppContext db) =>
        {
            db.Animal.Add(animal);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Animal/{animal.Id}",animal);
        })
        .WithName("CreateAnimal")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, Dierentuin_AppContext db) =>
        {
            var affected = await db.Animal
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteAnimal")
        .WithOpenApi();
    }
}

