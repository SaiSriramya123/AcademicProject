using EduTrackAcademics.Data;
using EduTrackAcademics.Model;
using EduTrackAcademics.Services;

namespace EduTrackAcademics.Repository
{
	public class RegistrationRepo : IRegistrationRepo
	{
		private readonly EduTrackAcademicsContext _context;
		private readonly IdService _idService;

		public RegistrationRepo(
			EduTrackAcademicsContext context,
			IdService idService)
		{
			_context = context;
			_idService = idService;
		}

		// STUDENT REGISTRATION
		public List<Student> StudentRegistration(Student student)
		{
			student.StudentId = _idService.GenerateStudentId();

			_context.Student.Add(student);
			_context.SaveChanges();

			return _context.Student.ToList();
		}

		// INSTRUCTOR REGISTRATION
		public List<Instructor> InstructorRegistration(Instructor ins)
		{
			if (_context.Instructor.Any(x => x.InstructorEmail == ins.InstructorEmail))
				throw new InvalidOperationException("Instructor already exists");

			ins.InstructorId = _idService.GenerateInstructorId();

			_context.Instructor.Add(ins);
			_context.SaveChanges();

			return _context.Instructor.ToList();
		}
	}
}