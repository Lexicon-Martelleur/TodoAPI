using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Entities;

public class TodoValueEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Title { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Author { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Description { get; set; }

    [Required]
    public bool Done { get; set; }

    [ForeignKey(nameof(TodoTimeEntityId))]
    public TodoTimeEntity? TodoEntity { get; set; }
    
    public int TodoTimeEntityId { get; set; }

    public TodoValueEntity(
        string title,
        string author,
        string description,
        bool done
    )
    {
        Title = title;
        Author = author;
        Description = description;
        Done = done;
    }
}
