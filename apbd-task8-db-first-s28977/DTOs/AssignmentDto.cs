namespace apbd_task8_db_first_s28977.DTOs;

public class AssignmentDto
{
    public int AssignmentId { get; set; }
    public string Title { get; set; } = null!;
    public DateTime DueDate { get; set; }
    public int MaxPoints { get; set; }
    public bool IsPublished { get; set; }
    public int SubmissionCount { get; set; }
}