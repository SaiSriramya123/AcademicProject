using EduTrackAcademics.Data;
using EduTrackAcademics.Exceptions;
using EduTrackAcademics.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AcademicReportController : ControllerBase
{
    private readonly IAcademicReportService _service;
    public AcademicReportController(IAcademicReportService service)
    {
        _service = service;
    }
    [HttpGet("get-single-report")]
    public IActionResult GetAllBatchPerformanceReport()
    {
        try
        {
            var result = _service.GetAllBatchPerformanceReport();
            return Ok(result);
        }
        catch (BatchNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (NoStudentsFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
