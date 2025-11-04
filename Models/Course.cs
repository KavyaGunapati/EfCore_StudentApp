using System.ComponentModel.DataAnnotations;
namespace StudentApp.Models;
public class Course
{
    [Key]
    public int Id { get; set; }
    [Required,StringLength(50)]
    public string Title { get; set; } = string.Empty;
    public List<Student> Students { get; set; } = new();
}