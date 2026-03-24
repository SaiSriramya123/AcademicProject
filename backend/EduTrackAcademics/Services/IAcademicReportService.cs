using EduTrackAcademics.Model;

namespace EduTrackAcademics.Services
{
    public interface IAcademicReportService
    {
        List<BatchAveragePerformanceDTO> GetAllBatchPerformanceReport();

    }
}
