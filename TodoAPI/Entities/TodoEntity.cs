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
    public string TimeStamp { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Title { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Author { get; set; }

    [Required]
    [MaxLength(10000)]
    public string Description { get; set; }

    [Required]
    public bool Done { get; set; }

    public TodoEntity(
        string timeStamp,
        string title,
        string author,
        string description,
        bool done
    )
    {
        TimeStamp = timeStamp;
        Title = title;
        Author = author;
        Description = description;
        Done = done;
    }
}
