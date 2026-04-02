using EduTrackAcademics.DTO;

public class BatchAveragePercentageDTO
{
	public string BatchId { get; set; }
	public string CourseName { get; set; }
	public decimal BatchAveragePercentage { get; set; }
	public List<BatchPerformanceDTO> Students { get; set; }
}