using EduTrackAcademics.DTO;
using EduTrackAcademics.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EduTrackAcademics.Controllers
{
	[ApiController]
	[Route("api/admin")]
	[EnableCors("MyCorsPolicy")]


	public class AdminDashboardController : ControllerBase
	{
		private readonly IAdminService _service;

		public AdminDashboardController(IAdminService service)
		{
			_service = service;
		}

		[HttpPost("qualification")]
		public IActionResult AddQualification([FromBody] QualificationDTO dto)
		{
			var result = _service.AddQualification(dto);
			return Ok(result);
		}

		[HttpPost("program")]
		public IActionResult AddProgram([FromBody] ProgramDTO dto)
		{
			var result = _service.AddProgram(dto);
			return Ok(result);
		}

		[HttpPost("academic-year")]
		public IActionResult AddAcademicYear([FromBody] AcademicYearDTO dto)
		{
			var result = _service.AddAcademicYear(dto);
			return Ok(result);
		}
		[HttpPost("rules")]
		public IActionResult AddRule([FromBody] AcademicRuleDTO dto)
		{
			var result = _service.AddRule(dto);
			return Ok(result);
		}

		[HttpGet("rules")]
		public IActionResult GetAllRules()
		{
			return Ok(_service.GetAllRules());
		}
	}
}

