using EduTrackAcademics.Data;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Dummy;
using EduTrackAcademics.Exceptions;
using EduTrackAcademics.Model;
using Microsoft.EntityFrameworkCore;

namespace EduTrackAcademics.Repository
{


	public class AcademicReportRepository : IAcademicReportRepository

	{

		private readonly EduTrackAcademicsContext _context;
		public AcademicReportRepository(EduTrackAcademicsContext context)

		{
			_context = context;

		}

		//method to get  batch reports
		//public List<AcademicReport> GetReport()

		//{

		//    var batches = _context.CourseBatches.ToList();

		//    if (batches == null || batches.Count == 0)

		//        throw new BatchNotFoundException("No batches available");

		//    var reports = new List<AcademicReport>();

		//    foreach (var batch in batches)

		//    {

		//        // Get students in batch

		//        var studentIds = _context.StudentBatchAssignments

		//                            .Where(s => s.BatchId == batch.BatchId)

		//                            .Select(s => s.StudentId)

		//                            .ToList();

		//        if (studentIds.Count == 0)

		//            continue;

		//        int totalStudents = studentIds.Count;

		//        // Get performance records

		//        var performances = _context.Performances

		//                            .Where(p => studentIds.Contains(p.StudentId))

		//                            .ToList();

		//        int completedStudents = performances

		//                                .Where(p => p.AvgScore >= 40)

		//                                .Count();

		//        decimal completionRate = totalStudents == 0

		//            ? 0

		//            : ((decimal)completedStudents / totalStudents) * 100;

		//        decimal avgScore = performances.Count == 0

		//            ? 0

		//            : performances.Average(p => p.AvgScore);

		//        int droppedStudents = totalStudents - completedStudents;

		//        decimal dropOutRate = totalStudents == 0

		//            ? 0

		//            : ((decimal)droppedStudents / totalStudents) * 100;

		//        // Check if report already exists

		//        var existingReport = _context.AcademicReport

		//                                .FirstOrDefault(r => r.Scope == batch.BatchId);

		//        if (existingReport != null)

		//        {

		//            // UPDATE

		//            existingReport.CompletionRate = completionRate;

		//            existingReport.AvgScore = avgScore;

		//            existingReport.DropOutRate = dropOutRate;

		//            existingReport.GeneratedDate = DateTime.Now;

		//            reports.Add(existingReport);

		//        }

		//        else

		//        {

		//            // INSERT

		//            var newReport = new AcademicReport

		//            {

		//                ReportId = Guid.NewGuid().ToString(),

		//                Scope = batch.BatchId,   // storing BatchId in Scope

		//                CompletionRate = completionRate,

		//                AvgScore = avgScore,

		//                DropOutRate = dropOutRate,

		//                GeneratedDate = DateTime.Now

		//            };

		//            _context.AcademicReport.Add(newReport);

		//            reports.Add(newReport);

		//        }

		//    }

		//    _context.SaveChanges();

		//    return reports;

		//}

		//method to get single batch report 

		public List<BatchAveragePerformanceDTO> GetAllBatchPerformanceReport()
		{
			var batches = _context.CourseBatches
				.Include(b => b.Course)
				.ToList();

			var result = new List<BatchAveragePerformanceDTO>();
			foreach (var batch in batches)
			{
				// Get assessments for course
				var assessments = _context.Assessments
					.Where(a => a.CourseId == batch.CourseId)
					.Select(a => new { a.AssessmentID, a.MaxMarks })
					.ToList();

				decimal totalMaxMarks = assessments.Sum(a => a.MaxMarks);
				var assessmentIds = assessments.Select(a => a.AssessmentID).ToList();

				//  Get students in batch
				var students = (from sba in _context.StudentBatchAssignments
								join s in _context.Student on sba.StudentId equals s.StudentId
								where sba.BatchId == batch.BatchId
								select new
								{
									s.StudentId,
									s.StudentName
								}).ToList();

				var studentResults = new List<BatchPerformanceDTO>();
				var percentageList = new List<decimal>();

				foreach (var student in students)
				{
					var submissions = _context.Submissions
						.Where(s => s.StudentID == student.StudentId &&
									assessmentIds.Contains(s.AssessmentId))
						.ToList();

					decimal totalScore = submissions.Sum(s => (decimal)s.Score);
					decimal avgScore = submissions.Any() ? submissions.Average(s => (decimal)s.Score) : 0;

					decimal percentage = totalMaxMarks > 0
						? (totalScore / totalMaxMarks) * 100
						: 0;

					DateTime? lastUpdated = submissions
						.OrderByDescending(s => s.SubmissionDate)
						.Select(s => (DateTime?)s.SubmissionDate)
						.FirstOrDefault();

					percentageList.Add(percentage);

					studentResults.Add(new BatchPerformanceDTO
					{
						StudentId = student.StudentId,
						StudentName = student.StudentName,
						CourseName = batch.Course.CourseName,
						TotalScore = totalScore,
						AvgScore = avgScore,
						CompletionPercentage = Math.Round(percentage, 2),
						LastUpdated = lastUpdated
					});
				}

				decimal batchAveragePercentage = percentageList.Any()
					? percentageList.Average()
					: 0;

				result.Add(new BatchAveragePerformanceDTO
				{
					BatchId = batch.BatchId,
					CourseName = batch.Course.CourseName,
					BatchAveragePercentage = Math.Round(batchAveragePercentage, 2),
					Students = studentResults
				});
			}

			return result;
		}

	}
}








