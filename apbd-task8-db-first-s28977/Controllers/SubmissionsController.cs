using apbd_task8_db_first_s28977.DTOs;
using apbd_task8_db_first_s28977.Exceptions;
using apbd_task8_db_first_s28977.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd_task8_db_first_s28977.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubmissionsController : ControllerBase
{
    private readonly ISubmissionService _submissionService;

    public SubmissionsController(ISubmissionService submissionService)
    {
        _submissionService = submissionService;
    }

    [HttpGet("{submissionId:int}")]
    public async Task<IActionResult> GetSubmissionById([FromRoute] int submissionId)
    {
        try
        {
            var submission = await _submissionService.GetSubmissionByIdAsync(submissionId);
            return Ok(submission);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateSubmission([FromBody] CreateSubmissionDto request)
    {
        try
        {
            var submission = await _submissionService.CreateSubmissionAsync(request);
            return CreatedAtAction(
                nameof(GetSubmissionById), 
                new { submissionId = submission.SubmissionId },
                submission);
        }
        catch (InvalidRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ConflictRequestException e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpPut("{submissionId:int}/grade")]
    public async Task<IActionResult> GradeSubmission([FromRoute] int submissionId,
        [FromBody] GradeSubmissionDto request)
    {
        try
        {
            await _submissionService.GradeSubmissionAsync(submissionId, request);
        }
        catch (InvalidRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }

        return Ok();
    }

    [HttpDelete("{submissionId:int}")]
    public async Task<IActionResult> DeleteSubmission([FromRoute] int submissionId)
    {
        try
        {
            await _submissionService.DeleteSubmissionAsync(submissionId);
        }
        catch (InvalidRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        return NoContent();
    }
}