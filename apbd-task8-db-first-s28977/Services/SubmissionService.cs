using apbd_task8_db_first_s28977.Data;
using apbd_task8_db_first_s28977.DTOs;
using apbd_task8_db_first_s28977.Exceptions;
using apbd_task8_db_first_s28977.Models;
using Microsoft.EntityFrameworkCore;

namespace apbd_task8_db_first_s28977.Services;

public class SubmissionService : ISubmissionService
{
    private readonly UniversityTasksDbContext _dbContext;

    public SubmissionService(UniversityTasksDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<SubmissionDto> GetSubmissionByIdAsync(int submissionId)
    {
        var submission = await _dbContext.Submissions
            .AsNoTracking()
            .Where(s => s.SubmissionId == submissionId)
            .Select(s => new SubmissionDto
            {
                SubmissionId = s.SubmissionId,
                StudentId = s.StudentId,
                StudentFullName = s.Student.FullName,
                AssignmentId = s.AssignmentId,
                AssignmentTitle = s.Assignment.Title,
                RepositoryUrl = s.RepositoryUrl,
                Status = s.Status,
                Score = s.Score,
                Feedback = s.Feedback
            })
            .FirstOrDefaultAsync();

        if (submission is null)
        {
            throw new NotFoundException("Submission not found");
        }

        return submission;
    }
    
    public async Task<SubmissionDto> CreateSubmissionAsync(CreateSubmissionDto request)
    {
        if (string.IsNullOrWhiteSpace(request.RepositoryUrl)
            || !request.RepositoryUrl.StartsWith("https://"))
        {
            throw new InvalidRequestException("Invalid repository url");
        }
        var student = await _dbContext.Students.AsNoTracking().FirstOrDefaultAsync(s => s.StudentId == request.StudentId);
        if (student is null)
        {
            throw new NotFoundException("Student not found");
        }

        if (!student.IsActive)
        {
            throw new InvalidRequestException("Student is not active");
        }
        
        var assignment = await _dbContext.Assignments.AsNoTracking().FirstOrDefaultAsync(a => a.AssignmentId == request.AssignmentId);
        if (assignment is null)
        {
            throw new NotFoundException("Assignment not found");
        }
        
        if(!assignment.IsPublished)
        {
            throw new InvalidRequestException("Assignment is not published");
        }
        
        var enrollment = await _dbContext.Enrollments.AsNoTracking()
            .Where(e => e.StudentId == request.StudentId)
            .Where(e => e.Course.Assignments.Any(a => a.AssignmentId == request.AssignmentId))
            .FirstOrDefaultAsync();

        if (enrollment is null)
        {
            throw new InvalidRequestException("Student is not enrolled in the course that owns this assignment");
        }

        if (enrollment.Status is not ("Active" or "Completed"))
        {
            throw new InvalidRequestException("Enrollment is neither active nor completed");
        }

        if (await _dbContext.Submissions.AsNoTracking()
                .AnyAsync(s => s.StudentId == request.StudentId && s.AssignmentId == request.AssignmentId))
        {
            throw new ConflictRequestException("Submission for this student already exists");
        }

        var submission = new Submission()
        {
            AssignmentId = request.AssignmentId,
            StudentId = request.StudentId,
            RepositoryUrl = request.RepositoryUrl,
            SubmittedAt = DateTime.UtcNow,
            Status = assignment.IsOverdue(DateTime.UtcNow) ? "Late" : "Submitted"
        };
        
        _dbContext.Submissions.Add(submission);
        await _dbContext.SaveChangesAsync();
        
        return new SubmissionDto
        {
            SubmissionId = submission.SubmissionId,
            StudentId = submission.StudentId,
            StudentFullName = student.FullName,
            AssignmentId = submission.AssignmentId,
            AssignmentTitle = assignment.Title,
            RepositoryUrl = submission.RepositoryUrl,
            Status = submission.Status,
            Score = submission.Score,
            Feedback = submission.Feedback
        };
    }

    public async Task GradeSubmissionAsync(int submissionId, GradeSubmissionDto request)
    {
        var submission = await _dbContext.Submissions
            .Include(submission => submission.Assignment)
            .FirstOrDefaultAsync(s => s.SubmissionId == submissionId);
        if (submission is null)
        {
            throw new NotFoundException("Submission not found");
        }

        if (request.Score < 0 || request.Score > submission.Assignment.MaxPoints)
        {
            throw new InvalidRequestException("Score out of range");
        }
        
        submission.Score = request.Score;
        submission.Feedback = request.Feedback;
        submission.Status = "Graded";

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteSubmissionAsync(int submissionId)
    {
        var submission = await _dbContext.Submissions.FirstOrDefaultAsync(s => s.SubmissionId == submissionId);
        if (submission is null)
        {
            throw new NotFoundException("Submission not found");
        }

        if (submission.Score.HasValue)
        {
            throw new InvalidRequestException("You cannot delete scored submission");
        }
        
        _dbContext.Submissions.Remove(submission);
        await _dbContext.SaveChangesAsync();
    }
}