using System.Threading.Tasks;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Model;
using EduTrackAcademics.Repository;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;

namespace EduTrackAcademics.Services
{
	public static class PasswordHelper { 
		public static string HashPassword(string password) { 
			return BCrypt.Net.BCrypt.HashPassword(password); 
		}
		public static bool VerifyPassword(string password, string hashedPassword) { 
			return BCrypt.Net.BCrypt.Verify(password, hashedPassword); 
		} 
	}
	public class RegistrationService : IRegistrationService
	{
		private readonly IRegistrationRepository _repo;
		private readonly IWebHostEnvironment _env;

		public RegistrationService(IRegistrationRepository repo, IWebHostEnvironment env)
		{
			_repo = repo;
			_env = env;
		}
		public async Task RegisterStudentAsync(StudentDTO dto)
		{
			var hashedPassword = PasswordHelper.HashPassword(dto.StudentPassword);

			var user = new Users
			{
				Email = dto.StudentEmail,
				Password = hashedPassword,
				Role = "Student"
			};

			var student = new Student
			{
				StudentName = dto.StudentName,
				StudentEmail = dto.StudentEmail,
				StudentPhone = dto.StudentPhone,
				StudentQualification = dto.StudentQualification,
				StudentProgram = dto.StudentProgram,   
				StudentAcademicYear = dto.StudentAcademicYear,
				Year = dto.year,
				StudentGender = dto.StudentGender,
				StudentPassword = dto.StudentPassword
			};


			await _repo.RegisterStudentAsync(student, user);
		}
		public async Task RegisterInstructorAsync(InstructorDTO dto)
		{
			var hashedPassword = PasswordHelper.HashPassword(dto.InstructorPassword);
			string folderName = "resumes";
			string pathToSave = Path.Combine(_env.WebRootPath, folderName);

			// 2. Create directory if it doesn't exist
			if (!Directory.Exists(pathToSave))
				Directory.CreateDirectory(pathToSave);

			// 3. Generate unique filename to prevent overwriting
			string uniqueFileName = $"{Guid.NewGuid()}_{dto.InstructorResume.FileName}";
			string fullPath = Path.Combine(pathToSave, uniqueFileName);

			using (var stream = new FileStream(fullPath, FileMode.Create))
			{
				await dto.InstructorResume.CopyToAsync(stream);
			}
			var user = new Users
			{
				Email = dto.InstructorEmail,
				Password = hashedPassword,
				Role = "Instructor"
			};

			var instructor = new Instructor
			{
				InstructorName = dto.InstructorName,
				InstructorEmail = dto.InstructorEmail,
				InstructorPhone = dto.InstructorPhone,
				InstructorQualification = dto.InstructorQualification,
				InstructorSkills = dto.InstructorSkills,
				InstructorExperience = dto.InstructorExperience,
				InstructorJoinDate = dto.InstructorJoinDate,
				InstructorGender = dto.InstructorGender,
				InstructorPassword = dto.InstructorPassword,
				ResumePath = Path.Combine(folderName, uniqueFileName)

			};

			await _repo.RegisterInstructorAsync(instructor, user);
		}
		public async Task RegisterCoordinatorAsync(CoordinatorDTO dto)
		{
			var hashpassword = PasswordHelper.HashPassword(dto.CoordinatorPassword);
			string folderName = "resumes";
			string pathToSave = Path.Combine(_env.WebRootPath, folderName);

			// 2. Create directory if it doesn't exist
			if (!Directory.Exists(pathToSave))
				Directory.CreateDirectory(pathToSave);

			// 3. Generate unique filename to prevent overwriting
			string uniqueFileName = $"{Guid.NewGuid()}_{dto.Resumepath.FileName}";
			string fullPath = Path.Combine(pathToSave, uniqueFileName);

			using (var stream = new FileStream(fullPath, FileMode.Create))
			{
				await dto.Resumepath.CopyToAsync(stream);
			}
			var user = new Users
			{
				Email = dto.CoordinatorEmail,
				Password = hashpassword,
				Role = "Coordinator"
			};

			var coordinator = new Coordinator
			{
				CoordinatorName = dto.CoordinatorName,
				CoordinatorEmail = dto.CoordinatorEmail,
				CoordinatorPhone = dto.CoordinatorPhone,
				CoordinatorQualification = dto.CoordinatorQualification,
				CoordinatorExperience = dto.CoordinatorExperience,
				CoordinatorGender = dto.CoordinatorGender,
				CoordinatorPassword = dto.CoordinatorPassword,
				Resumepath = Path.Combine(folderName, uniqueFileName)

			};

			await _repo.RegisterCoordinatorAsync(coordinator, user);
		}

	}
}

//private readonly IRegistrationRepo _repo;

//public RegistrationService(IRegistrationRepo repo)
//{
//	_repo = repo;
//}

//public List<Student> StuRegister(Student student)
//{
//	return _repo.StudentRegistration(student);
//}

//public List<Instructor> InsRegister(Instructor ins)
//{
//	return _repo.InstructorRegistration(ins);
//}