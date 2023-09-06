using System.ComponentModel.DataAnnotations;

namespace Entity
{
    /// <summary>
    /// Person domain model class
    /// </summary>
    public class Person
    {
        [Key]
        public Guid PersonID { get; set; }

        [StringLength(100)]
        public string? PersonName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [StringLength(10)]
        public string? Gender { get; set; }
        [StringLength(64)]
        public Guid? CountryID { get; set; }
        [StringLength(500)]
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
    }
}
