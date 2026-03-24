using EduTrackAcademics.Model;


namespace EduTrackAcademics.Repository
{
	public interface IRegistrationRepo
	{
		public List<Student> StudentRegistration(Student Student);
		public List<Instructor> InstructorRegistration(Instructor ins);
	}
}
