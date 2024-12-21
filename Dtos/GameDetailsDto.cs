using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos;

public record class GameDetailsDto(
    
    [Required] int Id,
    [Required][StringLength(50)] string Name,
    int GenreId,
    [Range(1, 100)] decimal Price,
    DateOnly ReleaseDate
 
    );

