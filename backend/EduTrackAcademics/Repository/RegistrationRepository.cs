using EduTrackAcademics.Data;
using EduTrackAcademics.Exceptions;
using EduTrackAcademics.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace EduTrackAcademics.Repository
{
	public class RegistrationRepository : IRegistrationRepository
	{
		private readonly EduTrackAcademicsContext _context;
		
		public RegistrationRepository(EduTrackAcademicsContext context)
		{
			_context = context;
		}

		public async Task RegisterStudentAsync(Student student, Users user)
		{
			var email = user.Email;

			if (!email.ToLower().EndsWith("@gmail.com"))
				throw new InvalidEmailDomainException("Only @gmail.com addresses are allowed.");
			// Duplicate email rule 
			if (_context.Users.Any(u => u.Email == email))
				throw new EmailAlreadyRegisteredException("Email already registered.");

			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			student.UserId = user.UserId;
			student.StudentId = GenerateStudentId(); // assign custom ID

			_context.Student.Add(student);
			await _context.SaveChangesAsync();
		}

		public string GenerateStudentId()
		{
			int count = _context.Student.Count();
			return $"S{(count + 1):D3}";
		}
		public string GenerateInstructorId()
		{
			int count = _context.Instructor.Count();
			return $"I{(count + 1):D3}";
		}

		public async Task RegisterInstructorAsync(Instructor instructor, Users user)
		{
			var email = user.Email;

			if (!email.ToLower().EndsWith("@gmail.com"))
				throw new InvalidEmailDomainException("Only @gmail.com addresses are allowed.");
			// Duplicate email rule 
			if (_context.Users.Any(u => u.Email == email))
				throw new EmailAlreadyRegisteredException("Email already registered.");

			// Save User first
			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			// Assign generated UserId to Instructor
			instructor.UserId = user.UserId;

			//add the generated id
			instructor.InstructorId = GenerateInstructorId(); // assign custom ID

			// Save Instructor record
			_context.Instructor.Add(instructor);
			await _context.SaveChangesAsync();
		}

		public string GenerateCoordinatorId()
		{
			int count = _context.Coordinator.Count();
			return $"C{(count + 1):D3}";
		}

		public async Task RegisterCoordinatorAsync(Coordinator coordinator, Users user)
		{	

			var email = user.Email;
			if (!email.ToLower().EndsWith("@gmail.com"))
				throw new InvalidEmailDomainException("Only @gmail.com addresses are allowed.");
			// Duplicate email rule 
			if (_context.Users.Any(u => u.Email == email))
				throw new EmailAlreadyRegisteredException("Email already registered.");

			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			// 2. Assign the Foreign Key (The auto-generated Int from Users)
			coordinator.UserId = user.UserId;

			
			// You were missing this or assigning it to the wrong property
			coordinator.CoordinatorId = GenerateCoordinatorId();

			// 4. Save Coordinator record
			_context.Coordinator.Add(coordinator);
			await _context.SaveChangesAsync();
		}	
	}
}
