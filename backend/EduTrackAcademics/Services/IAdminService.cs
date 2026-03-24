using EduTrackAcademics.DTO;

namespace EduTrackAcademics.Services
{
	public interface IAdminService
	{
		object AddQualification(QualificationDTO dto);
		object AddProgram(ProgramDTO dto);
		object AddAcademicYear(AcademicYearDTO dto);

		// 🔹 RULES
		object AddRule(AcademicRuleDTO dto);
		IEnumerable<AcademicRuleResponseDTO> GetAllRules();
	}
}