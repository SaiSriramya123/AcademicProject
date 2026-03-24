using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EduTrackAcademics.Model
{
	public class StudentBatchAssignment
	{
		[Key]
		//public int Id { get; set; } 

		//[Required]

		public string BatchId { get; set; }
		[Required]
		public string StudentId { get; set; }
		public Student Student { get; set; }
		public CourseBatch Batches { get; set; }
		public ICollection<Attendance> Attendances { get; set; }
	}
}