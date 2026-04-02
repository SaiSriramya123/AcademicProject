using EduTrackAcademics.DTO;

using EduTrackAcademics.Exceptions;

using EduTrackAcademics.Model;

using EduTrackAcademics.Repository;

using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

namespace EduTrackAcademics.Services

{

    public class PerformanceService : IPerformanceService

    {

        private readonly IPerformanceRepository _repo;

        public PerformanceService(IPerformanceRepository repo)

        {

            _repo = repo;

        }

        // ✅ ADD PERFORMANCE

        public async Task<string> AddPerformanceAsync(BatchPerformanceDTO dto)

        {

            // 🔹 Validation

            if (dto == null)

                throw new InvalidInputException("Performance data is required");

            if (string.IsNullOrEmpty(dto.EnrollmentId))

                throw new InvalidInputException("EnrollmentId is required");

            // 🔹 Business Logic (Generate ID)

            var last = await _repo.GetLastPerformanceAsync();

            int next = 1;

            if (last != null && last.ProgressID.StartsWith("P"))

            {

                int.TryParse(last.ProgressID.Substring(1), out next);

                next++;

            }

            // 🔹 Create Entity

            var performance = new Performance

            {

                ProgressID = $"P{next:D3}",

                EnrollmentId = dto.EnrollmentId,

                CompletionPercentage = dto.CompletionPercentage,

                AvgScore = dto.AvgScore,

                LastUpdated = DateTime.Now,

                InstructorId = dto.InstructorId,

                BatchId = dto.BatchId,

                StudentId = dto.StudentId

            };

            // 🔹 Save to DB

            await _repo.AddPerformanceAsync(performance);

            return performance.ProgressID;

        }

        // ✅ GET COUNT BASED INSERT

        public async Task<string> CreatePerformanceWithCountAsync(BatchPerformanceDTO dto)

        {

            if (dto == null)

                throw new InvalidInputException("Performance data is required");

            int count = await _repo.GetPerformanceCountAsync();

            var performance = new Performance

            {

                ProgressID = $"P{(count + 1):D3}",

                EnrollmentId = dto.EnrollmentId,

                CompletionPercentage = 0,

                AvgScore = 0,

                LastUpdated = DateTime.Now,

                InstructorId = dto.InstructorId,

                BatchId = dto.BatchId,

                StudentId = dto.StudentId

            };

            await _repo.AddPerformanceAsync(performance);

            return performance.ProgressID;

        }

        // ✅ GET AVERAGE SCORE

        public async Task<EnrollmentAverageScoreDTO> GetAverageScoreAsync(string enrollmentId)

        {

            if (string.IsNullOrEmpty(enrollmentId))

                throw new InvalidInputException("EnrollmentId is required");

            var result = await _repo.GetAverageScoreAsync(enrollmentId);

            if (result == null)

                throw new DataNotFoundException("Average score not found");

            return result;

        }

        // ✅ GET LAST UPDATED

        public async Task<LastUpdatedDTO> GetLastModifiedDateAsync(string enrollmentId)

        {

            if (string.IsNullOrEmpty(enrollmentId))

                throw new InvalidInputException("EnrollmentId is required");

            var result = await _repo.GetLastModifiedDateAsync(enrollmentId);

            if (result == null)

                throw new DataNotFoundException("Last updated data not found");

            return result;

        }

        // ✅ GET INSTRUCTOR BATCHES

        public async Task<List<InstructorBatchDTO>> GetInstructorBatchesAsync(string instructorId)

        {

            if (string.IsNullOrEmpty(instructorId))

                throw new InvalidInputException("InstructorId is required");

            var result = await _repo.GetInstructorBatchesAsync(instructorId);

            if (result == null || !result.Any())

                throw new DataNotFoundException("No batches found for this instructor");

            return result;

        }

        // ✅ GET BATCH PERFORMANCE

        public async Task<GetBatchReportDTO> GetBatchPerformanceAsync(string batchId)

        {

            if (string.IsNullOrEmpty(batchId))

                throw new InvalidInputException("BatchId is required");

            var result = await _repo.GetBatchPerformanceAsync(batchId);

            if (result == null)

                throw new DataNotFoundException("Batch performance not found");

            return result;

        }

    }

}
