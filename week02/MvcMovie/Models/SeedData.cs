using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;

namespace MvcMovie.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new MvcMovieContext(
            serviceProvider.GetRequiredService<DbContextOptions<MvcMovieContext>>());

        if (context.Movie.Any())
            return;

        context.Movie.AddRange(
            new Movie
            {
                Title = "When Harry Met Sally",
                ReleaseDate = DateTime.Parse("1989-2-12"),
                Genre = "Romantic Comedy",
                Price = 7.99M,
                Rating = "R"
            },
            new Movie
            {
                Title = "Ghostbusters",
                ReleaseDate = DateTime.Parse("1984-3-13"),
                Genre = "Comedy",
                Price = 8.99M,
                Rating = "PG"
            },
            new Movie
            {
                Title = "Ghostbusters 2",
                ReleaseDate = DateTime.Parse("1986-2-23"),
                Genre = "Comedy",
                Price = 9.99M,
                Rating = "PG"
            },
            new Movie
            {
                Title = "Rio Bravo",
                ReleaseDate = DateTime.Parse("1959-4-15"),
                Genre = "Western",
                Price = 3.99M,
                Rating = "NR"
            },
            new Movie
            {
                Title = "Napoleon Dynamite",
                ReleaseDate = DateTime.Parse("2004-6-11"),
                Genre = "Comedy",
                Price = 7.99M,
                Rating = "PG"
            },
            new Movie
            {
                Title = "Inception",
                ReleaseDate = DateTime.Parse("2010-7-16"),
                Genre = "Scifi",
                Price = 12.99M,
                Rating = "PG13"
            },
            new Movie
            {
                Title = "The Dark Knight",
                ReleaseDate = DateTime.Parse("2008-7-18"),
                Genre = "Action",
                Price = 11.99M,
                Rating = "PG13"
            }
        );

        context.SaveChanges();
    }
}
