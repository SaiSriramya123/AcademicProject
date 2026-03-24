using EduTrackAcademics.Data;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Exceptions;
using EduTrackAcademics.Model;
using EduTrackAcademics.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduTrackAcademics.Services
{
	public class InstructorService : IInstructorService
	{
		private readonly IInstructorRepo _repo;

		public InstructorService(IInstructorRepo repo)
		{
			_repo = repo;
		}

		// MODULE

		public async Task<(Module module, string message)> CreateModuleAsync(ModuleDTO dto)
		{
			var newId = await _repo.GenerateModuleIdAsync();

			var module = new Module
			{
				ModuleID = newId,
				CourseId = dto.CourseId,
				Name = dto.Name,
				SequenceOrder = dto.SequenceOrder,
				LearningObjectives = dto.LearningObjectives
			};

			await _repo.AddModuleAsync(module);

			return (module, "Module successfully created");
		}

		public async Task<IEnumerable<object>> GetModulesAsync(string courseId)
		{
			var modules = await _repo.GetModulesByCourseAsync(courseId);

			return modules.Select(m => new
			{
				m.ModuleID,
				m.CourseId,
				m.Name,
				m.SequenceOrder,
				m.LearningObjectives
			});
		}

		public async Task<string> UpdateModuleAsync(string moduleId, ModuleDTO dto)
		{
			var module = await _repo.GetModuleByIdAsync(moduleId);

			if (module == null)
				return "Module not found";

			module.Name = dto.Name;
			module.SequenceOrder = dto.SequenceOrder;
			module.LearningObjectives = dto.LearningObjectives;

			await _repo.UpdateModuleAsync(module);

			return "Module updated successfully";
		}

		// CONTENT

		public async Task<string> CreateContentAsync(ContentDTO dto)
		{
			if (!await _repo.ModuleExistsAsync(dto.ModuleId))
				throw new ModuleNotFoundException(dto.ModuleId);

			if (!new[] { "Video", "PDF", "Slide", "Lab" }.Contains(dto.ContentType))
				throw new InvalidContentTypeException(dto.ContentType);

			var content = new Content
			{
				ContentID = await _repo.GenerateContentIdAsync(),
				ModuleID = dto.ModuleId,
				Title = dto.Title,
				ContentType = dto.ContentType,
				ContentURI = dto.ContentURI,
				Status = "Draft"
			};

			await _repo.AddContentAsync(content);

			return $"Content created. ID = {content.ContentID}";
		}

		public async Task<List<Content>> GetContentByModuleAsync(string moduleId)
		{
			return await _repo.GetContentByModuleAsync(moduleId);
		}

		public async Task<Content> GetContentAsync(string id)
		{
			var content = await _repo.GetContentByIdAsync(id);
			if (content == null)
				throw new ContentNotFoundException(id);

			return content;
		}

		public async Task<string> UpdateContentAsync(string id, ContentDTO dto)
		{
			var content = await _repo.GetContentByIdAsync(id);
			if (content == null)
				throw new ContentNotFoundException(id);

			content.Title = dto.Title;
			content.ContentType = dto.ContentType;
			content.ContentURI = dto.ContentURI;

			await _repo.UpdateContentAsync(content);

			return "Content updated successfully";
		}

		public async Task<string> PublishContentAsync(string id)
		{
			var content = await _repo.GetContentByIdAsync(id);
			if (content == null)
				throw new ContentNotFoundException(id);

			if (content.Status == "Published")
				throw new ContentAlreadyPublishedException(id);

			content.Status = "Published";

			await _repo.UpdateContentAsync(content);

			return "Content published successfully";
		}

		public async Task<string> DeleteContentAsync(string id)
		{
			var content = await _repo.GetContentByIdAsync(id);
			if (content == null)
				throw new ContentNotFoundException(id);

			await _repo.DeleteContentAsync(content);

			return "Content deleted successfully";
		}

		// ASSESSMENT

		public async Task<string> CreateAssessmentAsync(AssessmentDTO dto)
		{
			var id = await _repo.GenerateAssessmentIdAsync();

			var dueDateUtc = DateTime.SpecifyKind(dto.DueDate, DateTimeKind.Utc);

			var assessment = new Assessment
			{
				AssessmentID = id,
				CourseId = dto.CourseId,
				Type = dto.Type,
				MaxMarks = dto.MaxMarks,
				DueDate = dueDateUtc,
				Status = dueDateUtc < DateTime.UtcNow ? "Closed" : "Open"
			};

			await _repo.AddAssessmentAsync(assessment);

			return $"Assessment created with ID {id} and status {assessment.Status}";
		}



		public async Task<Assessment> GetAssessmentByIdAsync(string id)
		{
			var assessment = await _repo.GetAssessmentByIdAsync(id);
			if (assessment == null)
				throw new ApplicationException("Assessment not found");
			return assessment;
		}

		public async Task<List<Assessment>> GetAssessmentsByCourseAsync(string courseId)
			=> await _repo.GetAssessmentsByCourseAsync(courseId);

		public async Task<string> UpdateAssessmentAsync(string id, AssessmentDTO dto)
		{
			var assessment = await _repo.GetAssessmentByIdAsync(id);
			if (assessment == null)
				throw new ApplicationException("Assessment not found");

			assessment.Type = dto.Type;
			assessment.MaxMarks = dto.MaxMarks;
			assessment.DueDate = dto.DueDate;

			await _repo.UpdateAssessmentAsync(assessment);
			return "Assessment updated successfully";
		}

		public async Task<string> DeleteAssessmentAsync(string id)
		{
			var assessment = await _repo.GetAssessmentByIdAsync(id);
			if (assessment == null)
				throw new ApplicationException("Assessment not found");

			await _repo.DeleteAssessmentAsync(assessment);
			return "Assessment deleted successfully";
		}

		public async Task<SubmissionResultDTO> AddFeedbackAsync(UpdateSubmissionDto dto)
		{
			var submission = await _repo.GetSubmissionAsync(dto.StudentId, dto.AssessmentId);

			if (submission == null)
			{
				return new SubmissionResultDTO
				{
					IsSubmitted = false,
					Score = 0,
					Percentage = 0
				};
			}
			submission.Feedback = dto.Feedback;
			submission.Score = dto.Score;

			await _repo.UpdateSubmissionAsync(submission);

			var totalMarks = await _repo.GetTotalMarksAsync(dto.AssessmentId);
			double percentage = 0;
			if (totalMarks > 0)
			{
				percentage = ((double)dto.Score / totalMarks) * 100;
			}

			return new SubmissionResultDTO
			{
				IsSubmitted = true,
				Score = dto.Score,
				Percentage = percentage
			};
		}

		// QUESTIONS

		public async Task<string> AddQuestionAsync(QuestionDTO dto)
		{
			var id = await _repo.GenerateQuestionIdAsync();

			var question = new Question
			{
				QuestionId = id,
				AssessmentId = dto.AssessmentId,
				QuestionType = dto.QuestionType,
				QuestionText = dto.QuestionText,
				OptionA = dto.OptionA,
				OptionB = dto.OptionB,
				OptionC = dto.OptionC,
				OptionD = dto.OptionD,
				CorrectOption = dto.CorrectOption,
				Marks = dto.Marks
			};

			await _repo.AddQuestionAsync(question);
			return $"Question added with ID {id}";
		}

		public async Task<Question> GetQuestionByIdAsync(string id)
		{
			var question = await _repo.GetQuestionByIdAsync(id);
			if (question == null)
				throw new ApplicationException("Question not found");
			return question;
		}

		public async Task<List<Question>> GetQuestionsByAssessmentAsync(string assessmentId)
			=> await _repo.GetQuestionsByAssessmentAsync(assessmentId);

		public async Task<string> UpdateQuestionAsync(string id, QuestionDTO dto)
		{
			var question = await _repo.GetQuestionByIdAsync(id);
			if (question == null)
				throw new ApplicationException("Question not found");

			question.QuestionText = dto.QuestionText;
			question.OptionA = dto.OptionA;
			question.OptionB = dto.OptionB;
			question.OptionC = dto.OptionC;
			question.OptionD = dto.OptionD;
			question.CorrectOption = dto.CorrectOption;
			question.Marks = dto.Marks;

			await _repo.UpdateQuestionAsync(question);
			return "Question updated successfully";
		}

		public async Task<string> DeleteQuestionAsync(string id)
		{
			var question = await _repo.GetQuestionByIdAsync(id);
			if (question == null)
				throw new ApplicationException("Question not found");

			await _repo.DeleteQuestionAsync(question);
			return "Question deleted successfully";
		}

		// ATTENDANCE

		public async Task<string> MarkAttendanceAsync(AttendanceDTO dto)
		{
			var enrollmentExists = await _repo.EnrollmentExistsAsync(dto.EnrollmentID);
			if (!enrollmentExists)
				throw new ApplicationException("Invalid Enrollment ID");

			var batchExists = await _repo.BatchExistsAsync(dto.BatchId);
			if (!batchExists)
				throw new ApplicationException("Invalid Batch ID");

			var alreadyMarked = await _repo.AttendanceExistsAsync(dto.EnrollmentID, dto.SessionDate);
			if (alreadyMarked)
				throw new ApplicationException("Attendance already marked for this date");

			var attendanceId = await _repo.GenerateAttendanceIdAsync();

			var attendance = new Attendance
			{
				AttendanceID = attendanceId,
				EnrollmentID = dto.EnrollmentID,
				BatchId = dto.BatchId,
				SessionDate = dto.SessionDate,
				Mode = dto.Mode,
				Status = dto.Status,
				IsDeleted = false
			};

			await _repo.AddAttendanceAsync(attendance);

			return $"Attendance marked successfully with ID {attendanceId}";
		}

		public async Task<List<object>> GetAllAttendanceAsync()
		{
			var attendances = await _repo.GetAllAttendanceAsync();

			return attendances.Select(a => new
			{
				a.AttendanceID,
				a.EnrollmentID,
				StudentName = a.Enrollment.Student.StudentName,
				CourseName = a.Enrollment.Course.CourseName,
				a.BatchId,
				a.SessionDate,
				a.Mode,
				a.Status
			}).ToList<object>();
		}

		public async Task<List<object>> GetAttendanceByDateAsync(DateTime date)
		{
			var attendances = await _repo.GetAttendanceByDateAsync(date);

			return attendances.Select(a => new
			{
				a.AttendanceID,
				a.EnrollmentID,
				StudentName = a.Enrollment.Student.StudentName,
				CourseName = a.Enrollment.Course.CourseName,
				a.BatchId,
				a.SessionDate,
				a.Mode,
				a.Status
			}).ToList<object>();
		}

		public async Task<List<object>> GetAttendanceByBatchAsync(string batchId)
		{
			var attendances = await _repo.GetAttendanceByBatchAsync(batchId);

			return attendances.Select(a => new
			{
				a.AttendanceID,
				a.EnrollmentID,
				StudentName = a.Enrollment.Student.StudentName,
				CourseName = a.Enrollment.Course.CourseName,
				a.BatchId,
				a.SessionDate,
				a.Mode,
				a.Status
			}).ToList<object>();
		}

		public async Task<List<object>> GetAttendanceByEnrollmentAsync(string enrollmentId)
		{
			var attendances = await _repo.GetAttendanceByEnrollmentAsync(enrollmentId);

			return attendances.Select(a => new
			{
				a.AttendanceID,
				a.EnrollmentID,
				StudentName = a.Enrollment.Student.StudentName,
				CourseName = a.Enrollment.Course.CourseName,
				a.BatchId,
				a.SessionDate,
				a.Mode,
				a.Status
			}).ToList<object>();
		}

		public async Task<string> UpdateAttendanceAsync(string attendanceId, AttendanceDTO dto)
		{
			var attendance = await _repo.GetAttendanceByIdAsync(attendanceId);

			if (attendance == null)
				throw new ApplicationException("Attendance not found");

			attendance.EnrollmentID = dto.EnrollmentID;
			attendance.BatchId = dto.BatchId;
			attendance.SessionDate = dto.SessionDate;
			attendance.Mode = dto.Mode;
			attendance.Status = dto.Status;
			attendance.UpdatedOn = DateTime.Now;

			await _repo.UpdateAttendanceAsync(attendance);

			return $"Attendance {attendanceId} updated successfully";
		}

		public async Task<string> DeleteAttendanceAsync(string attendanceId, string reason)
		{
			var attendance = await _repo.GetAttendanceByIdAsync(attendanceId);

			if (attendance == null)
				return "Attendance not found or already deleted";

			attendance.IsDeleted = true;
			attendance.DeletionReason = reason;
			attendance.DeletionDate = DateTime.Now;

			await _repo.SoftDeleteAttendanceAsync(attendance);

			return $"Attendance {attendanceId} deleted successfully";
		}

	}
}
