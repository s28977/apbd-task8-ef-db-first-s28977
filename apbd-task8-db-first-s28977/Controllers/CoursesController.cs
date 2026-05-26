using apbd_task8_db_first_s28977.Data;
using apbd_task8_db_first_s28977.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apbd_task8_db_first_s28977.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly UniversityTasksDbContext _dbContext;
    
    public CoursesController(UniversityTasksDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetCourses([FromQuery] bool? activeOnly)
    {
        var query = _dbContext.Courses.AsNoTracking();
        if (activeOnly is true)
        {
            query = query.Where(c => c.IsActive);
        }
        var courses = await query
            .Select( c => new CourseDto()
            {
                CourseId = c.CourseId,
                Code = c.Code,
                Name = c.Name,
                Credits = c.Credits,
                AssignmentCount = c.Assignments.Count
            }).ToListAsync();
        
        return Ok(courses);
    }

    [HttpGet("{courseId:int}/assignments")]
    public async Task<IActionResult> GetAssignments([FromRoute] int courseId, [FromQuery] bool? publishedOnly)
    {
        var courseExists = await _dbContext.Courses
            .AnyAsync(c => c.CourseId == courseId);

        if (!courseExists)
        {
            return NotFound();
        }

        var query = _dbContext.Assignments
            .AsNoTracking()
            .Where(a => a.CourseId == courseId);

        if (publishedOnly is true)
        {
            query = query.Where(a => a.IsPublished);
        }

        var assignments = await query
            .Select(a => new AssignmentDto
            {
                AssignmentId = a.AssignmentId,
                Title = a.Title,
                DueDate = a.DueDate,
                MaxPoints = a.MaxPoints,
                IsPublished = a.IsPublished,
                SubmissionCount = a.Submissions.Count
            })
            .ToListAsync();

        return Ok(assignments);
    }
}