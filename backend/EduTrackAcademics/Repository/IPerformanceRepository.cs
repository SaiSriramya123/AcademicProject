using EduTrackAcademics.DTO;
using EduTrackAcademics.Model;
using System.Collections.Generic;
namespace EduTrackAcademics.Repository
{
    public interface IPerformanceRepository

    {

        Task<EnrollmentAverageScoreDTO> GetAverageScoreAsync(string enrollmentId);

        Task<LastUpdatedDTO> GetLastModifiedDateAsync(string enrollmentId);

        Task<List<InstructorBatchDTO>> GetInstructorBatchesAsync(string instructorId);

        Task<GetBatchReportDTO> GetBatchPerformanceAsync(string batchId);

        Task<int> GetPerformanceCountAsync();

        Task AddPerformanceAsync(Performance performance);

        Task<Performance?> GetLastPerformanceAsync();

    }




}

