using EduTrackAcademics.DTO;
using EduTrackAcademics.Services;
using Microsoft.AspNetCore.Mvc;

namespace EduTrackAcademics.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RegistrationController : ControllerBase
	{
		private readonly IRegistrationService _registrationService;

		public RegistrationController(IRegistrationService registrationService)
		{
			_registrationService = registrationService;
		}
			// Student Registration
			[HttpPost("student")]
			public async Task<IActionResult> RegisterStudent([FromBody] StudentDTO dto)
			{
				if (!ModelState.IsValid)
					return BadRequest(ModelState);

			await _registrationService.RegisterStudentAsync(dto);
				return Ok(new {message = "Student registered successfully" });
			}

			//  Instructor Registration
			[HttpPost("instructor")]
			public async Task<IActionResult> RegisterInstructor([FromForm] InstructorDTO dto)
			{
			if (!ModelState.IsValid)
					return BadRequest(ModelState);

				await _registrationService.RegisterInstructorAsync(dto);
				return Ok(new { message = "Instructor registered successfully" });
			}

			//  Coordinator Registration
			[HttpPost("coordinator")]
			public async Task<IActionResult> RegisterCoordinator([FromForm] CoordinatorDTO dto)
			{
				if (!ModelState.IsValid)
					return BadRequest(ModelState);

				await _registrationService.RegisterCoordinatorAsync(dto);
				return Ok(new { message = "Coordinator registered successfully" });
			}
		}
}

