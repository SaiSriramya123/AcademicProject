using EduTrackAcademics.Data;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduTrackAcademics.Controllers
{
	[Route("api/instructorModuleContent")]
	[ApiController]
	public class InstructorModuleController : ControllerBase
	{
		private readonly IInstructorService _service;
		private readonly EduTrackAcademicsContext _context;
		public InstructorModuleController(EduTrackAcademicsContext context, IInstructorService service)
		{
			_service = service;
			_context = context;
		}
		// MODULE

		[HttpPost("module")]
		public async Task<IActionResult> CreateModule(ModuleDTO dto)
		{
			var result = await _service.CreateModuleAsync(dto);

			return Ok(new
			{
				message = result.message,
				moduleId = result.module.ModuleID,
				courseId = result.module.CourseId,
				name = result.module.Name,
				sequenceOrder = result.module.SequenceOrder,
				learningObjectives = result.module.LearningObjectives
			});
		}

		[HttpGet("modules/{courseId}")]
		public async Task<IActionResult> GetModules(string courseId)
		{
			var modules = await _service.GetModulesAsync(courseId);

			if (!modules.Any())
				return NotFound("No modules found for this course");

			return Ok(modules);
		}

		[HttpPut("module/{moduleId}")]
		public async Task<IActionResult> UpdateModule(string moduleId, ModuleDTO dto)
		{
			var message = await _service.UpdateModuleAsync(moduleId, dto);

			if (message == "Module not found")
				return NotFound(message);

			return Ok(new { message });
		}

		// CONTENT

		[HttpPost("content")]
		public async Task<IActionResult> Create(ContentDTO dto)
		{
			try { return Ok(await _service.CreateContentAsync(dto)); }
			catch (ApplicationException ex) { return BadRequest(ex.Message); }
		}

		[HttpGet("content/module/{moduleId}")]
		public async Task<IActionResult> GetByModule(string moduleId)
		{
			return Ok(await _service.GetContentByModuleAsync(moduleId));
		}

		[HttpGet("content/{id}")]
		public async Task<IActionResult> Get(string id)
		{
			try { return Ok(await _service.GetContentAsync(id)); }
			catch (ApplicationException ex) { return NotFound(ex.Message); }
		}

		[HttpPut("content/{id}")]
		public async Task<IActionResult> Update(string id, ContentDTO dto)
		{
			try { return Ok(await _service.UpdateContentAsync(id, dto)); }
			catch (ApplicationException ex) { return BadRequest(ex.Message); }
		}

		[HttpPut("content/publish/{id}")]
		public async Task<IActionResult> Publish(string id)
		{
			try { return Ok(await _service.PublishContentAsync(id)); }
			catch (ApplicationException ex) { return BadRequest(ex.Message); }
		}

		[HttpDelete("content/{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			try { return Ok(await _service.DeleteContentAsync(id)); }
			catch (ApplicationException ex) { return NotFound(ex.Message); }
		}
	}
}
