using apbd_task8_db_first_s28977.DTOs;

namespace apbd_task8_db_first_s28977.Services;

public interface ISubmissionService
{
    public Task<SubmissionDto> CreateSubmissionAsync(CreateSubmissionDto request);
    public Task GradeSubmissionAsync(int submissionId, GradeSubmissionDto request);
    public Task DeleteSubmissionAsync(int submissionId);
    public Task<SubmissionDto> GetSubmissionByIdAsync(int submissionId);
}