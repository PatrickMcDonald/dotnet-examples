using System.ComponentModel.DataAnnotations;

namespace SimpleJwt.Configuration;

public record JwtSettings
{
    public const string ConfigSection = "jwtSettings";

    [Required]
    public required string Secret { get; set; }

    [Required]
    public required string Issuer { get; init; }

    [Required]
    public required string Audience { get; init; }
}
