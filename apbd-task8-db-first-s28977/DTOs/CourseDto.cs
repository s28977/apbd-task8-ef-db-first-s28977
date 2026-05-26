namespace apbd_task8_db_first_s28977.DTOs;

public class CourseDto
{
    public int CourseId { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int Credits { get; set; }
    public int AssignmentCount { get; set; }
}