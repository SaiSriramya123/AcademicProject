using EduTrackAcademics.DTO;
using EduTrackAcademics.Model;
using System.Collections.Generic;
namespace EduTrackAcademics.Repository
{
    public interface IPerformanceRepository
    {
        
        EnrollmentAverageScoreDTO GetAverageScore(string studentId);
        BatchPerformanceDTO GetLastModifiedDate(string enrollmentId);
        List<InstructorBatchDTO>GetInstructorBatches(string  instructorId);
		BatchAveragePerformanceDTO GetBatchPerformance(string  batchId);
       

    }
}
