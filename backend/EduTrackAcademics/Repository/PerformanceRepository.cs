using EduTrackAcademics.Data;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Dummy;
using EduTrackAcademics.Exceptions;
using EduTrackAcademics.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EduTrackAcademics.Repository
{
    public class PerformanceRepository : IPerformanceRepository
    {
        private readonly EduTrackAcademicsContext _context;
        public PerformanceRepository(EduTrackAcademicsContext context)
        {
            _context = context;
        }


		//method for getbatchperformance
		public BatchAveragePerformanceDTO GetBatchPerformance(string batchId)
		{
			// 1️⃣ Get course info
			var batchInfo = _context.CourseBatches
				.Include(b => b.Course)
				.FirstOrDefault(b => b.BatchId == batchId);

			if (batchInfo == null)
				throw new ApplicationException("Batch not found");

			// 2️⃣ Get assessments
			var assessments = _context.Assessments
				.Where(a => a.CourseId == batchInfo.CourseId)
				.Select(a => new { a.AssessmentID, a.MaxMarks })
				.ToList();

			decimal totalMaxMarks = assessments.Sum(a => a.MaxMarks);

			// 3️⃣ Get students in batch
			var students = (from sba in _context.StudentBatchAssignments
							join s in _context.Student on sba.StudentId equals s.StudentId
							where sba.BatchId == batchId
							select new
							{
								s.StudentId,
								s.StudentName
							}).ToList();

			var studentResults = new List<BatchPerformanceDTO>();
			var percentageList = new List<decimal>();

			foreach (var student in students)
			{
				var scores = _context.Submissions
					.Where(s =>
						s.StudentID == student.StudentId &&
						assessments.Select(a => a.AssessmentID).Contains(s.AssessmentId))
					.Select(s => s.Score)
					.ToList();

				decimal totalScore = scores.Any() ? scores.Sum(s => (decimal)s) : 0;
				decimal avgScore = scores.Any() ? scores.Average(s => (decimal)s) : 0;

				decimal percentage = totalMaxMarks > 0
					? (totalScore / totalMaxMarks) * 100
					: 0;

				percentageList.Add(percentage);

				studentResults.Add(new BatchPerformanceDTO
				{
					StudentId = student.StudentId,
					StudentName = student.StudentName,
					CourseName = batchInfo.Course.CourseName,
					Marks = scores.LastOrDefault(),
					AvgScore = avgScore,
					CompletionPercentage = Math.Round(percentage, 2),
					LastUpdated = DateTime.Now
				});
			}

			//  Batch Average Percentage
			decimal batchAveragePercentage = percentageList.Any()
				? percentageList.Average()
				: 0;

			return new BatchAveragePerformanceDTO
			{
				BatchId = batchId,
				CourseName = batchInfo.Course.CourseName,
				BatchAveragePercentage = Math.Round(batchAveragePercentage, 2),
				Students = studentResults
			};
		}



		//method for AvgScore
		public EnrollmentAverageScoreDTO GetAverageScore(string enrollmentId)
		{
			// Get enrollment with student & course
			var enrollment = _context.Enrollment
				.Include(e => e.Student)
				.Include(e => e.Course)
				.FirstOrDefault(e => e.EnrollmentId == enrollmentId);

			if (enrollment == null)
				throw new EnrollmentNotExistsException("Enrollment not found", 404);

			// 2 Get all assessments of the course
			var assessmentIds = _context.Assessments
				.Where(a => a.CourseId == enrollment.CourseId)
				.Select(a => a.AssessmentID)
				.ToList();

			if (!assessmentIds.Any())
				throw new ApplicationException("No assessments found for this course");

			//  Get all submission scores of the student for those assessments
			var scores = _context.Submissions
				.Where(s =>
					s.StudentID == enrollment.StudentId &&
					assessmentIds.Contains(s.AssessmentId))
				.Select(s => s.Score)
				.ToList();

			// 4 Calculate Total & Average
			decimal totalScore = scores.Any()
				? scores.Sum(s => (decimal)s)
				: 0;

			decimal averageScore = scores.Any()
				? scores.Average(s => (decimal)s)
				: 0;

			//  Get total max marks of course
			decimal totalMaxMarks = _context.Assessments
				.Where(a => a.CourseId == enrollment.CourseId)
				.Sum(a => (decimal)a.MaxMarks);

			//  Calculate Percentage
			decimal percentage = totalMaxMarks > 0
				? (totalScore / totalMaxMarks) * 100
				: 0;

			//  Return DTO
			return new EnrollmentAverageScoreDTO
			{
				EnrollmentId = enrollment.EnrollmentId,
				StudentName = enrollment.Student.StudentName,
				CourseName = enrollment.Course.CourseName,
				TotalScore = totalScore,
				AverageScore = averageScore,
				Percentage = Math.Round(percentage, 2)
			};
		}




		//method for LastModifiedDate
		public BatchPerformanceDTO GetLastModifiedDate(string enrollmentId)
		{
			//  Get Enrollment + Student + Course
			var enrollmentData = (from e in _context.Enrollment
								  join s in _context.Student on e.StudentId equals s.StudentId
								  join c in _context.Course on e.CourseId equals c.CourseId
								  where e.EnrollmentId == enrollmentId
								  select new
								  {
									  e.StudentId,
									  StudentName = s.StudentName,
									  CourseName = c.CourseName,
									  e.CourseId
								  }).FirstOrDefault();

			if (enrollmentData == null)
				throw new EnrollmentNotExistsException("Enrollment not found", 404);

			//  Get Assessment IDs for the Course
			var assessmentIds = _context.Assessments
				.Where(a => a.CourseId == enrollmentData.CourseId)
				.Select(a => a.AssessmentID)
				.ToList();

			if (!assessmentIds.Any())
				throw new ApplicationException("No assessments found for course");

			//  Get LAST submission date for those assessments
			var lastModified = _context.Submission
				.Where(s => s.StudentID == enrollmentData.StudentId
						 && assessmentIds.Contains(s.AssessmentId))
				.OrderByDescending(s => s.SubmissionDate)
				.Select(s => s.SubmissionDate)
				.FirstOrDefault();

			if (lastModified == default)
				throw new ApplicationException("No submissions found");

			return new BatchPerformanceDTO
			{
				StudentId = enrollmentData.StudentId,
					
				StudentName = enrollmentData.StudentName,
				CourseName = enrollmentData.CourseName,
				LastUpdated = lastModified
			};
		}


		//method for getting Instructor Batches

		public List<InstructorBatchDTO> GetInstructorBatches(string instructorId)
        {
            var result = (from b in _context.CourseBatches
                          join c in _context.Course
                          on b.CourseId equals c.CourseId
                          where b.InstructorId == instructorId
                          select new InstructorBatchDTO
                          {
                              BatchId = b.BatchId,
                              InstructorId = b.InstructorId,
                              CourseName = c.CourseName
                          }).ToList();
            if (result.Count == 0)
                throw new InstructorBatchesNotFoundException("No batches found", 404);
            return result;
        }



    }
    }


