using EduTrackAcademics.DTO;
using EduTrackAcademics.Model;
using System.Collections.Generic;
namespace EduTrackAcademics.Services
{
    public interface IPerformanceService

    {

        Task<string> AddPerformanceAsync(BatchPerformanceDTO dto);

        Task<string> CreatePerformanceWithCountAsync(BatchPerformanceDTO dto);

        Task<EnrollmentAverageScoreDTO> GetAverageScoreAsync(string enrollmentId);

        Task<LastUpdatedDTO> GetLastModifiedDateAsync(string enrollmentId);

        Task<List<InstructorBatchDTO>> GetInstructorBatchesAsync(string instructorId);

        Task<GetBatchReportDTO> GetBatchPerformanceAsync(string batchId);

    }


}
