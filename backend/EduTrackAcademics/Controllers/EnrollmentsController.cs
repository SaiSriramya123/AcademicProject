using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduTrackAcademics.Data;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Exceptions;
using EduTrackAcademics.Model;
using EduTrackAcademics.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduTrackAcademics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
	public class EnrollmentController : ControllerBase
	{
		private readonly IEnrollmentService _service;

		public EnrollmentController(IEnrollmentService service)
		{
			_service = service;
		}

		//Insert into the enrollment table When student enrolled to a course
		[HttpPost]
		public async Task<IActionResult> AddEnrollment([FromBody] EnrollmentDto dto)
		{
			try
			{
				var result = await _service.AddEnrollmentAsync(dto);

				return Ok(new
				{
					status = 200,
					data =$"Inserted Into Enrollment Table with enrollment id {result}"
				});
			}
			catch (ApplicationException ex)
			{
				return StatusCode(500,
				new { status = 500, message = ex.Message });
			}
			
		}

		// Display the content of the enrolled course
		[HttpGet("content")]
		public async Task<IActionResult> ViewCourseContent([FromQuery]EnrollmentDto dto)
		{
			try
			{
				var modules = await _service
					.GetContentForStudentAsync(dto.StudentId, dto.CourseId);

				return Ok(new { status = 200, data = modules });
			}
			catch (ApplicationException ex)
			{
				return StatusCode(500,
					new { status = 500, message = ex.Message });
			}
			
		}

		//After viewing the content mark as completed by inserting the completed content into studentprogress table
		[HttpPost("mark-completed")]
		public async Task<IActionResult> MarkContentCompleted([FromBody] MarkCompletedDto dto)
		{
			var result = await _service.MarkContentCompletedAsync(dto.StudentId, dto.CourseId, dto.ContentId);

			return Ok(new
			{
				status = 200,
				message = $"{result} Content marked as completed successfully."
			});
		}

		// course progress(percentage) is calculated and displayed 
		[HttpGet("progress")]
		public async Task<IActionResult> GetCourseProgress([FromQuery]EnrollmentDto dto)
		{
			try
			{
				var progress = await _service
					.GetCourseProgressPercentageAsync(dto.StudentId, dto.CourseId);

				return Ok(new { status = 200, progress });
			}
			catch (ApplicationException ex)
			{
				return StatusCode(500,
					new { status = 500, message = ex.Message });
			}
			
		}

		//Update the status in enrollment table Active->completed
		[HttpPut("update-status")]
		public async Task<IActionResult> ProcessCourseCompletion([FromQuery] EnrollmentDto dto)
		{
			try
			{
				var result = await _service.ProcessCourseCompletionAsync(dto.StudentId, dto.CourseId);

				return Ok(new
				{
					status = 200,
					message = result
						? "Completed Successfully"
						: "Progress not 100%"
				});
			}
			catch (ApplicationException ex)
			{
				return StatusCode(500,
					new { status = 500, message = ex.Message });
			}

		}

		//Individual student attendance for the courses that student enrolled
		[HttpGet("student-attendance/{studentId}")]
		public async Task<IActionResult> GetStudentAttendance(string studentId)
		{
			var data = await _service.CalculateStudentAttendanceByStudentIdAsync(studentId);

			return Ok(new
			{
				status = 200,
				studentId,
				attendance = data
			});
		}

		// Returns the avarage attendace percentage of batches that are in particular course
		[HttpGet("course-batch-attendance/{courseId}")]
		public async Task<IActionResult> GetBatchWiseAttendance(string courseId)
		{
			try
			{
				var result = await _service.GetBatchWiseAttendanceAsync(courseId);

				if (result == null || result.Count == 0)
				{
					return NotFound(new
					{
						status = 404,
						message = "No batches found for this course"
					});
				}

				return Ok(new
				{
					status = 200,
					courseId,
					batchWiseAttendance = result
				});
			}
			catch (ApplicationException ex)
			{
				return StatusCode(500, new
				{
					status = 500,
					message = "Error fetching batch attendance",
					error = ex.Message
				});
			}
		}
	}
}

