using System.ComponentModel.DataAnnotations;

namespace apbd_task8_db_first_s28977.DTOs;

public class CreateSubmissionDto
{
    public int AssignmentId { get; set; }
    public int StudentId { get; set; }
    [Required]
    public string RepositoryUrl { get; set; } = null!;
}