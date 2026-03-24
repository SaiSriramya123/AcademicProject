using EduTrackAcademics.Data;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Model;
using EduTrackAcademics.Services;

namespace EduTrackAcademics.Repository
{
	public class AdminRepository : IAdminRepository
	{
		private readonly EduTrackAcademicsContext _context;
		private readonly IdService _idService;

		public AdminRepository(EduTrackAcademicsContext context, IdService idService)
		{
			_context = context;
			_idService = idService;
		}

		public object AddQualification(QualificationDTO dto)
		{
			var qualification = new Qualification
			{
				QualificationId = _idService.GenerateQualificationId(),
				QualificationName = dto.QualificationName
			};

			_context.Qualification.Add(qualification);
			_context.SaveChanges();

			return new { Message = "Qualification added", Id = qualification.QualificationId };
		}

		public object AddProgram(ProgramDTO dto)
		{
			var qualification = _context.Qualification
				.FirstOrDefault(q => q.QualificationId == dto.QualificationId);

			if (qualification == null)
				throw new ApplicationException("Qualification does not exist");

			var program = new ProgramEntity
			{
				ProgramId = _idService.GenerateProgramId(),
				ProgramName = dto.ProgramName,
				QualificationId = dto.QualificationId
			};

			_context.Programs.Add(program);
			_context.SaveChanges();

			return new { Message = "Program added", Id = program.ProgramId };
		}
		public object AddRule(AcademicRuleDTO dto)
		{
			var existingRule = _context.AcademicRules
				.FirstOrDefault(r => r.RuleName == dto.RuleName);

			if (existingRule != null)
				throw new ApplicationException("Rule already exists");

			var rule = new AcademicRule
			{
				RuleId = _idService.GenerateRuleId(),
				RuleName = dto.RuleName,
				RuleValue = dto.RuleValue,
				Description = dto.Description,
				LastUpdated = DateTime.UtcNow
			};

			_context.AcademicRules.Add(rule);
			_context.SaveChanges();

			return new
			{
				Message = "Rule added successfully",
				RuleId = rule.RuleId
			};
		}

		public IEnumerable<AcademicRuleResponseDTO> GetAllRules()
		{
			return _context.AcademicRules
				.Select(r => new AcademicRuleResponseDTO
				{
					RuleName = r.RuleName,
					RuleValue = r.RuleValue,
					Description = r.Description,
					LastUpdated = r.LastUpdated
				}).ToList();
		}

		public object AddAcademicYear(AcademicYearDTO dto)
		{
			var program = _context.Programs
				.FirstOrDefault(p => p.ProgramId == dto.ProgramId);

			if (program == null)
				throw new ApplicationException("Program does not exist");

			var year = new AcademicYear
			{
				AcademicYearId = _idService.GenerateAcademicYearId(),
				YearNumber = dto.YearNumber,
				ProgramId = dto.ProgramId
			};

			_context.AcademicYear.Add(year);
			_context.SaveChanges();

			return new { Message = "Academic year added", Id = year.AcademicYearId };
		}
	}
}
