using EduTrackAcademics.Data;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduTrackAcademics.Controllers
{
	[Route("api/instructorAttendance")]
	[ApiController]
	public class InstructorAttendanceController : ControllerBase
	{
		private readonly IInstructorService _service;
		private readonly EduTrackAcademicsContext _context;

		public InstructorAttendanceController(EduTrackAcademicsContext context, IInstructorService service)
		{
			_service = service;
			_context = context;
		}

		// ATTENDANCE

		[HttpPost("attendance")]
		public async Task<IActionResult> MarkAttendance([FromBody] AttendanceDTO dto)
		{
			var result = await _service.MarkAttendanceAsync(dto);
			return Ok(result);
		}

		[HttpGet("attendance")]
		public async Task<IActionResult> GetAllAttendance()
		{
			var result = await _service.GetAllAttendanceAsync();
			return Ok(result);
		}

		[HttpGet("attendance/date/{date}")]
		public async Task<IActionResult> GetAttendanceByDate(DateTime date)
		{
			var result = await _service.GetAttendanceByDateAsync(date);
			return Ok(result);
		}

		[HttpGet("attendance/batch/{batchId}")]
		public async Task<IActionResult> GetAttendanceByBatch(string batchId)
		{
			var result = await _service.GetAttendanceByBatchAsync(batchId);
			return Ok(result);
		}

		[HttpGet("attendance/enrollment/{enrollmentId}")]
		public async Task<IActionResult> GetAttendanceByEnrollment(string enrollmentId)
		{
			var result = await _service.GetAttendanceByEnrollmentAsync(enrollmentId);
			return Ok(result);
		}

		[HttpPut("attendance/{attendanceId}")]
		public async Task<IActionResult> UpdateAttendance(string attendanceId, [FromBody] AttendanceDTO dto)
		{
			var result = await _service.UpdateAttendanceAsync(attendanceId, dto);
			return Ok(result);
		}

		[HttpDelete("attendance/{attendanceId}")]
		public async Task<IActionResult> DeleteAttendance(string attendanceId, [FromQuery] string reason)
		{
			var result = await _service.DeleteAttendanceAsync(attendanceId, reason);
			if (result.Contains("not found"))
				return NotFound(result);

			return Ok(result);
		}
	}
}
