using EduTrackAcademics.Exceptions;
using EduTrackAcademics.Repository;
using Microsoft.EntityFrameworkCore;
using EduTrackAcademics.DTO;

namespace EduTrackAcademics.Services
{
	public class StudentProfileService : IStudentProfileService
	{
		private readonly IStudentProfileRepository _repo;
		public StudentProfileService(IStudentProfileRepository repo)
		{
			_repo = repo;
		}
		public async Task<StudentDTO> GetPersonalInfoAsync(string studentId)
		{
			var result = await _repo.GetPersonalInfoAsync(studentId);
			if (result == null)
				throw new StudentNotFoundException("Student not found");
			return result;
		}
		public async Task<StudentDTO> GetProgramDetails(string studentId)
		{
			var result = await _repo.GetProgramDetailsAsync(studentId);
			if (result == null)
				throw new StudentNotFoundException("Student not found");
			return result;
		}
		public async Task UpdateAdditionalInfo(string studentId, StudentAdditionalDetailsDTO dto)
		{
			var exists = await _repo.StudentExistsAsync(studentId);
			if (!exists)
				throw new StudentNotFoundException("Student not found");
			await _repo.UpdateAdditionalInfoAsync(studentId, dto);
		}
		private async Task ValidateStudent(string studentId)
		{
			var exists = await _repo.StudentExistsAsync(studentId);
			if (!exists)
				throw new StudentNotFoundException(studentId);
		}
		public async Task<StudentDTO> GetStudentDetails(string studentId)
		{
			await ValidateStudent(studentId);
			// Repository returns StudentDashboardDTO; extract inner StudentDTO
			var dashboardResult = await _repo.GetStudentDetailsAsync(studentId);
			if (dashboardResult == null || dashboardResult.StudentDetails == null)
				throw new StudentNotFoundException(studentId);
			return dashboardResult.StudentDetails;
		}
		public async Task<int> GetCreditPointsAsync(string studentId)
		{
			// Validate student existence
			if (!await _repo.StudentExistsAsync(studentId))
			{
				throw new ArgumentException($"Student with ID {studentId} does not exist.");
			}

			// Fetch credits from repository

			var totalCredits = await _repo.GetCreditPointsAsync(studentId);
			return totalCredits;
		}

		//check whether student is enrolled in course before fetching assignment details


		// to get assignment due
		public async Task<(DateTime DueDate, string Type, string CourseName)> GetAssignmentDetailsForStudentAsync(string studentId, string courseId)
		{
			// Validate enrollment
			if (!await _repo.IsStudentEnrolledInCourseAsync(studentId, courseId))
			{
				throw new ArgumentException($"Student {studentId} is not enrolled in course {courseId}.");
			} // Fetch assignment details

			var assignment = await _repo.GetAssignmentDetailsAsync(courseId);
			if (assignment == null)
			{
				throw new InvalidOperationException($"No assignment found for course {courseId}.");
			}
			return assignment.Value;
		}

		public async Task<AuditLogDTO> GetAuditLogAsync(string studentId)
		{
			var log = await _repo.GetStudentLoginHistoryAsync(studentId);
			if (log == null)
			{
				throw new ArgumentException($"No login history found for student {studentId}.");
			}
			return log;
		}

	}
}
