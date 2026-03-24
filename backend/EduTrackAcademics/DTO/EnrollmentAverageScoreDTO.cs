namespace EduTrackAcademics.DTO
{
	public class EnrollmentAverageScoreDTO
	{
		public string EnrollmentId { get; set; }
		public string StudentName { get; set; }
		public string CourseName { get; set; }
		public decimal TotalScore { get; set; }
		public decimal AverageScore { get; set; }
		public decimal Percentage { get; set; }
	}
}
