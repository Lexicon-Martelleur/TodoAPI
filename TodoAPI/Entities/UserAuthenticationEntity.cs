# nullable disable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoAPI.Entities;

public class UserAuthenticationEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(1000)]
    public required string Email { get; set; }

    [Required]
    [MaxLength(1000)]
    public required string UserName { get; set; }

    [Required]
    [MaxLength(1000)]
    public required string Password { get; set; }

    // Navigation Props
    public ICollection<TodoEntity> Todos { get; set; }
}
