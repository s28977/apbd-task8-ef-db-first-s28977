using apbd_task8_db_first_s28977.Data;
using apbd_task8_db_first_s28977.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apbd_task8_db_first_s28977.Controllers;


[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly UniversityTasksDbContext _dbContext;

    public StudentsController(UniversityTasksDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [Route("{studentId:int}/dashboard")]
    public async Task<IActionResult> GetStudentDashboard([FromRoute] int studentId)
    {
        var studentDashboard = await _dbContext.Students
            .AsNoTracking()
            .Where(s => s.StudentId == studentId)
            .Select(s => new StudentDashboardDto()
            {
                StudentId = s.StudentId,
                IndexNumber =  s.IndexNumber,
                FullName = s.FullName,
                IsActive =  s.IsActive,
                Enrollments = s.Enrollments.Select( e => new DashboardEnrollmentDto()
                {
                    CourseId = e.CourseId,
                    CourseCode = e.Course.Code,
                    CourseName = e.Course.Name,
                    Status = e.Status,
                    EnrolledAt = e.EnrolledAt
                }).ToList(),
                Submissions = s.Submissions.Select(sb => new DashboardSubmissionDto()
                {
                    SubmissionId = sb.SubmissionId,
                    AssignmentId = sb.AssignmentId,
                    AssignmentTitle = sb.Assignment.Title,
                    Status = sb.Status,
                    Score = sb.Score
                } ).ToList()
            })
            .FirstOrDefaultAsync();

        if (studentDashboard is null)
        {
            return NotFound();
        }
        return Ok(studentDashboard);
        
    }
    
}