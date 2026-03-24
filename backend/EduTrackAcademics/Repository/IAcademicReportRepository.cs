using EduTrackAcademics.Model;

namespace EduTrackAcademics.Repository
{
    public interface IAcademicReportRepository
    {


        List<BatchAveragePerformanceDTO> GetAllBatchPerformanceReport();

	}
}

