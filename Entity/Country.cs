using System.ComponentModel.DataAnnotations;

namespace Entity;

/// <summary>
/// Domain Model for Country
/// </summary>
public class Country
{
    [Key]
    public Guid CountryID { get; set; }

    public string? CountryName { get; set; }
}

