using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoAPI.Entities;

public class TodoEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(1000)]
    public required string TimeStamp { get; set; }

    [Required]
    [MaxLength(1000)]
    public required string Title { get; set; }

    [Required]
    [MaxLength(1000)]
    public required string Author { get; set; }

    [Required]
    [MaxLength(10000)]
    public required string Description { get; set; }

    [Required]
    public required bool Done { get; set; }

    [ForeignKey(nameof(UserAuthenticationEntity))]
    public required int UserAuthenticationId { get; set; }

    // Navigation Props
    public required UserAuthenticationEntity UserAuthentication { get; set; }
}
