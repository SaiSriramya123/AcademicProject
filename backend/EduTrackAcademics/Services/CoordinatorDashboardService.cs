using EduTrackAcademics.DTO;
using EduTrackAcademics.Exceptions;
using EduTrackAcademics.Repository;
using Microsoft.AspNetCore.Authorization;

namespace EduTrackAcademics.Service
{
	public class CoordinatorDashboardService : ICoordinatorDashboardService
	{
		private readonly ICoordinatorDashboardRepo _repo;

		public CoordinatorDashboardService(ICoordinatorDashboardRepo repo)
		{
			_repo = repo;
		}

		// ================== PROGRAMS ==================

		//[Authorize(Roles = "Coordinator")]
		public IEnumerable<object> GetPrograms()
		{
			var programs = _repo.GetPrograms();

			if (programs == null || !programs.Any())
				throw new CourseNotFoundException("Programs not found");

			return programs;
		}

		// ================== ACADEMIC YEARS ==================

		//[Authorize(Roles = "Coordinator")]
		public IEnumerable<object> GetAcademicYears(string programId)
		{
			if (string.IsNullOrWhiteSpace(programId))
				throw new InvalidDataException("Program ID is required");

			var years = _repo.GetAcademicYears(programId);

			if (years == null || !years.Any())
				throw new CourseNotFoundException($"Academic years not found for program: {programId}");

			return years;
		}

		// ================== COURSE ==================

		public object AddCourse(CourseDTO dto)
		{
			if (dto == null)
				throw new InvalidDataException("Course data is required");

			return _repo.AddCourse(dto);
		}
		public IEnumerable<object> GetAllCourse()

		{

			var course = _repo.GetAllCourse();


			if (course == null || !course.Any())

				throw new CourseNotFoundException("Courses not found");


			return course;

		}

		public IEnumerable<object> GetCourses(string yearId)
		{
			if (string.IsNullOrWhiteSpace(yearId))
				throw new InvalidDataException("Year ID is required");

			var courses = _repo.GetCourses(yearId);

			if (courses == null || !courses.Any())
				throw new CourseNotFoundException($"Courses not found for year: {yearId}");

			return courses;
		}

		public object UpdateCourse(string courseId, CourseDTO dto)
		{
			if (string.IsNullOrWhiteSpace(courseId))
				throw new InvalidDataException("Course ID is required");

			if (dto == null)
				throw new InvalidDataException("Course data is required");

			return _repo.UpdateCourse(courseId, dto);
		}

		public bool DeleteCourse(string courseId)
		{
			if (string.IsNullOrWhiteSpace(courseId))
				throw new InvalidDataException("Course ID is required");

			var result = _repo.DeleteCourse(courseId);

			if (!result)
				throw new CourseNotFoundException($"Course not found: {courseId}");

			return result;
		}

		// ================== STUDENTS ==================

		public IEnumerable<object> GetStudents(string qualification, string program, int year)
		{
			if (string.IsNullOrWhiteSpace(qualification) ||
				string.IsNullOrWhiteSpace(program))
				throw new InvalidDataException("Qualification and Program are required");

			var students = _repo.GetStudents(qualification, program, year);

			if (students == null || !students.Any())
				throw new StudentNotFoundException($"{qualification}-{program}-{year}");

			return students;
		}

		public IEnumerable<object> GetStudentsInBatch(string batchId)
		{
			if (string.IsNullOrWhiteSpace(batchId))
				throw new InvalidDataException("Batch ID is required");

			var students = _repo.GetStudentsInBatch(batchId);

			if (students == null || !students.Any())
				throw new BatchNotFoundException(batchId);

			return students;
		}

		// ================== INSTRUCTORS ==================

		public IEnumerable<object> GetInstructors(string skill)
		{
			if (string.IsNullOrWhiteSpace(skill))
				throw new InvalidDataException("Skill is required");

			var instructors = _repo.GetInstructors(skill);

			if (instructors == null || !instructors.Any())
				throw new InstructorNotFoundException(skill);

			return instructors;
		}

		public IEnumerable<object> GetInstructorBatches(string instructorId)
		{
			if (string.IsNullOrWhiteSpace(instructorId))
				throw new InvalidDataException("Instructor ID is required");

			var batches = _repo.GetInstructorBatches(instructorId);

			if (batches == null || !batches.Any())
				throw new InstructorBatchesNotFoundException(instructorId);

			return batches;
		}

		public IEnumerable<object> InstructorDashboard(string instructorId)
		{
			if (string.IsNullOrWhiteSpace(instructorId))
				throw new InvalidDataException("Instructor ID is required");

			var dashboard = _repo.InstructorDashboard(instructorId);

			if (dashboard == null || !dashboard.Any())
				throw new InstructorNotFoundException(instructorId);

			return dashboard;
		}

		// ================== BATCH ==================

		public IEnumerable<object> GetBatches(string program, int year)
		{
			if (string.IsNullOrWhiteSpace(program))
				throw new InvalidDataException("Program is required");

			var batches = _repo.GetBatches(program, year);

			if (batches == null || !batches.Any())
				throw new BatchNotFoundException($"{program}-{year}");

			return batches;
		}

		public object GetBatchCount(string program, int year)
		{
			if (string.IsNullOrWhiteSpace(program))
				throw new InvalidDataException("Program is required");

			return _repo.GetBatchCount(program, year);
		}

		public object AssignBatches(AutoAssignBatchDTO dto)
		{
			if (dto == null)
				throw new InvalidDataException("Batch assignment data is required");

			return _repo.AssignBatches(dto);
		}

		public object AssignSingleBatch(AutoAssignBatchDTO dto)
		{
			if (dto == null)
				throw new InvalidDataException("Batch assignment data is required");

			return _repo.AssignSingleBatch(dto);
		}
	}
}