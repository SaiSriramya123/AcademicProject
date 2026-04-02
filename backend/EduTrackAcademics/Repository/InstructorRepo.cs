using EduTrackAcademics.Data;
using EduTrackAcademics.Exceptions;
using EduTrackAcademics.Model;
using Microsoft.EntityFrameworkCore;

namespace EduTrackAcademics.Repository
{
	public class InstructorRepo : IInstructorRepo
	{
		private readonly EduTrackAcademicsContext _context;

		public InstructorRepo(EduTrackAcademicsContext context)
		{
			_context = context;
		}

		// MODULE

		public async Task<string> GenerateModuleIdAsync()
		{
			var lastModule = await _context.Modules
				.OrderByDescending(m => m.ModuleID)
				.FirstOrDefaultAsync();

			if (lastModule == null)
				return "M001";

			int num = int.Parse(lastModule.ModuleID.Substring(1));
			return $"M{(num + 1).ToString("D3")}";
		}

		public async Task AddModuleAsync(Module module)
		{
			await _context.Modules.AddAsync(module);
			await _context.SaveChangesAsync();
		}

		public async Task<List<Module>> GetModulesByCourseAsync(string courseId)
		{
			return await _context.Modules
				.Where(m => m.CourseId == courseId)
				.OrderBy(m => m.SequenceOrder)
				.ToListAsync();
		}

		public async Task<Module> GetModuleByIdAsync(string moduleId)
		{
			return await _context.Modules
				.FirstOrDefaultAsync(m => m.ModuleID == moduleId);
		}

		public async Task UpdateModuleAsync(Module module)
		{
			_context.Modules.Update(module);
			await _context.SaveChangesAsync();
		}

		// CONTENT

		public async Task<string> GenerateContentIdAsync()
		{
			var last = await _context.Contents
				.OrderByDescending(c => c.ContentID)
				.Select(c => c.ContentID)
				.FirstOrDefaultAsync();

			if (last == null) return "CT001";

			int num = int.Parse(last.Substring(3));
			return $"CT{num + 1:D3}";
		}

		public async Task<bool> ModuleExistsAsync(string moduleId)
		{
			return await _context.Modules.AnyAsync(m => m.ModuleID == moduleId);
		}

		public async Task AddContentAsync(Content content)
		{
			await _context.Contents.AddAsync(content);
			await _context.SaveChangesAsync();
		}

		public async Task<Content> GetContentByIdAsync(string contentId)
		{
			return await _context.Contents.FindAsync(contentId);
		}

		public async Task<List<Content>> GetContentByModuleAsync(string moduleId)
		{
			return await _context.Contents
				.Where(c => c.ModuleID == moduleId)
				.ToListAsync();
		}

