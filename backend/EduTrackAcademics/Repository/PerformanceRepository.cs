/*using EduTrackAcademics.Data;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Dummy;
using EduTrackAcademics.Exceptions;
using EduTrackAcademics.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;*/

using EduTrackAcademics.Data;

using EduTrackAcademics.DTO;

using EduTrackAcademics.Model;
using EduTrackAcademics.Services;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EduTrackAcademics.Repository

{

    public class PerformanceRepository : IPerformanceRepository

    {

        private readonly EduTrackAcademicsContext _context;
        private readonly IEnrollmentService _enrollmentService;

        public PerformanceRepository(EduTrackAcademicsContext context, IEnrollmentService enrollmentService)

        {

            _context = context;
            _enrollmentService = enrollmentService;

        }


        // ✅ COUNT

        public async Task<int> GetPerformanceCountAsync()

        {

            return await _context.Performances.CountAsync();

        }

        // ✅ ADD

        public async Task AddPerformanceAsync(Performance performance)

        {

            await _context.Performances.AddAsync(performance);

            await _context.SaveChangesAsync();

        }

        // ✅ GET LAST

        public async Task<Performance?> GetLastPerformanceAsync()

        {

            return await _context.Performances

                .OrderByDescending(p => p.ProgressID)

                .FirstOrDefaultAsync();

        }

        // ✅ UPSERT + AVG SCORE
        // ================= CONTENT PROGRESS METHOD (FIX FOR ERROR) =================

        public async Task<double> GetCourseProgressPercentageAsync(string studentId, string courseId)

        {

            var moduleIds = _context.Modules

                .Where(m => m.CourseId == courseId)

                .Select(m => m.ModuleID)

                .ToList();

            var contentIds = _context.Contents

                .Where(c => moduleIds.Contains(c.ModuleID))

                .Select(c => c.ContentID)

                .ToList();

            int totalContentCount = contentIds.Count();

            if (totalContentCount == 0)

                return 0;

            int completedItems = _context.StudentProgress

                .Count(p =>

                    p.StudentId == studentId &&

                    contentIds.Contains(p.ContentId) &&

                    p.IsCompleted);

            double percentage = ((double)completedItems / totalContentCount) * 100;

            return Math.Round(percentage, 2);

        }


        // ================= MAIN METHOD =================

        public async Task<EnrollmentAverageScoreDTO> GetAverageScoreAsync(string enrollmentId)

        {

            // 🔹 Get enrollment

            var enrollment = await _context.Enrollment

                .Include(e => e.Student)

                .Include(e => e.Course)

                .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);

            if (enrollment == null)

                return null;

            // 🔹 Get assessments

            var assessmentIds = await _context.Assessments

                .Where(a => a.CourseId == enrollment.CourseId)

                .Select(a => a.AssessmentID)

                .ToListAsync();

            int totalAssessments = assessmentIds.Count;

            // 🔹 Get submissions

            var submissions = await _context.Submission

                .Where(s =>

                    s.StudentID == enrollment.StudentId &&

                    assessmentIds.Contains(s.AssessmentId))

                .ToListAsync();

            int completedAssessments = submissions.Count;

            // 🔹 Scores

            decimal totalScore = submissions.Any() ? submissions.Sum(s => (decimal)s.Score) : 0;

            decimal avgScore = submissions.Any() ? (decimal)submissions.Average(s => (decimal)s.Score) : 0;

            // 🔹 Assessment %

            decimal assessmentPercentage = totalAssessments > 0

                ? (completedAssessments * 100m) / totalAssessments

                : 0;

            // 🔹 Content %

            double contentPercentage = await GetCourseProgressPercentageAsync(

                enrollment.StudentId,

                enrollment.CourseId

            );

            decimal finalPercentage = Math.Round((decimal)contentPercentage, 2);

            // ================= UPSERT =================

            var existing = await _context.Performances

                .FirstOrDefaultAsync(p => p.EnrollmentId == enrollmentId);

            if (existing != null)

            {

                existing.AvgScore = avgScore;

                existing.CompletionPercentage = finalPercentage;

                existing.LastUpdated = DateTime.Now;

                _context.Performances.Update(existing);

            }

            else

            {

                var courseBatch = await _context.CourseBatches

                    .FirstOrDefaultAsync(cb => cb.CourseId == enrollment.CourseId);

                var performance = new Performance

                {

                    ProgressID = Guid.NewGuid().ToString(),

                    EnrollmentId = enrollmentId,

                    StudentId = enrollment.StudentId,

                    AvgScore = avgScore,

                    CompletionPercentage = finalPercentage,

                    LastUpdated = DateTime.Now,

                    BatchId = courseBatch?.BatchId,

                    InstructorId = courseBatch?.InstructorId

                };

                await _context.Performances.AddAsync(performance);

            }

            await _context.SaveChangesAsync();

            // ================= RETURN DTO =================

            return new EnrollmentAverageScoreDTO

            {

                EnrollmentId = enrollment.EnrollmentId,

                StudentName = enrollment.Student?.StudentName ?? "N/A",

                CourseName = enrollment.Course?.CourseName ?? "N/A",

                TotalScore = totalScore,

                AverageScore = avgScore,


                TotalAssessments = totalAssessments,

                CompletedAssessments = completedAssessments,

                AssessmentPercentage = Math.Round(assessmentPercentage, 2)

            };

        }



        // ✅ LAST UPDATED

        public async Task<LastUpdatedDTO> GetLastModifiedDateAsync(string enrollmentId)

        {

            // ✅ Validation

            if (string.IsNullOrEmpty(enrollmentId))

                throw new ArgumentException("EnrollmentId is required");

            // ✅ Fetch latest record

            var data = await (from p in _context.Performances.AsNoTracking()

                              join e in _context.Enrollment on p.EnrollmentId equals e.EnrollmentId

                              join s in _context.Student on e.StudentId equals s.StudentId

                              join c in _context.Course on e.CourseId equals c.CourseId

                              join b in _context.CourseBatches on p.BatchId equals b.BatchId

                              where p.EnrollmentId == enrollmentId

                              orderby p.LastUpdated descending

                              select new

                              {

                                  s.StudentId,

                                  s.StudentName,

                                  c.CourseName,

                                  p.BatchId,

                                  b.InstructorId,

                                  p.EnrollmentId,

                                  p.LastUpdated

                              }).FirstOrDefaultAsync();

            // ✅ No data case

            if (data == null)

            {

                return new LastUpdatedDTO

                {

                    StudentName = "No Data Found",

                    EnrollmentId = enrollmentId

                };

            }

            // 🇮🇳 ✅ Convert UTC → IST

            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            DateTime istTime = TimeZoneInfo.ConvertTimeFromUtc(

                data.LastUpdated,

                istZone

            );

            DateTimeOffset istOffset = new DateTimeOffset(

                istTime,

                istZone.GetUtcOffset(istTime)

            );


            // ✅ Return DTO

            return new LastUpdatedDTO

            {

                StudentId = data.StudentId,

                StudentName = data.StudentName,

                CourseName = data.CourseName,

                BatchId = data.BatchId,

                InstructorId = data.InstructorId,

                EnrollmentId = data.EnrollmentId,

                LastUpdated = istOffset

            };

        }


        // ✅ INSTRUCTOR BATCHES

        public async Task<List<InstructorBatchDTO>> GetInstructorBatchesAsync(string instructorId)

        {

            // ✅ validation

            if (string.IsNullOrEmpty(instructorId))

                throw new ArgumentException("InstructorId is required");

            // ✅ main query

            var result = await (from cb in _context.CourseBatches.AsNoTracking()

                                join c in _context.Course on cb.CourseId equals c.CourseId

                                where cb.InstructorId == instructorId

                                select new InstructorBatchDTO

                                {

                                    InstructorId = cb.InstructorId,

                                    BatchId = cb.BatchId,

                                    CourseName = c.CourseName,

                                    // 🔥 correct student count

                                    StudentCount = _context.StudentBatchAssignments

                                        .Count(sba => sba.BatchId == cb.BatchId)

                                })

                                .ToListAsync();

            return result;

        }


        // ✅ BATCH PERFORMANCE

        public async Task<GetBatchReportDTO> GetBatchPerformanceAsync(string batchId)

        {

            // ✅ Get batch

            var batch = await _context.CourseBatches

                .FirstOrDefaultAsync(b => b.BatchId == batchId);

            if (batch == null)

                return null;

            // ✅ Get instructor

            var instructor = await _context.Instructor

                .FirstOrDefaultAsync(i => i.InstructorId == batch.InstructorId);

            // ✅ Get enrollments

            var enrollments = await _context.Enrollment

                .Include(e => e.Student)

                .Include(e => e.Course)

                .Where(e => e.CourseId == batch.CourseId)

                .ToListAsync();

            int totalStudents = enrollments.Count;

            var studentList = new List<StudentPerformanceDTO>();

            double totalBatchProgress = 0;

            double totalBatchAttendance = 0;

            decimal totalBatchScore = 0;

            int completedStudents = 0;

            foreach (var enrollment in enrollments)

            {

                // 🔹 1. Completion %

                double progress = await GetCourseProgressPercentageAsync(

                    enrollment.StudentId,

                    enrollment.CourseId

                );

                if (progress == 100)

                    completedStudents++;

                totalBatchProgress += progress;

                // 🔹 2. Attendance %

                var attendanceRecords = await _context.Attendances

                    .Where(a => a.EnrollmentID == enrollment.EnrollmentId && !a.IsDeleted)

                    .ToListAsync();

                double attendancePercentage = 0;

                if (attendanceRecords.Count > 0)

                {

                    int total = attendanceRecords.Count;

                    int present = attendanceRecords.Count(a =>

                        a.Status != null &&

                        (a.Status.ToLower() == "present" || a.Status.ToLower() == "active"));

                    attendancePercentage = ((double)present / total) * 100;

                }

                totalBatchAttendance += attendancePercentage;

                // 🔹 3. Score

                var scores = await _context.Submission

                    .Where(s => s.StudentID == enrollment.StudentId)

                    .Select(s => s.Score)

                    .ToListAsync();

                decimal totalScore = scores.Any() ? scores.Sum() : 0;

                decimal avgScore = scores.Any() ? (decimal)scores.Average() : 0;

                totalBatchScore += avgScore;

                // 🔹 4. Last Updated (student level only)

                var performance = await _context.Performances

                    .FirstOrDefaultAsync(p => p.EnrollmentId == enrollment.EnrollmentId);

                DateTime? lastUpdated = performance?.LastUpdated;

                // 🔹 Add student

                studentList.Add(new StudentPerformanceDTO

                {

                    StudentId = enrollment.StudentId,

                    StudentName = enrollment.Student?.StudentName,

                    CourseName = enrollment.Course?.CourseName,

                    AvgScore = avgScore,

                    TotalScore = totalScore,

                    CompletionPercentage = Math.Round(progress, 2),

                    AttendancePercentage = Math.Round(attendancePercentage, 2),

                    EnrollmentId = enrollment.EnrollmentId,

                    LastUpdated = lastUpdated,

                    Marks = totalScore

                });

            }

            // 🔹 Batch averages

            double batchAvgProgress = totalStudents > 0 ? totalBatchProgress / totalStudents : 0;

            double batchAvgAttendance = totalStudents > 0 ? totalBatchAttendance / totalStudents : 0;

            decimal batchAvgScore = totalStudents > 0 ? totalBatchScore / totalStudents : 0;

            // 🔹 Top Performer

            var topStudent = studentList

                .OrderByDescending(s => s.TotalScore)

                .FirstOrDefault();

            return new GetBatchReportDTO

            {

                BatchId = batch.BatchId,

                CourseName = enrollments.FirstOrDefault()?.Course?.CourseName,

                BatchAveragePercentage = (decimal)Math.Round(batchAvgProgress, 2),

                BatchAverageAttendance = (decimal)Math.Round(batchAvgAttendance, 2),

                BatchAverageScore = Math.Round(batchAvgScore, 2),

                TotalStudents = totalStudents,

                CompletedStudents = completedStudents,

                TopPerformer = topStudent?.StudentName,

                InstructorId = batch.InstructorId,

                InstructorName = instructor?.InstructorName,

                Students = studentList

            };

        }
    }
}
 