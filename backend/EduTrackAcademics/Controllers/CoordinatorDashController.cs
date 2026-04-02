using EduTrackAcademics.DTO;
using EduTrackAcademics.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EduTrackAcademics.Controllers
{
	[ApiController]
	[Route("api/coordinator")]
	[EnableCors("MyCorsPolicy")]
	public class CoordinatorDashboardController : ControllerBase
	{
		private readonly ICoordinatorDashboardService _service;

		public CoordinatorDashboardController(ICoordinatorDashboardService service)
		{
			_service = service;
		}

		[Authorize(Roles = "Coordinator,Admin")]

		[HttpGet("programs")]
		public IActionResult GetPrograms()
		{
			return Ok(_service.GetPrograms());
		}
		//[Authorize(Roles = "Coordinator,Admin")]

		[HttpGet("program/{programId}/years")]
		public IActionResult GetAcademicYears(string programId)
		{
			return Ok(_service.GetAcademicYears(programId));
		}
		//[Authorize(Roles = "Coordinator")]

		[HttpPost("course")]
		public IActionResult AddCourse([FromBody] CourseDTO dto)
		{
			return Ok(_service.AddCourse(dto));
		}

		[HttpGet("GetAllCOURSES")]
		public IActionResult GetAllCourse()
		{
			return Ok(_service.GetAllCourse());
		}

		[Authorize(Roles = "Coordinator")]
		[HttpPut("course/{id}")]
		public IActionResult UpdateCourse(string id, [FromBody] CourseDTO dto)
		{
			var updatedCourse = _service.UpdateCourse(id, dto);
			return Ok(updatedCourse);
		}
		//[Authorize(Roles = "Coordinator")]

		[HttpDelete("course/{id}")]
		public IActionResult DeleteCourse(string id)
		{
			_service.DeleteCourse(id);
			return NoContent();
		}
		//[Authorize(Roles = "Coordinator")]

		[HttpGet("academic-year/{yearId}/courses")]
		public IActionResult GetCourses(string yearId)
		{
			return Ok(_service.GetCourses(yearId));
		}
		//[Authorize(Roles = "Coordinator,Admin")]

		[HttpGet("students")]
		public IActionResult GetStudents(string qualification, string program, int year)
		{
			return Ok(_service.GetStudents(qualification, program, year));
		}
		//[Authorize(Roles = "Coordinator,Admin")]
		[HttpGet("instructors")]
		public IActionResult GetInstructors(string skill)
		{
			return Ok(_service.GetInstructors(skill));
		}
		//[Authorize(Roles = "Coordinator,Admin")]
		[HttpGet("batches")]
		public IActionResult GetBatches(string program, int year)
		{
			return Ok(_service.GetBatches(program, year));
		}

		//[HttpGet("batch-count")]
		//public IActionResult GetBatchCount(string program, int year)
		//{
		//	return Ok(_service.GetBatchCount(program, year));
		//}
		//[Authorize(Roles = "Coordinator,Admin")]

		[HttpGet("batch/{batchId}/students")]
		public IActionResult GetStudentsInBatch(string batchId)
		{
			return Ok(_service.GetStudentsInBatch(batchId));
		}

		//[HttpPost("assign-batches")]
		//public IActionResult AssignBatches([FromBody] AutoAssignBatchDTO dto)
		//{
		//	return Ok(_service.AssignBatches(dto));
		//}

		[HttpPost("assign-single-batch")]
		public IActionResult AssignSingleBatch([FromBody] AutoAssignBatchDTO dto)
		{
			return Ok(_service.AssignSingleBatch(dto));
		}
		//[Authorize(Roles = "Coordinator,Admin")]

		[HttpGet("instructor/{instructorId}/batches")]
		public IActionResult GetInstructorBatches(string instructorId)
		{
			return Ok(_service.GetInstructorBatches(instructorId));
		}

		//[HttpGet("instructor/{instructorId}/dashboard")]
		//public IActionResult InstructorDashboard(string instructorId)
		//{
		//	return Ok(_service.InstructorDashboard(instructorId));
		//}
	}
}
