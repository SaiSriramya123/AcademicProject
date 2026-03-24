using EduTrackAcademics.Services;
using Microsoft.AspNetCore.Mvc;
using EduTrackAcademics.DTO;
using System;
using System.Threading.Tasks;

namespace EduTrackAcademics.Controllers
{
		[ApiController]
		[Route("api/profile")]
		public class StudentProfileController : ControllerBase
		{
			private readonly IStudentProfileService _service;

			public StudentProfileController(IStudentProfileService service)
			{
				_service = service;
			}

			[HttpGet("personal-info/{studentId}")]
			public async Task<IActionResult> GetPersonalInfo(string studentId)
			{
				var result = await _service.GetPersonalInfoAsync(studentId);
				return Ok(result);
			}

			[HttpGet("program-details/{studentId}")]
			public async Task<IActionResult> GetProgramDetails(string studentId)
			{
				var result = await _service.GetProgramDetails(studentId);
				return Ok(result);
			}

			[HttpPut("additional-info/{studentId}")]
			public async Task<IActionResult> UpdateAdditionalInfo(string studentId, [FromBody] StudentAdditionalDetailsDTO dto)
			{
				await _service.UpdateAdditionalInfo(studentId, dto);
				return Ok(new { Message = "Additional information updated successfully." });
			}

			[HttpGet("student-details/{studentId}")]
			public async Task<IActionResult> GetStudentDetails(string studentId)
			{
				var student = await _service.GetStudentDetails(studentId);

				return Ok(new
				{
					StudentName = student.StudentName,
					StudentEmail = student.StudentEmail,
					StudentQualification = student.StudentQualification,
					StudentProgram = student.StudentProgram,
					StudentAcademicYear = student.StudentAcademicYear,
				});
			}

			[HttpGet("credit-points/{studentId}")]
			public async Task<IActionResult> GetCreditPoints(string studentId)
			{
				var credits = await _service.GetCreditPointsAsync(studentId);

				return Ok(new
				{
					StudentId = studentId,
					TotalCredits = credits
				});
			}

			[HttpGet("assignment-due")]
			public async Task<IActionResult> GetAssignmentDue(string studentId, string courseId)
			{
				var assignment = await _service.GetAssignmentDetailsForStudentAsync(studentId, courseId);

				return Ok(new
				{
					AssignmentDue = assignment.DueDate,
					AssignmentType = assignment.Type,
					CourseName = assignment.CourseName
				});
			}

			[HttpGet("audit-log/{studentId}")]
			public async Task<IActionResult> GetAuditLog(string studentId)
			{
				var log = await _service.GetAuditLogAsync(studentId);

				return Ok(new
				{
					StudentId = log.StudentId,
					TimeSpent = log.TimeSpent.ToString(),
					LoginDate = log.Date.ToShortDateString()
				});
			}
		}
}
