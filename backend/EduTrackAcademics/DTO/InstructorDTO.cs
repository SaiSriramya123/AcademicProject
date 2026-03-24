using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace EduTrackAcademics.DTO
{
	public class InstructorDTO
	{
		[Required]
		public string InstructorName { get; set; }

		[Required, EmailAddress]
		public string InstructorEmail { get; set; }

		[Required]
		public long InstructorPhone { get; set; }

		public string InstructorQualification { get; set; }

		public string InstructorSkills { get; set; }

		public int InstructorExperience { get; set; }

		public DateOnly InstructorJoinDate { get; set; }

		public string InstructorGender { get; set; }

		public string InstructorPassword { get; set; }

		[Required(ErrorMessage = "Resume is required")]
		public IFormFile InstructorResume { get; set; }
	}
}