using EduTrackAcademics.DTO;
using EduTrackAcademics.Repository;

namespace EduTrackAcademics.Services
{
	public class AdminService : IAdminService
	{
		private readonly IAdminRepository _repo;

		public AdminService(IAdminRepository repo)
		{
			_repo = repo;
		}

		public object AddQualification(QualificationDTO dto)
		{
			return _repo.AddQualification(dto);
		}

		public object AddProgram(ProgramDTO dto)
		{
			return _repo.AddProgram(dto);
		}

		public object AddAcademicYear(AcademicYearDTO dto)
		{
			return _repo.AddAcademicYear(dto);
		}
		public object AddRule(AcademicRuleDTO dto)
		{
			return _repo.AddRule(dto);
		}

		public IEnumerable<AcademicRuleResponseDTO> GetAllRules()
		{
			return _repo.GetAllRules();
		}
	}
}
