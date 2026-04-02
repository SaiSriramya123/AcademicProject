using EduTrackAcademics.DTO;
using EduTrackAcademics.Model;

namespace EduTrackAcademics.Repository
{
	public interface IAdminRepository
	{
	
			object AddQualification(QualificationDTO dto);
		    IEnumerable<QualificationDTO> GetAllQualifications();
			object AddProgram(ProgramDTO dto);
			object AddAcademicYear(AcademicYearDTO dto);

			// 🔹 RULES
			object AddRule(AcademicRuleDTO dto);
			IEnumerable<AcademicRuleResponseDTO> GetAllRules();
		}
	}
