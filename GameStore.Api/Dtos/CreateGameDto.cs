using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record CreateGameDto
(
    [Required][StringLength(50, MinimumLength = 3)] string Name,
    [Required][StringLength(20, MinimumLength = 3)] string Genre,
    [Required][Range(1, 1000)] decimal Price,
    DateOnly ReleaseDate
);
