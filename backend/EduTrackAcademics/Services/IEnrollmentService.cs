using System;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Model;

namespace EduTrackAcademics.Services
{
	public interface IEnrollmentService
	{
		Task<string> AddEnrollmentAsync(EnrollmentDto dto);
		Task<List<ModuleWithContentDto>> GetContentForStudentAsync(string studentId, string courseId);
		Task<double> GetCourseProgressPercentageAsync(string studentId, string courseId);
		Task<bool> ProcessCourseCompletionAsync(string studentId, string courseId);
		Task<List<BatchAttendanceDto>> GetBatchWiseAttendanceAsync(string courseId);
		Task<int> MarkContentCompletedAsync(string studentId, string courseId, string contentId);
		Task<List<StudentCourseAttendanceDto>> CalculateStudentAttendanceByStudentIdAsync(string studentId);
	}
}
