namespace apbd_task8_db_first_s28977.DTOs;

public class SubmissionDto
{
    public int SubmissionId { get; set; }
    public int StudentId { get; set; }
    public string StudentFullName { get; set; } = null!;
    public int AssignmentId { get; set; }
    public string AssignmentTitle { get; set; } = null!;
    public string RepositoryUrl { get; set; } = null!;
    public string Status { get; set; } = null!;
    public int? Score { get; set; }
    public string? Feedback { get; set; }
}