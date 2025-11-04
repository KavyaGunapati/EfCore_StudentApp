using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace StudentApp.Models;
public class Student
{
    [Key]
    public int Id { get; set; }
    [Required, StringLength(50)]
    public string Name { get; set; } = string.Empty;
    [Range(10, 100)]
    public int Age { get; set; }
    
    public int CourseId { get; set; }
    [ForeignKey(nameof(CourseId))]
    public Course? Course{ get; set; }
    
}
