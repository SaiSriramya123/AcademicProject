using EduTrackAcademics.DTO;
using EduTrackAcademics.Model;

namespace EduTrackAcademics.Services
{
	public interface IInstructorService
	{
		// MODULE
		Task<(Module module, string message)> CreateModuleAsync(ModuleDTO dto);
		Task<IEnumerable<object>> GetModulesAsync(string courseId);
		Task<string> UpdateModuleAsync(string moduleId, ModuleDTO dto);

		// CONTENT
		Task<string> CreateContentAsync(ContentDTO dto);
		Task<List<Content>> GetContentByModuleAsync(string moduleId);
		Task<Content> GetContentAsync(string contentId);
		Task<string> UpdateContentAsync(string id, ContentDTO dto);
		Task<string> PublishContentAsync(string id);
		Task<string> DeleteContentAsync(string id);

		// ASSESSMENT
		Task<string> CreateAssessmentAsync(AssessmentDTO dto);
		Task<Assessment> GetAssessmentByIdAsync(string assessmentId);
		Task<List<Assessment>> GetAssessmentsByCourseAsync(string courseId);
		Task<string> UpdateAssessmentAsync(string assessmentId, AssessmentDTO dto);
		Task<string> DeleteAssessmentAsync(string assessmentId);
		Task<SubmissionResultDTO> AddFeedbackAsync(UpdateSubmissionDto dto);

		// QUESTIONS
		Task<string> AddQuestionAsync(QuestionDTO dto);
		Task<Question> GetQuestionByIdAsync(string questionId);
		Task<List<Question>> GetQuestionsByAssessmentAsync(string assessmentId);
		Task<string> UpdateQuestionAsync(string questionId, QuestionDTO dto);
		Task<string> DeleteQuestionAsync(string questionId);

		// ATTENDANCE
		Task<string> MarkAttendanceAsync(AttendanceDTO dto);
		Task<List<object>> GetAllAttendanceAsync();
		Task<List<object>> GetAttendanceByDateAsync(DateTime date);
		Task<List<object>>GetAttendanceByBatchAsync(string batchId);
		Task<List<object>>GetAttendanceByEnrollmentAsync(string enrollmentId);
		Task<string>UpdateAttendanceAsync(string attendanceId, AttendanceDTO dto);
		Task<string> DeleteAttendanceAsync(string attendanceId, string reason);
	}
}
