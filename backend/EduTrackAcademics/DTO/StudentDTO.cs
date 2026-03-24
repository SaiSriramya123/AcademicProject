using System.ComponentModel.DataAnnotations;

namespace EduTrackAcademics.DTO
{
	

	public class StudentDTO
	{
		[Required]
		public string StudentName { get; set; }

		[Required, EmailAddress]
		public string StudentEmail { get; set; }

		[Required]
		[RegularExpression(@"^[6-9]\d{9}$")]
		public long StudentPhone { get; set; }

		public string StudentQualification { get; set; }
		public string StudentProgram { get; set; }
		public DateTime StudentAcademicYear { get; set; }
		public int year {  get; set; } = 0;
		public string StudentGender { get; set; }

		[Required]
		public string StudentPassword { get; set; }
	}
}
