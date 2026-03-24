using EduTrackAcademics.Data;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Exceptions;
using EduTrackAcademics.Model;
using EduTrackAcademics.Repository;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace EduTrackAcademics.Services
{
	public class EnrollmentService : IEnrollmentService
	{
		private readonly IEnrollmentRepository _repo;
		private readonly EduTrackAcademicsContext _context;

		public EnrollmentService(IEnrollmentRepository repo, EduTrackAcademicsContext context)
		{
			_repo = repo;
			_context = context;
		}

		public async Task<string> AddEnrollmentAsync(EnrollmentDto dto)
		{
			// Validate course (course already mapped to year)
			var course = await _context.Course.FindAsync(dto.CourseId);
			if (course == null)
				throw new ApplicationException("Course not found");

			// Prevent duplicate enrollment (Student + Course)
			bool alreadyEnrolled = await _context.Enrollment.AnyAsync(e =>
				e.StudentId == dto.StudentId &&
				e.CourseId == dto.CourseId &&
				e.Status == "Active");

			if (alreadyEnrolled)
				throw new EnrollmentAlreadyExistsException("Student already enrolled for this course");

			// Generate SAFE unique EnrollmentId
			int count = await _repo.GetEnrollmentCountAsync();
			var Enrollment_Id = $"E{(count + 1):D3}";

			// Create enrollment (NO batch here)
			var enrollment = new Enrollment
			{
				EnrollmentId = Enrollment_Id,
				StudentId = dto.StudentId,
				CourseId = dto.CourseId,
				EnrollmentDate = DateTime.Now,
				Status = "Active",
				Credits = course.Credits
			};

			await _repo.AddEnrollmentAsync(enrollment);
			return Enrollment_Id;
		}



		public async Task<List<ModuleWithContentDto>> GetContentForStudentAsync(string studentId, string courseId)
		{
			if (!await _repo.IsEnrolledAsync(studentId, courseId))
				throw new EnrollmentNotExistsException("You must enroll first", 403);

			return await _repo.GetModulesByCourseAsync(courseId);
		}

		public async Task<int> MarkContentCompletedAsync(string studentId, string courseId, string contentId)
		{
			if (await _repo.CheckIfProgressExistsAsync(studentId, contentId))
				throw new ProgressRecordAlreadyExistsException("Already completed");

			int count = await _repo.GetProgressCountAsync();
			string newId = $"SP{(count + 1):D3}";

			var progress = new StudentProgress
			{
				ProgressID = Guid.NewGuid().ToString(),
				StudentId = studentId,
				CourseId = courseId,
				ContentId = contentId,
				IsCompleted = true,
				CompletionDate = DateTime.Now
			};

			return await _repo.MarkContentCompletedAsync(progress);
		}

		public async Task<double> GetCourseProgressPercentageAsync(string studentId, string courseId)
		{
			return await _repo.GetCourseProgressPercentageAsync(studentId, courseId);
		}

		public async Task<bool> ProcessCourseCompletionAsync(string studentId, string courseId)
		{
			double progress = await GetCourseProgressPercentageAsync(studentId, courseId);

			if (progress >= 100)
			{
				await _repo.UpdateEnrollmentStatusAsync(studentId, courseId, "Completed");
				return true;
			}

			return false;
		}

		public async Task<List<StudentCourseAttendanceDto>> CalculateStudentAttendanceByStudentIdAsync(string studentId)
		{
			var enrollments = await _repo.GetEnrollmentsByStudentIdAsync(studentId);

			if (enrollments == null || enrollments.Count == 0)
				return new List<StudentCourseAttendanceDto>();

			var result = new List<StudentCourseAttendanceDto>();

			var groupedCourses = enrollments.GroupBy(e => new { e.CourseId, e.Course.CourseName });

			foreach (var group in groupedCourses)
			{
				int totalSessions = 0;
				int totalPresent = 0;

				foreach (var enrollment in group)
				{
					var records = await _repo.GetStudentAttendanceAsync(enrollment.EnrollmentId);

					if (records != null && records.Count > 0)
					{
						totalSessions += records
							.Select(a => a.SessionDate.Date)
							.Distinct()
							.Count();

						totalPresent += records.Count(a => a.Status == "Present");
					}
				}

				double percentage = 0;

				if (totalSessions > 0)
				{
					percentage = ((double)totalPresent / totalSessions) * 100;
				}

				result.Add(new StudentCourseAttendanceDto
				{
					CourseId = group.Key.CourseId,
					CourseName = group.Key.CourseName,
					AttendancePercentage = Math.Round(percentage, 2)
				});
			}
			return result;
		}

		public async Task<List<BatchAttendanceDto>> GetBatchWiseAttendanceAsync(string courseId)
		{
			// Get all active batches for the course
			var batches = await _repo.GetBatchesByCourseAsync(courseId);

			if (batches == null || batches.Count == 0)
				return new List<BatchAttendanceDto>();

			var batchAttendanceList = new List<BatchAttendanceDto>();

			// Loop through each batch
			foreach (var batch in batches)
			{
				var attendanceRecords = await _repo.GetBatchAttendanceAsync(batch.BatchId);

				double percentage = 0;

				if (attendanceRecords != null && attendanceRecords.Count > 0)
				{
					int totalRecords = attendanceRecords.Count;
					int totalPresent = attendanceRecords.Count(a => a.Status == "Present");

					percentage = ((double)totalPresent / totalRecords) * 100;
					percentage = Math.Round(percentage, 2);
				}

				batchAttendanceList.Add(new BatchAttendanceDto
				{
					BatchId = batch.BatchId,
					CourseId = batch.CourseId,
					AttendancePercentage = percentage
				});
			}

			return batchAttendanceList;
		}
		
	}
}
