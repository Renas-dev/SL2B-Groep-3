using System;
using System.ComponentModel.DataAnnotations;

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