		public async Task UpdateContentAsync(Content content)
		{
			_context.Contents.Update(content);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteContentAsync(Content content)
		{
			_context.Contents.Remove(content);
			await _context.SaveChangesAsync();
		}

		// ASSESSMENT

		public async Task<string> GenerateAssessmentIdAsync()
		{
			var last = await _context.Assessments
				.OrderByDescending(a => a.AssessmentID)
				.Select(a => a.AssessmentID)
				.FirstOrDefaultAsync();

			int next = last == null ? 1 : int.Parse(last.Substring(1)) + 1;
			return $"A{next:000}";

		}

		public async Task AddAssessmentAsync(Assessment assessment)
		{
			await _context.Assessments.AddAsync(assessment);
			await _context.SaveChangesAsync();
		}
		  
		public async Task<Assessment?> GetAssessmentByIdAsync(string id)
			=> await _context.Assessments.FindAsync(id);

		public async Task<List<Assessment>> GetAssessmentsByCourseAsync(string courseId)
			=> await _context.Assessments
				.Where(a => a.CourseId == courseId)
				.ToListAsync();

		public async Task UpdateAssessmentAsync(Assessment assessment)
		{
			_context.Assessments.Update(assessment);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAssessmentAsync(Assessment assessment)
		{
			_context.Assessments.Remove(assessment);
			await _context.SaveChangesAsync();

		}

		public async Task<Submission> GetSubmissionAsync(string studentId, string assessmentId)
		{
			return await _context.Submission
				.FirstOrDefaultAsync(s =>
					s.StudentID == studentId &&
					s.AssessmentId == assessmentId);
		}

		public async Task<int> GetTotalMarksAsync(string assessmentId)
		{
			return await _context.Questions
				.Where(q => q.AssessmentId == assessmentId)
				.SumAsync(q => q.Marks);
		}

		public async Task UpdateSubmissionAsync(Submission submission)
		{
			_context.Submission.Update(submission);
			await _context.SaveChangesAsync();
		}

		// QUESTIONS

		public async Task<string> GenerateQuestionIdAsync()
		{
			var last = await _context.Questions
				.OrderByDescending(q => q.QuestionId)
				.Select(q => q.QuestionId)
				.FirstOrDefaultAsync();

			int next = last == null ? 1 : int.Parse(last.Substring(1)) + 1;
			return $"Q{next:000}";
		}

		public async Task<bool> QuestionExistsAsync(string id)
			=> await _context.Questions.AnyAsync(q => q.QuestionId == id);

		public async Task AddQuestionAsync(Question question)
		{
			await _context.Questions.AddAsync(question);
			await _context.SaveChangesAsync(); // <-- critical
		}


		public async Task<Question?> GetQuestionByIdAsync(string id)
			=> await _context.Questions.FindAsync(id);

		public async Task<List<Question>> GetQuestionsByAssessmentAsync(string assessmentId)
			=> await _context.Questions
				.Where(q => q.AssessmentId == assessmentId)
				.ToListAsync();

		public async Task UpdateQuestionAsync(Question question)
		{
			_context.Questions.Update(question);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteQuestionAsync(Question question)
		{
			_context.Questions.Remove(question);
			await _context.SaveChangesAsync();
		}

		// ATTENDANCE

		public async Task<bool> EnrollmentExistsAsync(string enrollmentId)
		{
			return await _context.Enrollment
				.AnyAsync(e => e.EnrollmentId == enrollmentId);
		}

		public async Task<bool> BatchExistsAsync(string batchId)
		{
			return await _context.CourseBatches
				.AnyAsync(b => b.BatchId == batchId);
		}

		public async Task<bool> AttendanceExistsAsync(string enrollmentId, DateTime date)
		{
			return await _context.Attendances
				.AnyAsync(a =>
					a.EnrollmentID == enrollmentId &&
					a.SessionDate.Date == date.Date &&
					!a.IsDeleted);
		}

		public async Task<string> GenerateAttendanceIdAsync()
		{
			var last = await _context.Attendances
				.OrderByDescending(a => a.AttendanceID)
				.FirstOrDefaultAsync();

			if (last == null)
				return "A001";

			int number = int.Parse(last.AttendanceID.Substring(1));
			return $"A{(number + 1).ToString("D3")}";
		}

		public async Task AddAttendanceAsync(Attendance attendance)
		{
			_context.Attendances.Add(attendance);
			await _context.SaveChangesAsync();
		}

		public async Task<List<Attendance>> GetAllAttendanceAsync()
		{
			return await _context.Attendances
				.Include(a => a.Enrollment)
					.ThenInclude(e => e.Student)
				.Include(a => a.Enrollment)
					.ThenInclude(e => e.Course)
				.Where(a => !a.IsDeleted)
				.ToListAsync();
		}

		public async Task<List<Attendance>> GetAttendanceByDateAsync(DateTime date)
		{
			return await _context.Attendances
				.Include(a => a.Enrollment)
					.ThenInclude(e => e.Student)
				.Include(a => a.Enrollment)
					.ThenInclude(e => e.Course)
				.Where(a => a.SessionDate.Date == date.Date && !a.IsDeleted)
				.ToListAsync();
		}

		public async Task<List<Attendance>> GetAttendanceByBatchAsync(string batchId)
		{
			return await _context.Attendances
				.Include(a => a.Enrollment)
					.ThenInclude(e => e.Student)
				.Include(a => a.Enrollment)
					.ThenInclude(e => e.Course)
				.Where(a => a.BatchId == batchId && !a.IsDeleted)
				.ToListAsync();
		}

		public async Task<List<Attendance>> GetAttendanceByEnrollmentAsync(string enrollmentId)
		{
			return await _context.Attendances
				.Include(a => a.Enrollment)
					.ThenInclude(e => e.Student)
				.Include(a => a.Enrollment)
					.ThenInclude(e => e.Course)
				.Where(a => a.EnrollmentID == enrollmentId && !a.IsDeleted)
				.ToListAsync();
		}

		public async Task<Attendance> GetAttendanceByIdAsync(string attendanceId)
		{
			return await _context.Attendances
				.FirstOrDefaultAsync(a => a.AttendanceID == attendanceId && !a.IsDeleted);
		}

		public async Task UpdateAttendanceAsync(Attendance attendance)
		{
			_context.Attendances.Update(attendance);
			await _context.SaveChangesAsync();
		}

		public async Task SoftDeleteAttendanceAsync(Attendance attendance)
		{
			_context.Attendances.Update(attendance);
			await _context.SaveChangesAsync();
		}

	}
}
