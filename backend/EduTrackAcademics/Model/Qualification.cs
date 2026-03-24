
using System.ComponentModel.DataAnnotations;

namespace EduTrackAcademics.Model
{ 
	public class Qualification
	{
		[Key]
		public string QualificationId { get; set; }
		[Required]
		public string QualificationName { get; set; }
		//[Required]
		//public string CollegeCode { get; set; }

		public ICollection<ProgramEntity> Programs { get; set; }
	}
}
