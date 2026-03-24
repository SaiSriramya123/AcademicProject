using EduTrackAcademics.DTO;
using EduTrackAcademics.Model;
using System.Threading.Tasks;
namespace EduTrackAcademics.Services
{
	public interface IRegistrationService
	{
		//public List<Student> StuRegister(Student stu);
		//public List<Instructor> InsRegister(Instructor ins);
			Task RegisterStudentAsync(StudentDTO dto);
			Task RegisterInstructorAsync(InstructorDTO dto);
			Task RegisterCoordinatorAsync(CoordinatorDTO dto);
		}
}

