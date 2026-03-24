using System.ComponentModel.DataAnnotations.Schema;

namespace EduTrackAcademics.Model
{
	public class StudentLoginHistory
	{
		public int Id { get; set; }
		[ForeignKey("Student")]
		public string StudentId { get; set; }
		public DateTime LoginTime { get; set; }
		public DateTime? LogoutTime { get; set; }

	}
}
