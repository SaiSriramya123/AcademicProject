using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduTrackAcademics.Model
{
	public class Coordinator
	{
		[Key]
		public string CoordinatorId { get; set; }

		[ForeignKey("User")] 
		public int? UserId { get; set; }
		public Users User { get; set; } // Navigation property to Users

		[Required]
		public string CoordinatorName { get; set; }

		[Required, EmailAddress]
		public string CoordinatorEmail { get; set; }

		public string Role { get; set; } ="Coordinator";

		public long CoordinatorPhone { get; set; }

		public string CoordinatorQualification { get; set; }

		public int CoordinatorExperience { get; set; }

		public string CoordinatorGender { get; set; }
		
		public string Resumepath { get; set; }

		[Required]
		public string CoordinatorPassword { get; set; }
		[Required]
		public bool IsFirstLogin { get; set; }
		[Required]
		public bool IsActive { get; set; }

	}
}
