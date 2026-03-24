using System.Threading.Tasks;
using EduTrackAcademics.DTO;
namespace EduTrackAcademics.Services
{
	public interface IStudentProfileService
	{
		Task<StudentDTO> GetPersonalInfoAsync(string studentId);
		Task<StudentDTO> GetProgramDetails(string studentId);
		Task UpdateAdditionalInfo(string studentId, StudentAdditionalDetailsDTO dto);
		Task<StudentDTO> GetStudentDetails(string studentId);
		Task<int> GetCreditPointsAsync(string studentId);
		Task<(DateTime DueDate, string Type, string CourseName)> GetAssignmentDetailsForStudentAsync(string studentId, string courseId);

		Task<AuditLogDTO> GetAuditLogAsync(string studentId);
	}
}