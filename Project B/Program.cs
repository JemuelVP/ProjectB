using Microsoft.EntityFrameworkCore;
await using var db = new DataBaseConnection();

Console.WriteLine($"DataBase path; {db.DbPath}");
Console.WriteLine("Queryinh for films");

var results =
    from film in db.Films
    where film.ID == 1
    select film;

await foreach(var film in results.AsAsyncEnumerable())
{
    Console.WriteLine($"Id: {film.ID}, Title: {film.Title}, Year: {film.Year}, Description: {film.Description}, Author: {film.Authors}, Category: {film.Categories}, Age: {film.Age}, DurationInMin: {film.DurationInMin}");
}