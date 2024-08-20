using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoAPI.Models.ValueObject;

namespace TodoAPI.Entities;

public class TodoTimeEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(1000)]
    public string TimeStamp { get; set; }

    [Required]
    public TodoValueEntity? Todo { get; set; }

    public TodoTimeEntity(
        string timeStamp
    )
    {
        TimeStamp = timeStamp;
    }
}
