//using EduTrackAcademics.Data;
//using EduTrackAcademics.DTO;
//using EduTrackAcademics.Model;
//using EduTrackAcademics.Services;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.CodeAnalysis.Scripting;
//using Microsoft.EntityFrameworkCore;

//namespace EduTrackAcademics.Controllers
//{
//	[Route("api/[controller]")]
//	[ApiController]
//	public class RegistrationsController : ControllerBase
//	{
//		private readonly IRegistrationService _sr;
//		private readonly EduTrackAcademicsContext _context;
//		private readonly IdService _idService;
//		private readonly PasswordService _passwordService;
//		private readonly EmailService _emailService;

//		public RegistrationsController(
//			IRegistrationService sr,
//			EduTrackAcademicsContext context,
//			IdService idService,
//			PasswordService passwordService,
//			EmailService emailService)
//		{
//			_sr = sr;
//			_context = context;
//			_idService = idService;
//			_passwordService = passwordService;
//			_emailService = emailService;
//		}



//		[HttpPost("StudentDetails")]
//		public IActionResult StudentRegister(StudentDTO dto)
//		{
//			var student = new Student
//			{
//				StudentName = dto.StudentName,
//				StudentEmail = dto.StudentEmail,
//				StudentPhone = dto.StudentPhone,
//				StudentQualification = dto.StudentQualification,
//				StudentProgram = dto.StudentProgram,
//				StudentAcademicYear = dto.StudentAcademicYear,
//				Year = dto.year,
//				StudentGender = dto.StudentGender,

//				 🔐 HASH PASSWORD
//				StudentPassword = BCrypt.Net.BCrypt.HashPassword(dto.StudentPassword),

//				 ✅ ROLE ASSIGNED HERE
//				Role = "Student"
//			};

//			var result = _sr.StuRegister(student);
//			return Ok(result);
//		}

//		[HttpGet("StudentsInfo")]
//		public IActionResult GetStudents()
//		{
//			return Ok(_context.Student.ToList());
//		}


//		INSTRUCTOR REGISTRATION

//	   [HttpPost("InstructorReg")]
//		public async Task<IActionResult> RegisterInstructor([FromForm] InstructorDTO dto)
//		{
//			if (!ModelState.IsValid)
//				return BadRequest(ModelState);

//			var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "resumes");
//			Directory.CreateDirectory(uploadFolder);

//			var extension = Path.GetExtension(dto.Resume.FileName).ToLower();
//			var allowed = new[] { ".pdf", ".doc", ".docx" };

//			if (!allowed.Contains(extension))
//				return BadRequest("Only PDF, DOC, DOCX files allowed");

//			var fileName = $"{Guid.NewGuid()}{extension}";
//			var path = Path.Combine(uploadFolder, fileName);

//			using var stream = new FileStream(path, FileMode.Create);
//			await dto.Resume.CopyToAsync(stream);

//			var instructor = new Instructor
//			{
//				InstructorName = dto.InstructorName,
//				InstructorEmail = dto.InstructorEmail,
//				InstructorPhone = dto.InstructorPhone,
//				InstructorQualification = dto.InstructorQualification,
//				InstructorSkills = dto.InstructorSkills,
//				InstructorExperience = dto.InstructorExperience,
//				InstructorJoinDate = dto.InstructorJoinDate,
//				InstructorGender = dto.InstructorGender,

//				InstructorPassword = BCrypt.Net.BCrypt.HashPassword(dto.InstructorPassword),

//				Role = "Instructor", // ✅

//				ResumePath = $"resumes/{fileName}"
//			};
//			return Ok(new
//			{
//				Message = "Instructor registered successfully",
//				Data = instructor
//			});
//		}

//		[HttpGet("Instructors")]
//		public IActionResult GetInstructors()
//		{
//			return Ok(_context.Instructor.ToList());
//		}


//		COORDINATOR REGISTRATION(ADMIN ONLY)

//		[HttpPost("CoordinatorReg")]
//		public async Task<IActionResult> RegisterCoordinator([FromForm] CoordinatorDTO dto)
//		{
//			if (_context.Coordinator.Any(x => x.CoordinatorEmail == dto.CoordinatorEmail))
//				return BadRequest("Coordinator already exists");

//			Resume upload
//			var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "coordinator-resumes");
//			Directory.CreateDirectory(folder);

//			var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Resume.FileName)}";
//			var filePath = Path.Combine(folder, fileName);

//			using var stream = new FileStream(filePath, FileMode.Create);
//			await dto.Resume.CopyToAsync(stream);

//			Generate temp password
//		   var tempPassword = _passwordService.GenerateRandomPassword();

//			var coordinator = new Coordinator
//			{
//				CoordinatorId = $"C{(_context.Coordinator.Count() + 1):D3}",
//				CoordinatorName = dto.CoordinatorName,
//				CoordinatorEmail = dto.CoordinatorEmail,
//				CoordinatorPhone = dto.CoordinatorPhone,
//				CoordinatorQualification = dto.CoordinatorQualification,
//				CoordinatorExperience = dto.CoordinatorExperience,
//				CoordinatorGender = dto.CoordinatorGender,

//				CoordinatorPassword = BCrypt.Net.BCrypt.HashPassword(tempPassword),

//				Role = "Coordinator", // ✅ VERY IMPORTANT

//				ResumePath = $"coordinator-resumes/{fileName}",
//				IsFirstLogin = true,
//				IsActive = true
//			};
//			return Ok(new
//			{
//				Message = "Coordinator registered successfully",
//				Data = coordinator
//			});
//		}


//		COORDINATOR LOGIN

//	   [HttpPost("CoordinatorLogin")]
//		public IActionResult CoordinatorLogin(LoginDTO dto)
//		{
//			var coord = _context.Coordinator
//				.FirstOrDefault(x => x.CoordinatorEmail == dto.Email);

//			if (coord == null ||
//				!BCrypt.Net.BCrypt.Verify(dto.Password, coord.CoordinatorPassword))
//				return Unauthorized("Invalid credentials");

//			if (coord.IsFirstLogin)
//				return Ok(new { Status = "CHANGE_PASSWORD_REQUIRED" });

//			return Ok("Login successful");
//		}



//		[HttpPost("CoordinatorChangePassword")]
//		public IActionResult ChangeCoordinatorPassword(ChangePasswordDTO dto)
//		{
//			var coord = _context.Coordinator
//				.FirstOrDefault(x => x.CoordinatorEmail == dto.Email);

//			if (coord == null)
//				return NotFound();

//			coord.CoordinatorPassword = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
//			coord.IsFirstLogin = false;

//			_context.SaveChanges();
//			return Ok("Password changed successfully");
//		}
//	}
//}