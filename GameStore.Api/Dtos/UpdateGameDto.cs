using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record UpdateGameDto
(
    [Required][StringLength(50, MinimumLength = 3)] string Name,
    [Required][Range(1, 50)] int GenreId,
    [Required][Range(1, 1000)] decimal Price,
    DateOnly ReleaseDate
);
