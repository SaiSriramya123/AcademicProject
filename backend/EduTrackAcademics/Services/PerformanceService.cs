using EduTrackAcademics.DTO;
using EduTrackAcademics.Exceptions;

using EduTrackAcademics.Model;
using EduTrackAcademics.Repository;
using static EduTrackAcademics.Services.PerformanceService;

namespace EduTrackAcademics.Services
{

    public class PerformanceService : IPerformanceService
    {

        private readonly IPerformanceRepository _repo;
        public PerformanceService(IPerformanceRepository rep)
        {
            _repo = rep;
        }
      
        public EnrollmentAverageScoreDTO GetAverageScore(string enrollmentId)
        {
            return _repo.GetAverageScore( enrollmentId);
        }

        public BatchPerformanceDTO GetLastModifiedDate(string enrollmentId)
        {
            return _repo.GetLastModifiedDate(enrollmentId);
        }

        public List <InstructorBatchDTO> GetInstructorBatches(string instructorId)
        {
            return _repo.GetInstructorBatches(instructorId);
        }

        public BatchAveragePerformanceDTO GetBatchPerformance(string batchId)
        {
            return _repo.GetBatchPerformance(batchId);
        }
    }
}